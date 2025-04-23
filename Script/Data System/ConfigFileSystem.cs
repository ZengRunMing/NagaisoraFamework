using NagaisoraFamework;
using System.IO;
using System.Text;

namespace NagaisoraFamework.DataFileSystem
{
	public class ConfigFileSystem
	{
		public static void SaveConfig(Stream stream, ConfigData ConfigData)
		{
			stream.SetLength(0);
			stream.Flush();

			BinaryWriter ConfigWriter = new(stream, Encoding.UTF8, true);
			ConfigWriter.BaseStream.Seek(0x00, SeekOrigin.Begin);

			ConfigWriter.Write(Encoding.UTF8.GetBytes($"{MainSystem.SCLENAME}\\CONFIGDATA"));
			ConfigWriter.Write(ConfigData.ToBinary());

			ConfigWriter.Close();
		}

		public static ConfigData LoadConfig(Stream stream)
		{
			BinaryReader ConfigReader = new(stream, Encoding.UTF8, true);
			ConfigReader.BaseStream.Seek(0x00, SeekOrigin.Begin);

			string hider = $"{MainSystem.SCLENAME}\\CONFIGDATA";
			int hiderbufferlength = Encoding.UTF8.GetBytes(hider).Length;

			byte[] readhiderbuffer = ConfigReader.ReadBytes(hiderbufferlength);

			if (Encoding.UTF8.GetString(readhiderbuffer) != hider)
			{
				throw new InvalidDataException("文件头不匹配");
			}

			ConfigData ConfigData = ConfigData.FromBinary(ConfigReader.ReadBytes((int)ConfigReader.BaseStream.Length - hiderbufferlength));
			ConfigReader.Close();

			return ConfigData;
		}
	}
}

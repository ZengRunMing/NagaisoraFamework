using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;
using System;

namespace NagaisoraFamework
{

	[Serializable]
	public struct ConfigData
	{
		public byte MusicVolume;
		public byte SEVolume;
		public byte BGMode;
		public byte OutputDeviceID;
		public byte Resolution;
		public byte DrawMode;
		public byte Frame;


		public Dictionary<string, KeyConfig> KeyConfigs;

		public static ConfigData Default = new()
		{
			MusicVolume = 9,
			SEVolume = 7,
			BGMode = 0,
			OutputDeviceID = 1,
			Resolution = 0,
			DrawMode = 0,
			Frame = 2,
			KeyConfigs = new Dictionary<string, KeyConfig>()
			{
				{"default", KeyConfig.Default },
			},
		};

		public byte[] ToBinary()
		{
			MemoryStream memoryStream = new();
			BinaryWriter writer = new(memoryStream, Encoding.UTF8, true);
			writer.BaseStream.Seek(0, SeekOrigin.Begin);

			writer.Write(MusicVolume);
			writer.Write(SEVolume);
			writer.Write(BGMode);
			writer.Write(OutputDeviceID);
			writer.Write(Resolution);
			writer.Write(DrawMode);
			writer.Write(Frame);

			string[] keys = KeyConfigs.Keys.ToArray();
			KeyConfig[] values = KeyConfigs.Values.ToArray();

			writer.Write((uint)KeyConfigs.Count);

			for (uint i = 0; i < KeyConfigs.Count; i++)
			{
				writer.Write(keys[i]);

				byte[] bytes = values[i].ToBinary();
				
				writer.Write((uint)bytes.Length);
				writer.Write(bytes);
			}

			writer.Close();

			return memoryStream.ToArray();
		}

		public static ConfigData FromBinary(byte[] binary)
		{
			MemoryStream memoryStream = new(binary);
			BinaryReader binaryReader = new(memoryStream, Encoding.UTF8);

			ConfigData configData = new()
			{
				MusicVolume = binaryReader.ReadByte(),
				SEVolume = binaryReader.ReadByte(),
				BGMode = binaryReader.ReadByte(),
				OutputDeviceID = binaryReader.ReadByte(),
				Resolution = binaryReader.ReadByte(),
				DrawMode = binaryReader.ReadByte(),
				Frame = binaryReader.ReadByte(),
			};

			uint count = binaryReader.ReadUInt32();

			configData.KeyConfigs = new();

			for (uint i = 0; i < count; i++)
			{
				string name = binaryReader.ReadString();

				uint length = binaryReader.ReadUInt32();
				byte[] buffer = binaryReader.ReadBytes((int)length);
				KeyConfig keyConfig = KeyConfig.FromBinary(buffer);

				configData.KeyConfigs.Add(name, keyConfig);
			}

			binaryReader.Close();
			memoryStream.Close();

			return configData;
		}
	}

}

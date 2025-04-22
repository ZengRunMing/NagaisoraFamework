using System;
using System.IO;
using System.Text;

namespace NagaisoraFamework
{
	public abstract class IDiskInfo
	{
		public abstract long StartSector { get; }
		public abstract long EndSector { get; }
		public abstract long SectorSize { get; }
		public abstract long DataLength { get; }
		public abstract byte[] SaveID { get; }

		public static IDiskInfo GetFormByte(byte[] vs)
		{
			if (vs.Length < 512)
			{
				return null;
			}

			MemoryStream memoryStream = new MemoryStream(vs);
			BinaryReader binaryReader = new BinaryReader(memoryStream);

			byte[] Hider = binaryReader.ReadBytes(9);

			byte[] Hider1 = Hider;

			Array.Resize(ref Hider1, 8);

			binaryReader.BaseStream.Position = 0;

			if (Encoding.UTF8.GetString(Hider) == "TRFS\\DISK")
			{
				return TRFSInfo.GetFormBytes((binaryReader.ReadBytes(512)));
			}
			else if (Encoding.UTF8.GetString(Hider1) == "RTA\\PART")
			{
				return RTA.LoadFormSectorData(binaryReader.ReadBytes(512));
			}
			else
			{
				return null;
			}
		}
	}
}

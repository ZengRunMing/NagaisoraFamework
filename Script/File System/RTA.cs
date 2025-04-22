using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NagaisoraFamework
{
	using Underlyingsystem;

	public class RTA : IDiskInfo, DiskSystemObj
	{
		public byte[] Version { get; }
		public override long StartSector { get; }
		public override long EndSector { get; }
		public override long SectorSize { get; }
		public override long DataLength { get; }
		public override byte[] SaveID { get; }

		public Guid Id { get; private set; }
		public Dictionary<string, Partition> Partitions;

		public long PartitionCount => Partitions.Count;

		public long BootPartitionCount;

		private long _PartitionCount;

		public RTA()
		{
			Id = Guid.NewGuid();
			Partitions = new Dictionary<string, Partition>();
		}

		public RTA(long startSector, long endSector, long sectorSize, long dataLength, long partitionCount, long bootPartitionCount, byte[] version, byte[] saveID, Guid guid) : this()
		{
			StartSector = startSector;
			EndSector = endSector;
			SectorSize = sectorSize;
			DataLength = dataLength;
			_PartitionCount = partitionCount;
			BootPartitionCount = bootPartitionCount;
			Version = version;
			SaveID = saveID;
			Id = guid;
		}

		public void LoadPartitions(DiskStream physicalDisk)
		{
			byte[][] PartitionDatas = physicalDisk.ReadSectorsArray(1, _PartitionCount);

			foreach (byte[] PartitionData in PartitionDatas)
			{
				Partition partition = Partition.LoadFormBytes(PartitionData);

				if (partition.IsBootPartition)
				{
					BootPartitionCount++;
				}

				AddPartitionRange(partition);
			}
		}

		public Partition[] GetAllBootPartitions()
		{
			List<Partition> partitions = new List<Partition>();

			foreach (Partition partition in Partitions.Values)
			{
				if (partition.IsBootPartition)
				{
					partitions.Add(partition);
				}
			}

			return partitions.ToArray();
		}

		public byte[] GetData()
		{
			MemoryStream Sector0Memory = new MemoryStream(new byte[512]);
			BinaryWriter Sector0Writer = new BinaryWriter(Sector0Memory);
			Sector0Writer.BaseStream.Position = 0;

			Sector0Writer.Write(Encoding.UTF8.GetBytes("RTA PART"));
			Sector0Writer.Write(Version);
			Sector0Writer.BaseStream.Position = 16;
			Sector0Writer.Write(SaveID);
			Sector0Writer.Write(Id.ToByteArray());

			Sector0Writer.Write(StartSector);
			Sector0Writer.Write(EndSector);
			Sector0Writer.Write(SectorSize);
			Sector0Writer.Write(DataLength);
			Sector0Writer.Write((long)Partitions.Count);
			Sector0Writer.Write(BootPartitionCount);

			Sector0Writer.BaseStream.Position = 510;
			Sector0Writer.Write(new byte[] { 0x55, 0xAA });

			MemoryStream memoryStream = new MemoryStream();
			BinaryWriter binaryWriter = new BinaryWriter(memoryStream);

			binaryWriter.Write(Sector0Memory.ToArray());

			foreach (Partition partition in Partitions.Values)
			{
				binaryWriter.Write(partition.GetSectorData());
			}

			return memoryStream.ToArray();
		}

		public static RTA LoadFormSectorData(byte[] bytes)
		{
			if (bytes == null)
			{
				return null;
			}

			byte[] Version;
			long StartSector;
			long EndSector;
			long SectorSize;
			long DataLength;
			byte[] SaveID;
			Guid Id;
			long BootPartitionCount;
			long _PartitionCount;

			BinaryReader Reader = new BinaryReader(new MemoryStream(bytes));

			Reader.BaseStream.Position = 0;

			byte[] hider = Reader.ReadBytes(8);

			if (Encoding.UTF8.GetString(hider) != "RTA PART")
			{
				throw new InvalidDataException("数据头不正确");
			}

			Version = Reader.ReadBytes(4);

			Reader.BaseStream.Position = 16;
			SaveID = Reader.ReadBytes(16);
			Id = new Guid(Reader.ReadBytes(16));

			StartSector = Reader.ReadInt64();
			EndSector = Reader.ReadInt64();
			SectorSize = Reader.ReadInt64();
			DataLength = Reader.ReadInt64();
			_PartitionCount = Reader.ReadInt64();
			BootPartitionCount = Reader.ReadInt64();

			RTA RTA = new RTA(StartSector, EndSector, SectorSize, DataLength, _PartitionCount, BootPartitionCount, Version, SaveID, Id);

			return RTA;
		}

		public static RTA LoadFormDisk(DiskStream physicalDisk)
		{
			if (physicalDisk == null)
			{
				return null;
			}

			byte[] Version;
			long StartSector;
			long EndSector;
			long SectorSize;
			long DataLength;
			byte[] SaveID;
			Guid Id;
			long BootPartitionCount;
			long _PartitionCount;

			BinaryReader Reader = new BinaryReader(new MemoryStream(physicalDisk.ReadSector(0)));

			byte[] hider = Reader.ReadBytes(8);

			if (Encoding.UTF8.GetString(hider) != "RTA PART")
			{
				throw new InvalidDataException("数据头不正确");
			}

			Version = Reader.ReadBytes(4);

			Reader.BaseStream.Position = 16;
			SaveID = Reader.ReadBytes(16);
			Id = new Guid(Reader.ReadBytes(16));

			StartSector = Reader.ReadInt64();
			EndSector = Reader.ReadInt64();
			SectorSize = Reader.ReadInt64();
			DataLength = Reader.ReadInt64();
			_PartitionCount = Reader.ReadInt64();
			BootPartitionCount = Reader.ReadInt64();

			RTA RTA = new RTA(StartSector, EndSector, SectorSize, DataLength, _PartitionCount, BootPartitionCount, Version, SaveID, Id);

			for (int i = 0; i < _PartitionCount; i++)
			{
				RTA.AddPartitionRange(Partition.LoadFormBytes(physicalDisk.ReadSector(i + 1)));
			}

			return RTA;
		}

		public void AddPartitionRange(params Partition[] partitions)
		{
			foreach (var partition in partitions)
			{
				Partitions?.Add(partition.Name, partition);
			}
		}

		public void DeletePartition(string partitionName)
		{
			Partitions.Remove(partitionName);
		}

		public new string ToString()
		{
			return "\nRTA分区表 信息\n" +
				  $"                    版本 : {BitConverter.ToString(Version).Replace('-',' ')}\n" +
				  $"                起始扇区 : {StartSector}\n" +
				  $"                终止扇区 : {EndSector}\n" +
				  $"                扇区大小 : {SectorSize}\n" +
				  $"                数据长度 : {DataLength}\n" +
				  $"                分区数量 : {PartitionCount}\n" +
				  $"可执行启动引导的分区数量 : {BootPartitionCount}\n" +
				  $"              写盘设备ID : {BitConverter.ToString(SaveID).Replace('-', ' ')}\n";
		}
	}
}

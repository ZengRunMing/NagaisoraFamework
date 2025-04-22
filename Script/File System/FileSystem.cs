using System.Collections.Generic;
using System;
using System.IO;
using System.Text;

namespace NagaisoraFamework
{
	using Cryptography;
	using System.ComponentModel;

	public interface TRFileSystemObj : DiskSystemObj
	{
		TRFileSystemObj[] SubFiles { get; set; }

		string Name { get; set; }

		string FullName { get; set; }

		DateTime CarteTime { get; }
		DateTime SaveTime { get; set; }

		string MD5String { get; }

		MemoryStream Buffer { get; set; }
	}

	public class TRFSInfo : IDiskInfo
	{
		public override long StartSector { get; }
		public override long EndSector { get; }
		public override long SectorSize { get; }
		public override long DataLength { get; }
		public override byte[] SaveID { get; }

		public DateTime CreateTime;
		public DateTime SaveTime;
		public long SaveIndex;
		public bool Encrypted;
		public bool CanList;
		public bool CanExport;
		public bool CanSet;

		public TRFSInfo(long startSector, long endSector, long sectorSize, long dataLength, byte[] saveID, DateTime createTime, DateTime saveTime, long saveIndex, bool encrypted, bool canList, bool canExport, bool canSet)
		{
			StartSector = startSector;
			EndSector = endSector;
			SectorSize = sectorSize;
			DataLength = dataLength;
			SaveID = saveID;
			CreateTime = createTime;
			SaveTime = saveTime;
			SaveIndex = saveIndex;
			Encrypted = encrypted;
			CanList = canList;
			CanExport = canExport;
			CanSet = canSet;
		}

		public static TRFSInfo GetFormBytes(byte[] vs)
		{
			MemoryStream memoryStream = new MemoryStream(vs);
			BinaryReader br = new BinaryReader(memoryStream);

			byte[] bytes = br.ReadBytes(9);

			return new TRFSInfo(br.ReadInt64(), br.ReadInt64(), br.ReadInt64(), br.ReadInt64(), br.ReadBytes(16),
						new DateTime(br.ReadInt32(), br.ReadInt32(), br.ReadInt32(), br.ReadInt32(), br.ReadInt32(), br.ReadInt32(), br.ReadInt32()),
						new DateTime(br.ReadInt32(), br.ReadInt32(), br.ReadInt32(), br.ReadInt32(), br.ReadInt32(), br.ReadInt32(), br.ReadInt32()),
						br.ReadInt64(), br.ReadBoolean(), br.ReadBoolean(), br.ReadBoolean(), br.ReadBoolean());
		}

		public byte[] WriteTRFSInfo()
		{
			return WriteTRFSInfo(this);
		}

		public static byte[] WriteTRFSInfo(TRFSInfo Info)
		{
			if (Info == null)
			{
				return null;
			}

			MemoryStream memory = new MemoryStream();
			BinaryWriter writer = new BinaryWriter(memory);

			writer.Write(Encoding.UTF8.GetBytes("TRFS\\DISK"));

			writer.Write(Info.StartSector);
			writer.Write(Info.EndSector);
			writer.Write(Info.SectorSize);

			writer.Write(Info.DataLength);

			writer.Write(Info.SaveID);

			writer.Write(Info.CreateTime.Year);
			writer.Write(Info.CreateTime.Month);
			writer.Write(Info.CreateTime.Day);
			writer.Write(Info.CreateTime.Hour);
			writer.Write(Info.CreateTime.Minute);
			writer.Write(Info.CreateTime.Second);
			writer.Write(Info.CreateTime.Millisecond);

			writer.Write(Info.SaveTime.Year);
			writer.Write(Info.SaveTime.Month);
			writer.Write(Info.SaveTime.Day);
			writer.Write(Info.SaveTime.Hour);
			writer.Write(Info.SaveTime.Minute);
			writer.Write(Info.SaveTime.Second);
			writer.Write(Info.SaveTime.Millisecond);

			writer.Write(Info.SaveIndex);

			writer.Write(Info.Encrypted);
			writer.Write(Info.CanList);
			writer.Write(Info.CanExport);
			writer.Write(Info.CanSet);

			writer.Close();

			return memory.ToArray();
		}
	}

	public class TRFile : TRFileSystemObj
	{
		[CategoryAttribute("基本信息"), DescriptionAttribute("名称")]
		public string Name { get; set; }

		[Browsable(false)]
		public string FullName { get; set; }

		[CategoryAttribute("基本信息"), DescriptionAttribute("创建时间")]
		public DateTime CarteTime { get; }
		[CategoryAttribute("基本信息"), DescriptionAttribute("最后修改时间")]
		public DateTime SaveTime { get; set; }

		[CategoryAttribute("底层信息"), DescriptionAttribute("自动分配底层信息")]
		public bool Auto { get; set; }

		[CategoryAttribute("底层信息"), DescriptionAttribute("文件映像地址")]
		public long Position { get; set; }
		[CategoryAttribute("底层信息"), DescriptionAttribute("文件大小 (byte)")]
		public long Size { get; }

		public byte[] FileMD5;

		[CategoryAttribute("底层信息"), DescriptionAttribute("MD5字符串格式校验值")]
		public string MD5String { get { return MD5GetString(); } }

		[CategoryAttribute("文件本体信息"), DescriptionAttribute("子对象")]
		public TRFileSystemObj[] SubFiles { get { return null; } set { return; } }

		[CategoryAttribute("文件本体信息"), DescriptionAttribute("文件包含字节")]
		public MemoryStream Buffer { get; set; }

		public TRFile(string name, string fullname, DateTime cartetime, DateTime savetime, MemoryStream buffer, long position = 0, bool auto = true)
		{
			Name = name;
			FullName = fullname;
			CarteTime = cartetime;
			SaveTime = savetime;
			Buffer = buffer;

			FileMD5 = MD5.MD5Encrypt16Byte(Buffer.ToArray());
			Auto = auto;
			Size = buffer.Length;
		}

		public string MD5GetString()
		{
			return BitConverter.ToString(FileMD5).Replace('-', ' ');
		}

		public void SetSaveTime(DateTime dateTime)
		{
			SaveTime = dateTime;
		}
	}

	public class TRFolder : TRFileSystemObj
	{
		[CategoryAttribute("基本信息"), DescriptionAttribute("名称")]
		public string Name { get; set; }

		[Browsable(false)]
		public string FullName { get; set; }

		[CategoryAttribute("基本信息"), DescriptionAttribute("创建时间")]
		public DateTime CarteTime { get; }
		[CategoryAttribute("基本信息"), DescriptionAttribute("最后修改时间")]
		public DateTime SaveTime { get; set; }

		[CategoryAttribute("基本信息"), DescriptionAttribute("包含项目")]
		public long SubItemCount { get; }

		[CategoryAttribute("底层信息"), DescriptionAttribute("MD5字符串格式校验值")]
		public string MD5String { get { return null; } }

		public TRFileSystemObj[] SubFiles { get; set; }

		[Browsable(false)]
		public MemoryStream Buffer { get { return null; } set { Buffer = null; } }

		public TRFolder(string name, string fullname, DateTime cartetime, DateTime savetime, TRFileSystemObj[] files = null)
		{
			Name = name;
			FullName = fullname;
			FullName = fullname;
			CarteTime = cartetime;
			SaveTime = savetime;

			SubFiles = files;
		}

		public void SetSaveTime(DateTime dateTime)
		{
			SaveTime = dateTime;
		}
	}

	public class TRFSTree
	{
		public TRFileSystemObj FileObj;

		public List<TRFSTree> STLFileList;

		public TRFSTree()
		{
			STLFileList = new List<TRFSTree>();
		}
	}

	public static class TRFileSystem
	{
		public static byte[] GetTRFSFileImage(TRFileSystemObj[] file)
		{
			if (file == null || file.Length == 0)
			{
				throw new ArgumentNullException("文件列表数据为空");
			}

			MemoryStream Hiden = new MemoryStream();
			MemoryStream RHiden = new MemoryStream();
			MemoryStream Buffer = new MemoryStream();

			BinaryWriter HBW = new BinaryWriter(Hiden, Encoding.UTF8);
			BinaryWriter RHBW = new BinaryWriter(RHiden, Encoding.UTF8);
			BinaryWriter SBW = new BinaryWriter(Buffer, Encoding.UTF8);

			HBW.Write(Encoding.UTF8.GetBytes("TRFS"));

			(long, long, long) SIMT = GetTRFileTree(file);

			HBW.Write(SIMT.Item1);
			HBW.Write(SIMT.Item2);
			HBW.Write(SIMT.Item3);

			(long, long, long) CIMT = GetTRFileLength(file);

			HBW.Write(CIMT.Item1);
			HBW.Write(CIMT.Item2);
			HBW.Write(CIMT.Item3);

			WriteTRFileData(file, RHBW, SBW);

			HBW.Write(RHBW.BaseStream.Length + HBW.BaseStream.Length + 8);

			HBW.Close();
			RHBW.Close();
			SBW.Close();

			byte[] HidenData = Hiden.ToArray();
			byte[] RHidenData = RHiden.ToArray();
			byte[] BufferData = Buffer.ToArray();

			MemoryStream MainMemoryStream = new MemoryStream();
			MainMemoryStream.Write(HidenData, 0x00, HidenData.Length);
			MainMemoryStream.Write(RHidenData, 0x00, RHidenData.Length);
			MainMemoryStream.Write(BufferData, 0x00, BufferData.Length);

			return MainMemoryStream.ToArray();
		}

		public static TRFileSystemObj GetTRFSFileForImage(byte[] image)
		{
			if (image == null && image.Length == 0)
			{
				throw new ArgumentNullException("映像为空");
			}

			MemoryStream memoryStream = new MemoryStream(image);
			BinaryReader binaryReader = new BinaryReader(memoryStream, Encoding.UTF8);

			string HDIR = Encoding.UTF8.GetString(binaryReader.ReadBytes(4));

			if (HDIR != "TRFS")
			{
				throw new Exception($"文件头不正确 {HDIR}");
			}

			long TotalLength = binaryReader.ReadInt64();
			long FolderLength = binaryReader.ReadInt64();
			long FileLength = binaryReader.ReadInt64();

			long RTotalLength = binaryReader.ReadInt64();
			long RFolderLength = binaryReader.ReadInt64();
			long RFileLength = binaryReader.ReadInt64();

			long HideLength = binaryReader.ReadInt64();

			TRFileSystemObj[] IM = ReadTRFileData(binaryReader, RTotalLength, RFolderLength, RFileLength, HideLength);

			return IM[0];
		}

		public static void WriteTRFileData(TRFileSystemObj[] files, BinaryWriter HBW, BinaryWriter SBW)
		{
			if (files == null || files.Length == 0)
			{
				return;
			}

			for (int i = 0; i < files.Length; i++)
			{
				if (files[i].GetType() == typeof(TRFolder))
				{
					TRFileSystemObj tRFolder = files[i];

					HBW.Write((byte)0);

					HBW.Write(tRFolder.FullName);
					HBW.Write(tRFolder.Name);

					HBW.Write(tRFolder.CarteTime.Year);
					HBW.Write(tRFolder.CarteTime.Month);
					HBW.Write(tRFolder.CarteTime.Day);
					HBW.Write(tRFolder.CarteTime.Hour);
					HBW.Write(tRFolder.CarteTime.Minute);
					HBW.Write(tRFolder.CarteTime.Second);

					HBW.Write(tRFolder.SaveTime.Year);
					HBW.Write(tRFolder.SaveTime.Month);
					HBW.Write(tRFolder.SaveTime.Day);
					HBW.Write(tRFolder.SaveTime.Hour);
					HBW.Write(tRFolder.SaveTime.Minute);
					HBW.Write(tRFolder.SaveTime.Second);

					TRFileSystemObj[] tRFileSystemObjs = tRFolder.SubFiles;

					(long, long, long) IMT = GetTRFileLength(tRFileSystemObjs);

					HBW.Write(IMT.Item1);
					HBW.Write(IMT.Item2);
					HBW.Write(IMT.Item3);

					WriteTRFileData(tRFileSystemObjs, HBW, SBW);
					continue;
				}

				TRFile tRFile = (TRFile)files[i];

				HBW.Write((byte)1);

				HBW.Write(tRFile.FullName);
				HBW.Write(tRFile.Name);

				HBW.Write(tRFile.CarteTime.Year);
				HBW.Write(tRFile.CarteTime.Month);
				HBW.Write(tRFile.CarteTime.Day);
				HBW.Write(tRFile.CarteTime.Hour);
				HBW.Write(tRFile.CarteTime.Minute);
				HBW.Write(tRFile.CarteTime.Second);

				HBW.Write(tRFile.SaveTime.Year);
				HBW.Write(tRFile.SaveTime.Month);
				HBW.Write(tRFile.SaveTime.Day);
				HBW.Write(tRFile.SaveTime.Hour);
				HBW.Write(tRFile.SaveTime.Minute);
				HBW.Write(tRFile.SaveTime.Second);

				HBW.Write(tRFile.FileMD5);
				HBW.Write(tRFile.Auto);

				if (tRFile.Auto)
				{
					HBW.Write(SBW.BaseStream.Position);
					HBW.Write((long)tRFile.Buffer.Length);
					SBW.Write(tRFile.Buffer.ToArray());
					continue;
				}

				SBW.BaseStream.Seek(tRFile.Position, SeekOrigin.Begin);
				HBW.Write(SBW.BaseStream.Position);
				HBW.Write((long)tRFile.Buffer.Length);
				SBW.Write(tRFile.Buffer.ToArray());
			}
		}

		public static TRFileSystemObj[] ReadTRFileData(BinaryReader binaryReader, long TotalLength, long FolderLength, long FileLength, long HideLength)
		{
			List<TRFileSystemObj> IM = new List<TRFileSystemObj>();

			for (long i = 0; i < TotalLength; i++)
			{
				byte FFD = binaryReader.ReadByte();

				string FullName = binaryReader.ReadString();
				string Name = binaryReader.ReadString();

				DateTime CarteTime = new DateTime(binaryReader.ReadInt32(), binaryReader.ReadInt32(), binaryReader.ReadInt32(), binaryReader.ReadInt32(), binaryReader.ReadInt32(), binaryReader.ReadInt32());
				DateTime SaveTime = new DateTime(binaryReader.ReadInt32(), binaryReader.ReadInt32(), binaryReader.ReadInt32(), binaryReader.ReadInt32(), binaryReader.ReadInt32(), binaryReader.ReadInt32());

				if (FFD == 0)
				{
					long RTotalLength = binaryReader.ReadInt64();
					long RFolderLength = binaryReader.ReadInt64();
					long RFileLength = binaryReader.ReadInt64();

					TRFileSystemObj[] tRFSObjs = ReadTRFileData(binaryReader, RTotalLength, RFolderLength, RFileLength, HideLength);

					IM.Add(new TRFolder(Name, FullName, CarteTime, SaveTime, tRFSObjs));
				}
				else
				{
					byte[] FileMD5 = binaryReader.ReadBytes(16);
					bool Auto = binaryReader.ReadBoolean();
					long Position = binaryReader.ReadInt64();
					long Size = binaryReader.ReadInt64();

					long MOVPosition = binaryReader.BaseStream.Position;

					binaryReader.BaseStream.Seek(HideLength + Position, SeekOrigin.Begin);
					MemoryStream memory = new MemoryStream();
					memory.SetLength(Size);

					long size = Size / 4096;
					long lsize = Size % 4096;
					byte[] vs;
					for (int im = 0; im < size; im++)
					{
						vs = binaryReader.ReadBytes(4096);
						memory.Write(vs, 0x00, vs.Length);
					}

					vs = binaryReader.ReadBytes((int)lsize);
					memory.Write(vs, 0x00, vs.Length);

					binaryReader.BaseStream.Seek(MOVPosition, SeekOrigin.Begin);

					string TRM = BitConverter.ToString(FileMD5);
					string SRM = BitConverter.ToString(MD5.MD5Encrypt16Byte(memory.ToArray()));

					if (SRM != TRM)
					{
						//throw (string.Format("{0}的MD5校验不匹配, 读取值{1}, 计算值{2}", FullName, TRM.Replace("-", " "), SRM.Replace("-", " ")));
						continue;
					}

					IM.Add(new TRFile(Name, FullName, CarteTime, SaveTime, memory, Position, Auto));
				}
			}

			return IM.ToArray();
		}

		public static (long TotalLength, long FileLength, long FolderLength) GetTRFileTree(TRFileSystemObj[] files)
		{
			if (files == null || files.Length == 0)
			{
				return (0, 0, 0);
			}

			long Totallength = 0;
			long FileLength = 0;
			long FolderLength = 0;

			for (int i = 0; i < files.Length; i++)
			{
				if (files[i].GetType() == typeof(TRFolder))
				{
					if (files[i].SubFiles != null && files[i].SubFiles.Length != 0)
					{
						(long, long, long) IMT = GetTRFileTree(files[i].SubFiles);

						Totallength += IMT.Item1;
						FileLength += IMT.Item2;
						FolderLength += IMT.Item3;
					}

					FolderLength++;
				}
				else
				{
					FileLength++;
				}

				Totallength++;
			}

			return (Totallength, FileLength, FolderLength);
		}

		public static (long TotalLength, long FileLength, long FolderLength) GetTRFileLength(TRFileSystemObj[] files)
		{
			if (files == null || files.Length == 0)
			{
				return (0, 0, 0);
			}

			long Totallength = 0;
			long FileLength = 0;
			long FolderLength = 0;

			for (int i = 0; i < files.Length; i++)
			{
				if (files[i].GetType() == typeof(TRFolder))
				{
					FolderLength++;
				}
				else
				{
					FileLength++;
				}

				Totallength++;
			}

			return (Totallength, FileLength, FolderLength);
		}

		public static TRFileSystemObj GetForPath(TRFileSystemObj obj, string path)
		{
			foreach (string str in path.Split(new char[] { '\\' }, StringSplitOptions.RemoveEmptyEntries))
			{
				obj = GetForName(obj, str);
			}

			return obj;
		}

		public static TRFileSystemObj GetForName(TRFileSystemObj obj, string name)
		{
			foreach (TRFileSystemObj obj1 in obj.SubFiles)
			{
				if (obj1.Name == name)
				{
					return obj1;
				}
			}
			return null;
		}
	}

	public static class FileCom
	{
		public static (string Path, string Name, string Extension) getFileName(string path)
		{
			string FilePath = Path.GetDirectoryName(path); //文件路径
			string FileName = Path.GetFileName(path); //全文件名
			string LSRCFileName = Path.GetExtension(path); //拓展名

			return (FilePath, FileName, LSRCFileName);
		}
	}
}

using System;
using System.IO;
using System.Text;
using System.Linq;
using System.ComponentModel;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;

using Microsoft.Win32.SafeHandles;

using UnityEngine;

namespace NagaisoraFamework
{
	using STGSystem;
	using DataFileSystem;
	using Cryptography;

	using static MainSystem;

	public enum BlendMode
	{
		[Description("Alpha混合")]
		AlphaBlend,
		[Description("叠加（高光）")]
		Additive
	}

	//按需求添加
	public enum SEType
	{
		AsName,
		Select,
		Ok,
		Cancel,
		Warning,
		Pause,
		Big,
		Bonus,
		Bonus2,
		Bonus4,
		Boon00,
		Boon01,
		CardGet,
		Cat00,
		Ch00,
		Ch01,
		Ch02,
		Ch03,
		Danmege00,
		Danmege01,
		Don00,
		Enep00,
		Enep01,
		Enep02,
		Etbreak,
		Extend,
		Extend2,
		Fault,
		Graze,
		Gun00,
		Heal,
		Invalid,
		Item00,
		Item01,
		Kira00,
		Kira01,
		Kira02,
		Lazer00,
		Lazer01,
		Lazer02,
		Lgods1,
		Lgods2,
		Lgods3,
		Lgods4,
		LgodsGet,
		Msl,
		Msl2,
		Msl3,
		Nep00,
		Nodamage,
		Noise,
		Pin00,
		Pin01,
		Pidead00,
		Pidead01,
		Plst00,
		Power0,
		Power1,
		Powerup,
		Slash,
		Tan00,
		Tan01,
		Tan02,
		Tan03,
		Timeout,
		Timeout2,
		Wolf,
	}

	public enum LaserType
	{
		Long,
		Segmental,
	}

	public enum LSLogType
	{
		Info,
		Warning,
		Error,
		Default,
	}

	public enum IMMType
	{
		Int = 0,
		Long = 1,
		Float = 2,
		Double = 3,
		Bool = 4,
		String = 5,
		Char = 6,
		Time = 7,
		Vector2 = 8,
		Vector3 = 9,
		Vector6 = 10,
	}

	public enum ShowFormType
	{
		CenterScene = 0,
		WindowsDefaultLocation = 1,

		Top = 2,
		Bottom = 3,
		Left = 4,
		Right = 5,

		LeftBottom = 6,
		LeftTop = 7,

		RightBottom = 8,
		RightTop = 9,
	}

	public enum AccessFlag : uint
	{
		GenericRead = 0x80000000,
		GenericWrite = 0x40000000
	}

	public enum ShareMode : uint
	{
		FileShareRead = 0x00000001,
		FileShareWrite = 0x00000002,
		FileShareDelete = 0x00000004
	}

	public enum CreateMode : uint
	{
		CreateNew = 1,
		CreateAlways = 2,
		OpenExisting = 3
	}

	public enum EMoveMethod : uint
	{
		Begin = 0,
		Current = 1,
		End = 2
	}

	public enum WMIPath
	{
		// 硬件 
		Win32_Processor,         // CPU 处理器 
		Win32_PhysicalMemory,    // 物理内存条 
		Win32_Keyboard,          // 键盘 
		Win32_PointingDevice,    // 点输入设备，包括鼠标。 
		Win32_FloppyDrive,       // 软盘驱动器 
		Win32_DiskDrive,         // 硬盘驱动器 
		Win32_CDROMDrive,        // 光盘驱动器 
		Win32_BaseBoard,         // 主板 
		Win32_BIOS,              // BIOS 芯片 
		Win32_ParallelPort,      // 并口 
		Win32_SerialPort,        // 串口 
		Win32_SerialPortConfiguration, // 串口配置 
		Win32_SoundDevice,       // 多媒体设置，一般指声卡。 
		Win32_SystemSlot,        // 主板插槽 (ISA & PCI & AGP) 
		Win32_USBController,     // USB 控制器 
		Win32_NetworkAdapter,    // 网络适配器 
		Win32_NetworkAdapterConfiguration, // 网络适配器设置 
		Win32_Printer,           // 打印机 
		Win32_PrinterConfiguration, // 打印机设置 
		Win32_PrintJob,          // 打印机任务 
		Win32_TCPIPPrinterPort,  // 打印机端口 
		Win32_POTSModem,         // MODEM 
		Win32_POTSModemToSerialPort, // MODEM 端口 
		Win32_DesktopMonitor,    // 显示器 
		Win32_DisplayConfiguration, // 显卡 
		Win32_DisplayControllerConfiguration, // 显卡设置 
		Win32_VideoController,  // 显卡细节。 
		Win32_VideoSettings,    // 显卡支持的显示模式。 

		// 操作系统 
		Win32_TimeZone,         // 时区 
		Win32_SystemDriver,     // 驱动程序 
		Win32_DiskPartition,    // 磁盘分区 
		Win32_LogicalDisk,      // 逻辑磁盘 
		Win32_LogicalDiskToPartition,     // 逻辑磁盘所在分区及始末位置。 
		Win32_LogicalMemoryConfiguration, // 逻辑内存配置 
		Win32_PageFile,         // 系统页文件信息 
		Win32_PageFileSetting,  // 页文件设置 
		Win32_BootConfiguration, // 系统启动配置 
		Win32_ComputerSystem,   // 计算机信息简要 
		Win32_OperatingSystem,  // 操作系统信息 
		Win32_StartupCommand,   // 系统自动启动程序 
		Win32_Service,          // 系统安装的服务 
		Win32_Group,            // 系统管理组 
		Win32_GroupUser,        // 系统组帐号 
		Win32_UserAccount,      // 用户帐号 
		Win32_Process,          // 系统进程 
		Win32_Thread,           // 系统线程 
		Win32_Share,            // 共享 
		Win32_NetworkClient,    // 已安装的网络客户端 
		Win32_NetworkProtocol,  // 已安装的网络协议 
	}

	[Serializable]
	public struct PlayerInfo
	{
		public string Name;

		public GameObject Prefabricate;
		public Vector2 Determine_Offset;
		public float AngleOffsetCompensation;
		public float Determine_Radius;
		public float Normoal_Speed;
		public float Low_Speed;
		public PlayerBulletInfo[] PlayerBullet;
	}

	[Serializable]
	public struct PlayerBulletInfo
	{
		public int Type;
		public Sprite Sprite;
		public Vector2 Normoal_Size;
		public Vector2 Determine_Offset;
		public float AngleOffsetCompensation;
		public float Determine_Radius;
	}

	[Serializable]
	public struct EnemyInfo
	{
		public int Type;
		public Vector2 Normoal_Size;
		public Vector2 Determine_Offset;
		public float AngleOffsetCompensation;
		public float Determine_Radius;
		public EnemyObject[] Info;
	}

	[Serializable]
	public struct EnemyBulletInfo
	{
		public int Type;
		public Vector2 Normoal_Size;
		public Vector2 Determine_Offset;
		public float AngleOffsetCompensation;
		public float Determine_Radius;
		public BulletObject[] Info;
	}

	[Serializable]
	public struct EnemyLongLaserInfo
	{
		public Vector2 Normoal_Size;
		public float Determine_Radius;
		public BulletObject[] Info;
	}

	[Serializable]
	public struct BulletObject
	{
		public int Color;
		public Sprite Sprite;
	}


	[Serializable]
	public struct EnemyObject
	{
		public int Color;
		public RuntimeAnimatorController AnimatorController;
	}

	[Serializable]
	public struct KeyConfig
	{
		public KeyCode Up;
		public KeyCode Down;
		public KeyCode Left;
		public KeyCode Right;
		public KeyCode[] SubmitKeys;
		public KeyCode[] CancelKeys;
		public KeyCode ESC;
		public KeyCode R;
		public KeyCode Slow;
		public KeyCode Change;
		public KeyCode Ctrl;
		public KeyCode J_SubmitKey;
		public KeyCode J_CancelKey;
		public KeyCode J_ESC;
		public KeyCode J_Slow;
		public KeyCode J_Change;
		public KeyCode J_Ctrl;


		public static KeyConfig Default = new()
		{
			Up = KeyCode.UpArrow,
			Down = KeyCode.DownArrow,
			Left = KeyCode.LeftArrow,
			Right = KeyCode.RightArrow,

			SubmitKeys = new KeyCode[] { KeyCode.Z, KeyCode.Return },
			J_SubmitKey = KeyCode.JoystickButton0,

			CancelKeys = new KeyCode[] { KeyCode.X, KeyCode.Escape },
			J_CancelKey = KeyCode.JoystickButton6,

			ESC = KeyCode.Escape,
			J_ESC = KeyCode.JoystickButton1,

			R = KeyCode.R,

			Slow = KeyCode.LeftShift,
			J_Slow = KeyCode.JoystickButton7,

			Change = KeyCode.C,
			J_Change = KeyCode.JoystickButton3,

			Ctrl = KeyCode.LeftControl,
			J_Ctrl = KeyCode.JoystickButton2,
		};
	}

	[Serializable]
	public struct ConfigData
	{
		public float MusicVolume;
		public float SEVolume;
		public int FPS;
		public Dictionary<string, KeyConfig> KeyConfigs;

		public static ConfigData Default = new()
		{
			MusicVolume = 100,
			SEVolume = 80,
			FPS = 120,
			KeyConfigs = new Dictionary<string, KeyConfig>()
			{
				{"default", KeyConfig.Default },
			},
		};

		public readonly byte[] ToBinary()
		{
			MemoryStream memoryStream = new();
			BinaryWriter writer = new(memoryStream, Encoding.UTF8, true);
			writer.BaseStream.Seek(0, SeekOrigin.Begin);

			writer.Write(MusicVolume);
			writer.Write(SEVolume);
			writer.Write(FPS);

			string[] keys = KeyConfigs.Keys.ToArray();
			KeyConfig[] values = KeyConfigs.Values.ToArray();

			writer.Write(KeyConfigs.Count);

			for (int i = 0; i < KeyConfigs.Count; i++)
			{
				writer.Write(keys[i]);

				writer.Write((ushort)values[i].Up);
				writer.Write((ushort)values[i].Down);
				writer.Write((ushort)values[i].Left);
				writer.Write((ushort)values[i].Right);

				writer.Write((ushort)values[i].R);

				writer.Write((ushort)values[i].ESC);
				writer.Write((ushort)values[i].J_ESC);

				writer.Write((ushort)values[i].Slow);
				writer.Write((ushort)values[i].J_Slow);

				writer.Write((ushort)values[i].Change);
				writer.Write((ushort)values[i].J_Change);

				writer.Write((ushort)values[i].Ctrl);
				writer.Write((ushort)values[i].J_Ctrl);

				writer.Write((byte)values[i].SubmitKeys.Length);
				foreach (KeyCode key in values[i].SubmitKeys)
				{
					writer.Write((ushort)key);
				}
				
				writer.Write((ushort)values[i].J_SubmitKey);

				writer.Write((ushort)values[i].CancelKeys.Length);
				foreach (KeyCode key in values[i].CancelKeys)
				{
					writer.Write((ushort)key);
				}

				writer.Write((ushort)values[i].J_CancelKey);
			}

			writer.Close();

			return memoryStream.ToArray();
		}

		public static ConfigData FormBinary(byte[] binary)
		{
			MemoryStream memoryStream = new(binary);
			BinaryReader binaryReader = new(memoryStream, Encoding.UTF8);

			ConfigData configData = new()
			{
				MusicVolume = binaryReader.ReadSingle(),
				SEVolume = binaryReader.ReadSingle(),
				FPS = binaryReader.ReadInt32(),
			};

			int count = binaryReader.ReadInt32();

			configData.KeyConfigs = new();

			for (int i = 0; i < count; i++)
			{
				string name = binaryReader.ReadString();

				KeyConfig keyconfig = new()
				{
					Up = (KeyCode)binaryReader.ReadUInt16(),
					Down = (KeyCode)binaryReader.ReadUInt16(),
					Left = (KeyCode)binaryReader.ReadUInt16(),
					Right = (KeyCode)binaryReader.ReadUInt16(),

					R = (KeyCode)binaryReader.ReadUInt16(),

					ESC = (KeyCode)binaryReader.ReadUInt16(),
					J_ESC = (KeyCode)binaryReader.ReadUInt16(),

					Slow = (KeyCode)binaryReader.ReadUInt16(),
					J_Slow = (KeyCode)binaryReader.ReadUInt16(),

					Change = (KeyCode)binaryReader.ReadUInt16(),
					J_Change = (KeyCode)binaryReader.ReadUInt16(),

					Ctrl = (KeyCode)binaryReader.ReadUInt16(),
					J_Ctrl = (KeyCode)binaryReader.ReadUInt16(),
				};

				byte submitkeycount = binaryReader.ReadByte();
				keyconfig.SubmitKeys = new KeyCode[submitkeycount];
				for (int a = 0; a < submitkeycount; a++)
				{
					keyconfig.SubmitKeys[i] = (KeyCode)binaryReader.ReadUInt16();
				}
				keyconfig.J_SubmitKey = (KeyCode)binaryReader.ReadUInt16();

				byte cancelkeycount = binaryReader.ReadByte();
				keyconfig.CancelKeys = new KeyCode[cancelkeycount];
				for (int a = 0; a < cancelkeycount; a++)
				{
					keyconfig.CancelKeys[i] = (KeyCode)binaryReader.ReadUInt16();
				}
				keyconfig.J_CancelKey = (KeyCode)binaryReader.ReadUInt16();

				configData.KeyConfigs.Add(name, keyconfig);
			}

			binaryReader.Close();
			memoryStream.Close();

			return configData;
		}
	}

	[Serializable]
	public struct PlayerData
	{
		public int Player;
		public int Rank;

		public DateTime CreateTime;

		public TimeSpan TotalPlayerTime;
		public int PlayerIndex;
		public int ClearIndex;

		public List<PlayerDataScore> Scores;
		public List<SpellCardScore> SpellCards;

		public PlayerData(int player, int rank, DateTime createtime, TimeSpan totalplayertime, int playerindex, int clearindex, List<PlayerDataScore> scores, List<SpellCardScore> spellcards)
		{
			Player = player;
			Rank = rank;

			CreateTime = createtime;

			TotalPlayerTime = totalplayertime;
			PlayerIndex = playerindex;
			ClearIndex = clearindex;

			Scores = scores;
			SpellCards = spellcards;
		}

		public readonly void ScourceAdd(params PlayerDataScore[] scource) => Scores.AddRange(scource);

		public readonly void SpellDardsAdd(params SpellCardScore[] spellcards) => SpellCards.AddRange(spellcards);

		public byte[] ToBinary()
		{
			MemoryStream FM = new();

			MemoryStream HM = new();
			BinaryWriter HW = new(HM, Encoding.UTF8);

			MemoryStream DM = new();
			BinaryWriter DW = new(DM, Encoding.UTF8);

			DW.BaseStream.Seek(0x00, SeekOrigin.Begin);

			if (Scores != null)
			{
				foreach (PlayerDataScore score in Scores)
				{
					DW.Write(score.Name);
					DW.Write(score.Scource);

					DW.Write(score.Time.ToBinary());

					DW.Write(score.Clear);
				}
			}

			foreach (SpellCardScore spellcardscore in SpellCards)
			{
				DW.Write(spellcardscore.Name);
				DW.Write(spellcardscore.Load);
				DW.Write(spellcardscore.Clear);
			}

			DW.Close();
			DW.Dispose();

			HW.BaseStream.Seek(0x00, SeekOrigin.Begin);

			HW.Write(Encoding.UTF8.GetBytes(string.Format("{0}\\PLAYERDATA", SCLENAME)));

			HW.Write(Player);
			HW.Write(Rank);

			HW.Write(CreateTime.ToBinary());
			HW.Write(TotalPlayerTime.Ticks);

			HW.Write(PlayerIndex);
			HW.Write(ClearIndex);

			HW.Write(Scores != null ? Scores.Count : 0);
			HW.Write(SpellCards.Count);
			HW.Close();
			HW.Dispose();

			FM.Write(HM.ToArray(), 0x00, HM.ToArray().Length);
			FM.Write(DM.ToArray(), 0x00, DM.ToArray().Length);

			HM.Close();
			DM.Close();

			HM.Dispose();
			DM.Dispose();

			return FM.ToArray();
		}

		public static PlayerData FromBinary(byte[] binary)
		{
			MemoryStream FM = new(binary);
			BinaryReader BR = new(FM);

			string Hider = Encoding.UTF8.GetString(BR.ReadBytes(SCLENAME.Length + 11));

			if (Hider != string.Format("{0}\\PLAYERDATA", SCLENAME))
			{
				throw new InvalidDataException($"PlayerDataLoad() => 文件头不匹配");
			}

			int player = BR.ReadInt32();
			int rank = BR.ReadInt32();

			DateTime createTime = DateTime.FromBinary(BR.ReadInt64());
			TimeSpan totalplayertime = TimeSpan.FromTicks(BR.ReadInt64());

			int playerindex = BR.ReadInt32();
			int clearindex = BR.ReadInt32();

			int scoreindex = BR.ReadInt32();
			int spellindex = BR.ReadInt32();

			byte[] buffer = BR.ReadBytes(Convert.ToInt32(BR.BaseStream.Length));

			BR.Close();
			FM.Close();

			BR.Dispose();
			FM.Dispose();

			MemoryStream memory = new(buffer);
			BinaryReader DBR = new(memory);

			DBR.BaseStream.Seek(0x00, SeekOrigin.Begin);

			PlayerDataScore[] playerDataScores = new PlayerDataScore[scoreindex];

			for (int i = 0; i < playerDataScores.Length; i++)
			{
				string name = DBR.ReadString();
				long scource = DBR.ReadInt64();
				DateTime dateTime = DateTime.FromBinary(DBR.ReadInt64());
				int clear = DBR.ReadInt32();
				
				playerDataScores[i] = new PlayerDataScore(name, scource, dateTime, clear);
			}

			SpellCardScore[] spellCardScores = new SpellCardScore[spellindex];
			
			for (int i = 0;i < spellCardScores.Length; i++)
			{
				string name = DBR.ReadString();
				int load = DBR.ReadInt32();
				int clear = DBR.ReadInt32();

				spellCardScores[i] = new SpellCardScore(name, load, clear);
			}

			DBR.Close();
			memory.Close();

			DBR.Dispose();
			memory.Dispose();

			return new PlayerData(player, rank, createTime, totalplayertime, playerindex, clearindex, playerDataScores.Length == 0 ? null : playerDataScores.ToList(), spellCardScores.ToList());
		}

		public override readonly bool Equals(object obj) => base.Equals(obj);

		public override readonly int GetHashCode() => base.GetHashCode();

		public override readonly string ToString() => base.ToString();
	}

	[Serializable]
	public struct PlayerDataScore
	{
		public string Name;
		public long Scource;
		public DateTime Time;
		public int Clear;

		public PlayerDataScore(string name, long scource, DateTime time, int clear)
		{
			Name = name;
			Scource = scource;
			Time = time;
			Clear = clear;
		}
	}

	[Serializable]
	public struct SpellCardScore
	{
		public string Name;
		public int Load;
		public int Clear;

		public SpellCardScore(string name, int load, int clear)
		{
			Name = name;
			Load = load;
			Clear = clear;
		}
	}

	[Serializable]
	public struct ReplayData
	{
		public string Name;
		public DateTime SaveTime;
		public int Score;
		public byte Livel;
		public byte Player;
		public byte EndStage;
		public string User;

		public byte StartLife;
		public byte StartBomb;
		public byte EndLife;
		public byte EndBomb;
		public float Power;
		public int Point;
		public int Graze;
		public int GetSpellCardCount;
		public byte DeathCount;

		public StageReplayData[] StageReplayDatas;

		public static ReplayData FromBinary(byte[] binary)
		{
			MemoryStream memory = new(binary);
			BinaryReader BR = new(memory);
			BR.BaseStream.Seek(0x00, SeekOrigin.Begin);

			try
			{
				string H0 = Encoding.UTF8.GetString(BR.ReadBytes(SCLENAME.Length + 7));
				if (H0 != $"{SCLENAME}\\REPLAY")
				{
					throw new InvalidDataException($"数据标识头不匹配");
				}

				ReplayData replayData = new()
				{
					Name = BR.ReadString(),
					User = BR.ReadString(),
					SaveTime = DateTime.FromBinary(BR.ReadInt64()),
					Player = BR.ReadByte(),
					Livel = BR.ReadByte(),
					Score = BR.ReadInt32(),
					Power = BR.ReadSingle(),
					Point = BR.ReadInt32(),
					Graze = BR.ReadInt32(),
					EndStage = BR.ReadByte(),
					StartLife = BR.ReadByte(),
					StartBomb = BR.ReadByte(),
					EndLife = BR.ReadByte(),
					EndBomb = BR.ReadByte(),
					GetSpellCardCount = BR.ReadInt32(),
					DeathCount = BR.ReadByte()
				};

				List<StageReplayData> stageReplayDatas = new();

				int Length = BR.ReadInt32();
				for (int a = 0; a < Length; a++)
				{
					StageReplayData stageReplayData = new()
					{
						Name = BR.ReadString(),
					};

					Dictionary<string, List<ReplayActionData>> actionDatas = new();

					int actionscount = BR.ReadInt32();

					for (int ld = 0; ld < actionscount; ld++)
					{
						string keyname = BR.ReadString();
						int valuecount = BR.ReadInt32();

						actionDatas.Add(keyname, new());

						for (int b = 0; b < valuecount; b++)
						{
							actionDatas[keyname].Add(new(BR.ReadUInt32(), BR.ReadUInt16()));
						}
					}

					stageReplayData.ActionDatas = actionDatas;

					stageReplayDatas.Add(stageReplayData);
				}

				replayData.StageReplayDatas = stageReplayDatas.ToArray();
				BR.Close();
				return replayData;
			}
			catch (Exception E)
			{
				BR.Close();
				throw new FileLoadException($"数据无法正常读取，错误信息 : {E.Message}");
			}
		}

		public readonly byte[] ToBinary()
		{
			MemoryStream memoryStream = new();
			BinaryWriter binaryWriter = new(memoryStream, Encoding.UTF8, true);

			binaryWriter.Write(Encoding.UTF8.GetBytes($"{SCLENAME}\\REPLAY"));

			binaryWriter.Write(Name);
			binaryWriter.Write(User);
			binaryWriter.Write(SaveTime.ToBinary());
			binaryWriter.Write(Player);
			binaryWriter.Write(Livel);
			binaryWriter.Write(Score);
			binaryWriter.Write(Power);
			binaryWriter.Write(Point);
			binaryWriter.Write(Graze);
			binaryWriter.Write(EndStage);
			binaryWriter.Write(StartLife);
			binaryWriter.Write(StartBomb);
			binaryWriter.Write(EndLife);
			binaryWriter.Write(EndBomb);
			binaryWriter.Write(GetSpellCardCount);
			binaryWriter.Write(DeathCount);

			if (StageReplayDatas == null)
			{
				binaryWriter.Write(0);
				goto Return;
			}

			binaryWriter.Write(StageReplayDatas.Length);
			foreach (StageReplayData item in StageReplayDatas)
			{
				binaryWriter.Write(item.Name);

				if (item == null)
				{
					binaryWriter.Write(0);
					continue;
				}

				binaryWriter.Write(item.ActionDatasCount);

				foreach (KeyValuePair<string, List<ReplayActionData>> item0 in item.ActionDatas)
				{
					binaryWriter.Write(item0.Key);
					binaryWriter.Write(item0.Value.Count);
					foreach (ReplayActionData item1 in item0.Value)
					{
						binaryWriter.Write(item1.GameTime);
						binaryWriter.Write(item1.DownKeys);
					}
				}
			}

			Return:

			binaryWriter.Close();
			return memoryStream.ToArray();
		}
	}

	[Serializable]
	public class ReplayActionData
	{
		public uint GameTime;
		public ushort DownKeys;

		public ReplayActionData()
		{

		}

		public ReplayActionData(uint gametime)
		{
			GameTime = gametime;
		}

		public ReplayActionData(uint gametime, ushort downKeys)
		{
			GameTime = gametime;
			DownKeys = downKeys;
		}
	}

	[Serializable]
	public class StageReplayData
	{
		public int ActionDatasCount => ActionDatas.Count();

		public string Name;

		public int Score;

		public Dictionary<string, List<ReplayActionData>> ActionDatas;
		public Dictionary<uint, int> Keys;

		public StageReplayData()
		{
			ActionDatas = new Dictionary<string, List<ReplayActionData>>();
			Keys = new Dictionary<uint, int>();
		}

		public StageReplayData(string name, int score) : this()
		{
			Name = name;
			Score = score;
		}

		public void AddAction(string name, ReplayActionData data)
		{
			if (!ActionDatas.ContainsKey(name))
			{
				ActionDatas.Add(name, new());
			}

			ActionDatas[name].Add(data);
		}

		public void RemoveAction(string name, ReplayActionData data)
		{
			ActionDatas[name].Remove(data);
		}

		public void AddKey(uint gametime, int value)
		{
			Keys.Add(gametime, value);
		}

		public void RemoveKey(uint gametime)
		{
			Keys.Remove(gametime);
		}

		public void Clear() 
		{
			ActionDatas.Clear();
		}
	}

	[Serializable]
	public class ScoreData
	{
		public DateTime SaveTime;
		public uint MaxScore;
		public TimeSpan TotalRunTime;
		public TimeSpan TotalPlayTime;
		public bool[] MusicRoomGetData;
		public PlayerData[][] PlayerDatas;

		public ScoreData(DateTime SaveTime, uint MaxScore, TimeSpan TotalRunTime, TimeSpan TotalPlaytime, bool[] MusicRoomGetData, PlayerData[][] PlayerDatas)
		{
			this.SaveTime = SaveTime;
			this.MaxScore = MaxScore;
			this.TotalRunTime = TotalRunTime;
			this.TotalPlayTime = TotalPlaytime;
			this.PlayerDatas = PlayerDatas;
			this.MusicRoomGetData = MusicRoomGetData;
		}

		public static ScoreData Default()
		{
			bool[] MusicRoomGetData = new bool[18];

			for (int i = 0; i < MusicRoomGetData.Length; i++)
			{
				if (i == 0)
				{
					MusicRoomGetData[i] = true;
				}
				else
				{
					MusicRoomGetData[i] = false;
				}
			}

			PlayerData[][] PlayerDatas = new PlayerData[PlayerLength][];

			for (int i = 0; i < PlayerDatas.Length; i++)
			{
				PlayerData[] keyValues = new PlayerData[RankLength];

				for (int a = 0; a < keyValues.Length; a++)
				{
					List<SpellCardScore> SpellData = new();

					for (int c = 0; c < 33; c++)
					{
						SpellData.Add(new SpellCardScore("Spell_" + c, 0, 0));
					}

					PlayerData playerData = new(i, a, DateTime.Now, TimeSpan.Zero, 0, 0, null, SpellData);

					keyValues[a] = playerData;
				}
				PlayerDatas[i] = keyValues;
			}

			return new ScoreData(DateTime.Now, 0, TimeSpan.Zero, TimeSpan.Zero, MusicRoomGetData, PlayerDatas);
		}

		public static ScoreData FromBinary(byte[] binary)
		{
			MemoryStream MainMemory = new(binary);
			BinaryReader binaryReader = new(MainMemory, Encoding.UTF8);

			binaryReader.BaseStream.Seek(0, SeekOrigin.Begin);

			string Hider = Encoding.UTF8.GetString(binaryReader.ReadBytes(SCLENAME.Length + 10));
			if (Hider != $"{SCLENAME}\\SCOREDATA")
			{
				throw new InvalidDataException($"数据标识头不匹配");
			}

			byte[] MD5Data = binaryReader.ReadBytes(16);

			DateTime savetime = DateTime.FromBinary(binaryReader.ReadInt64());
			TimeSpan totalruntime = TimeSpan.FromTicks(binaryReader.ReadInt64());
			TimeSpan totalplayertime = TimeSpan.FromTicks(binaryReader.ReadInt64());

			uint maxscore = binaryReader.ReadUInt32();
			int musicroom_getlength = binaryReader.ReadInt32();
			int datalength = binaryReader.ReadInt32();

			byte[] buffer = binaryReader.ReadBytes(Convert.ToInt32(binaryReader.BaseStream.Length));

			if (BitConverter.ToString(MD5.MD5Encrypt16Byte(buffer)) != BitConverter.ToString(MD5Data))
			{
				throw new InvalidDataException($"MD5校验不匹配");
			}

			MemoryStream memory = new(buffer);
			BinaryReader DBR = new(memory);

			bool[] bools = new bool[musicroom_getlength];

			for (int i = 0; i < bools.Length; i++)
			{
				bools[i] = DBR.ReadBoolean();
			}

			PlayerData[][] datas = PlayerDataSystem.ReadPlayerData(DBR.ReadBytes(datalength));

			return new(savetime, maxscore, totalruntime, totalplayertime, bools, datas);
		}

		public byte[] ToBinady()
		{
			MemoryStream TotalMemory = new();
			BinaryWriter TotalWriter = new(TotalMemory, Encoding.UTF8);

			MemoryStream HeadMemory = new();
			BinaryWriter HeadWriter = new(HeadMemory, Encoding.UTF8);

			MemoryStream DataMemory = new();
			BinaryWriter DataWriter = new(DataMemory, Encoding.UTF8);

			for (int i = 0; i < MusicRoomGetData.Length; i++)
			{
				DataWriter.Write(MusicRoomGetData[i]);
			}

			byte[] bytes = PlayerDataSystem.WritePlayerData(PlayerDatas);
			if (bytes != null)
			{
				DataWriter.Write(bytes);
			}

			DataWriter.Close();

			byte[] data = DataMemory.ToArray();

			DataMemory.Close();

			HeadWriter.Write(Encoding.UTF8.GetBytes($"{SCLENAME}\\SCOREDATA"));

			HeadWriter.Write(MD5.MD5Encrypt16Byte(data));

			HeadWriter.Write(SaveTime.ToBinary());
			HeadWriter.Write(TotalRunTime.Ticks);
			HeadWriter.Write(TotalPlayTime.Ticks);

			HeadWriter.Write(MaxScore);

			HeadWriter.Write(MusicRoomGetData.Length);

			if (bytes != null)
			{
				HeadWriter.Write(bytes.Length);
			}
			else
			{
				HeadWriter.Write(0);
			}

			HeadWriter.Close();

			TotalWriter.Write(HeadMemory.ToArray());
			TotalWriter.Write(DataMemory.ToArray());

			TotalWriter.Close();

			return TotalMemory.ToArray();
		}

		public override bool Equals(object obj)
		{
			return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override string ToString()
		{
			return base.ToString();
		}
	}

	public interface IBGMPackData
	{
		string Name { get; set; }
		string Text { get; set; }

		string DataPath { get; set; }

		TimeSpan StartTime { get; set; }
		TimeSpan LoopStartTime { get; set; }
		TimeSpan LoopEndTime { get; set; }

		IBGMPackData Copy();
	}

	[Serializable]
	public class WaveBGMData : IBGMPackData
	{
		public string Name { get; set; } = "";
		public string Text { get; set; } = "";
		
		public AudioClip Data { get; set; }
		public string DataPath { get; set; } = "";

		public TimeSpan StartTime { get; set; }
		public TimeSpan LoopStartTime { get; set; }
		public TimeSpan LoopEndTime { get; set; }

		public WaveBGMData()
		{

		}

		public WaveBGMData(AudioClip data, string text, TimeSpan startime, TimeSpan loopstartime, TimeSpan loopendtime)
		{
			Data = data;
			Text = text;
			StartTime = startime;
			LoopStartTime = loopstartime;
			LoopEndTime = loopendtime;
		}

		public WaveBGMData(AudioClip data, string name, string text, TimeSpan startime, TimeSpan loopstartime, TimeSpan loopendtime)
		{
			Data = data;
			Name = name;
			Text = text;
			StartTime = startime;
			LoopStartTime = loopstartime;
			LoopEndTime = loopendtime;
		}

		public IBGMPackData Copy()
		{
			return new WaveBGMData(Data, Name, Text, StartTime, LoopStartTime, LoopEndTime)
			{
				DataPath = DataPath
			};
		}
	}

	[Serializable]
	public class MidiBGMData : IBGMPackData
	{
		public string Name { get; set; } = "";
		public string Text { get; set; } = " ";

		public MemoryStream Data;
		public string DataPath { get; set; } = "";

		public TimeSpan StartTime { get; set; }
		public TimeSpan LoopStartTime { get; set; }
		public TimeSpan LoopEndTime { get; set; }

		public MidiBGMData()
		{

		}

		public MidiBGMData(MemoryStream data, TextAsset text, TimeSpan startime, TimeSpan loopstartime, TimeSpan loopendtime)
		{
			Data = data;
			Text = text.text;
			StartTime = startime;
			LoopStartTime = loopstartime;
			LoopEndTime = loopendtime;
		}

		public MidiBGMData(MemoryStream data, string text, TimeSpan startime, TimeSpan loopstartime, TimeSpan loopendtime)
		{
			Data = data;
			Text = text;
			StartTime = startime;
			LoopStartTime = loopstartime;
			LoopEndTime = loopendtime;
		}

		public MidiBGMData(MemoryStream data, string name, string text, TimeSpan startime, TimeSpan loopstartime, TimeSpan loopendtime)
		{
			Data = data;
			Name = name;
			Text = text;
			StartTime = startime;
			LoopStartTime = loopstartime;
			LoopEndTime = loopendtime;
		}

		public MidiBGMData(string filepath, string name, string text, TimeSpan startime, TimeSpan loopstartime, TimeSpan loopendtime)
		{
			DataPath = filepath;
			Name = name;
			Text = text;
			StartTime = startime;
			LoopStartTime = loopstartime;
			LoopEndTime = loopendtime;
		}

		public IBGMPackData Copy()
		{
			return new MidiBGMData(Data, Name, Text, StartTime, LoopStartTime, LoopEndTime)
			{
				DataPath = DataPath
			};
		}
	}

	public struct SysTime
	{
		public int time;
		public float time_fen;
		public int time_now;
		public int fangshi;
		public int jiajian;
		public float fendu;
		public int duixiang;
		public float zhi;
	}

	public static class WindowsAPI
	{
		[DllImport("Kernel32.dll", SetLastError = true)]
		public static extern SafeFileHandle CreateFile(string FileName, AccessFlag accessFlag, ShareMode shareMode, IntPtr securityAttr, CreateMode createMode, uint flagsAndAttributes, IntPtr templateFile);
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool ReadFile(IntPtr hFile, byte[] lpBuffer, uint nNumberOfBytesToRead, out uint lpNumberOfBytesRead, IntPtr lpOverlapped);
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool WriteFile(IntPtr hFile, byte[] lpBuffer, uint nNumberOfBytesToWrite, out uint lpNumberOfBytesWritten, IntPtr lpOverlapped);
		[DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		public static extern uint SetFilePointer([In] SafeFileHandle hFile, [In] long lDistanceToMove, IntPtr lpDistanceToMoveHigh, [In] EMoveMethod dwMoveMethod);
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool SetFilePointerEx([In] SafeFileHandle hFile, [In] long lDistanceToMove, IntPtr lpDistanceToMoveHigh, [In] EMoveMethod dwMoveMethod);
		[DllImport("kernel32.dll")]
		public static extern SafeFileHandle OpenProcess(uint flag, bool ihh, int processid);
		[DllImport("kernel32.dll")]
		public static extern bool ReadProcessMemory(SafeFileHandle handle, int address, int[] buffer, int size, int[] nor);
		[DllImport("kernel32.dll")]
		public static extern SafeFileHandle WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [In, Out] byte[] buffer, int size, out IntPtr lpNumberOfBytesWritten);
		[DllImport("kernel32", EntryPoint = "CreateRemoteThread")]
		public static extern int CreateRemoteThread(int hProcess, int lpThreadAttributes, int dwStackSize, int lpStartAddress, int lpParameter, int dwCreationFlags, ref int lpThreadId);
		[DllImport("Kernel32.dll")]
		public static extern SafeFileHandle VirtualAllocEx(IntPtr hProcess, int lpAddress, int dwSize, short flAllocationType, short flProtect);
		[DllImport("kernel32.dll", EntryPoint = "CloseHandle")]
		public static extern int CloseHandle(int hObject);
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool CloseHandle(IntPtr hObject);

		public const uint GENERIC_READ = 0x80000000;
		public const uint GENERIC_WRITE = 0x40000000;
		public const uint FILE_SHARE_READ = 0x00000001;
		public const uint FILE_SHARE_WRITE = 0x00000002;
		public const uint OPEN_EXISTING = 3;
		public const uint FILE_ATTRIBUTE_NORMAL = 0x00000080;
	}

	public class Token
	{
		public long Position;
		public int Type;

		public Token(long position, int type)
		{
			Position = position;
			Type = type;
		}
	}

	[Serializable]
	public class Program
	{
		public string ProgramName;
		public List<ProgramStep> ProgramSteps;
	}

	[Serializable]
	public class ProgramStep
	{
		public List<ProgramAction> Actions;
		public ProgramAction Transition;

		public Queue<ProgramAction> ActionsQueue;

		public void Init()
		{
			if (Actions == null)
			{
				return;
			}

			ActionsQueue = new Queue<ProgramAction>();

			foreach (ProgramAction action in Actions)
			{
				ActionsQueue.Enqueue(action);
			}
		}
	}

	[Serializable]
	public class ProgramAction
	{
		public uint Command;
		public List<string> CommandValue;
	}

	public class Executable
	{
		public string Name;
		public string Type;
		public MapDictionary<string, Token> token;
		public byte[] Memory;
		public MapDictionary<string, long> PrToken;
		public MapDictionary<long, long> CmToken;
		public byte[] Program;

		public Executable(string name, string type, MapDictionary<string, Token> TOK, byte[] memory, byte[] prtoken, byte[] program)
		{
			Name = name;
			Type = type;
			token = TOK;
			Memory = memory;
			Program = program;

			PrToken = new MapDictionary<string, long>();
			CmToken = new MapDictionary<long, long>();

			MemoryStream TMS = new(prtoken);
			BinaryReader binary = new(TMS, Encoding.UTF8);

			long prtl = binary.ReadInt64();
			long cmtl = binary.ReadInt64();

			for (long i = 0; i < prtl; i++)
			{
				string TName = binary.ReadString();
				long TPTR = binary.ReadInt64();
				PrToken.Add(TName, TPTR);
			}

			for (long i = 0; i < cmtl; i++)
			{
				long Index = binary.ReadInt64();
				long TPTR = binary.ReadInt64();
				CmToken.Add(Index, TPTR);
			}
		}
	}

	[Serializable]
	public struct SEAudio
	{
		public SEType SEType;
		public string Name;
		public bool TimeScale;
		public AudioClip AudioClip;

		public SEAudio(AudioClip audioClip, string name = "", bool timescale = false, SEType type = SEType.AsName)
		{
			if (type == SEType.AsName)
			{
				Name = name;
				SEType = SEType.AsName;
			}
			else
			{
				Name = Enum.GetName(typeof(SEType), type);
				SEType = type;
			}
			TimeScale = timescale;
			AudioClip = audioClip;
		}
	}

	public interface IFlag
	{
		string FlagName { get; }
		bool MultipleExecutions { get; }

		bool Condition();
		void Action();
	}

	public interface ISTGComponentFlag : IFlag
	{
		STGComponent Component { get; set; }
	}

	[Serializable]
	public class MemoryMappedViewAccessorStream
	{
		public MemoryMappedViewAccessor ViewAccessor;

		public long Position = 0;

		public MemoryMappedViewAccessorStream(MemoryMappedViewAccessor viewAccessor)
		{
			ViewAccessor = viewAccessor;
		}

		public void Seek(long value, SeekOrigin origin)
		{
			switch (origin)
			{
				case SeekOrigin.Begin:
					Position = 0 + value;
					break;
				case SeekOrigin.Current:
					Position += value;
					break;
				case SeekOrigin.End:
					Position -= value;
					break;
			}
		}
	}
}
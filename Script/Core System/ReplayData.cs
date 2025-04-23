using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NagaisoraFamework
{
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
				string H0 = Encoding.UTF8.GetString(BR.ReadBytes(MainSystem.SCLENAME.Length + 7));
				if (H0 != $"{MainSystem.SCLENAME}\\REPLAY")
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

			binaryWriter.Write(Encoding.UTF8.GetBytes($"{MainSystem.SCLENAME}\\REPLAY"));

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
}

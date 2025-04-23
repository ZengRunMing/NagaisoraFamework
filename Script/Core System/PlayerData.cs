using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace NagaisoraFamework
{
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

			HW.Write($"{MainSystem.SCLENAME}\\PLAYERDATA");

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

			string Hider = Encoding.UTF8.GetString(BR.ReadBytes(MainSystem.SCLENAME.Length + 11));

			if (Hider != $"{MainSystem.SCLENAME}\\PLAYERDATA")
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

			for (int i = 0; i < spellCardScores.Length; i++)
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

}

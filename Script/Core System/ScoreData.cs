using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace NagaisoraFamework
{
	using Cryptography;
	using DataFileSystem;

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

			PlayerData[][] PlayerDatas = new PlayerData[MainSystem.PlayerLength][];

			for (int i = 0; i < PlayerDatas.Length; i++)
			{
				PlayerData[] keyValues = new PlayerData[MainSystem.RankLength];

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

			string Hider = Encoding.UTF8.GetString(binaryReader.ReadBytes(MainSystem.SCLENAME.Length + 10));
			if (Hider != $"{MainSystem.SCLENAME}\\SCOREDATA")
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

			HeadWriter.Write(Encoding.UTF8.GetBytes($"{MainSystem.SCLENAME}\\SCOREDATA"));

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
}

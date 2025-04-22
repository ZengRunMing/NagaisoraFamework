using System.IO;
using System.Text;
using System;

using UnityEngine;

namespace NagaisoraFamework
{
	using Miedia;
	using DataFileSystem;

	using static MainSystem;

	public class SystemStart : CommMonoScriptObject
	{
		public int PlayerLength;
		public int RankLength;

		public string[] arguments;

		public string ServerPort;
		public string ThisPort;

		public long MaxBullet;

		public void Awake()
		{
			Debug.Log("设置全屏");
			Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
			Screen.SetResolution(1440, 1080, true);

			Debug.Log("MainSystem初始化");
			StartTime = DateTime.Now;

			string ConfigFileName = $"{DataPath}\\Config.cfg";

			Debug.Log($"SystemStart.Awake() Calling ConfigLoad({ConfigFileName}) => 开始装载配置文件");

			MainSystem.ConfigData = ConfigData.Default;

			if (!File.Exists(ConfigFileName))
			{
				Debug.Log($"配置文件不存在, 装载默认设置并保存");
			}

			FileStream ConfigDataStream = new(ConfigFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);

			try
			{
				BinaryReader ConfigReader = new(ConfigDataStream, Encoding.UTF8);
				ConfigReader.BaseStream.Seek(0x00, SeekOrigin.Begin);

				string hider = $"{SCLENAME}\\CONFIGDATA";
				int hiderbufferlength = Encoding.UTF8.GetBytes(hider).Length;

				byte[] readhiderbuffer = ConfigReader.ReadBytes(hiderbufferlength);

				if (Encoding.UTF8.GetString(readhiderbuffer) != hider)
				{
					throw new InvalidDataException("文件头不匹配");
				}

				MainSystem.ConfigData = ConfigData.FormBinary(ConfigReader.ReadBytes((int)ConfigReader.BaseStream.Length - hiderbufferlength));
				ConfigReader.Close();
			}
			catch (Exception e)
			{
				Debug.LogException(e);
				BinaryWriter ConfigWriter = new(ConfigDataStream, Encoding.UTF8, true);
				ConfigWriter.BaseStream.Seek(0x00, SeekOrigin.Begin);

				ConfigWriter.Write(Encoding.UTF8.GetBytes($"{SCLENAME}\\CONFIGDATA"));
				ConfigWriter.Write(MainSystem.ConfigData.ToBinary());

				ConfigWriter.Close();
			}

			Debug.Log("SystemStart.Awake() Calling ScoreDataSystem.ScoreDataLoad(default) => 装载ScoreData");
			MainSystem.ScoreData = ScoreDataSystem.ScoreDataLoad(null);

			if (DataPathFileExists("thwav.dat"))
			{
				Debug.Log($"SystemStart.Awake() Calling WavBGMDataPack.ReadWavBGMData(null) => 装载Wave BGM数据");
				WaveBGMData[] WaveBGMData = WaveBGMDataPack.ReadWaveBGMData(null);
				MainSystem.WaveBGMData = WaveBGMData;
			}
			else
			{
				Debug.Log("SystemStart.Awake() => 不存在Wave BGM数据 不进行装载");
			}

			if (DataPathFileExists("thmid.dat"))
			{
				Debug.Log($"SystemStart.Awake() Calling MidiBGMDataPack.AudioFileRead(null) => 装载Midi BGM数据");
				MidiBGMData[] MidiBGMData = MidiBGMDataPack.ReadMidiBGMData(null);
				MainSystem.MidiBGMData = MidiBGMData;
			}
			else
			{
				Debug.Log("SystemStart.Awake() => 不存在Midi BGM数据 不进行装载");
			}
		}
	}
}
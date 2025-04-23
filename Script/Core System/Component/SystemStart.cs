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

			Debug.Log($"SystemStart.Awake() Calling ConfigLoad({ConfigFileName}) => 开始装载配置文件");

			MainSystem.ConfigData = ConfigData.Default;

			FileStream ConfigDataStream = new(ConfigFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite);

			if (ConfigDataStream.Length == 0)
			{
				Debug.Log($"配置文件不存在, 装载默认设置并保存");

				ConfigFileSystem.SaveConfig(ConfigDataStream, MainSystem.ConfigData);
			}

			try
			{
				MainSystem.ConfigData = ConfigFileSystem.LoadConfig(ConfigDataStream);
			}
			catch (Exception e)
			{
				Debug.LogException(e);
			}

			ConfigDataStream.Close();

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
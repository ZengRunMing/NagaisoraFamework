using System;
using System.IO;

using UnityEngine;

namespace NagaisoraFamework
{
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
}
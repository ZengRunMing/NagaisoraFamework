using System;
using UnityEngine;

namespace NagaisoraFamework
{
	public class MainSystemControler : CommMonoScriptObject
	{
		public int SetFps;

		public float FPSNow;

		public int frames = 0;
		public float lastInterval;
		public float updateInterval = 0.5f;

		public void Update()
		{
			Application.targetFrameRate = SetFps;

			MainSystem.GolbalDeltaTime = Time.deltaTime;

			++frames;
			float timeNow = Time.realtimeSinceStartup;
			if (timeNow >= lastInterval + updateInterval)
			{
				FPSNow = frames / (timeNow - lastInterval);
				frames = 0;
				lastInterval = timeNow;
				MainSystem.Fps = FPSNow;
			}

			MainSystem.RunTime = DateTime.Now - MainSystem.StartTime;
			MainSystem.TotalRunTime = MainSystem.ScoreData.TotalRunTime + MainSystem.RunTime;
		}

		public void FixedUpdate()
		{
			MainSystem.GolbalFixedDeltaTime = Time.fixedDeltaTime;
		}
	}
}
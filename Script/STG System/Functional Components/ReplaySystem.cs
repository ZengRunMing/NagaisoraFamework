using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace NagaisoraFamework.STGSystem
{
	using static MainSystem;

	public class ReplaySystem : CommMonoScriptObject
	{
		public STGManager STGManager;

		public bool IsGolbal;

		public uint GameTime;

		public ReplayActionData[] ActionDatas;

		public bool IsRecording = false;
		public bool IsReplaying = false;

		public Vector2 LastVector = Vector2.zero;
		public Dictionary<uint, ReplayActionData> Actions = null;
		public Dictionary<uint, int> Keys = null;

		public ushort LastKeys;
		public ushort DownKeys;

		public void Awake()
		{
			if (IsGolbal)
			{
				GolbalReplaySystem = this;

				MainSystem.KeyDown += KeyDown;
			}
		}

		public void OnDisable()
		{
			MainSystem.KeyDown -= KeyDown;
		}

		public void OnUpdate()
		{
			if (IsRecording)
			{
				if (STGManager != null)
				{
					Keys?.Add(GameTime, STGManager.RandomKey);
				}
			}

			if (IsReplaying)
			{
				if (Actions.ContainsKey(GameTime))
				{
					ReplayActionData data = Actions[GameTime];
					DownKeys = data.DownKeys;
				}

				STGManager.CallKeyDown(DownKeys);
			}
		}

        public void KeyDown(bool[] keys)
        {
			if (IsRecording)
			{
				if (Actions.ContainsKey(GameTime))
				{
					return;
				}

				BitArray bitArray = new(keys);

				byte[] data = new byte[bitArray.Count / 8];
				bitArray.CopyTo(data, 0);
				ushort nowkeys = BitConverter.ToUInt16(data);

				if (nowkeys == LastKeys)
				{
					return;
				}

				Actions.Add(GameTime, new(GameTime, nowkeys));
				LastKeys = nowkeys;
			}
		}

		public void RecordStart()
        {
            Actions = new();
			Keys = new();
			IsRecording = true;
        }

		internal void RecordContinue()
		{
			IsRecording = true;
		}

		public void RecordStop()
		{
            ActionDatas = Actions.Values.ToArray();
            IsRecording = false;
		}

        public void ReplayStart()
        {
			if (ActionDatas == null)
			{
				return;
			}

			Actions = new();

			foreach(ReplayActionData actionData in ActionDatas)
			{
				Actions.Add(actionData.GameTime, actionData);
			}

			IsReplaying = true;
        }

		public void ReplayStop()
		{
			IsReplaying = false;
		}

		public void ReplayContinue()
		{
			IsReplaying = true;
		}
	}
}
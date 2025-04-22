using System;
using System.Collections;
using UnityEngine.UI;

namespace NagaisoraFamework
{
	public class ActionDataViwer : CommMonoScriptObject
	{
		public ReplayActionData data;

		public Text GameTime;
		public Text Keys;

		// Start is called before the first frame update
		public void OnEnable()
		{
			GameTime.text = data.GameTime.ToString();
			Keys.text = GetKeysData(data.DownKeys);
		}

		public string GetKeysData(uint DK)
		{
			string Out = string.Empty;

			BitArray array = new(BitConverter.GetBytes(DK));

			foreach(bool b in array)
			{
				Out += b == true ? "1" : "0";
			}

			return Out;
		}
	}
}
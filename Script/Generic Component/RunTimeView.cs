using UnityEngine;
using UnityEngine.UI;

namespace NagaisoraFamework
{
	using static MainSystem;

	public class RunTimeView : MonoBehaviour
	{
		public Text RunTimeText;

		public Text TotalRunTimeText;

		private void Update()
		{
			if (RunTimeText != null && RunTime != null)
			{
				RunTimeText.text = $"RunTime: {RunTime:hh\\:mm\\:ss}";
			}

			if (TotalRunTimeText != null && MainSystem.ScoreData != null)
			{
				TotalRunTimeText.text = $"TotalRunTime: {TotalRunTime:hh\\:mm\\:ss}";
			}
		}
	}
}

using UnityEngine;

namespace NagaisoraFamework
{
	using static MainSystem;

	public class CameraMove : CommMonoScriptObject
	{
		public Vector3 speed_start;

		public int[] time_change;

		public Vector3[] speed_change;

		public int[] time_change_long;

		public bool[] Execute;

		public void Start()
		{
			for (int i = 0; i < time_change.Length; i++)
			{
				speed_change[i] = speed_change[i] / (float)time_change_long[i];
			}
			Execute = new bool[time_change.Length];
		}

		public void FixedUpdate()
		{
			base.transform.position += speed_start;
			for (int i = 0; i < time_change.Length; i++)
			{
				if (Gametime >= time_change[i])
				{
					if (Gametime == time_change[i])
					{
						Execute[i] = true;
					}
					if (time_change_long[i] > 0 && Execute[i])
					{
						speed_start += speed_change[i];
						time_change_long[i]--;
					}
				}
			}
		}
	}
}

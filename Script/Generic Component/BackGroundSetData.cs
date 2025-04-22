using UnityEngine;

namespace NagaisoraFamework
{
	using static MainSystem;

	public class BackGroundSetData : CommMonoScriptObject
	{
		public int[] set_time;
		public int[] set_speed;

		public BackGroundLoop Loop;

		public void Start()
		{
			Loop = GetComponent<BackGroundLoop>();
		}

		public void FixedUpdate()
		{
			for (int i = 0; i < set_time.Length; i++)
			{
				if (Gametime == set_time[i])
				{
					Loop.time_start = set_speed[i];
				}
			}
		}
	}
}

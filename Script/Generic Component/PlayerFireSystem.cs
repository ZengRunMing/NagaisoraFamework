using UnityEngine;

namespace NagaisoraFamework
{
	using STGSystem;

	using static MainSystem;

	public class PlayerFireSystem : CommMonoScriptObject
    {
		public STGSystemData PSD;
		public STGManager STGManager;

		public int Count = 0;
		public int OutputInterval = 4;

		public void Awake()
		{

		}

		public void Shoot(int i)
		{
			if (Count % 4 == 0)
			{
				
				Count = 0;
			}
			Count++;
		}

		public void ShootUp(int i)
		{
			Count = 0;
		}

		public void Spell(int i)
		{

		}

		public void SlotDown(int i)
		{

		}

		public void SlotUp(int i)
		{

		}
	}
}

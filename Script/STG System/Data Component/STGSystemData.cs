using UnityEngine;

namespace NagaisoraFamework.STGSystem
{
	public class STGSystemData : CommMonoScriptObject
	{
		public EnemyInfo[] Enemy;
		public EnemyBulletInfo[] EnemyBullet;
		public EnemyLongLaserInfo EnemyLongLaser;
		public PlayerInfo[] Player;
		public PlayerBulletInfo[] PlayerBullet;
		public Vector2 EffectSize;
		public BulletObject[] EnemyBulletEffect;
		public BulletObject[] PlayerBulletEffect;

		public void Awake()
		{
			MainSystem.STGSystemData = this;
		}
	}
}
using UnityEngine;
using UnityEngine.UIElements;

namespace NagaisoraFamework.STGSystem
{
	using static MainSystem;

	//玩家子弹控制系统
	public class PlayerBulletControl : BulletControl
	{
		public PlayerBulletInfo bulletData;

		public override void Init()
		{
			bulletData = STGManager.STGSystemData.PlayerBullet[Type];

			Determine_Offset = bulletData.Determine_Offset;
			Determine_Radius = bulletData.Determine_Radius;
			AngleOffsetCompensation = bulletData.AngleOffsetCompensation;   //设定显示角度偏移补偿

			float angle = ViewAngle + AngleOffsetCompensation;
			transform.eulerAngles = new Vector3(0f, 0f, angle);

			base.Init();

			SpriteRender.sprite = bulletData.Sprite;
			SpriteRender.size = bulletData.Normoal_Size;
		}

		public override void OnUpdate()
		{
			foreach (var enemy in STGManager.Enemys)
			{
				Check(enemy);
			}

			base.OnUpdate();
		}

		public override void Check(STGComponent Targe)
		{
			if (HitCheck(Targe))
			{
				BaseDelete();
				Targe.BaseDelete();
				return;
			}
		}
	}
}
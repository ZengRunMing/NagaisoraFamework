using UnityEngine;

namespace NagaisoraFamework.STGSystem
{
    public class BulletControl : STGComponent
	{
		public int Type;

		public bool Determing = true;
		public bool Delete_Effect = false;

		public override void Init()
		{
			SpriteRender.drawMode = SpriteDrawMode.Sliced;
			SpriteRender.sortingLayerName = "StageMain";
			SpriteRender.sortingOrder = Order;
			       //设定SpriteRender材质

			base.Init();
		}

		public override void OnUpdate()
		{
			base.OnUpdate();

			float angle = ViewAngle + AngleOffsetCompensation;

			if (transform.eulerAngles.z != angle)
			{
				transform.eulerAngles = new Vector3(0f, 0f, angle);
			}

			Move();
		}

		public virtual void Check(STGComponent Targe)
		{

		}

		public override void BaseDelete()
		{
			base.BaseDelete();

			transform.localPosition = STGManager.DisablePosition;

			STGManager.EnemyBullets.Remove(this);
		}
	}
}

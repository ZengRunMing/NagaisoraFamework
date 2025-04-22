using UnityEngine;

namespace NagaisoraFamework.STGSystem
{
	using static MainSystem;

	public class EnemyControl : STGComponent
	{
		EnemyInfo EnemyInfo;

		public int Type;
		public int Color;

		public bool Determing = true;
		public bool Delete_Effect = false;

		public Animator Animator;

		public float LastMoveVectorX;

		public override void Awake()
		{
			base.Awake();
			Animator = gameObject.AddComponent<Animator>();
		}

		public override void Init()
		{
			EnemyInfo = STGManager.STGSystemData.Enemy[Type];

			Determine_Offset = EnemyInfo.Determine_Offset;
			Determine_Radius = EnemyInfo.Determine_Radius;

			SpriteRender.drawMode = SpriteDrawMode.Sliced;
			SpriteRender.sortingLayerName = "StageMain";
			SpriteRender.sortingOrder = Order;

			EnemyObject enemyObject = EnemyInfo.Info[Color];

			Animator.runtimeAnimatorController = enemyObject.AnimatorController;

			SetAnimatorNormal();

			base.Init();
		}

		public virtual void SetAnimatorNormal()
		{
			if (Animator == null)
			{
				return;
			}
			Animator.SetBool("Moveing", false);
		}

		public virtual void SetAnimatorMoveLeft()
		{
			if (Animator == null)
			{
				return;
			}
			Animator.SetBool("MoveR", false);
			Animator.SetBool("Moveing", true);
		}

		public virtual void SetAnimatorMoveRight()
		{
			if (Animator == null)
			{
				return;
			}
			Animator.SetBool("MoveR", true);
			Animator.SetBool("Moveing", true);
		}

		public override void BaseDelete()
		{
			if (Delete_Effect)
			{
				STGManager.NewEffect<EffectControl>(Color, Order - 21, TransformPosition);
			}

			base.BaseDelete();

			transform.localPosition = STGManager.DisablePosition;

			STGManager.EnemyBullets.Remove(this);
		}
	}
}

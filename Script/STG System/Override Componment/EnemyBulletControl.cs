using UnityEngine;

namespace NagaisoraFamework.STGSystem
{
	using static MainSystem;

	//敌机子弹控制系统
	public class EnemyBulletControl : BulletControl
	{
		public int Color;													//子弹颜色类型

		public EnemyBulletInfo bulletData;									//子弹设定缓存

		public override void Init()											//基于父类派生重写的初始化方法，此处编写初始化程序
		{
			bulletData = STGManager.STGSystemData.EnemyBullet[Type];		//通过类型获取设定缓存数据

			Determine_Offset = bulletData.Determine_Offset;					//设定判定偏移
			Determine_Radius = bulletData.Determine_Radius;                 //设定判定半径
			AngleOffsetCompensation = bulletData.AngleOffsetCompensation;	//设定显示角度偏移补偿

			base.Init();													//调用父类的初始化方法

			SpriteRender.sprite = bulletData.Info[Color].Sprite;			//通过颜色类型从设定缓存提取具体贴图至SpriteRender
			SpriteRender.size = bulletData.Normoal_Size;                    //设置SpriteRender的渲染大小
		}

		public override void OnUpdate()                                     //基于父类派生重写的逻辑更新方法
		{
			Check(STGManager.Player);										//调用判定检查方法
			base.OnUpdate();												//调用父类的逻辑更新方法
		}

		public override void Check(STGComponent Targe)                      //基于父类派生重写的判定检查方法
		{
			if (GrazeCheck(Targe))                                          //判断指定对象(玩家)是否在Graze判定范围内
			{
				if (ThisTime % 2 == 0)										//每间隔一次更新执行一次
				{
					SEManager.PlaySE("Graze");								//调用播放音效方法
				}
			}

			if (ThisTime < 5 || !Determing)									//判断本地逻辑更新时间是否小于5以及是否未开启判定
			{
				return;														//执行无条件返回
			}

			if (HitCheck(Targe))											//判断指定对象(玩家)是否在判定范围内
			{																
				STGManager.LifeSub();                                       //调用STG管理器玩家残机减一函数
				BaseDelete();												//销毁自身 (入列对象池)
				return;														//执行无条件返回
			}
		}

		public virtual bool GrazeCheck(STGComponent Target)                 //基于父类派生重写的Graze判定方法，原理与标准判定类似
		{
			if (Determine_Radius <= 0f)
			{
				return false;
			}

			float fy = Determine_Radius * Scale.y;
			float fx = Determine_Radius * Scale.x;

			float dy = TransformPosition.y - Target.TransformPosition.y - Determine_Offset.y;
			float dx = TransformPosition.x - Target.TransformPosition.x - Determine_Offset.x;

			float ey = fy + STGManager.Graze_Vector;						//在此添加了Graze判定半径向量
			float ex = fx + STGManager.Graze_Vector;                        //在此添加了Graze判定半径向量

			float ady = Mathf.Abs(dy);
			float adx = Mathf.Abs(dx);

			if (ady < ey && adx < ex && ey * ex > dy * dy + dx * dx)
			{
				return true;
			}

			return false;
		}

		public override void BaseDelete()                                   //基于父类派生重写的销毁自身方法
		{
			if (Delete_Effect)
			{
				STGManager.NewEffect<EffectControl>(Color, Order - 21, TransformPosition);
			}

			base.BaseDelete();
		}
	}
}

using UnityEngine;

namespace NagaisoraFamework.STGSystem
{
	//Effect控制系统
	public class EffectControl : STGComponent
	{
		public int Color;

		BulletObject EffectInfo;

		public override void Init()
		{
			EffectInfo = STGManager.STGSystemData.EnemyBulletEffect[Color];

			SpriteRender.drawMode = SpriteDrawMode.Sliced;				//设定SpriteRender渲染模式
			SpriteRender.sortingLayerName = "StageMain";				//设定SpriteRender渲染图层
			SpriteRender.sortingOrder = Order;							//设定SpriteRender图层排序号
			SpriteRender.material = STGManager.HighLightMaterial;       //设定SpriteRender材质

			SpriteRender.color = UnityEngine.Color.white;				//初始化SpriteRender的RGBA值
			SpriteRender.sprite = EffectInfo.Sprite;					//通过颜色类型从设定缓存提取具体贴图至SpriteRender
			SpriteRender.size = EffectInfo.Sprite.rect.size;			//设置SpriteRender的渲染大小

			base.Init();                                                //调用父类的初始化方法
		}

		public override void OnUpdate()
		{
			base.OnUpdate();

			if (ThisTime < 5)
			{
				return;
			}

			Color color = SpriteRender.color;					//获取当前SpriteRender的RGBA值
			color.a -= 0.1f;									//Aphla减指定量

			SpriteRender.color = color;							//覆盖当前SpriteRender的RGBA值

			if (color.a <= 0)
			{
				BaseDelete();
			}
		}
	}
}
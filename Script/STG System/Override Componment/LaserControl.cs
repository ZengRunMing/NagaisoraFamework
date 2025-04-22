using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace NagaisoraFamework.STGSystem
{
	using static MainSystem;

	public class LaserControl : STGComponent
	{
		public LaserType Type;
		public int Color;

		public bool Determing = true;
		public bool Delete_Effect = false;

		public Material LaserMaterial;
		public BendLaserMesh BendLaserMesh;

		public int LaserLength;
		public float Width = 16f;

		public List<Vector2> KeyPoints;

		public FixedLengthQueue<Vector2> KeyPointQueue;

		public Vector2 HeadPosition;
		public Vector2 LastPosition;

		public override void Awake()
		{
			transform = gameObject.transform;
		}

		public override void Init()
		{
			Determine_Radius = STGManager.STGSystemData.EnemyLongLaser.Determine_Radius;

			if (Type == LaserType.Long)
			{
				LaserLength = 2000;

				GameObject objectT = new GameObject()
				{
					name = "LaserObject",
					layer = 6,
				};

				objectT.transform.SetParent(transform);

				SpriteRender = objectT.AddComponent<SpriteRenderer>();

				SpriteRender.drawMode = SpriteDrawMode.Sliced;
				SpriteRender.sprite = STGManager.STGSystemData.EnemyLongLaser.Info[Color].Sprite;
				SpriteRender.size = STGManager.STGSystemData.EnemyLongLaser.Normoal_Size;
				SpriteRender.sortingLayerName = "StageMain";
				SpriteRender.sortingOrder = Order;

				objectT.AddComponent<PositionofSize>();
			}
			else
			{
				KeyPointQueue = new FixedLengthQueue<Vector2>(LaserLength);

				BendLaserMesh = gameObject.AddComponent<BendLaserMesh>();
				transform.localPosition = Vector3.zero;

				BendLaserMesh.Init("StageMain", Order);

				SetBendLaserMesh();
			}

			base.Init();
		}

		public override void OnUpdate()
		{
			base.OnUpdate();

			Check();

			if (Type == LaserType.Segmental)
			{
				if (HeadPosition != LastPosition)
				{
					KeyPointQueue.Enqueue(HeadPosition);
					LastPosition = HeadPosition;
				}

				Vector2[] vector2s = KeyPointQueue.ToArray();
				Array.Reverse(vector2s);
				KeyPoints = vector2s.ToList();

				SetBendLaserMesh();
			}
		}

		public override void Move()
		{
			if (MoveVector == Vector2.zero)
			{
				return;
			}

			HeadPosition += MoveVector * Speed;
		}

		public void Check()
		{
			if (ThisTime < 5 || !Determing)
			{
				return;
			}

			if (HitCheck(STGManager.Player))
			{
				//STGManager.LifeSub();
				//BaseDelete();

				Debug.Log($"[{name}] >> HitCheck() -> True");
				return;
			}

			if (GrazeCheck(STGManager.Player))
			{
				if (ThisTime % 2 == 0)
				{
					SEManager.PlaySE("Graze");
				}
			}
		}

		public override bool HitCheck(STGComponent Target)
		{
			return HitCheck(Target, Determine_Radius);
		}

		public override bool HitCheck(STGComponent Target, float Determine_Radius)
		{
			if (Determine_Radius <= 0f)
			{
				return false;
			}

			float ADSAngles = EulerAngles_ADS(ProgramAngle);

			float fx = Determine_Radius * Scale.x;
			float fy = Determine_Radius * Scale.y;

			float px = fx + STGManager.Determine_Vector;
			float py = fy + STGManager.Determine_Vector;

			for (int i = 0; i < LaserLength; i++)
			{
				float ax = TransformPosition.x;
				float ay = TransformPosition.y;

				if (Type == LaserType.Segmental)
				{
					if (i >= KeyPoints.Count)
					{
						break;
					}

					if (i < 3)
					{
						continue;
					}

					ax += KeyPoints[i].x;
					ay += KeyPoints[i].y;
				}
				else
				{
					ax += i * Dsin(ADSAngles);
					ay += i * Dcos(ADSAngles);
				}

				float dx = ax - Target.TransformPosition.x - Determine_Offset.x;
				float dy = ay - Target.TransformPosition.y - Determine_Offset.y;

				if (Mathf.Abs(dy) < py && Mathf.Abs(dx) < px && py * px > dy * dy + dx * dx)
				{
					return true;
				}
			}

			return false;
		}

		public override bool OutSizeCheck()
		{
			if (KeyPoints is null || KeyPoints.Count == 0)
			{
				return false;
			}

			Vector2 position = KeyPoints.Last();

			if (position.x > STGManager.MaxPosition.x || position.x < -STGManager.MaxPosition.x || position.y > STGManager.MaxPosition.y || position.y < -STGManager.MaxPosition.y)
			{
				return true;
			}
			return false;
		}

		public virtual bool GrazeCheck(STGComponent Target)
		{
			return HitCheck(STGManager.Player, Determine_Radius + 4f);
		}

		public void SetBendLaserMesh()
		{
			if (BendLaserMesh == null)
			{
				return;
			}

			BendLaserMesh.material = LaserMaterial;
			
			if (KeyPoints != null)
			{
				BendLaserMesh.KeyPoints = KeyPoints.ToArray();
			}

			BendLaserMesh.ColorType = Color;
			BendLaserMesh.Width = Width;
		}

		public override void BaseDelete()
		{
			if (BendLaserMesh != null)
			{
				BendLaserMesh.Clear();
				Destroy(BendLaserMesh);
			}
			if (SpriteRender != null)
			{
				Destroy(SpriteRender);
			}

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

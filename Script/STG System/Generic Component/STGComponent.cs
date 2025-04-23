using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace NagaisoraFamework.STGSystem
{
	using static MainSystem;

	public class STGComponent : CommMonoScriptObject
	{
		public int ComponentType = 0;

		public SpriteRenderer SpriteRender;

		public new Transform transform;

		public Vector2 TransformPosition;

		public uint ThisTime;

		public float Angle;
		public float Determine_Radius;
		public float Direction;

		public Vector2 Determine_Offset;
		public Vector2 Scale = Vector2.one;
		public bool AngleWithDirection;

		public STGManager STGManager;
		public Vector2[] LastPositions;

		public float Speed;
		public Vector2 MoveVector;

		public float ViewAngle;
		public float AngleOffsetCompensation;

		public float ProgramAngle;

		public int Order;

		public Dictionary<string, ISTGComponentFlag> Flags;
		List<ISTGComponentFlag> RemovedConditionFlags;
		public Dictionary<string, ISTGComponentFlag> RunningFlags;
		List<ISTGComponentFlag> RemovedRunningFlags;

		public uint GameTime
		{
			get
			{
				return STGManager.GameTime;
			}
		}

		public List<STGComponent> EnemyBullets
		{
			get
			{
				return STGManager.EnemyBullets;
			}
		}

		public List<PlayerBulletControl> PlayerBullets
		{
			get
			{
				return STGManager.PlayerBullets;
			}
		}

		public List<EnemyControl> Enemys
		{
			get
			{
				return STGManager.Enemys;
			}
		}

		public virtual void Awake()
		{
			if (!TryGetComponent(out SpriteRender))
			{
				SpriteRender = gameObject.AddComponent<SpriteRenderer>();
			}

			transform = gameObject.transform;
		}

		public virtual void OnEnable()
		{

		}

		public virtual void OnDisable()
		{
			UnBindUpdateEvent();
		}

		public virtual void Init()
		{
			Flags = new();
			RemovedConditionFlags = new();
			RunningFlags = new();
			RemovedRunningFlags = new();

			ThisTime = 0;

			BindUpdateEvent();
		}

		public virtual void Update()
		{
			TransformPosition = transform.localPosition;
		}

		public virtual void OnUpdate()
		{
			ThisTime++;

			Move();

			if (ThisTime < 5)
			{
				return;
			}

			if (OutSizeCheck())
			{
				BaseDelete();
				return;
			}

			FlagCheck();
		}

		public virtual void KeyDown(bool[] keys)
		{

		}

		public virtual void FlagCheck()
		{
			if ((Flags == null || Flags.Count == 0) && (RunningFlags == null || RunningFlags.Count == 0))
			{
				return;
			}

			ISTGComponentFlag[] ConditionFlagsArray = Flags.Values.ToArray();
			RemovedConditionFlags.Clear();
			foreach (var flag in ConditionFlagsArray)
			{
				if (!flag.Condition())
				{
					continue;
				}

				AddRunningFlags(flag);
				RemovedConditionFlags.Add(flag);
			}

			ISTGComponentFlag[] RemovedConditionFlagsArray = RemovedConditionFlags.ToArray();
			foreach (var flag in RemovedConditionFlagsArray)
			{
				RemoveFlags(flag);
			}

			ISTGComponentFlag[] RunningFlagsArray = RunningFlags.Values.ToArray();
			RemovedRunningFlags.Clear();
			foreach (var flag in RunningFlagsArray)
			{
				flag.Action();

				if (!flag.MultipleExecutions)
				{
					RemovedRunningFlags.Add(flag);
				}
			}

			ISTGComponentFlag[] RemovedRunningFlagsArray = RemovedRunningFlags.ToArray();
			foreach (var flag in RemovedRunningFlagsArray)
			{
				RemoveRunningFlags(flag);
			}
		}

		public virtual void Move()
		{
			if (MoveVector == Vector2.zero)
			{
				return;
			}

			transform.localPosition += (Vector3)MoveVector * Speed;
		}

		public virtual void MoveToPosition(Vector2 position)
		{
			transform.localPosition = Vector2.MoveTowards(TransformPosition, position, Speed);
		}

		public virtual bool HitCheck(STGComponent Target)
		{
			return HitCheck(Target, Determine_Radius);
		}

		public virtual bool HitCheck(STGComponent Target, float Determine_Radius)
		{
			if (Determine_Radius <= 0f)
			{
				return false;
			}

			float fy = Determine_Radius * Scale.y;
			float fx = Determine_Radius * Scale.x;

			float dy = TransformPosition.y - Target.TransformPosition.y - Determine_Offset.y;
			float dx = TransformPosition.x - Target.TransformPosition.x - Determine_Offset.x;

			float py = fy + STGManager.Determine_Vector;
			float px = fx + STGManager.Determine_Vector;

			float ady = Mathf.Abs(dy);
			float adx = Mathf.Abs(dx);

			if (Scale.x != Scale.y)
			{
				float EADS_Angle = Mathf.Atan2(dy, dx) * 57.29578f - transform.rotation.eulerAngles.z - 90f;
				float EADS = EulerAngles_ADS(EADS_Angle);
				float num1 = Mathf.Pow(dx * dx + dy * dy, 0.5f) * Dcos(EADS);
				float num2 = Mathf.Pow(dx * dx + dy * dy, 0.5f) * Dsin(EADS);
				if (num1 / px * (num1 / px) + num2 / py * (num2 / py) < 1f)
				{
					return true;
				}
			}
			else
			{
				if (ady < py && adx < px && py * px > dy * dy + dx * dx)
				{
					return true;
				}
			}

			return false;
		}

		public virtual bool OutSizeCheck()
		{
			if (TransformPosition.x > STGManager.MaxPosition.x || TransformPosition.x < -STGManager.MaxPosition.x || TransformPosition.y > STGManager.MaxPosition.y || TransformPosition.y < -STGManager.MaxPosition.y)
			{
				return true;
			}

			return false;
		}

		public float GetDirection(STGComponent Target)
		{
			return GetDirection(Target.TransformPosition);
		}

		public float GetDirection(Vector2 TargetGamePosition)
		{
			return (float)Math.PI + Mathf.Atan2(TransformPosition.y - TargetGamePosition.y, TransformPosition.x - TargetGamePosition.x);
		}

		public float GetDistance(STGComponent Target)
		{
			return GetDistance(Target.TransformPosition);
		}

		public float GetDistance(Vector2 TargetGamePosition)
		{
			return (TargetGamePosition - TransformPosition).magnitude;
		}

		public void BindUpdateEvent()
		{
			STGManager.OnUpdate += OnUpdate;
		}

		public void UnBindUpdateEvent()
		{
			STGManager.OnUpdate -= OnUpdate;
		}

		public void BindKeyEvent()
		{
			STGManager.KeyDown += KeyDown;
		}

		public void UnBindKeyEvent()
		{
			STGManager.KeyDown -= KeyDown;
		}

		public virtual void AddFlags(params ISTGComponentFlag[] flags)
		{
			foreach(var flag in flags)
			{
				if (Flags.ContainsKey(flag.FlagName))
				{
					throw new Exception($"Flag {flag.FlagName} already exists in {ComponentType}.");
				}
				flag.Component = this;
				Flags.Add(flag.FlagName, flag);
			}
		}

		public virtual void RemoveFlags(params IFlag[] flags)
		{
			foreach (var flag in flags)
			{
				Flags.Remove(flag.FlagName);
			}
		}


		public virtual void AddRunningFlags(params ISTGComponentFlag[] flags)
		{
			foreach (var flag in flags)
			{
				if (RunningFlags.ContainsKey(flag.FlagName))
				{
					throw new Exception($"RunningFlag {flag.FlagName} already exists in {ComponentType}.");
				}
				flag.Component = this;
				RunningFlags.Add(flag.FlagName, flag);
			}
		}

		public virtual void RemoveRunningFlags(params ISTGComponentFlag[] flags)
		{
			foreach (var flag in flags)
			{
				RunningFlags.Remove(flag.FlagName);
			}
		}

		public virtual void BaseDelete()
		{
			STGManager.PoolManager.Delete_Object(ComponentType, gameObject);

			UnBindUpdateEvent();
		}
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NagaisoraFamework.STGSystem
{
	using static MainSystem;

	public class STGManager : CommMonoScriptObject
	{
		public bool IsMaster = true;
		public bool IsGolbal = true;

		public bool Test_Status = false;

		public STGManager Master;

		public PlayerControl Player;

		public ReplaySystem ReplaySystem;

		public GameObject Parent;
		public PoolManager PoolManager;

		public STGSystemData STGSystemData;

		public RuntimeAnimatorController EnemyAnimatorController;

		public int DefLife;
		public int DefBomb;

		public float Determine_Vector;
		public float Graze_Vector;

		public Vector2 MaxPosition;
		public Vector2 DisablePosition;
		public long MaxEnemyBulletCount;

		public bool StatusLock;
		public int Life;
		public int Bomb;
		public uint GameTime = 0;
		public int RandomKey;

		public Vector2 PlayerMaxPosition;

		public bool IsRunning = false;
		public bool IsReplaying = false;

		public List<STGComponent> EnemyBullets;
		public List<EnemyControl> Enemys;
		public List<PlayerBulletControl> PlayerBullets;

		public long BulletEffectCount;
		public long EnemyBulletCount;

		public delegate void ComponentUpdate();

		public event ComponentUpdate OnUpdate;

		public Sprite DetermineObjectImage;

		public Material LaserMaterial;
		public Material HighLightMaterial;

		public KeyConfig KeyConfig;
		public Vector2 AxisVector;
		
		public delegate void KeyDownEvent(bool[] bools);
		public event KeyDownEvent KeyDown;

		public ushort DownKey;

		public void Awake()
		{
			PoolManager = new PoolManager();

			if (!gameObject.TryGetComponent(out ReplaySystem))
			{
				ReplaySystem = gameObject.AddComponent<ReplaySystem>();
			}

			ReplaySystem.STGManager = this;

			DefLife = 2;
			DefBomb = 2;

			Determine_Vector = 3f;
			Graze_Vector = 15f;

			PlayerReset();
		}

		public void Start()
		{
			if (IsGolbal)
			{
				GolbalSTGManager = this;
			}

			STGSystemData = MainSystem.STGSystemData;
			KeyConfig = KeyConfig.Default;
		}

		public void OnEnable()
		{
			if (!IsMaster && Master == null)
			{
				Debug.LogWarning(new NullReferenceException($"STGManager::[ID = {GUIDMD5String}]设定为从动体模式，但没有为该从动体指定必要的驱动体, 从动体无驱动体将无法正常工作"));
			}

			KeyDown += ReplaySystem.KeyDown;
			OnUpdate += ReplaySystem.OnUpdate;
		}

		public void OnDisable()
		{
			KeyDown -= ReplaySystem.KeyDown;
			OnUpdate -= ReplaySystem.OnUpdate;
		}

		public void Update()
		{
			InputCheck();

			EnemyBulletCount = EnemyBullets.Count;
			IsReplaying = ReplaySystem != null && ReplaySystem.IsReplaying;
		}

		public void FixedUpdate()
		{
			if (!IsRunning)
			{
				return;
			}

			if (ReplaySystem != null)
			{
				ReplaySystem.GameTime = GameTime;
			}

			CallKeyDown(DownKey);

			RandomKey = RandomInt();

			OnUpdate?.Invoke();

			if (IsMaster)
			{
				GameTime++;
			}
			else if (Master != null)
			{
				Gametime = Master.GameTime;
			}
		}

		public virtual void SystemReset()
		{
			GameTime = 0;

			if (ReplaySystem != null)
			{
				ReplaySystem.GameTime = GameTime;
			}
		}

		public virtual void Run()
		{
			IsRunning = true;
		}

		public virtual void Stop()
		{
			IsRunning = false;
		}

		public virtual void InputCheck()
		{
			BitArray bitArray = new(16, false);

			AxisVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

			if (AxisVector.x != 0 || AxisVector.y != 0)
			{
				if (AxisVector.y > 0.5f)
				{
					bitArray[0] = true;
				}
				if (AxisVector.y < -0.5f)
				{
					bitArray[1] = true;
				}
				if (AxisVector.x < -0.5f)
				{
					bitArray[2] = true;
				}
				if (AxisVector.x > 0.5f)
				{
					bitArray[3] = true;
				}
			}

			if (InputModule.GetKeys(KeyConfig.SubmitKeys) || Input.GetKey(KeyConfig.J_SubmitKey))
			{
				bitArray[4] = true;
			}
			if (InputModule.GetKeys(KeyConfig.CancelKeys) || Input.GetKey(KeyConfig.J_CancelKey))
			{
				bitArray[5] = true;
			}

			if (Input.GetKey(KeyConfig.Slow) || Input.GetKey(KeyConfig.J_Slow))
			{
				bitArray[6] = true;
			}

			byte[] data = new byte[bitArray.Count / 8];
			bitArray.CopyTo(data, 0);
			DownKey = BitConverter.ToUInt16(data);
		}

		public void CallKeyDown(ushort keys)
		{
			BitArray bitArray = new(BitConverter.GetBytes(keys));

			bool[] bools = new bool[bitArray.Count];
			bitArray.CopyTo(bools, 0);

			KeyDown?.Invoke(bools);
		}

		public GameObject NewObject(int type, string name, GameObject parent, Vector2 position)
		{
			GameObject Object = PoolManager.New_Object(type);

			if (Object.transform.parent != parent.transform)
			{
				Object.transform.SetParent(parent.transform);
			}

			Object.layer = parent.layer;
			Object.transform.localPosition = position;
			Object.name = name;
			Object.transform.localScale = new Vector3(1, 1, 1);

			return Object;
		}

		public GameObject NewEnemy<T>(int type, int color, string name, int order, Vector2 position) where T : EnemyControl
		{
			EnemyInfo EnemyInfo = STGSystemData.Enemy[type];

			GameObject Object = NewObject(0, name, Parent, position);

			if (!Object.TryGetComponent(out T component))
			{
				component = Object.AddComponent<T>();
				component.STGManager = this;
			}

			component.ComponentType = 0;
			component.Type = type;
			component.Color = color;
			component.Determine_Offset = EnemyInfo.Determine_Offset;
			component.Determine_Radius = EnemyInfo.Determine_Radius;
			component.Order = 21 + order;

			Enemys.Add(component);

			component.Init();

			return Object;
		}

		public GameObject NewEnemyBullet<T>(int type, int color, string name, int order, Vector2 position, float angle) where T : EnemyBulletControl
		{
			if (EnemyBullets.Count >= MaxEnemyBulletCount)
			{
				Debug.Log("BulletNumberOutMaxCount");
				return null;
			}

			GameObject Object = NewObject(1, name, Parent, position);

			if (!Object.TryGetComponent(out T component))
			{
				component = Object.AddComponent<T>();
				component.STGManager = this;
			}

			component.ComponentType = 1;
			component.Type = type;
			component.Color = color;
			component.Order = 21 + order;
			component.ProgramAngle = angle;
			component.ViewAngle = -angle;

			EnemyBullets.Add(component);

			component.Init();
			return Object;
		}

		public GameObject NewEnemyLaser<T>(LaserType type, int color, int length, string name, int order, Vector2 position, float angle) where T : LaserControl
		{
			if (EnemyBullets.Count >= MaxEnemyBulletCount)
			{
				Debug.Log("BulletNumberOutMaxCount");
				return null;
			}

			GameObject Object = NewObject(2, name, Parent, Vector2.zero);

			if (!Object.TryGetComponent(out T component))
			{
				component = Object.AddComponent<T>();
				component.STGManager = this;
			}

			component.ComponentType = 2;
			component.HeadPosition = position;
			component.LaserMaterial = LaserMaterial;
			component.Type = type;
			component.Color = color;
			component.LaserLength = length;
			component.Order = 21 + order;
			component.ProgramAngle = angle;
			component.ViewAngle = -angle;

			EnemyBullets.Add(component);

			component.Init();
			return Object;
		}


		public GameObject NewPlayerBullet<T>(int type, string name, int order, Vector2 position, float angle) where T : PlayerBulletControl
		{
			GameObject Object = NewObject(11, name, Parent, position);

			if (!Object.TryGetComponent(out T component))
			{
				component = Object.AddComponent<T>();
			}

			component.STGManager = this;

			component.ComponentType = 11;
			component.Type = type;
			component.Order = 21 + order;
			component.ProgramAngle = angle;
			component.ViewAngle = -angle;

			PlayerBullets.Add(component);

			component.Init();
			return Object;
		}

		public GameObject NewEffect<T>(int color, int order, Vector3 position) where T : EffectControl
		{
			GameObject Object = NewObject(20, "Effect", Parent, position);

			if (!Object.TryGetComponent(out T component))
			{
				component = Object.AddComponent<T>();
			}

			component.STGManager = this;

			component.ComponentType = 20;
			component.Color = color;
			component.Order = 21 + order;

			BulletEffectCount++;

			component.Init();
			return Object;
		}

		public void DeleteEffect(GameObject Object)
		{
			PoolManager.Delete_Object(10, Object);
			BulletEffectCount--;
			return;
		}

		public void PlayerReset()
		{
			Life = DefLife;
			Bomb = DefBomb;
		}

		public void LifeSub()
		{
			if (!StatusLock)
			{
				Life--;
				StatusLock = true;
			}
		}
	}
}
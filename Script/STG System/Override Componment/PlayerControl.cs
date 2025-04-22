using System;
using System.Linq;

using UnityEngine;

namespace NagaisoraFamework.STGSystem
{
	using DataFileSystem;

	using static MainSystem;

	public class PlayerControl : STGComponent
	{
		public Animator Animator;
		public float HighSpeed;
		public float SoltSpeed;

		public GameObject Effect;
		public GameObject SlowEffectL;
		public GameObject SlowEffectR;

		public float RD = 10f;

		public bool IsSolt = false;

		public bool IsShoot;

		public float MoveVectorX;
		public float MoveVectorY;

		public override void Awake()
		{
			base.Awake();

			Animator = GetComponent<Animator>();
		}

		public override void OnEnable()
		{
			Init();
		}

		public override void OnDisable()
		{
			UnBindKeyEvent();

			base.OnDisable();
		}

		public override void Init()
		{
			if (!STGManager.IsReplaying)
			{
				BindKeyEvent();
			}

			base.Init();
		}

		public override void Update()
		{
			base.Update();

			if (IsSolt)
			{
				Effect.SetActive(true);
			}
			else
			{
				Effect.SetActive(false);
			}

			if (Input.GetKeyDown(KeyCode.F1))
			{
				STGManager.ReplaySystem.RecordStart();
			}

			if (Input.GetKeyDown(KeyCode.F2))
			{
				STGManager.ReplaySystem.RecordStop();
			}

			if (Input.GetKeyDown(KeyCode.F3))
			{
				STGManager.ReplaySystem.RecordContinue();
			}

			if (Input.GetKeyDown(KeyCode.F4))
			{
				STGManager.ReplaySystem.ReplayStart();
				UnBindKeyEvent();
				STGManager.SystemReset();
			}

			if (Input.GetKeyDown(KeyCode.F5))
			{
				STGManager.ReplaySystem.ReplayStop();
				BindKeyEvent();
			}

			if (Input.GetKeyDown(KeyCode.F6))
			{
				STGManager.ReplaySystem.ReplayContinue();
			}

			if (Input.GetKeyDown(KeyCode.F7))
			{
				ReplayData replayData = ReplayDataSystem.LoadReplayData($"{DataPath}\\Replay\\{SCLENAME}_Data.RPY");
				STGManager.ReplaySystem.ActionDatas = replayData.StageReplayDatas[0].ActionDatas["default"].ToArray();
			}

			if (Input.GetKeyDown(KeyCode.F8))
			{
				StageReplayData data = new("0", 0)
				{
					ActionDatas = new()
					{
						{ "default", STGManager.ReplaySystem.ActionDatas.ToList() }
					}
				};

				ReplayData ReplayData = new()
				{
					Name = "System",
					User = "Default",
					SaveTime = DateTime.Now,
					Player = 0,
					Livel = 1,
					Score = int.MaxValue,
					Power = 4,
					Point = 10000,
					Graze = 5000,
					EndStage = 2,
					StartLife = 4,
					StartBomb = 2,
					EndLife = 2,
					EndBomb = 3,
					GetSpellCardCount = 0,
					DeathCount = 0,
					StageReplayDatas = new StageReplayData[1]
				};
				ReplayData.StageReplayDatas[0] = data;

				ReplayDataSystem.SaveReplay($"{DataPath}\\Replay\\{SCLENAME}_Data.RPY", ReplayData);
			}
		}

		public override void OnUpdate()
		{
			SlowEffectL.transform.Rotate(new Vector3(0, 0, RD) * Time.fixedDeltaTime, Space.Self);
			SlowEffectR.transform.Rotate(new Vector3(0, 0, -RD) * Time.fixedDeltaTime, Space.Self);

			Speed= HighSpeed;

			if (IsSolt)
			{
				Speed = SoltSpeed;
			}

			if (IsShoot)
			{
				Shoot();
			}

			AxisMove(MoveVectorX, MoveVectorY);

			base.OnUpdate();
		}

		public override void KeyDown(bool[] keys)
		{
			if (!STGManager.IsRunning)
			{
				return;
			}

			float x = 0;
			float y = 0;

			if (keys[0])
			{
				y = 1;
			}
			else if (keys[1])
			{
				y = -1;
			}
			else
			{
				y = 0;
			}

			if (keys[2])
			{
				x = -1;
			}
			else if (keys[3])
			{
				x = 1;
			}
			else
			{
				x = 0;
			}

			if (keys[4])
			{
				IsShoot = true;
			}
			else
			{
				IsShoot = false;
			}

			if (keys[6])
			{
				IsSolt = true;
			}
			else
			{
				IsSolt = false;
			}

			MoveVectorX = x;
			MoveVectorY = y;
		}

		public void AxisMove(float x, float y)
		{
			Vector2 Vector = new(x, y);

			if (x < 0)
			{
				Animator.SetBool("LI", true);
			}
			else
			{
				Animator.SetBool("LI", false);
			}

			if (x > 0)
			{
				Animator.SetBool("RI", true);
			}
			else
			{
				Animator.SetBool("RI", false);
			}

			if ((TransformPosition.y >= STGManager.PlayerMaxPosition.y && Vector.y > 0) || (TransformPosition.y <= -STGManager.PlayerMaxPosition.y && Vector.y < 0))
			{
				Vector.y = 0;
			}
			if ((TransformPosition.x >= STGManager.PlayerMaxPosition.x && Vector.x > 0) || (TransformPosition.x <= -STGManager.PlayerMaxPosition.x && Vector.x < 0))
			{
				Vector.x = 0;
			}

			MoveVector = Vector;
		}

		public virtual void Shoot()
		{

		}
	}
}
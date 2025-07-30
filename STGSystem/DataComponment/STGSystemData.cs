﻿using System;
using UnityEngine;

namespace NagaisoraFramework.STGSystem
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

	[Serializable]
	public struct PlayerInfo
	{
		public string Name;

		public GameObject Prefabricate;
		public Vector2 DetermineOffset;
		public float AngleOffsetCompensation;
		public float DetermineRadius;
		public float Normoal_Speed;
		public float Low_Speed;
		public PlayerBulletInfo[] PlayerBullet;
	}

	[Serializable]
	public struct PlayerBulletInfo
	{
		public int Type;
		public Sprite Sprite;
		public Vector2 Normoal_Size;
		public Vector2 DetermineOffset;
		public float AngleOffsetCompensation;
		public float DetermineRadius;
	}

	[Serializable]
	public struct EnemyInfo
	{
		public int Type;
		public Vector2 Normoal_Size;
		public Vector2 DetermineOffset;
		public float AngleOffsetCompensation;
		public float DetermineRadius;
		public EnemyObject[] Info;
	}

	[Serializable]
	public struct EnemyBulletInfo
	{
		public int Type;
		public Vector2 Normoal_Size;
		public Vector2 DetermineOffset;
		public float AngleOffsetCompensation;
		public float DetermineRadius;
		public BulletObject[] Info;
	}

	[Serializable]
	public struct EnemyLongLaserInfo
	{
		public Vector2 Normoal_Size;
		public float DetermineRadius;
		public BulletObject[] Info;
	}

	[Serializable]
	public struct BulletObject
	{
		public int Color;
		public Sprite Sprite;
	}

	[Serializable]
	public struct EnemyObject
	{
		public int Color;
		public RuntimeAnimatorController AnimatorController;
	}

}
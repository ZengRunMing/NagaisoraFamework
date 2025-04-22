using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;


namespace NagaisoraFamework
{
	using Miedia;
	using STGSystem;
	using DataFileSystem;

	public static class MainSystem
	{
		public static string FameWorkVersion;
		public static string SCLENAME;

		public static string DataPath = GetDataPath();

		public static STGManager GolbalSTGManager;

		public static PoolManager GolbalPoolManager;

		public static ReplaySystem GolbalReplaySystem;

		public static BGMControl BGMControl;
		public static AudioControl AudioControl;
		public static MidiControl MidiControl;
		
		public static SEManager SEManager;
		public static InputModule InputModule;
		public static STGSystemData STGSystemData;

		public static ScoreData ScoreData;
		public static ConfigData ConfigData;
		public static WaveBGMData[] WaveBGMData;
		public static MidiBGMData[] MidiBGMData;

		public static int GameModer;

		public static int Rank;
		public static int Player;

		public static int PlayerLength;
		public static int RankLength;

		public static float Music_Volume;
		public static float SE_Volume;

		public static int FPS;

		public static float[] zsin;
		public static float[] zcos;
		public static System.Random r1;

		public static int gameseed;

		public static byte[] key;

		public static long Gametime;
		public static long FixedGameTime;
		public static DateTime SystemTime = DateTime.Now;
		public static DateTime StartTime;
		public static TimeSpan RunTime;
		public static TimeSpan TotalRunTime;
		public static TimeSpan PlayerTime;

		public static float GolbalDeltaTime;
		public static float GolbalFixedDeltaTime;

		public delegate void SelectControl(int STMA);

		public delegate void KeyDownEvent(bool[] bools);
		public static event KeyDownEvent KeyDown;

		public static event SelectControl SelMove;

		public static event EventHandler OnApplicationQuit;

		public static float Fps;

		static MainSystem()
		{
			GolbalPoolManager = new PoolManager();

			FameWorkVersion = "1.0.0 Beta";
			SCLENAME = "THSSD";

			gameseed = UnityEngine.Random.Range(0, 20000000);
			r1 = new System.Random(gameseed);

			zsin = new float[9000];
			zcos = new float[9000];

			for (int i = 0; i < 9000; i++)
			{
				zsin[i] = Mathf.Sin(i * 0.04f * ((float)Math.PI / 180f));
				zcos[i] = Mathf.Cos(i * 0.04f * ((float)Math.PI / 180f));
			}

			key = new byte[8] { 2, 7, 8, 6, 7, 2, 5, 5 };
		}

		public static void CallKeyDown(ushort keys)
		{
			BitArray bitArray = new(BitConverter.GetBytes(keys));

			bool[] bools = new bool[bitArray.Count];
			bitArray.CopyTo(bools, 0);

			CallKeyDown(bools);
		}

		public static void CallKeyDown(bool[] keys)
		{
			if (KeyDown == null)
			{
				return;
			}

			KeyDown(keys);
		}

		public static void CallSelMove(int i)
		{
			SelMove?.Invoke(i);
		}

		public static string Readtxt(string[] strs, int linenumber)
		{
			if (linenumber == 0 || linenumber > strs.Length)
			{
				return string.Empty;
			}
			return strs[linenumber].Trim();
		}


		public static string GetDataPath()
		{
			return Path.GetDirectoryName(Application.dataPath);
		}

		public static bool DataPathFileExists(string path)
		{
			if (File.Exists($"{DataPath}\\{path}"))
			{
				return true;
			}
			return false;
		}

		public static bool DataPathDirectoryExists(string path)
		{
			if (Directory.Exists($"{DataPath}\\{path}"))
			{
				return true;
			}
			return false;
		}

		#region Convert

		public static string IntToString(int i)
		{
			return i.ToString();
		}

		public static string FloatToString(float i)
		{
			return i.ToString();
		}

		public static float StringToFloat(string str)
		{
			return float.Parse(str);
		}

		public static int StringToInt(string str)
		{
			return int.Parse(str);
		}

		#endregion

		#region EulerAnglesMath

		public static float Dsin(float In)
		{
			return zsin[(int)(In * 25f)];
		}

		public static float Dcos(float In)
		{
			return zcos[(int)(In * 25f)];
		}

		public static float Distance(Transform a, Transform b)
		{
			float num = Mathf.Pow((a.position.x - b.position.x) * (a.position.x - b.position.x) + (a.position.y - b.position.y) * (a.position.y - b.position.y), 0.5f);
			if (num < 0f)
			{
				num = 0f - num;
			}
			return num;
		}

		public static float EulerAngles_ADS(float a)
		{
			a %= 360f;
			if (a < 0f)
			{
				a += 360f;
			}
			if (a == 360f)
			{
				a = 0f;
			}
			return a;
		}

		public static float EulerAngles_ADS2(float a)
		{
			a %= 360f;
			Debug.Log(a);
			if (a < 0f)
			{
				a += 360f;
			}
			if (a >= 180f)
			{
				a -= 360f;
			}
			if (a == 180f)
			{
				a = -180f;
			}
			return a;
		}

		#endregion

		#region Distances
		public static float Distance(Vector2 p1, Vector2 p2)
		{
			return Mathf.Sqrt((p1.x - p2.x) * (p1.x - p2.x) + (p1.y - p2.y) * (p1.y - p2.y));
		}

		public static float DistanceLine(Vector2 a, Vector2 b, Vector2 c)
		{
			Vector2 vector = b - a;
			Vector2 rhs = c - a;
			float num = Vector2.Dot(vector, rhs);
			if (num < 0f)
			{
				return Distance(c, a);
			}
			float num2 = Vector2.Dot(vector, vector);
			if (num > num2)
			{
				return Distance(c, b);
			}
			num /= num2;
			Vector2 p = a + num * vector;
			return Distance(c, p);
		}

		public static Vector2 Point2lineVerticalPointPosition(Vector2 m, Vector2 a, Vector2 b)
		{
			Vector2 vector = b - a;
			Vector2 rhs = m - a;
			float num = Vector2.Dot(vector, rhs);
			float num2 = Vector2.Dot(vector, vector);
			num /= num2;
			return a + num * vector;
		}
		#endregion

		public static float Seedrandom(float a, float b)
		{
			return (float)r1.NextDouble() * (b - a) + a;
		}

		public static float Seedrandom2(float a)
		{
			if (a == 0f)
			{
				return 0f;
			}
			return ((float)r1.NextDouble() * 2f - 1f) * a;
		}

		public static int RandomInt()
		{
			return r1.Next();
		}

		public static float RandomFloat()
		{
			return (float)r1.NextDouble();
		}

		public static byte RandomByte()
		{
			return (byte)r1.Next(byte.MinValue, byte.MaxValue);
		}

		public static int RandomInt(int min, int max)
		{
			return r1.Next(min, max);
		}

		public static int RandomFloat(int min, int max)
		{
			return r1.Next(min, max);
		}

		public static byte RandomByte(int min, int max)
		{
			return (byte)r1.Next(min, max);
		}

		#region Logic

		public static bool IF(float a, float orz, float b)
		{
			bool result = false;
			if (orz == 0f)
			{
				result = a >= b;
			}
			if (orz == 1f)
			{
				result = Mathf.Abs(a - b) <= 0.07f;
			}
			if (orz == 2f)
			{
				result = a < b;
			}
			return result;
		}

		public static float DO(float a, int orz, float b)
		{
			if (orz == 0)
			{
				a += b;
			}
			if (orz == 1)
			{
				a = b;
			}
			if (orz == 2)
			{
				a -= b;
			}
			return a;
		}

		public static bool OR(bool[] i, bool value)
		{
			foreach (bool rds in i)
			{
				if (rds == value)
				{
					return true;
				}
			}
			return false;
		}

		public static bool AND(bool[] i, bool value)
		{
			foreach (bool rds in i)
			{
				if (rds != value)
				{
					return false;
				}
			}
			return true;
		}

		#endregion

		public static float DO_time(float a, int orz, float b, int fangshi, int time_now, float time_fen, float fendu = 0f)
		{
			if (orz != 2)
			{
				if (fangshi <= 0)
				{
					a += fendu;
				}
				if (fangshi == 1)
				{
					a += (Mathf.Sin((float)(time_now + 1) * time_fen * ((float)Math.PI / 180f)) - Mathf.Sin((float)time_now * time_fen * ((float)Math.PI / 180f))) * b;
				}
				if (fangshi == 2)
				{
					a += 0f - (Mathf.Cos((float)(time_now + 1) * time_fen * ((float)Math.PI / 180f)) - Mathf.Cos((float)time_now * time_fen * ((float)Math.PI / 180f))) * b;
				}
			}
			if (orz == 2)
			{
				if (fangshi <= 0)
				{
					a -= fendu;
				}
				if (fangshi == 1)
				{
					a -= (Mathf.Sin((float)(time_now + 1) * time_fen * ((float)Math.PI / 180f)) - Mathf.Sin((float)time_now * time_fen * ((float)Math.PI / 180f))) * b;
				}
				if (fangshi == 2)
				{
					a += (Mathf.Cos((float)(time_now + 1) * time_fen * ((float)Math.PI / 180f)) - Mathf.Cos((float)time_now * time_fen * ((float)Math.PI / 180f))) * b;
				}
			}
			return a;
		}

		#region Scale

		public static float Scale(float IN, float IN_Min, float IN_Max, float OUT_Min, float OUT_Max)
		{
			float Std = IN - IN_Min / IN_Max - IN_Min;
			float SAD = Std * (OUT_Max - OUT_Min) + OUT_Min;
			return SAD;
		}

		public static int Scale(int IN, int IN_Min, int IN_Max, int OUT_Min, int OUT_Max)
		{
			int Std = IN - IN_Min / IN_Max - IN_Min;
			int SAD = Std * (OUT_Max - OUT_Min) + OUT_Min;
			return SAD;
		}

		public static long Scale(long IN, long IN_Min, long IN_Max, long OUT_Min, long OUT_Max)
		{
			long Std = IN - IN_Min / IN_Max - IN_Min;
			long SAD = Std * (OUT_Max - OUT_Min) + OUT_Min;
			return SAD;
		}

		public static double Scale(double IN, double IN_Min, double IN_Max, double OUT_Min, double OUT_Max)
		{
			double Std = IN - IN_Min / IN_Max - IN_Min;
			double SAD = Std * (OUT_Max - OUT_Min) + OUT_Min;
			return SAD;
		}

		#endregion

		public static void SetTimeScale(float i)
		{
			Time.timeScale = i;
		}

		public static void SetAudioSourceScale(AudioSource source, float i)
		{
			source.pitch = i;
		}

		public static void Quit()
		{
			OnApplicationQuit?.Invoke(null, null);

			if (ScoreData != null)
			{
				ScoreData.TotalRunTime = TotalRunTime;
				ScoreDataSystem.ScoreDataSave(ScoreData, null);
			}

			#if UNITY_EDITOR
				UnityEditor.EditorApplication.isPlaying = false;
			#else
				Application.Quit();
			#endif
		}

		public static EventTriggerListener AddEventTriggerListener(GameObject Object)
		{
			return EventTriggerListener.Get(Object);
		}

		public static IEnumerator MoveToPosition(Transform transform, Vector3 position, float Speed)
		{
			while (transform.localPosition != position)
			{
				transform.localPosition = Vector3.MoveTowards(transform.localPosition, position, Speed);
				yield return 0;
			}

			yield break;
		}
	}
}
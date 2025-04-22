using System;
using System.Text;
using System.IO;
using dn = System.Diagnostics;
using System.Collections.Generic;

using UnityEngine;

namespace NagaisoraFamework
{
	using Cryptography;
	using VirtualMemorySystem;

	using static MainSystem;

	[Serializable]
	public class RASMActuator
	{

		public Dictionary<string, Action> Tack = new Dictionary<string, Action>();

		public Executable executable;

		BinaryReader EBR;
		MemoryManagerSystem MBR;
		long Lim = 0;
		dn.Stopwatch sw;
		public bool IsRunning;
		public GameObject BaseGameObject;

		public long Index = 0;

		public long NowTimeQueue;

		public long NowTime;

		public Stack<long> JumpIndex;

		//构造指令执行系统函数
		public RASMActuator(byte[] Input)
		{
			if (Input == null || Input.Length == 0)
			{
				return;
			}

			JumpIndex = new Stack<long>();

			//指令表初期化
			#region 程序基本
			Tack.Add(BitConverter.ToString(new byte[4] { 41, 12, 19, 87 }), COUT);
			Tack.Add(BitConverter.ToString(new byte[4] { 45, 21, 15, 48 }), SETVALUE);
			Tack.Add(BitConverter.ToString(new byte[4] { 44, 51, 31, 81 }), JUMP);
			Tack.Add(BitConverter.ToString(new byte[4] { 44, 51, 31, 82 }), CALL);
			Tack.Add(BitConverter.ToString(new byte[4] { 46, 48, 16, 14 }), RETURN);
			Tack.Add(BitConverter.ToString(new byte[4] { 46, 48, 15, 31 }), WAIT);
			Tack.Add(BitConverter.ToString(new byte[4] { 46, 18, 86, 16 }), END);
			#endregion

			#region 变量定义
			Tack.Add(BitConverter.ToString(new byte[4] { 47, 25, 61, 84 }), NEWINT);
			Tack.Add(BitConverter.ToString(new byte[4] { 47, 55, 54, 21 }), NEWLONG);
			Tack.Add(BitConverter.ToString(new byte[4] { 47, 56, 15, 48 }), NEWFLOAT);
			Tack.Add(BitConverter.ToString(new byte[4] { 47, 58, 152, 155 }), NEWFDOUBLE);
			Tack.Add(BitConverter.ToString(new byte[4] { 47, 57, 52, 14 }), NEWBOOL);
			Tack.Add(BitConverter.ToString(new byte[4] { 47, 65, 84, 25 }), NEWSTRING);
			Tack.Add(BitConverter.ToString(new byte[4] { 47, 57, 23, 15 }), NEWCHAR);
			Tack.Add(BitConverter.ToString(new byte[4] { 47, 20, 21, 54 }), NEWTIME);
			Tack.Add(BitConverter.ToString(new byte[4] { 47, 57, 15, 42 }), NEWVECTOR2);
			Tack.Add(BitConverter.ToString(new byte[4] { 47, 69, 85, 21 }), NEWVECTOR3);
			Tack.Add(BitConverter.ToString(new byte[4] { 47, 60, 14, 32 }), NEWVECTOR6);
			#endregion

			#region 数学运算
			Tack.Add(BitConverter.ToString(new byte[4] { 43, 80, 20, 62 }), ADD);
			Tack.Add(BitConverter.ToString(new byte[4] { 43, 89, 74, 12 }), SUB);
			Tack.Add(BitConverter.ToString(new byte[4] { 43, 54, 40, 1 }), MUL);
			Tack.Add(BitConverter.ToString(new byte[4] { 43, 50, 14, 75 }), DIV);
			Tack.Add(BitConverter.ToString(new byte[4] { 43, 59, 70, 14 }), INC);
			Tack.Add(BitConverter.ToString(new byte[4] { 43, 65, 84, 71 }), DEC);
			Tack.Add(BitConverter.ToString(new byte[4] { 43, 51, 42, 255 }), SIN);
			Tack.Add(BitConverter.ToString(new byte[4] { 43, 95, 84, 15 }), COS);
			Tack.Add(BitConverter.ToString(new byte[4] { 43, 94, 250, 14 }), TAN);
			Tack.Add(BitConverter.ToString(new byte[4] { 43, 255, 255, 150 }), ASIN);
			Tack.Add(BitConverter.ToString(new byte[4] { 43, 240, 23, 145 }), ACOS);
			Tack.Add(BitConverter.ToString(new byte[4] { 43, 252, 253, 250 }), ATAN);
			Tack.Add(BitConverter.ToString(new byte[4] { 43, 15, 74, 31 }), ATAN2);
			#endregion

			#region 系统调用
			Tack.Add(BitConverter.ToString(new byte[4] { 48, 57, 26, 34 }), DSIN);
			Tack.Add(BitConverter.ToString(new byte[4] { 48, 52, 14, 65 }), DCOS);

			Tack.Add(BitConverter.ToString(new byte[4] { 43, 10, 21, 1 }), RND);
			Tack.Add(BitConverter.ToString(new byte[4] { 43, 10, 18, 41 }), RNDA);
			Tack.Add(BitConverter.ToString(new byte[4] { 43, 10, 91, 45 }), RNDC);
			Tack.Add(BitConverter.ToString(new byte[4] { 43, 10, 84, 51 }), RNDAC);
			#endregion

			executable = Executable_Load(Input);
		}

		public Executable Executable_Load(byte[] I)
		{
			MemoryStream memoryStream = new MemoryStream();
			memoryStream.Write(I, 0x00, I.Length);

			BinaryReader BR = new BinaryReader(memoryStream, System.Text.Encoding.UTF8);
			BR.BaseStream.Seek(0x00, SeekOrigin.Begin);

			string Hider = Encoding.UTF8.GetString(BR.ReadBytes(3));

			if (Hider != "SCE")
			{
				throw new InvalidDataException($"Executable Load -> 文件标识头错误");
			}

			string MD5S = BitConverter.ToString(BR.ReadBytes(16));
			string NAME = BR.ReadString();
			string TYPE = BR.ReadString();
			long MEMORYLength = BR.ReadInt64();
			long PRTOKENLength = BR.ReadInt64();
			long PROGRAMLength = BR.ReadInt64();

			MapDictionary<string, Token> IMM = new MapDictionary<string, Token>();

			byte[] vf = BR.ReadBytes(Convert.ToInt32(MEMORYLength));
			byte[] vt = BR.ReadBytes(Convert.ToInt32(PRTOKENLength));
			byte[] vs = BR.ReadBytes(Convert.ToInt32(PROGRAMLength));

			MemoryStream memory = new MemoryStream();
			memory.Write(vf, 0x00, vf.Length);
			memory.Write(vt, 0x00, vt.Length);
			memory.Write(vs, 0x00, vs.Length);

			if (MD5S != BitConverter.ToString(MD5.MD5Encrypt16Byte(memory.ToArray())))
			{
				memory.Close();
				throw new InvalidDataException($"Executable Load -> MD5不匹配");
			}

			memory.Close();
			return new Executable(NAME, TYPE, IMM, vf, vt, vs);
		}

		public void RUN(GameObject gameObject)
		{
			if (IsRunning)
			{
				return;
			}

			BaseGameObject = gameObject;

			MBR = new MemoryManagerSystem(executable.Memory);

			MemoryStream memoryStream = new MemoryStream();
			memoryStream.Write(executable.Program, 0x00, executable.Program.Length);
			EBR = new BinaryReader(memoryStream, Encoding.UTF8);

			Lim = 0;

			if (!executable.PrToken.Forward.ContainsKey("MAIN"))
			{
				//EBR.BaseStream.Seek(0x00, SeekOrigin.Begin);
				ShowLog(LSLogType.Error, executable, Lim, "没有MAIN入口, 程序无法运行");
				return;
			}
			else
			{
				long ptr = executable.PrToken.Forward["MAIN"];
				EBR.BaseStream.Seek(ptr, SeekOrigin.Begin);
				Index = executable.CmToken.Reverse[ptr];
			}

			sw = new dn.Stopwatch();

			sw.Start();

			IsRunning = true;
		}

		public void Time_CLK(long now_time)
		{
			NowTime = now_time;

			if (!IsRunning)
			{
				return;
			}

			if (now_time <= NowTimeQueue)
			{
				return;
			}

			EBR.BaseStream.Seek(executable.CmToken.Forward[Index], SeekOrigin.Begin);
			Index++;

			//Debug.Log("Command Postion " + EBR.BaseStream.Position);

			int ASYNC = EBR.ReadByte();

			Lim = EBR.BaseStream.Position;

			byte[] vm = EBR.ReadBytes(4);

			string vs = BitConverter.ToString(vm);

			//Debug.Log(vm[0] + " " + vm[1] + " " + vm[2] + " " + vm[3] + " | " + vs);

			if (!Tack.ContainsKey(vs))
			{
				ShowLog(LSLogType.Error, executable, Lim, "未知的指令, 不予执行");
			}
			
			Tack[vs]();

			return;
		}

		#region 程序基本方法蔟
		public void COUT()
		{
			NRtype Mess = Getvic(executable, MBR, EBR, 5);

			Debug.Log(executable.Name + " -> " + Mess.Value);
		}
		public void SETVALUE()
		{
			int type = EBR.ReadByte();

			NRtype a = Getvic(executable, MBR, EBR, type);
			NRtype b = Getvic(executable, MBR, EBR, type);

			if (a.Type == b.Type)
			{
				a.Value = b.Value;
			}

			MBR.WriteObject(executable.token.Forward[a.Name].Position, a.Value, executable.token.Forward[a.Name].Type);
		}
		public void JUMP()
		{
			string STM = (string)Getvic(executable, MBR, EBR, IMMType.String).Value;
			long ptr = executable.PrToken.Forward[STM];
			Index = executable.CmToken.Reverse[ptr];
		}

		public void CALL()
		{
			JumpIndex.Push(Index);

			string STM = (string)Getvic(executable, MBR, EBR, IMMType.String).Value;
			long ptr = executable.PrToken.Forward[STM];
			Index = executable.CmToken.Reverse[ptr];
		}

		public void RETURN()
		{
			Index = JumpIndex.Pop();
		}
		public void WAIT()
		{
			NowTimeQueue = NowTime + (long)Getvic(executable, MBR, EBR, IMMType.Long).Value;
		}

		public void END()
		{
			IsRunning = false;
			sw.Stop();
			TimeSpan ts = sw.Elapsed;
			string Mess =
			"程序 " + executable.Name + " 执行完成, 从 0x" + EBR.BaseStream.Position.ToString("X8") + " 处退出程序\n" +
			"耗时 " + ts.TotalMilliseconds + " ms";

			ShowLog(LSLogType.Default, executable, Lim, Mess);
		}
		#endregion

		#region 变量定义方法蔟
		public void NEWINT()
		{
			EBR.ReadByte();
			string Name = EBR.ReadString();
			long Postion = MBR.WritePostion;

			executable.token.Add(Name, new Token(MBR.WriteEnd, 0));
			MBR.WriteInt(MBR.WriteEnd, 0);

			ShowLog(LSLogType.Info, executable, Lim, string.Format("INT类型变量 {0} 已创建, 地址 0x{1}", Name, Postion.ToString("X8")));
		}
		public void NEWLONG()
		{
			EBR.ReadByte();
			string Name = EBR.ReadString();
			long Postion = MBR.WritePostion;

			executable.token.Add(Name, new Token(MBR.WriteEnd, 1));
			MBR.WriteLong(MBR.WriteEnd, 0);

			ShowLog(LSLogType.Info, executable, Lim, string.Format("LONG类型变量 {0} 已创建, 地址 0x{1}", Name, Postion.ToString("X8")));
		}
		public void NEWFLOAT()
		{
			EBR.ReadByte();
			string Name = EBR.ReadString();
			long Postion = MBR.WritePostion;

			executable.token.Add(Name, new Token(MBR.WriteEnd, 2));
			MBR.WriteFloat(MBR.WriteEnd, 0);

			ShowLog(LSLogType.Info, executable, Lim, string.Format("FLOAT类型变量 {0} 已创建, 地址 0x{1}", Name, Postion.ToString("X8")));
		}
		public void NEWFDOUBLE()
		{
			EBR.ReadByte();
			string Name = EBR.ReadString();
			long Postion = MBR.WritePostion;

			executable.token.Add(Name, new Token(MBR.WriteEnd, 3));
			MBR.WriteDouble(MBR.WriteEnd, 0);

			ShowLog(LSLogType.Info, executable, Lim, string.Format("DOUBLE类型变量 {0} 已创建, 地址 0x{1}", Name, Postion.ToString("X8")));
		}
		public void NEWBOOL()
		{
			EBR.ReadByte();
			string Name = EBR.ReadString();
			long Postion = MBR.WritePostion;

			executable.token.Add(Name, new Token(MBR.WriteEnd, 4));
			MBR.WriteBool(MBR.WriteEnd, false);

			ShowLog(LSLogType.Info, executable, Lim, string.Format("BOOL类型变量 {0} 已创建, 地址 0x{1}", Name, Postion.ToString("X8")));
		}
		public void NEWSTRING()
		{
			EBR.ReadByte();
			string Name = EBR.ReadString();
			long Postion = MBR.WritePostion;

			executable.token.Add(Name, new Token(MBR.WriteEnd, 5));
			MBR.WriteString(MBR.WriteEnd, "");

			ShowLog(LSLogType.Info, executable, Lim, string.Format("STRING类型变量 {0} 已创建, 地址 0x{1}", Name, Postion.ToString("X8")));
		}
		public void NEWCHAR()
		{
			EBR.ReadByte();
			string Name = EBR.ReadString();
			long Postion = MBR.WritePostion;

			executable.token.Add(Name, new Token(MBR.WriteEnd, 6));
			MBR.WriteChar(MBR.WriteEnd, ' ');

			ShowLog(LSLogType.Info, executable, Lim, string.Format("CHAR类型变量 {0} 已创建, 地址 0x{1}", Name, Postion.ToString("X8")));
		}
		public void NEWTIME()
		{
			EBR.ReadByte();
			string Name = EBR.ReadString();
			long Postion = MBR.WritePostion;

			executable.token.Add(Name, new Token(MBR.WriteEnd, 7));
			MBR.WriteDateTime(MBR.WriteEnd, new DateTime(0, 0, 0, 0, 0, 0, 0));

			ShowLog(LSLogType.Info, executable, Lim, string.Format("TIME类型变量 {0} 已创建, 地址 0x{1}", Name, Postion.ToString("X8")));
		}
		public void NEWVECTOR2()
		{
			EBR.ReadByte();
			string Name = EBR.ReadString();
			long Postion = MBR.WritePostion;

			executable.token.Add(Name, new Token(MBR.WriteEnd, 8));
			MBR.WriteVector2(MBR.WriteEnd, new Vector2(0, 0));

			ShowLog(LSLogType.Info, executable, Lim, string.Format("VECTOR2类型变量 {0} 已创建, 地址 0x{1}", Name, Postion.ToString("X8")));
		}
		public void NEWVECTOR3()
		{
			EBR.ReadByte();
			string Name = EBR.ReadString();
			long Postion = MBR.WritePostion;

			executable.token.Add(Name, new Token(MBR.WriteEnd, 9));
			MBR.WriteVector3(MBR.WriteEnd, new Vector3(0, 0, 0));

			ShowLog(LSLogType.Info, executable, Lim, string.Format("VECTOR3类型变量 {0} 已创建, 地址 0x{1}", Name, Postion.ToString("X8")));
		}
		public void NEWVECTOR6()
		{
			EBR.ReadByte();
			string Name = EBR.ReadString();
			long Postion = MBR.WritePostion;

			executable.token.Add(Name, new Token(MBR.WriteEnd, 10));
			MBR.WriteVector6(MBR.WriteEnd, new Vector6(0, 0, 0, 0, 0, 0));

			ShowLog(LSLogType.Info, executable, Lim, string.Format("VECTOR6类型变量 {0} 已创建, 地址 0x{1}", Name, Postion.ToString("X8")));
		}
		#endregion

		#region 数学运算方法蔟
		public void ADD()
		{
			int type = EBR.ReadByte();
			NRtype a = Getvic(executable, MBR, EBR, type);
			NRtype b = Getvic(executable, MBR, EBR, type);
			NRtype c = Getvic(executable, MBR, EBR, type);

			c.Value = a + b;

			MBR.WriteObject(executable.token.Forward[c.Name].Position, c.Value, executable.token.Forward[c.Name].Type);
		}
		public void SUB()
		{
			int type = EBR.ReadByte();
			NRtype a = Getvic(executable, MBR, EBR, type);
			NRtype b = Getvic(executable, MBR, EBR, type);
			NRtype c = Getvic(executable, MBR, EBR, type);

			c.Value = a - b;

			MBR.WriteObject(executable.token.Forward[c.Name].Position, c.Value, executable.token.Forward[c.Name].Type);
		}
		public void MUL()
		{
			int type = EBR.ReadByte();
			NRtype a = Getvic(executable, MBR, EBR, type);
			NRtype b = Getvic(executable, MBR, EBR, type);
			NRtype c = Getvic(executable, MBR, EBR, type);

			c.Value = a * b;

			MBR.WriteObject(executable.token.Forward[c.Name].Position, c.Value, executable.token.Forward[c.Name].Type);
		}
		public void DIV()
		{
			int type = EBR.ReadByte();
			NRtype a = Getvic(executable, MBR, EBR, type);
			NRtype b = Getvic(executable, MBR, EBR, type);
			NRtype c = Getvic(executable, MBR, EBR, type);

			c.Value = a / b;

			MBR.WriteObject(executable.token.Forward[c.Name].Position, c.Value, executable.token.Forward[c.Name].Type);
		}
		public void INC()
		{
			int type = EBR.ReadByte();
			NRtype a = Getvic(executable, MBR, EBR, type);

			a++;

			MBR.WriteObject(executable.token.Forward[a.Name].Position, a.Value, executable.token.Forward[a.Name].Type);
		}
		public void DEC()
		{
			int type = EBR.ReadByte();
			NRtype a = Getvic(executable, MBR, EBR, type);

			a--;

			MBR.WriteObject(executable.token.Forward[a.Name].Position, a.Value, executable.token.Forward[a.Name].Type);
		}
		public void SIN()
		{
			NRtype a = Getvic(executable, MBR, EBR, IMMType.Float);
			NRtype b = Getvic(executable, MBR, EBR, IMMType.Float);

			b.Value = Mathf.Sin((float)a.Value);

			MBR.WriteObject(executable.token.Forward[b.Name].Position, b.Value, executable.token.Forward[b.Name].Type);
		}
		public void COS()
		{
			NRtype a = Getvic(executable, MBR, EBR, IMMType.Float);
			NRtype b = Getvic(executable, MBR, EBR, IMMType.Float);

			b.Value = Mathf.Cos((float)a.Value);

			MBR.WriteObject(executable.token.Forward[b.Name].Position, b.Value, executable.token.Forward[b.Name].Type);
		}
		public void TAN()
		{
			NRtype a = Getvic(executable, MBR, EBR, IMMType.Float);
			NRtype b = Getvic(executable, MBR, EBR, IMMType.Float);

			b.Value = Mathf.Tan((float)a.Value);

			MBR.WriteObject(executable.token.Forward[b.Name].Position, b.Value, executable.token.Forward[b.Name].Type);
		}
		public void ASIN()
		{
			NRtype a = Getvic(executable, MBR, EBR, IMMType.Float);
			NRtype b = Getvic(executable, MBR, EBR, IMMType.Float);

			b.Value = Mathf.Asin((float)a.Value);

			MBR.WriteObject(executable.token.Forward[b.Name].Position, b.Value, executable.token.Forward[b.Name].Type);
		}
		public void ACOS()
		{
			NRtype a = Getvic(executable, MBR, EBR, IMMType.Float);
			NRtype b = Getvic(executable, MBR, EBR, IMMType.Float);

			b.Value = Mathf.Acos((float)a.Value);

			MBR.WriteObject(executable.token.Forward[b.Name].Position, b.Value, executable.token.Forward[b.Name].Type);
		}
		public void ATAN()
		{
			NRtype a = Getvic(executable, MBR, EBR, IMMType.Float);
			NRtype b = Getvic(executable, MBR, EBR, IMMType.Float);

			b.Value = Mathf.Atan((float)a.Value);

			MBR.WriteObject(executable.token.Forward[b.Name].Position, b.Value, executable.token.Forward[b.Name].Type);
		}
		public void ATAN2()
		{
			NRtype a = Getvic(executable, MBR, EBR, IMMType.Float);
			NRtype b = Getvic(executable, MBR, EBR, IMMType.Float);
			NRtype c = Getvic(executable, MBR, EBR, IMMType.Float);

			c.Value = Mathf.Atan2((float)a.Value, (float)b.Value);

			MBR.WriteObject(executable.token.Forward[c.Name].Position, c.Value, executable.token.Forward[c.Name].Type);
		}
		#endregion

		#region 系统调用方法蔟
		public void DSIN()
		{
			NRtype a = Getvic(executable, MBR, EBR, IMMType.Float);
			NRtype b = Getvic(executable, MBR, EBR, IMMType.Float);

			b.Value = Dsin((float)a.Value);

			MBR.WriteObject(executable.token.Forward[b.Name].Position, b.Value, executable.token.Forward[b.Name].Type);
		}
		public void DCOS()
		{
			NRtype a = Getvic(executable, MBR, EBR, IMMType.Float);
			NRtype b = Getvic(executable, MBR, EBR, IMMType.Float);

			b.Value = Dcos((float)a.Value);

			MBR.WriteObject(executable.token.Forward[b.Name].Position, b.Value, executable.token.Forward[b.Name].Type);
		}
		public void RND()
		{
			int type = EBR.ReadByte();
			NRtype a = Getvic(executable, MBR, EBR, type);

			if (a.Type == 0)
			{
				a.Value = RandomInt();
			}
			else if (a.Type == 2)
			{
				a.Value = RandomFloat();
			}

			MBR.WriteObject(executable.token.Forward[a.Name].Position, a.Value, executable.token.Forward[a.Name].Type);
		}

		public void RNDC()
		{
			int type = EBR.ReadByte();
			NRtype a = Getvic(executable, MBR, EBR, type);
			NRtype b = Getvic(executable, MBR, EBR, type);
			NRtype c = Getvic(executable, MBR, EBR, type);

			if (c.Type == 0)
			{
				a.Value = RandomInt((int)a.Value, (int)b.Value);
			}
			else if (a.Type == 2)
			{
				Reset:
				float s = RandomFloat();
				if (s < (float)a.Value || s > (float)b.Value)
				{
					goto Reset;
				}

				c.Value = s;
			}

			MBR.WriteObject(executable.token.Forward[a.Name].Position, a.Value, executable.token.Forward[a.Name].Type);
		}

		public void RNDA()
		{
			int type = EBR.ReadByte();
			NRtype a = Getvic(executable, MBR, EBR, type);

			if (a.Type == 0)
			{
				a.Value = (int)a.Value + RandomInt();
			}
			else if (a.Type == 2)
			{
				a.Value = (float)a.Value + RandomFloat();
			}

			MBR.WriteObject(executable.token.Forward[a.Name].Position, a.Value, executable.token.Forward[a.Name].Type);
		}

		public void RNDAC()
		{
			int type = EBR.ReadByte();
			NRtype a = Getvic(executable, MBR, EBR, type);
			NRtype b = Getvic(executable, MBR, EBR, type);
			NRtype c = Getvic(executable, MBR, EBR, type);

			if (c.Type == 0)
			{
				c.Value = (int)c.Value + RandomInt((int)a.Value, (int)b.Value);
			}
			else if (c.Type == 2)
			{
				Reset:
				float s = RandomFloat();
				if (s < (float)a.Value || s > (float)b.Value)
				{
					goto Reset;
				}

				c.Value = s;
			}

			MBR.WriteObject(executable.token.Forward[c.Name].Position, c.Value, executable.token.Forward[c.Name].Type);
		}
		#endregion

		public void ShowLog(LSLogType logType, Executable executable, long Lim, string Info)
		{
			switch (logType)
			{
				case LSLogType.Info:
					Debug.Log("程序 " + executable.Name + " 位于 0x" + Lim.ToString("X8") + " -> " + Info);
					break;
				case LSLogType.Warning:
					Debug.LogWarning("程序 " + executable.Name + " 位于 0x" + Lim.ToString("X8") + " -> " + Info);
					break;
				case LSLogType.Error:
					Debug.LogError("程序 " + executable.Name + " 位于 0x" + Lim.ToString("X8") + " -> " + Info);
					break;
				case LSLogType.Default:
					Debug.Log(Info);
					break;
			}
		}

		public NRtype Getvic(Executable executable, MemoryManagerSystem MBR, BinaryReader EBR, IMMType type)
		{
			byte ISMBR = EBR.ReadByte();

			string PublicTokenName = "";

			object obj;

			if (ISMBR == 0)
			{
				switch (type)
				{
					case IMMType.Int:
						obj = EBR.ReadInt32();
						break;
					case IMMType.Long:
						obj = EBR.ReadInt64();
						break;
					case IMMType.Float:
						obj = EBR.ReadSingle();
						break;
					case IMMType.Double:
						obj = EBR.ReadDouble();
						break;
					case IMMType.Bool:
						obj = EBR.ReadBoolean();
						break;
					case IMMType.String:
						obj = EBR.ReadString();
						break;
					case IMMType.Char:
						obj = EBR.ReadChar();
						break;
					case IMMType.Time:
						DateTime dateTime = new DateTime();

						dateTime.AddYears(EBR.ReadInt32());
						dateTime.AddMonths(EBR.ReadInt32());
						dateTime.AddDays(EBR.ReadInt32());
						dateTime.AddHours(EBR.ReadInt32());
						dateTime.AddMinutes(EBR.ReadInt32());
						dateTime.AddSeconds(EBR.ReadInt32());
						dateTime.AddMilliseconds(EBR.ReadInt32());

						obj = dateTime;
						break;
					case IMMType.Vector2:
						obj = new Vector2(EBR.ReadSingle(), EBR.ReadSingle());
						break;
					case IMMType.Vector3:
						obj = new Vector3(EBR.ReadSingle(), EBR.ReadSingle(), EBR.ReadSingle());
						break;
					case IMMType.Vector6:
						obj = new Vector6(EBR.ReadSingle(), EBR.ReadSingle(), EBR.ReadSingle(), EBR.ReadSingle(), EBR.ReadSingle(), EBR.ReadSingle());
						break;
					default:
						obj = null;
						break;
				}
			}
			else
			{
				PublicTokenName = EBR.ReadString();
				long postion = executable.token.Forward[PublicTokenName].Position;

				switch (type)
				{
					case IMMType.Int:
						obj = MBR.ReadInt(postion);
						break;
					case IMMType.Long:
						obj = MBR.ReadLong(postion);
						break;
					case IMMType.Float:
						obj = MBR.ReadFloat(postion);
						break;
					case IMMType.Double:
						obj = MBR.ReadDouble(postion);
						break;
					case IMMType.Bool:
						obj = MBR.ReadBool(postion);
						break;
					case IMMType.String:
						obj = MBR.ReadString(postion);
						break;
					case IMMType.Char:
						obj = MBR.ReadChar(postion);
						break;
					case IMMType.Time:
						DateTime dateTime = new DateTime();

						dateTime.AddYears(MBR.ReadInt(postion));
						dateTime.AddMonths(MBR.ReadInt(postion));
						dateTime.AddDays(MBR.ReadInt(postion));
						dateTime.AddHours(MBR.ReadInt(postion));
						dateTime.AddMinutes(MBR.ReadInt(postion));
						dateTime.AddSeconds(MBR.ReadInt(postion));
						dateTime.AddMilliseconds(MBR.ReadInt(postion));

						obj = dateTime;
						break;
					case IMMType.Vector2:
						obj = MBR.ReadVector2(postion);
						break;
					case IMMType.Vector3:
						obj = MBR.ReadVector3(postion);
						break;
					case IMMType.Vector6:
						obj = MBR.ReadVector6(postion);
						break;
					default:
						obj = null;
						break;
				}
			}
			return new NRtype(obj, (int)type, PublicTokenName);
		}

		public NRtype Getvic(Executable executable, MemoryManagerSystem MBR, BinaryReader EBR, int type)
		{
			byte ISMBR = EBR.ReadByte();

			string PublicTokenName = "";

			object obj;

			if (ISMBR == 0)
			{
				switch (type)
				{
					case 0:
						obj = EBR.ReadInt32();
						break;
					case 1:
						obj = EBR.ReadInt64();
						break;
					case 2:
						obj = EBR.ReadSingle();
						break;
					case 3:
						obj = EBR.ReadDouble();
						break;
					case 4:
						obj = EBR.ReadBoolean();
						break;
					case 5:
						obj = EBR.ReadString();
						break;
					case 6:
						obj = EBR.ReadChar();
						break;
					case 7:
						DateTime dateTime =	new DateTime();

						dateTime.AddYears(EBR.ReadInt32());
						dateTime.AddMonths(EBR.ReadInt32());
						dateTime.AddDays(EBR.ReadInt32());
						dateTime.AddHours(EBR.ReadInt32());
						dateTime.AddMinutes(EBR.ReadInt32());
						dateTime.AddSeconds(EBR.ReadInt32());
						dateTime.AddMilliseconds(EBR.ReadInt32());

						obj = dateTime;
						break;
					case 8:
						obj = new Vector2(EBR.ReadSingle(), EBR.ReadSingle());
						break;
					case 9:
						obj = new Vector3(EBR.ReadSingle(), EBR.ReadSingle(), EBR.ReadSingle());
						break;
					case 10:
						obj = new Vector6(EBR.ReadSingle(), EBR.ReadSingle(), EBR.ReadSingle(), EBR.ReadSingle(), EBR.ReadSingle(), EBR.ReadSingle());
						break;
					default:
						obj = null;
						break;
				}
			}
			else
			{
				PublicTokenName = EBR.ReadString();
				int tokentype = executable.token.Forward[PublicTokenName].Type;
				long postion = executable.token.Forward[PublicTokenName].Position;

				if (tokentype == type)
				{
					switch (type)
					{
						case 0:
							obj = MBR.ReadInt(postion);
							break;
						case 1:
							obj = MBR.ReadLong(postion);
							break;
						case 2:
							obj = MBR.ReadFloat(postion);
							break;
						case 3:
							obj = MBR.ReadDouble(postion);
							break;
						case 4:
							obj = MBR.ReadBool(postion);
							break;
						case 5:
							obj = MBR.ReadString(postion);
							break;
						case 6:
							obj = MBR.ReadChar(postion);
							break;
						case 7:
							DateTime dateTime = new DateTime();

							dateTime.AddYears(MBR.ReadInt(postion++));
							dateTime.AddMonths(MBR.ReadInt(postion++));
							dateTime.AddDays(MBR.ReadInt(postion++));
							dateTime.AddHours(MBR.ReadInt(postion++));
							dateTime.AddMinutes(MBR.ReadInt(postion++));
							dateTime.AddSeconds(MBR.ReadInt(postion++));
							dateTime.AddMilliseconds(MBR.ReadInt(postion++));

							obj = dateTime;
							break;
						case 8:
							obj = MBR.ReadVector2(postion);
							break;
						case 9:
							obj = MBR.ReadVector3(postion);
							break;
						case 10:
							obj = MBR.ReadVector6(postion);
							break;
						default:
							obj = null;
							break;
					}
				}
				else
				{
					object am;

					switch (tokentype)
					{
						case 0:
							am = MBR.ReadInt(postion);
							break;
						case 1:
							am = MBR.ReadLong(postion);
							break;
						case 2:
							am = MBR.ReadFloat(postion);
							break;
						case 3:
							am = MBR.ReadDouble(postion);
							break;
						case 4:
							am = MBR.ReadBool(postion);
							break;
						case 5:
							am = MBR.ReadString(postion);
							break;
						case 6:
							am = MBR.ReadChar(postion);
							break;
						case 7:
							DateTime dateTime = new DateTime();

							dateTime.AddYears(MBR.ReadInt(postion++));
							dateTime.AddMonths(MBR.ReadInt(postion++));
							dateTime.AddDays(MBR.ReadInt(postion++));
							dateTime.AddHours(MBR.ReadInt(postion++));
							dateTime.AddMinutes(MBR.ReadInt(postion++));
							dateTime.AddSeconds(MBR.ReadInt(postion++));
							dateTime.AddMilliseconds(MBR.ReadInt(postion++));

							am = dateTime;
							break;
						case 8:
							am = MBR.ReadVector2(postion);
							break;
						case 9:
							am = MBR.ReadVector3(postion);
							break;
						case 10:
							am = MBR.ReadVector6(postion);
							break;
						default:
							am = null;
							break;
					}

					switch (type)
					{
						case 0:
							obj = Convert.ToInt32(am);
							break;
						case 1:
							obj = Convert.ToInt64(am);
							break;
						case 2:
							obj = Convert.ToSingle(am);
							break;
						case 3:
							obj = Convert.ToDouble(am);
							break;
						case 4:
							obj = Convert.ToBoolean(am);
							break;
						case 5:
							obj = Convert.ToString(am);
							break;
						case 6:
							obj = Convert.ToChar(am);
							break;
						case 7:
							obj = Convert.ToDateTime(am);
							break;
						case 8:
							if (am.GetType().ToString().ToLower() != "Vector2")
							{
								obj = null;
							}
							else
							{
								obj = am;
							}
							break;
						case 9:
							if (am.GetType().ToString().ToLower() != "Vector3")
							{
								obj = null;
							}
							else
							{
								obj = am;
							}
							break;
						case 10:
							if (am.GetType().ToString().ToLower() != "Vector6")
							{
								obj = null;
							}
							else
							{
								obj = am;
							}
							break;
						default:
							obj = null;
							break;
					}
				}
			}
			return new NRtype(obj, type, PublicTokenName);
		}
	}
}
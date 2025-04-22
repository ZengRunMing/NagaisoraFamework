using UnityEngine;
using System.IO;
using System;

namespace NagaisoraFamework.VirtualMemorySystem
{
	public class MemoryManagerSystem
	{
		MemoryStream MemoryStream;
		BinaryWriter BinaryWriter;
		BinaryReader BinaryReader;

		public long ReadPostion;
		public long ReadEnd;

		public long WritePostion;
		public long WriteEnd;

		public MemoryManagerSystem(byte[] Memory)
		{
			MemoryStream = new MemoryStream(999999999);
			MemoryStream.Write(Memory, 0x00, Memory.Length);
			BinaryWriter = new BinaryWriter(MemoryStream);
			BinaryReader = new BinaryReader(MemoryStream);
		}

		#region Read
		public short ReadShort(long Postion)
		{
			BinaryReader.BaseStream.Seek(Postion, SeekOrigin.Begin);
			short a = BinaryReader.ReadInt16();
			UpdateWritePostion(BinaryReader.BaseStream.Position);
			return a;
		}

		public ushort ReadUShort(long Postion)
		{
			BinaryReader.BaseStream.Seek(Postion, SeekOrigin.Begin);
			ushort a = BinaryReader.ReadUInt16();
			UpdateWritePostion(BinaryReader.BaseStream.Position);
			return a;
		}

		public int ReadInt(long Postion)
		{
			BinaryReader.BaseStream.Seek(Postion, SeekOrigin.Begin);
			int a = BinaryReader.ReadInt32();
			UpdateWritePostion(BinaryReader.BaseStream.Position);
			return a;
		}

		public uint ReadUInt(long Postion)
		{
			BinaryReader.BaseStream.Seek(Postion, SeekOrigin.Begin);
			uint a = BinaryReader.ReadUInt32();
			UpdateWritePostion(BinaryReader.BaseStream.Position);
			return a;
		}

		public long ReadLong(long Postion)
		{
			BinaryReader.BaseStream.Seek(Postion, SeekOrigin.Begin);
			long a = BinaryReader.ReadInt64();
			UpdateWritePostion(BinaryReader.BaseStream.Position);
			return a;
		}

		public ulong ReadULong(long Postion)
		{
			BinaryReader.BaseStream.Seek(Postion, SeekOrigin.Begin);
			ulong a = BinaryReader.ReadUInt64();
			UpdateWritePostion(BinaryReader.BaseStream.Position);
			return a;
		}

		public double ReadDouble(long Postion)
		{
			BinaryReader.BaseStream.Seek(Postion, SeekOrigin.Begin);
			double a = BinaryReader.ReadDouble();
			UpdateWritePostion(BinaryReader.BaseStream.Position);
			return a;
		}

		public float ReadFloat(long Postion)
		{
			BinaryReader.BaseStream.Seek(Postion, SeekOrigin.Begin);
			float a = BinaryReader.ReadSingle();
			UpdateWritePostion(BinaryReader.BaseStream.Position);
			return a;
		}

		public char ReadChar(long Postion)
		{
			BinaryReader.BaseStream.Seek(Postion, SeekOrigin.Begin);
			char a = BinaryReader.ReadChar();
			UpdateWritePostion(BinaryReader.BaseStream.Position);
			return a;
		}

		public char[] ReadChars(long Postion, int Size)
		{
			BinaryReader.BaseStream.Seek(Postion, SeekOrigin.Begin);
			char[] a = BinaryReader.ReadChars(Size);
			UpdateWritePostion(BinaryReader.BaseStream.Position);
			return a;
		}

		public string ReadString(long Postion)
		{
			BinaryReader.BaseStream.Seek(Postion, SeekOrigin.Begin);
			string a = BinaryReader.ReadString();
			UpdateWritePostion(BinaryReader.BaseStream.Position);
			return a;
		}

		public bool ReadBool(long Postion)
		{
			BinaryReader.BaseStream.Seek(Postion, SeekOrigin.Begin);
			bool a = BinaryReader.ReadBoolean();
			UpdateWritePostion(BinaryReader.BaseStream.Position);
			return a;
		}

		public byte ReadByte(long Postion)
		{
			BinaryReader.BaseStream.Seek(Postion, SeekOrigin.Begin);
			byte a = BinaryReader.ReadByte();
			UpdateWritePostion(BinaryReader.BaseStream.Position);
			return a;
		}

		public byte[] ReadBytes(long Postion, int Size)
		{
			BinaryReader.BaseStream.Seek(Postion, SeekOrigin.Begin);
			byte[] a = BinaryReader.ReadBytes(Size);
			UpdateWritePostion(BinaryReader.BaseStream.Position);
			return a;
		}

		public sbyte ReadSbyte(long Postion)
		{
			BinaryReader.BaseStream.Seek(Postion, SeekOrigin.Begin);
			sbyte a = BinaryReader.ReadSByte();
			UpdateWritePostion(BinaryReader.BaseStream.Position);
			return a;
		}

		public decimal ReadDecimal(long Postion)
		{
			BinaryReader.BaseStream.Seek(Postion, SeekOrigin.Begin);
			decimal a = BinaryReader.ReadDecimal();
			UpdateWritePostion(BinaryReader.BaseStream.Position);
			return a;
		}

		public Vector2 ReadVector2(long Postion)
		{
			BinaryReader.BaseStream.Seek(Postion, SeekOrigin.Begin);
			Vector2 a = new Vector2(BinaryReader.ReadSingle(), BinaryReader.ReadSingle());
			UpdateWritePostion(BinaryReader.BaseStream.Position);
			return a;
		}

		public Vector2Int ReadVector2Int(long Postion)
		{
			BinaryReader.BaseStream.Seek(Postion, SeekOrigin.Begin);
			Vector2Int a = new Vector2Int(BinaryReader.ReadInt32(), BinaryReader.ReadInt32());
			UpdateWritePostion(BinaryReader.BaseStream.Position);
			return a;
		}

		public Vector3 ReadVector3(long Postion)
		{
			BinaryReader.BaseStream.Seek(Postion, SeekOrigin.Begin);
			Vector3 a = new Vector3(BinaryReader.ReadSingle(), BinaryReader.ReadSingle(), BinaryReader.ReadSingle());
			UpdateWritePostion(BinaryReader.BaseStream.Position);
			return a;
		}

		public Vector3Int ReadVector3Int(long Postion)
		{
			BinaryReader.BaseStream.Seek(Postion, SeekOrigin.Begin);
			Vector3Int a = new Vector3Int(BinaryReader.ReadInt32(), BinaryReader.ReadInt32(), BinaryReader.ReadInt32());
			UpdateWritePostion(BinaryReader.BaseStream.Position);
			return a;
		}

		public Vector4 ReadVector4(long Postion)
		{
			BinaryReader.BaseStream.Seek(Postion, SeekOrigin.Begin);
			Vector4 a = new Vector4(BinaryReader.ReadSingle(), BinaryReader.ReadSingle(), BinaryReader.ReadSingle(), BinaryReader.ReadSingle());
			UpdateWritePostion(BinaryReader.BaseStream.Position);
			return a;
		}

		public Vector6 ReadVector6(long Postion)
		{
			BinaryReader.BaseStream.Seek(Postion, SeekOrigin.Begin);
			Vector6 a = new Vector6(BinaryReader.ReadSingle(), BinaryReader.ReadSingle(), BinaryReader.ReadSingle(), BinaryReader.ReadSingle(), BinaryReader.ReadSingle(), BinaryReader.ReadSingle()); ;
			UpdateWritePostion(BinaryReader.BaseStream.Position);
			return a;
		}

		public void UpdateReadPostion(long Postion)
		{
			ReadPostion = Postion;
			ReadEnd = BinaryReader.BaseStream.Length;
		}
		#endregion

		#region Write

		public void WriteObject(long Postion, object Value, int type)
		{
			BinaryWriter.BaseStream.Seek(Postion, SeekOrigin.Begin);

			switch (type)
			{
				case 0:
					BinaryWriter.Write((int)Value);
					break;
				case 1:
					BinaryWriter.Write((long)Value);
					break;
				case 2:
					BinaryWriter.Write((float)Value);
					break;
				case 3:
					BinaryWriter.Write((double)Value);
					break;
				case 4:
					BinaryWriter.Write((bool)Value);
					break;
				case 5:
					BinaryWriter.Write((string)Value);
					break;
				case 6:
					BinaryWriter.Write((char)Value);
					break;
				case 7:
					DateTime dateTime = (DateTime)Value;

					BinaryWriter.Write(dateTime.Year);
					BinaryWriter.Write(dateTime.Month);
					BinaryWriter.Write(dateTime.Day);
					BinaryWriter.Write(dateTime.Hour);
					BinaryWriter.Write(dateTime.Minute);
					BinaryWriter.Write(dateTime.Second);
					BinaryWriter.Write(dateTime.Millisecond);
					break;
				case 8:
					Vector2 vector2 = (Vector2)Value;

					BinaryWriter.Write(vector2.x);
					BinaryWriter.Write(vector2.y);
					break;
				case 9:
					Vector3 vector3 = (Vector3)Value;

					BinaryWriter.Write(vector3.x);
					BinaryWriter.Write(vector3.y);
					BinaryWriter.Write(vector3.z);
					break;
				case 10:
					Vector6 vector6 = (Vector6)Value;

					BinaryWriter.Write(vector6.x);
					BinaryWriter.Write(vector6.y);
					BinaryWriter.Write(vector6.z);
					BinaryWriter.Write(vector6.a);
					BinaryWriter.Write(vector6.b);
					BinaryWriter.Write(vector6.c);
					break;
				default:
					break;
			}

			UpdateWritePostion(BinaryWriter.BaseStream.Position);
		}

		public void WriteShort(long Postion, short Value)
		{
			BinaryWriter.BaseStream.Seek(Postion, SeekOrigin.Begin);
			BinaryWriter.Write(Value);
			UpdateWritePostion(BinaryWriter.BaseStream.Position);
		}

		public void WriteUShort(long Postion, ushort Value)
		{
			BinaryWriter.BaseStream.Seek(Postion, SeekOrigin.Begin);
			BinaryWriter.Write(Value);
			UpdateWritePostion(BinaryWriter.BaseStream.Position);
		}

		public void WriteInt(long Postion, int Value)
		{
			BinaryWriter.BaseStream.Seek(Postion, SeekOrigin.Begin);
			BinaryWriter.Write(Value);
			UpdateWritePostion(BinaryWriter.BaseStream.Position);
		}

		public void WriteUInt(long Postion, uint Value)
		{
			BinaryWriter.BaseStream.Seek(Postion, SeekOrigin.Begin);
			BinaryWriter.Write(Value);
			UpdateWritePostion(BinaryWriter.BaseStream.Position);
		}

		public void WriteLong(long Postion, long Value)
		{
			BinaryWriter.BaseStream.Seek(Postion, SeekOrigin.Begin);
			BinaryWriter.Write(Value);
			UpdateWritePostion(BinaryWriter.BaseStream.Position);
		}

		public void WriteULong(long Postion, ulong Value)
		{
			BinaryWriter.BaseStream.Seek(Postion, SeekOrigin.Begin);
			BinaryWriter.Write(Value);
			UpdateWritePostion(BinaryWriter.BaseStream.Position);
		}

		public void WriteDouble(long Postion, double Value)
		{
			BinaryWriter.BaseStream.Seek(Postion, SeekOrigin.Begin);
			BinaryWriter.Write(Value);
			UpdateWritePostion(BinaryWriter.BaseStream.Position);
		}

		public void WriteFloat(long Postion, float Value)
		{
			BinaryWriter.BaseStream.Seek(Postion, SeekOrigin.Begin);
			BinaryWriter.Write(Value);
			UpdateWritePostion(BinaryWriter.BaseStream.Position);
		}

		public void WriteChar(long Postion, char Value)
		{
			BinaryWriter.BaseStream.Seek(Postion, SeekOrigin.Begin);
			BinaryWriter.Write(Value);
			UpdateWritePostion(BinaryWriter.BaseStream.Position);
		}

		public void WriteChars(long Postion, char[] Value)
		{
			BinaryWriter.BaseStream.Seek(Postion, SeekOrigin.Begin);
			BinaryWriter.Write(Value);
			UpdateWritePostion(BinaryWriter.BaseStream.Position);
		}

		public void WriteString(long Postion, string Value)
		{
			BinaryWriter.BaseStream.Seek(Postion, SeekOrigin.Begin);
			BinaryWriter.Write(Value);
			UpdateWritePostion(BinaryWriter.BaseStream.Position);
		}

		public void WriteBool(long Postion, bool Value)
		{
			BinaryWriter.BaseStream.Seek(Postion, SeekOrigin.Begin);
			BinaryWriter.Write(Value);
			UpdateWritePostion(BinaryWriter.BaseStream.Position);
		}

		public void WriteDateTime(long Postion, DateTime Value)
		{
			BinaryWriter.BaseStream.Seek(Postion, SeekOrigin.Begin);
			BinaryWriter.Write(Value.Year);
			BinaryWriter.Write(Value.Month);
			BinaryWriter.Write(Value.Day);
			BinaryWriter.Write(Value.Hour);
			BinaryWriter.Write(Value.Minute);
			BinaryWriter.Write(Value.Second);
			BinaryWriter.Write(Value.Millisecond);
			UpdateWritePostion(BinaryWriter.BaseStream.Position);
		}

		public void WriteByte(long Postion, byte Value)
		{
			BinaryWriter.BaseStream.Seek(Postion, SeekOrigin.Begin);
			BinaryWriter.Write(Value);
			UpdateWritePostion(BinaryWriter.BaseStream.Position);
		}

		public void WriteBytes(long Postion, byte[] Value)
		{
			BinaryWriter.BaseStream.Seek(Postion, SeekOrigin.Begin);
			BinaryWriter.Write(Value);
			UpdateWritePostion(BinaryWriter.BaseStream.Position);
		}

		public void WriteSByte(long Postion, sbyte Value)
		{
			BinaryWriter.BaseStream.Seek(Postion, SeekOrigin.Begin);
			BinaryWriter.Write(Value);
			UpdateWritePostion(BinaryWriter.BaseStream.Position);
		}

		public void WriteDecimal(long Postion, decimal Value)
		{
			BinaryWriter.BaseStream.Seek(Postion, SeekOrigin.Begin);
			BinaryWriter.Write(Value);
			UpdateWritePostion(BinaryWriter.BaseStream.Position);
		}

		public void WriteVector2(long Postion, Vector2 Value)
		{
			BinaryWriter.BaseStream.Seek(Postion, SeekOrigin.Begin);
			BinaryWriter.Write(Value.x);
			BinaryWriter.Write(Value.y);
			UpdateWritePostion(BinaryWriter.BaseStream.Position);
		}

		public void WriteVector2Int(long Postion, Vector2Int Value)
		{
			BinaryWriter.BaseStream.Seek(Postion, SeekOrigin.Begin);
			BinaryWriter.Write(Value.x);
			BinaryWriter.Write(Value.y);
			UpdateWritePostion(BinaryWriter.BaseStream.Position);
		}

		public void WriteVector3(long Postion, Vector3 Value)
		{
			BinaryWriter.BaseStream.Seek(Postion, SeekOrigin.Begin);
			BinaryWriter.Write(Value.x);
			BinaryWriter.Write(Value.y);
			BinaryWriter.Write(Value.z);
			UpdateWritePostion(BinaryWriter.BaseStream.Position);
		}

		public void WriteVector3Int(long Postion, Vector3Int Value)
		{
			BinaryWriter.BaseStream.Seek(Postion, SeekOrigin.Begin);
			BinaryWriter.Write(Value.x);
			BinaryWriter.Write(Value.y);
			BinaryWriter.Write(Value.z);
			UpdateWritePostion(BinaryWriter.BaseStream.Position);
		}

		public void WriteVector6(long Postion, Vector6 Value)
		{
			BinaryWriter.BaseStream.Seek(Postion, SeekOrigin.Begin);
			BinaryWriter.Write(Value.x);
			BinaryWriter.Write(Value.y);
			BinaryWriter.Write(Value.z);
			BinaryWriter.Write(Value.a);
			BinaryWriter.Write(Value.b);
			BinaryWriter.Write(Value.c);
			UpdateWritePostion(BinaryWriter.BaseStream.Position);
		}

		public void UpdateWritePostion(long Postion)
		{
			WritePostion = Postion;
			WriteEnd = BinaryWriter.BaseStream.Length;
		}

		#endregion
	}
}
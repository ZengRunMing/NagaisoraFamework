using System;
using System.IO;
using System.Text;

using UnityEngine;

namespace NagaisoraFamework
{
	[Serializable]
	public struct KeyConfig
	{
		public KeyCode Up;
		public KeyCode Down;
		public KeyCode Left;
		public KeyCode Right;
		public KeyCode[] SubmitKeys;
		public KeyCode[] CancelKeys;
		public KeyCode ESC;
		public KeyCode R;
		public KeyCode Slow;
		public KeyCode Change;
		public KeyCode Ctrl;
		public KeyCode J_SubmitKey;
		public KeyCode J_CancelKey;
		public KeyCode J_ESC;
		public KeyCode J_Slow;
		public KeyCode J_Change;
		public KeyCode J_Ctrl;


		public static KeyConfig Default = new()
		{
			Up = KeyCode.UpArrow,
			Down = KeyCode.DownArrow,
			Left = KeyCode.LeftArrow,
			Right = KeyCode.RightArrow,

			SubmitKeys = new KeyCode[] { KeyCode.Z, KeyCode.Return },
			J_SubmitKey = KeyCode.JoystickButton0,

			CancelKeys = new KeyCode[] { KeyCode.X, KeyCode.Escape },
			J_CancelKey = KeyCode.JoystickButton6,

			ESC = KeyCode.Escape,
			J_ESC = KeyCode.JoystickButton1,

			R = KeyCode.R,

			Slow = KeyCode.LeftShift,
			J_Slow = KeyCode.JoystickButton7,

			Change = KeyCode.C,
			J_Change = KeyCode.JoystickButton3,

			Ctrl = KeyCode.LeftControl,
			J_Ctrl = KeyCode.JoystickButton2,
		};

		public byte[] ToBinary()
		{
			MemoryStream stream = new();
			BinaryWriter writer = new(stream, Encoding.UTF8, true);

			writer.Write((ushort)Up);
			writer.Write((ushort)Down);
			writer.Write((ushort)Left);
			writer.Write((ushort)Right);

			writer.Write((ushort)R);

			writer.Write((ushort)ESC);
			writer.Write((ushort)J_ESC);

			writer.Write((ushort)Slow);
			writer.Write((ushort)J_Slow);

			writer.Write((ushort)Change);
			writer.Write((ushort)J_Change);

			writer.Write((ushort)Ctrl);
			writer.Write((ushort)J_Ctrl);

			writer.Write((byte)SubmitKeys.Length);
			foreach (KeyCode key in SubmitKeys)
			{
				writer.Write((ushort)key);
			}

			writer.Write((ushort)J_SubmitKey);

			writer.Write((ushort)CancelKeys.Length);
			foreach (KeyCode key in CancelKeys)
			{
				writer.Write((ushort)key);
			}

			writer.Write((ushort)J_CancelKey);

			writer.Close();

			return stream.ToArray();
		}

		public static KeyConfig FromBinary(byte[] binary)
		{
			MemoryStream stream = new(binary);
			BinaryReader binaryReader = new BinaryReader(stream, Encoding.UTF8, false);

			KeyConfig keyconfig = new()
			{
				Up = (KeyCode)binaryReader.ReadUInt16(),
				Down = (KeyCode)binaryReader.ReadUInt16(),
				Left = (KeyCode)binaryReader.ReadUInt16(),
				Right = (KeyCode)binaryReader.ReadUInt16(),

				R = (KeyCode)binaryReader.ReadUInt16(),

				ESC = (KeyCode)binaryReader.ReadUInt16(),
				J_ESC = (KeyCode)binaryReader.ReadUInt16(),

				Slow = (KeyCode)binaryReader.ReadUInt16(),
				J_Slow = (KeyCode)binaryReader.ReadUInt16(),

				Change = (KeyCode)binaryReader.ReadUInt16(),
				J_Change = (KeyCode)binaryReader.ReadUInt16(),

				Ctrl = (KeyCode)binaryReader.ReadUInt16(),
				J_Ctrl = (KeyCode)binaryReader.ReadUInt16(),
			};

			byte submitkeycount = binaryReader.ReadByte();
			keyconfig.SubmitKeys = new KeyCode[submitkeycount];
			for (int a = 0; a < submitkeycount; a++)
			{
				keyconfig.SubmitKeys[a] = (KeyCode)binaryReader.ReadUInt16();
			}
			keyconfig.J_SubmitKey = (KeyCode)binaryReader.ReadUInt16();

			byte cancelkeycount = binaryReader.ReadByte();
			keyconfig.CancelKeys = new KeyCode[cancelkeycount];
			for (int a = 0; a < cancelkeycount; a++)
			{
				keyconfig.CancelKeys[a] = (KeyCode)binaryReader.ReadUInt16();
			}
			keyconfig.J_CancelKey = (KeyCode)binaryReader.ReadUInt16();

			binaryReader.Close();
			stream.Close();

			return keyconfig;
		}
	}

}

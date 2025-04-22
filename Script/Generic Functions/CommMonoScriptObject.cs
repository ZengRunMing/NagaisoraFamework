using System;
using UnityEngine;

namespace NagaisoraFamework
{
	using Cryptography;

	public class CommMonoScriptObject : MonoBehaviour
	{
		public Guid GUID = Guid.NewGuid();

		public string GUIDMD5String => BitConverter.ToString(GetGUIDMD5()).Replace("-", "").ToUpper();

		public byte[] GetGUIDMD5()
		{
			return MD5.MD5Encrypt16Byte(GUID.ToByteArray());
		}
	}
}

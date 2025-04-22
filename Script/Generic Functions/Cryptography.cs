using System.Security.Cryptography;
using System.Text;
using System.IO;
using System;
using System.Collections.Generic;

namespace NagaisoraFamework.Cryptography
{
	public struct EnigmaObject
	{
		public List<int> L0;
		public List<int> L1;

		public EnigmaObject(List<int> l0, List<int> l1)
		{
			L0 = l0;
			L1 = l1;
		}
	}

    public static class MD5
    {
        public static byte[] MD5Encrypt16Byte(string IN)
        {
            var md5 = new MD5CryptoServiceProvider();
            return md5.ComputeHash(Encoding.UTF8.GetBytes(IN));
        }

        public static byte[] MD5Encrypt16Byte(byte[] IN)
        {
            var md5 = new MD5CryptoServiceProvider();
            return md5.ComputeHash(IN);
        }
    }

	public class RSA : IDisposable
	{
		public RSACryptoServiceProvider rsa;

		public string PublicKey;
		public string PrivateKey;

		public RSA(int keylength)
		{
			rsa = new RSACryptoServiceProvider();
			RSACreateKey(keylength);
		}

		public void RSACreateKey(int keyLength)
		{
			PublicKey = rsa.ToXmlString(false);
			PrivateKey = rsa.ToXmlString(true);
		}

		public byte[] RSAEncrypt(byte[] data, string publickey = null)
		{
			if (publickey == null || publickey == "")
			{
				publickey = PublicKey;
			}

			rsa.FromXmlString(publickey);
			byte[] cipherBytes = rsa.Encrypt(data, false);

			return cipherBytes;
		}

		public byte[] RSADecrypt(byte[] data, string privatekey = null)
		{
			if (privatekey == null || privatekey == "")
			{
				privatekey = PrivateKey;
			}

			rsa.FromXmlString(privatekey);
			byte[] cipherBytes = rsa.Decrypt(data, false);

			return cipherBytes;
		}

		public byte[] RSAEncryptEX(byte[] data, string publickey = null)
		{
			if (data == null)
			{
				return null;
			}

			int length = data.Length / 117;
			int remain = data.Length % 117;

			MemoryStream memory0 = new MemoryStream(data);
			MemoryStream memory1 = new MemoryStream();

			for (int i = 0; i < length; i++)
			{
				byte[] d = new byte[117];

				memory0.Read(d, 0, d.Length);

				byte[] Out = RSAEncrypt(d, publickey);

				memory1.Write(Out, 0, Out.Length);
			}

			if (remain > 0)
			{
				byte[] rd = new byte[remain];

				memory0.Read(rd, 0, rd.Length);

				byte[] ROut = RSAEncrypt(rd, publickey);

				memory1.Write(ROut, 0, ROut.Length);
			}

			memory0.Close();

			return memory1.ToArray();
		}

		public byte[] RSADecryptEX(byte[] data, string privatekey = null)
		{
			if (data == null)
			{
				return null;
			}

			int length = data.Length / (117 + 11);
			int remain = data.Length % (117 + 11);

			MemoryStream memory0 = new MemoryStream(data);
			MemoryStream memory1 = new MemoryStream();

			for (int i = 0; i < length; i++)
			{
				byte[] d = new byte[117 + 11];

				memory0.Read(d, 0, d.Length);

				byte[] Out = RSADecrypt(d, privatekey);

				memory1.Write(Out, 0, Out.Length);
			}

			if (remain > 0)
			{
				byte[] rd = new byte[remain];

				memory0.Read(rd, 0, rd.Length);

				byte[] ROut = RSADecrypt(rd, privatekey);

				memory1.Write(ROut, 0, ROut.Length);
			}

			memory0.Close();

			return memory1.ToArray();
		}

		public void Dispose()
		{
			rsa.Dispose();
		}
	}

	public static class DES
	{
		public static string DESEncrypt(string str, byte[] key)
		{
			return Convert.ToBase64String(DESEncrypt(Encoding.UTF8.GetBytes(str), key));
		}

		public static byte[] DESEncrypt(byte[] data, byte[] key)
		{
			DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider();
			MemoryStream memoryStream = new MemoryStream();
			CryptoStream cryptoStream = new CryptoStream(memoryStream, dESCryptoServiceProvider.CreateEncryptor(key, key), CryptoStreamMode.Write);
			cryptoStream.Write(data, 0, data.Length);
			cryptoStream.FlushFinalBlock();
			return memoryStream.ToArray();
		}

		public static string DESDecrypt(string str, byte[] key)
		{
			return Encoding.UTF8.GetString(DESDecrypt(Convert.FromBase64String(str), key));
		}

		public static byte[] DESDecrypt(byte[] data, byte[] key)
		{
			DESCryptoServiceProvider dESCryptoServiceProvider = new DESCryptoServiceProvider();
			MemoryStream memoryStream = new MemoryStream();
			CryptoStream cryptoStream = new CryptoStream(memoryStream, dESCryptoServiceProvider.CreateDecryptor(key, key), CryptoStreamMode.Write);
			cryptoStream.Write(data, 0, data.Length);
			cryptoStream.FlushFinalBlock();
			return memoryStream.ToArray();
		}
	}

	//----------------------------------------------------
	//恩尼格玛加密系统
	//未完工
	//----------------------------------------------------
	public class Enigma
	{
		public static List<List<EnigmaObject>> lists;

		public Enigma(int Count)
		{
			for (int i = 0; i < Count; i++)
			{
				List<EnigmaObject> objects = new List<EnigmaObject>();
				
				List<int> CL0 = new List<int>();
				List<int> CL1 = new List<int>();

				for (int c = 0; c < 255; c++)
				{
					RD0:
					int out0 = UnityEngine.Random.Range(0, 255);
					if (CL0.Contains(out0))
					{
						goto RD0;
					}
					CL0.Add(out0);

					RD1:
					int out1 = UnityEngine.Random.Range(0, 255);
					if (CL1.Contains(out1))
					{
						goto RD1;
					}
					CL1.Add(out1);
				}

				objects.Add(new EnigmaObject(CL0, CL1));
				lists.Add(objects);
			}
		}

		public byte[] Encrypt(byte[] data, params int[] keys)
		{
			if (keys.Length != lists.Count)
			{
				throw new ArgumentException("传入的密钥长度未达到或超过设定的密钥长度");
			}

			List<byte> outdata = new List<byte>();

			//int lastind = 0;

			for (int i = 0; i < data.Length; i++)
			{
				for (int j = 0; j < keys.Length; j++)
				{
					List<EnigmaObject> enigma = lists[j];

					 enigma[keys[j]].L0.IndexOf(data[i]);
				}
			}

			return outdata.ToArray();
		}
	}
}
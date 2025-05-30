﻿using System.IO;

using System.Runtime.Serialization.Formatters.Binary;

namespace NagaisoraFamework
{
	public static class SerializeSystem
	{
		public static byte[] SerializeBinary<T>(T obj)
		{
			MemoryStream stream = new MemoryStream();

			var binaryFormatter = new BinaryFormatter();
			binaryFormatter.Serialize(stream, obj);

			return stream.ToArray();
		}

		public static T DeserializeBinary<T>(byte[] binary)
		{
			MemoryStream stream = new MemoryStream(binary);

			BinaryFormatter binaryFormatter = new BinaryFormatter();
			return (T)binaryFormatter.Deserialize(stream);
		}
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NagaisoraFamework
{
	public class BGMPackDataCollection
	{
		public IBGMPackData this[int index]
		{
			get
			{
				return owners[index];
			}
			set
			{
				owners[index] = value;
			}
		}

		public int Count => owners.Count;

		public object SyncRoot => this;

		public bool IsSynchronized => false;

		List<IBGMPackData> owners;

		public BGMPackDataCollection()
		{
			owners = new List<IBGMPackData>();
		}

		public void Add(IBGMPackData item)
		{
			owners.Add(item);
		}

		public void Remove(IBGMPackData item)
		{
			if (item == null)
			{
				throw new ArgumentNullException(item.Name);
			}

			IBGMPackData[] datas = owners.ToArray();

			owners.Clear();

			foreach (IBGMPackData data in datas)
			{
				if (!data.Equals(item))
				{
					owners.Add(data);
				}
			}
		}

		public IBGMPackData[] ToArray()
		{
			return owners.ToArray();
		}

		public List<IBGMPackData> ToList()
		{
			return owners;
		}

		public BGMPackDataCollection Copy()
		{
			BGMPackDataCollection ret = new BGMPackDataCollection();

			foreach(var data in owners)
			{
				ret.Add(data.Copy());
			}

			return ret;
		}

		public IEnumerator GetEnumerator()
		{
			IBGMPackData[] histories = owners.ToArray();
			if (histories != null)
			{
				return histories.GetEnumerator();
			}

			return Array.Empty<IBGMPackData>().GetEnumerator();
		}

		public override bool Equals(object obj)
		{
			if (obj.GetType() != typeof(BGMPackDataCollection))
			{
				return false;
			}

			BGMPackDataCollection b = (BGMPackDataCollection)obj;

			if (Count != b.Count)
			{
				return false;
			}

			for (int i = 0; i < Count; i++)
			{
				if (owners[i].Name != b[i].Name || owners[i].DataPath != b[i].DataPath || owners[i].Text != b[i].Text)
				{
					return false;
				}

				if (owners[i].GetType() != b[i].GetType())
				{
					return false;
				}

				if (owners[i].StartTime != b[i].StartTime || owners[i].LoopStartTime != b[i].LoopStartTime || owners[i].LoopEndTime != b[i].LoopEndTime)
				{
					return false;
				}
			}

			return true;
		}

		public static BGMPackDataCollection FromBinary(byte[] binary)
		{
			return FromStream(new MemoryStream(binary));
		}

		public static BGMPackDataCollection FromStream(Stream stream)
		{
			var ret = new BGMPackDataCollection();

			BinaryReader reader = new BinaryReader(stream, Encoding.UTF8, false);

			string header = Encoding.UTF8.GetString(reader.ReadBytes(3));

			if (header != "BPD")
			{
				return ret;
			}

			byte type = reader.ReadByte();

			if (type == 2)
			{
				return ret;
			}

			uint count = reader.ReadUInt32();

			for (uint i = 0; i < count; i++)
			{
				IBGMPackData data = null;

				if (type == 0)
				{
					data = new WaveBGMData();
				}
				else
				{
					data = new MidiBGMData();
				}

				string Name = reader.ReadString();
				string DataPath = reader.ReadString();
				string Text = reader.ReadString();

				data.StartTime = TimeSpan.FromTicks(reader.ReadInt64());
				data.LoopStartTime = TimeSpan.FromTicks(reader.ReadInt64());
				data.LoopEndTime = TimeSpan.FromTicks(reader.ReadInt64());

				data.Name = Name;
				data.DataPath = DataPath;
				data.Text = Text;

				ret.Add(data);
			}

			reader.Close();
			stream.Close();

			return ret;
		}

		public byte[] ToBinary()
		{
			MemoryStream memoryStream = new MemoryStream();
			BinaryWriter writer = new BinaryWriter(memoryStream, Encoding.UTF8, true);

			writer.Write(Encoding.UTF8.GetBytes("BPD"));

			if (owners == null || owners.Count != 0)
			{
				if (owners[0].GetType() == typeof(WaveBGMData))
				{
					writer.Write((byte)0);
				}
				else
				{
					writer.Write((byte)1);
				}
			}
			else
			{
				writer.Write((byte)2);
			}

			writer.Write((uint)Count);

			foreach (IBGMPackData data in owners)
			{
				writer.Write(data.Name);
				writer.Write(data.DataPath);
				writer.Write(data.Text);

				writer.Write(data.StartTime.Ticks);
				writer.Write(data.LoopStartTime.Ticks);
				writer.Write(data.LoopEndTime.Ticks);
			}

			writer.Close();

			return memoryStream.ToArray();
		}

		public override int GetHashCode()
		{
			return 0;
		}
	}
}

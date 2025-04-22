using System.Collections.Generic;

using UnityEngine;

namespace NagaisoraFamework
{
	public class PoolManager
	{
		public Dictionary<int, Queue<GameObject>> Stack = new();

		public GameObject New_Object(int Type)
		{
			GameObject Object;

			if (Stack.ContainsKey(Type) && Stack[Type].Count > 0)
			{
				Object = Stack[Type].Dequeue();
			}
			else
			{
				Object = new();
			}

			return Object;
		}

		public void Delete_Object(int Type, GameObject Object)
		{
			if (Stack.ContainsKey(Type))
			{
				Stack[Type].Enqueue(Object);
			}
			else
			{
				Queue<GameObject> queue = new();

				queue.Enqueue(Object);

				Stack.Add(Type, queue);
			}
			return;
		}
	}
}
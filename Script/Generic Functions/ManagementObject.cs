using System.Collections.Generic;

namespace NagaisoraFamework
{
	public class ManagementObject
	{
		public Dictionary<string, object> Properties;

		public ManagementObject()
		{
			Properties = new Dictionary<string, object>();
		}

		public void AddProperty(string name, object value)
		{
			if (Properties.ContainsKey(name))
			{
				return;
			}

			Properties.Add(name, value);
		}

		public void RemoveProperty(string name)
		{
			if (!Properties.ContainsKey(name))
			{
				return;
			}

			Properties.Remove(name);
		}

		public object GetValue(string name)
		{
			return Properties[name];
		}
	}
}

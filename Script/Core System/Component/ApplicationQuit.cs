using UnityEngine;

namespace NagaisoraFamework
{
	using static MainSystem;

	public class ApplicationQuit : MonoBehaviour
	{
		public void Awake()
		{
			OnApplicationQuit += ApplicationQuitMethod;
		}

		public void ApplicationQuitMethod()
		{
			OnApplicationQuit -= ApplicationQuitMethod;
		}
	}
}

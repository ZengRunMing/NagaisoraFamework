using UnityEngine;

namespace NagaisoraFamework
{
	public class ObjectRotate : CommMonoScriptObject
	{
		public float Speed;

		public Vector3 RotateR;

		void Update()
		{
			base.transform.localEulerAngles += Speed * Time.deltaTime * RotateR;
		}
	}
}

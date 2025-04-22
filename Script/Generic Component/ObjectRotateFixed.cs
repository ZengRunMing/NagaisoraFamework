using UnityEngine;

namespace NagaisoraFamework
{
	public class ObjectRotateFixed : CommMonoScriptObject
	{
		public float Speed;

		public Vector3 RotateR;

		void FixedUpdate()
		{
			base.transform.localEulerAngles += Speed * Time.deltaTime * RotateR;
		}
	}
}

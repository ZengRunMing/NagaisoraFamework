using UnityEngine;

namespace NagaisoraFamework
{
	using static MainSystem;

	public class RotatePoint : CommMonoScriptObject
	{
		public GameObject PasteObject;

		public float RotateSpeed;

		public float Radius;

		public float RotateAngle;

		public Vector3 Position;

		public void FixedUpdate()
		{
			float ADS_Angle = EulerAngles_ADS(RotateAngle);

			float x = Dsin(ADS_Angle);
			float y = Dcos(ADS_Angle);

			Vector3 p3 = PasteObject.transform.localPosition;

			Position = new Vector3(p3.x + x  * Radius, p3.y + y * Radius, p3.z);

			transform.localPosition = Position;

			RotateAngle += RotateSpeed;

			if(RotateAngle == 360)
			{
				RotateAngle = 0;
			}
		}
	}
}

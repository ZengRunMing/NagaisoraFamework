using System.Collections.Generic;

using UnityEngine;

namespace NagaisoraFamework.STGSystem
{
	using static MainSystem;

	public class BulletMontage : BulletControl
	{
		public BulletControl[] Bullets;

		public bool AutoGetVectors;
		public Vector3[] BulletVectors;
		public Vector3[] BulletAngles;

		//public override void OnEnable()
		//{
		//	base.OnEnable();

		//	List<Vector3> vectors = new();
		//	List<Vector3> angles = new();

		//	foreach (BulletControl control in Bullets)
		//	{
		//		if (control.transform.parent == transform)
		//		{
		//			vectors.Add(control.transform.localPosition);
		//			angles.Add(control.transform.localEulerAngles);
		//			//control.transform.SetParent(transform.parent);
		//		}
		//		else
		//		{
		//			vectors.Add(transform.localPosition + control.transform.localPosition);
		//			angles.Add(transform.localEulerAngles + control.transform.localEulerAngles);
		//		}

		//		control.IsMove = false;

		//		if (control.GetType() != typeof(LaserControl))
		//		{
		//			control.Determing = true;
		//		}
		//		else
		//		{
		//			control.Determing = false;
		//			((LaserControl)control).LaserDeterming = true;
		//		}
		//	}

		//	BulletVectors = vectors.ToArray();
		//	BulletAngles = angles.ToArray();

		//	int i = 0;
		//	foreach(BulletControl control1 in Bullets)
		//	{
		//		float angle = ViewAngle + BulletAngles[i].z;
		//		control1.ViewAngle = angle;
		//		i++;
		//	}
		//}

		//public override void OnFixedUpdate()
		//{
		//	base.OnFixedUpdate();

		//	if (Bullets == null || Bullets.Length == 0)
		//	{
		//		return;
		//	}

		//	int i = 0;
		//	foreach(BulletControl control in Bullets)
		//	{
		//		float angle = ViewAngle + BulletAngles[i].z;

		//		control.ProgramAngle = -angle;

		//		float EADS = EulerAngles_ADS(angle);

		//		float x = Dsin(EADS);
		//		float y = Dcos(EADS);

		//		//control.transform.localPosition = TransformPosition + new Vector3(BulletVectors[i].x, BulletVectors[i].y);

		//		control.Position_Offset = transform.localPosition;

		//		i++;
		//	}
		//}
	}
}
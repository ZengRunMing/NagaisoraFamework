using UnityEngine;

using NagaisoraFamework;

public class BackGroundLoop : CommMonoScriptObject
{
	public Vector3 pos_add;

	public int time_start;

	public int time;

	public void Start()
	{
		time = time_start;
	}

	public void FixedUpdate()
	{
		if (time == 0)
		{
			time = time_start;
			transform.position += pos_add;
		}
		time--;
	}
}

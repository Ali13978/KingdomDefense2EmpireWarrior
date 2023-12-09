using UnityEngine;

public class StraightPath : WildPath
{
	public override Vector3 GetPosition(float t)
	{
		return Vector3.Lerp(base.StartPoints, base.FinishPoint, t);
	}
}

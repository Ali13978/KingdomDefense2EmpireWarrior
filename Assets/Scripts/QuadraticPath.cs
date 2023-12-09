using UnityEngine;

public class QuadraticPath : WildPath
{
	public float controlMagnitude = 1f;

	private Vector3 controlPoint;

	public float direction;

	public override Vector3 GetPosition(float t)
	{
		return (1f - t) * (1f - t) * base.StartPoints + 2f * (1f - t) * t * controlPoint + t * t * base.FinishPoint;
	}

	protected override void OnPointsSet()
	{
		Vector3 vector = base.StartPoints - base.FinishPoint;
		Vector3 a = (base.StartPoints + base.FinishPoint) / 2f;
		Vector3 vector2 = default(Vector3);
		vector2.x = vector.y * direction;
		vector2.y = (0f - vector.x) * direction;
		Vector3 vector3 = vector2;
		vector3 = vector3.normalized * controlMagnitude;
		controlPoint = a + vector3;
	}
}

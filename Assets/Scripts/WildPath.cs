using UnityEngine;

public abstract class WildPath : MonoBehaviour
{
	private Vector3 startPoint;

	private Vector3 finishPoint;

	private bool pointsSet;

	public Vector3 StartPoints => startPoint;

	public Vector3 FinishPoint => finishPoint;

	public abstract Vector3 GetPosition(float t);

	public void SetPoints(Vector3 startPoint, Vector3 finishPoint)
	{
		this.startPoint = startPoint;
		this.finishPoint = finishPoint;
		OnPointsSet();
		pointsSet = true;
	}

	protected virtual void OnPointsSet()
	{
	}
}

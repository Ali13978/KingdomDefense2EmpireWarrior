using UnityEngine;

public class TargetScript : MonoBehaviour
{
	private Vector3 cachedPosition;

	private float timeTrackingUpdateCachePos = 0.1f;

	public Vector3 preMovePos;

	public float speed;

	public Vector3 CachedPosition => cachedPosition;

	private void Update()
	{
		Vector3 vector = base.transform.position - CachedPosition;
		UnityEngine.Debug.Log("Vector di chuyen = " + vector);
		preMovePos = base.transform.position + speed * vector;
		UnityEngine.Debug.Log("Vi tri sap di chuyen toi = " + preMovePos);
		if (timeTrackingUpdateCachePos == 0f)
		{
			UpdateCachedPosition();
		}
		timeTrackingUpdateCachePos = Mathf.MoveTowards(timeTrackingUpdateCachePos, 0f, Time.deltaTime);
	}

	private void UpdateCachedPosition()
	{
		cachedPosition = base.transform.position;
		timeTrackingUpdateCachePos = 0.5f;
	}
}

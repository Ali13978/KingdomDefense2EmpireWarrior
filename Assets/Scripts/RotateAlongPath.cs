using UnityEngine;

public class RotateAlongPath : MonoBehaviour
{
	private Vector3 myPreviousPosition;

	[SerializeField]
	private float timeUpdatePosition = 0.2f;

	private float timeTracking;

	private void Start()
	{
		timeTracking = timeUpdatePosition;
	}

	private void Update()
	{
		base.transform.right = base.transform.localPosition - myPreviousPosition;
		if (timeTracking == 0f)
		{
			myPreviousPosition = base.transform.localPosition;
			timeTracking = timeUpdatePosition;
		}
		timeTracking = Mathf.MoveTowards(timeTracking, 0f, Time.deltaTime);
	}

	private void LookAtDirection2D(Vector2 direction)
	{
		float angle = Mathf.Atan2(direction.y, direction.x) * 57.29578f;
		base.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
	}
}

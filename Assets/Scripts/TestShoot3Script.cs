using UnityEngine;

public class TestShoot3Script : MonoBehaviour
{
	public Transform A;

	public Transform B;

	public float speed;

	private float startTime;

	public float journeyLength;

	private void Start()
	{
		startTime = Time.time;
		journeyLength = Vector3.Distance(A.position, B.position);
	}

	private void Update()
	{
		float num = (Time.time - startTime) * speed;
		float t = num / journeyLength;
		base.transform.position = Vector3.Lerp(A.position, B.position, t);
	}
}

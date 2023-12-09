using UnityEngine;

public class F3DWarpTunnel : MonoBehaviour
{
	public float MaxRotationSpeed;

	public float AdaptationFactor;

	private float speed;

	private float newSpeed;

	private void Start()
	{
		speed = 0f;
		OnDirectionChange();
	}

	private void OnDirectionChange()
	{
		newSpeed = UnityEngine.Random.Range(0f - MaxRotationSpeed, MaxRotationSpeed);
		F3DTime.time.AddTimer(UnityEngine.Random.Range(1, 5), 1, OnDirectionChange);
	}

	private void Update()
	{
		speed = Mathf.Lerp(speed, newSpeed, Time.deltaTime * AdaptationFactor);
		base.transform.rotation = Quaternion.Lerp(base.transform.rotation, base.transform.rotation * Quaternion.Euler(speed, 0f, 0f), Time.deltaTime);
	}
}

using UnityEngine;

public class AutoRotationZ : MonoBehaviour
{
	private Vector3 angle;

	public float rotateSpd = 360f;

	private void Start()
	{
		angle = base.transform.eulerAngles;
	}

	private void Update()
	{
		angle.z += Time.deltaTime * rotateSpd;
		base.transform.eulerAngles = angle;
	}
}

using System;
using UnityEngine;

public class CameraMultiPlatform : MonoBehaviour
{
	public bool isOrthographic;

	private float ratio;

	private float lastScreenAspectRatio;

	[SerializeField]
	private float offsetForShake = 0.1f;

	private void Awake()
	{
		SmartSize();
		lastScreenAspectRatio = Camera.main.aspect;
	}

	private void Update()
	{
		if (lastScreenAspectRatio != Camera.main.aspect)
		{
			UnityEngine.Debug.Log("Screen aspect change!");
			SmartSize();
			lastScreenAspectRatio = Camera.main.aspect;
		}
	}

	public void OnDrawGizmosSelected()
	{
	}

	[ContextMenu("SmartSize")]
	private void SmartSize()
	{
		Camera component = GetComponent<Camera>();
		if (isOrthographic)
		{
			component.orthographicSize = CameraOrthographicSizeCalculator() - offsetForShake;
		}
		else
		{
			component.fieldOfView = CameraPerspectiveFieldCalculator();
		}
	}

	private float CameraOrthographicSizeCalculator()
	{
		return 1f / Camera.main.aspect * 6.39999962f;
	}

	private float CameraPerspectiveFieldCalculator()
	{
		float num = 1280f / Camera.main.aspect;
		Vector3 position = Camera.main.transform.position;
		float num2 = Mathf.Abs(position.z);
		float num3 = 100f;
		float f = num / 2f / (num2 * num3);
		return (float)Math.Round(Mathf.Atan(f) * 57.29578f * 2f - 0.1f, 1);
	}
}

using System;
using UnityEngine;

public class CannonBallBehavior : MonoBehaviour
{
	public float vX;

	public float vY;

	public float gravity;

	public bool isReady;

	private float timeCount;

	private Vector2 shotDirection;

	public float lifeTime = 10f;

	public float firingAngle;

	private Vector3 target;

	private void Start()
	{
		Invoke("AutoDestroy", lifeTime);
	}

	public void Init(Vector3 target)
	{
		this.target = target;
		CalculateParameter();
	}

	private void Update()
	{
		if (isReady)
		{
			UpdatePosition();
		}
	}

	private void UpdatePosition()
	{
		timeCount += Time.deltaTime;
		Vector3 position = base.gameObject.transform.position;
		position.x += vX * Time.deltaTime;
		position.y += vY * Time.deltaTime + gravity * Time.deltaTime * timeCount;
		base.transform.position = position;
	}

	private void AutoDestroy()
	{
		isReady = false;
		UnityEngine.Object.Destroy(base.gameObject, 1f);
	}

	private void CalculateParameter()
	{
		float num = Vector3.Distance(base.transform.position, target);
		UnityEngine.Debug.Log("distance = " + num);
		float f = num / (Mathf.Sin(2f * firingAngle * ((float)Math.PI / 180f)) / gravity);
		f = Mathf.Abs(f);
		UnityEngine.Debug.Log(" target velocity " + f);
		shotDirection = GetFiringDirection(target);
		UnityEngine.Debug.Log("shot direction = " + shotDirection);
		vX = Mathf.Sqrt(f) * Mathf.Cos(firingAngle * ((float)Math.PI / 180f)) * shotDirection.x;
		vY = Mathf.Sqrt(f) * Mathf.Sin(firingAngle * ((float)Math.PI / 180f)) * shotDirection.y;
		UnityEngine.Debug.Log(vX + " " + vY);
		isReady = true;
	}

	private void ButtonGetReady()
	{
		isReady = true;
	}

	private Vector2 GetFiringDirection(Vector3 target)
	{
		Vector2 result = default(Vector2);
		Vector3 value = target - base.gameObject.transform.position;
		value = Vector3.Normalize(value);
		if (value.x > 0f)
		{
			if (value.y > 0f)
			{
				result.x = 1f;
				result.y = 1f;
			}
			else
			{
				result.x = 1f;
				result.y = -1f;
			}
		}
		else if (value.y > 0f)
		{
			result.x = -1f;
			result.y = 1f;
		}
		else
		{
			result.x = -1f;
			result.y = -1f;
		}
		return result;
	}

	private float GetFiringAngle(Vector3 target)
	{
		return 0f;
	}
}

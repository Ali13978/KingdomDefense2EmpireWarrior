using System;
using System.Collections;
using UnityEngine;

public class Test2DCannonShot : MonoBehaviour
{
	public Transform Target;

	public float firingAngle = 45f;

	public float gravity = 9.8f;

	public Transform Projectile;

	public Transform startMarker;

	public Transform endMarker;

	public float speed = 1f;

	private float startTime;

	private float journeyLength;

	private void Start()
	{
		startTime = Time.time;
		journeyLength = Vector3.Distance(startMarker.position, endMarker.position);
		StartCoroutine(SimulateProjectile());
	}

	private void Update()
	{
		float num = (Time.time - startTime) * speed;
		float num2 = num / journeyLength;
	}

	private IEnumerator SimulateProjectile()
	{
		yield return new WaitForSeconds(1.5f);
		float target_Distance = Vector3.Distance(Projectile.position, Target.position);
		UnityEngine.Debug.Log("distance = " + target_Distance);
		float projectile_Velocity = target_Distance / (Mathf.Sin(2f * firingAngle * ((float)Math.PI / 180f)) / gravity);
		UnityEngine.Debug.Log(" target velocity " + projectile_Velocity);
		float Vx = Mathf.Sqrt(projectile_Velocity) * Mathf.Cos(firingAngle * ((float)Math.PI / 180f));
		float Vy = Mathf.Sqrt(projectile_Velocity) * Mathf.Sin(firingAngle * ((float)Math.PI / 180f));
		UnityEngine.Debug.Log(Vx + " " + Vy);
		float flightDuration = target_Distance / Vx;
		Projectile.rotation = Quaternion.LookRotation(Target.position - Projectile.position);
		float elapse_time = 0f;
		while (elapse_time < flightDuration)
		{
			Projectile.Translate(0f, (Vy - gravity * elapse_time) * Time.deltaTime, Vx * Time.deltaTime);
			elapse_time += Time.deltaTime;
			yield return null;
		}
	}
}

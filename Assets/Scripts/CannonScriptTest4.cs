using System.Collections;
using UnityEngine;

public class CannonScriptTest4 : MonoBehaviour
{
	public Transform Target;

	public float duration;

	public int angleValue;

	public GameObject bulletPrefab;

	private void Start()
	{
	}

	private void MoveComplete()
	{
		UnityEngine.Debug.Log("move complete!");
	}

	private void Update()
	{
		if (UnityEngine.Input.GetKeyDown(KeyCode.Q))
		{
			CreateBullet();
		}
	}

	private void CreateBullet()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate(bulletPrefab);
		bulletPrefab.transform.position = base.gameObject.transform.position;
		WildMover component = gameObject.GetComponent<WildMover>();
		QuadraticPath component2 = gameObject.GetComponent<QuadraticPath>();
		component.StartPos = base.gameObject.transform.position;
		component.FinishPos = Target.position;
		component.Duration = duration;
		component2.controlMagnitude = angleValue;
		Vector3 vector = Target.transform.position - base.gameObject.transform.position;
		if (vector.x > 0f)
		{
			component2.direction = 1f;
		}
		else
		{
			component2.direction = -1f;
		}
		component.StartMoving();
	}

	public IEnumerator MoveOverSeconds(Vector3 startMove, Vector3 end, float seconds)
	{
		float elapsedTime = 0f;
		while (elapsedTime < seconds)
		{
			base.transform.position = Vector3.Lerp(startMove, end, elapsedTime / seconds);
			elapsedTime += Time.deltaTime;
			yield return new WaitForEndOfFrame();
		}
		base.transform.position = end;
		UnityEngine.Debug.Log("da den dich!");
	}
}

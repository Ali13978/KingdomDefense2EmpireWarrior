using UnityEngine;

public class Projectile : MonoBehaviour
{
	public Transform target;

	public Transform throwPoint;

	public GameObject ball;

	public float gravity;

	private void Start()
	{
	}

	private void Update()
	{
		if (UnityEngine.Input.GetKeyDown(KeyCode.Q))
		{
			Throw();
		}
	}

	private void Throw()
	{
		Vector3 position = target.position;
		float x = position.x;
		Vector3 position2 = throwPoint.position;
		float num = x - position2.x;
		Vector3 position3 = target.position;
		float y = position3.y;
		Vector3 position4 = throwPoint.position;
		float num2 = y - position4.y;
		float num3 = Mathf.Atan((num2 + (0f - gravity) / 2f) / num);
		UnityEngine.Debug.Log("Góc bắn = " + num3);
		float num4 = num / Mathf.Cos(num3);
		float num5 = num4 * Mathf.Cos(num3);
		float y2 = num4 * Mathf.Sin(num3);
		Vector3 position5 = target.transform.position;
		float x2 = position5.x;
		Vector3 position6 = base.gameObject.transform.position;
		float num6 = Mathf.Abs((x2 - position6.x) / num5);
		UnityEngine.Debug.Log("Thời gian bắn = " + num6);
		GameObject gameObject = UnityEngine.Object.Instantiate(ball, throwPoint.position, Quaternion.Euler(new Vector3(0f, 0f, 0f)));
		Rigidbody2D component = gameObject.GetComponent<Rigidbody2D>();
		gameObject.GetComponent<BulletScript>().Init(num6);
		component.velocity = new Vector2(num5, y2);
	}
}

using UnityEngine;

public class HomingMissile : MonoBehaviour
{
	public float speed = 5f;

	public float rotatingSpeed = 200f;

	public GameObject target;

	private Rigidbody2D rb;

	private void Start()
	{
		target = GameObject.FindGameObjectWithTag("Player");
		rb = GetComponent<Rigidbody2D>();
	}

	private void FixedUpdate()
	{
		Vector2 v = (Vector2)base.transform.position - (Vector2)target.transform.position;
		v.Normalize();
		Vector3 vector = Vector3.Cross(v, base.transform.right);
		float z = vector.z;
		rb.angularVelocity = rotatingSpeed * z;
		rb.velocity = base.transform.right * speed;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag != "deadly")
		{
			UnityEngine.Object.Destroy(base.gameObject, 0.02f);
		}
	}
}

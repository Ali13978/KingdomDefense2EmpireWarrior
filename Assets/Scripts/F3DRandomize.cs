using UnityEngine;

public class F3DRandomize : MonoBehaviour
{
	private new Transform transform;

	private Vector3 defaultScale;

	public bool RandomScale;

	public bool RandomRotation;

	public float MinScale;

	public float MaxScale;

	public float MinRotation;

	public float MaxRotaion;

	private void Awake()
	{
		transform = GetComponent<Transform>();
		defaultScale = transform.localScale;
	}

	private void OnEnable()
	{
		if (RandomScale)
		{
			transform.localScale = defaultScale * UnityEngine.Random.Range(MinScale, MaxScale);
		}
		if (RandomRotation)
		{
			transform.rotation *= Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(MinRotation, MaxRotaion));
		}
	}
}

using UnityEngine;

public class RandomRotationOnAppear : MonoBehaviour
{
	[SerializeField]
	private float minDegree;

	[SerializeField]
	private float maxDegree;

	private float currentDegree;

	private Vector3 rotation;

	private void Start()
	{
		currentDegree = UnityEngine.Random.Range(minDegree, maxDegree);
		rotation.Set(0f, 0f, currentDegree);
		base.transform.rotation = Quaternion.Euler(rotation);
	}

	private void OnEnable()
	{
		currentDegree = UnityEngine.Random.Range(minDegree, maxDegree);
		rotation.Set(0f, 0f, currentDegree);
		base.transform.rotation = Quaternion.Euler(rotation);
	}
}

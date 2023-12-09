using MyCustom;
using UnityEngine;

public class RandomRotationAtStart : CustomMonoBehaviour
{
	[SerializeField]
	private float minValue;

	[SerializeField]
	private float maxValue;

	private float currentRotationValue;

	private Vector3 localRotation;

	private void Start()
	{
		currentRotationValue = Random.Range(minValue, maxValue);
		localRotation.Set(0f, 0f, currentRotationValue);
		base.gameObject.transform.rotation = Quaternion.Euler(localRotation);
	}

	private void Update()
	{
	}
}

using UnityEngine;

public class AutoRotation : MonoBehaviour
{
	public float timeRepeat;

	private Vector3 localScale;

	private void Start()
	{
		InvokeRepeating("Rotate", timeRepeat, timeRepeat);
	}

	private void Rotate()
	{
		localScale = base.transform.localScale;
		localScale.x *= -1f;
		base.transform.localScale = localScale;
	}
}

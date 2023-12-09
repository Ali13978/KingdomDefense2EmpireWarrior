using UnityEngine;

public class AnimationSpeedSetter : MonoBehaviour
{
	[SerializeField]
	private float minSpeed;

	[SerializeField]
	private float maxSpeed;

	private void Start()
	{
		GetComponent<Animator>().speed = UnityEngine.Random.Range(minSpeed, maxSpeed);
	}
}

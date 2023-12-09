using UnityEngine;

namespace SSR.Core.HelperComponent
{
	public class RandomAnimatorSpeedSetter : MonoBehaviour
	{
		[SerializeField]
		private float minSpeed;

		[SerializeField]
		private float maxSpeed;

		private void Start()
		{
			Set();
		}

		private void Set()
		{
			float speed = UnityEngine.Random.Range(minSpeed, maxSpeed);
			GetComponent<Animator>().speed = speed;
		}
	}
}

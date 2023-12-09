using UnityEngine;

namespace SSR.Core.HelperComponent
{
	public class AnimatorSpeedSetter : MonoBehaviour
	{
		[SerializeField]
		private Animator animator;

		[SerializeField]
		private float speed = 1f;

		[SerializeField]
		private bool setAtAwake;

		public void Awake()
		{
			if (setAtAwake)
			{
				Set();
			}
		}

		[ContextMenu("Set")]
		public void Set()
		{
			animator.speed = speed;
		}

		public void Reset()
		{
			animator = GetComponent<Animator>();
		}
	}
}

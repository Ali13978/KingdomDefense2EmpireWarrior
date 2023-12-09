using MyCustom;
using UnityEngine;

namespace Gameplay
{
	public class UICinematicEffectController : CustomMonoBehaviour
	{
		private Animator animator;

		[SerializeField]
		private float duration;

		private void Awake()
		{
			animator = GetComponent<Animator>();
		}

		public void OpenCinematic()
		{
			animator.SetTrigger("Open");
			CustomInvoke(CloseCinematic, duration / 1000f);
		}

		public void CloseCinematic()
		{
			animator.SetTrigger("Close");
		}

		public void MoveUIOut()
		{
			animator.SetTrigger("Out");
		}
	}
}

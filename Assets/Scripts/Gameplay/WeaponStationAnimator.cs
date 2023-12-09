using MyCustom;
using UnityEngine;

namespace Gameplay
{
	public class WeaponStationAnimator : CustomMonoBehaviour
	{
		[SerializeField]
		private Animator animator;

		public void PlayAnimAttack()
		{
			animator.Play("Attack");
		}

		public void PlayAnimAppear()
		{
			animator.Play("Appear");
		}

		public void PlayAnimDisappear()
		{
			animator.Play("Disappear");
		}

		public void Reset()
		{
			animator.Play("Appear");
		}
	}
}

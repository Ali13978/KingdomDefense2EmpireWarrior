using UnityEngine;

namespace Gameplay
{
	public class PhoenixMinionData
	{
		public GameObject minion;

		public float lifetimeCountdown;

		public bool isAttacking;

		public bool isDestroyed;

		private Animator minionAnimator;

		public PhoenixMinionData(GameObject minion, float lifetime)
		{
			this.minion = minion;
			lifetimeCountdown = lifetime;
			minionAnimator = minion.GetComponent<Animator>();
			PlayIdle();
		}

		public void PlayIdle()
		{
			minionAnimator.Play("Idle");
		}

		public void PlayJump()
		{
			minionAnimator.Play("Jump");
		}
	}
}

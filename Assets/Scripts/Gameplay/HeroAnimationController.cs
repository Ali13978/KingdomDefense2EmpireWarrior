using UnityEngine;

namespace Gameplay
{
	public class HeroAnimationController : HeroController, IAnimationController
	{
		public static string animRun = "Run";

		public static string animAttackMeLee = "MeleeAttack";

		public static string animAttackRange = "RangeAttack";

		public static string animDie = "Die";

		public static string animIdle = "Idle";

		public static string animPlay = "Play";

		public static string animActiveSkill = "ActiveSkill";

		public static string animPassiveSkill_0 = "Passive0";

		public static string animPassiveSkill_1 = "Passive1";

		public static string animPassiveSkill_2 = "Passive2";

		private Animator animator;

		public override void Initialize()
		{
			base.Initialize();
			GetAllComponents();
		}

		public override void OnAppear()
		{
			base.OnAppear();
		}

		public override void OnReturnPool()
		{
			base.OnReturnPool();
		}

		private void GetAllComponents()
		{
			animator = GetComponent<Animator>();
		}

		public void ToWakeupState()
		{
		}

		public void ToRunState()
		{
			animator.Play(animRun);
		}

		public void ToIdleState()
		{
			animator.Play(animIdle);
		}

		public void ToMeleeAttackState()
		{
			animator.Play(animAttackMeLee);
		}

		public void ToRangeAttackState()
		{
			animator.Play(animAttackRange);
		}

		public void ToDieState()
		{
			animator.Play(animDie);
		}

		public void ToAppearState()
		{
			ToIdleState();
		}

		public void ToPlayState()
		{
			animator.Play(animPlay);
		}

		public bool ContainAppearAnim()
		{
			return false;
		}

		public void ToSpecialState(string animationName, float duration)
		{
			animator.Play(animationName);
		}
	}
}

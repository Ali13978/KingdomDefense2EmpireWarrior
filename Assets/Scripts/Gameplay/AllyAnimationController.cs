using UnityEngine;

namespace Gameplay
{
	public class AllyAnimationController : AllyController, IAnimationController
	{
		public static string animAppear = "Appear";

		public static string animDisappear = "Disappear";

		public static string animRun = "Run";

		public static string animMeleeAttack = "MeleeAttack";

		public static string animRangeAttack = "RangeAttack";

		public static string animDie = "Die";

		public static string animIdle = "Idle";

		public static string appearTrigger = "AppearTrigger";

		public static string animActiveSkill = "ActiveSkill";

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

		public void ToRunState()
		{
			animator.Play(animRun);
		}

		public void ToIdleState()
		{
			animator.Play(animIdle);
		}

		public void ToPlayState()
		{
			ToIdleState();
		}

		public void ToMeleeAttackState()
		{
			animator.Play(animMeleeAttack);
		}

		public void ToRangeAttackState()
		{
			animator.Play(animRangeAttack);
		}

		public void ToDieState()
		{
			animator.Play(animDie);
		}

		public void ToAppearState()
		{
			animator.SetTrigger(appearTrigger);
		}

		public void ToDisappearState()
		{
			animator.Play(animDisappear);
		}

		public bool ContainAppearAnim()
		{
			AnimatorControllerParameter[] parameters = animator.parameters;
			AnimatorControllerParameter[] array = parameters;
			foreach (AnimatorControllerParameter animatorControllerParameter in array)
			{
				if (animatorControllerParameter.type == AnimatorControllerParameterType.Trigger && animatorControllerParameter.name == appearTrigger)
				{
					return true;
				}
			}
			return false;
		}

		public void ToSpecialState(string animationName, float duration)
		{
			animator.Play(animationName);
		}
	}
}

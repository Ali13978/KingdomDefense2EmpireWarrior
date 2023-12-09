using UnityEngine;

namespace Gameplay
{
	public class EnemyAnimationController : EnemyController
	{
		public static string animRunRight = "Run-Right";

		public static string animRunUp = "Run-Up";

		public static string animRunDown = "Run-Down";

		public static string animRunUnderground = "RunUnderground";

		public static string animAttackMelee = "MeleeAttack";

		public static string animAttackRange = "RangeAttack";

		public static string animSpecialAttack = "SpecialAttack";

		public static string animDie = "Die";

		public static string animIdle = "Idle";

		public static string animAppear = "Appear";

		public static string animHidingAlert = "HidingAlert";

		private Animator animator;

		private EnemyAnimationSpeedController enemyAnimationSpeedController;

		public override void Initialize()
		{
			base.Initialize();
			GetAllComponents();
		}

		public override void OnAppear()
		{
			base.OnAppear();
		}

		private void GetAllComponents()
		{
			animator = GetComponent<Animator>();
			enemyAnimationSpeedController = base.EnemyModel.GetComponent<EnemyAnimationSpeedController>();
		}

		public void ToSpawnFromGroundState()
		{
			animator.Play(animAppear);
		}

		public void ToRunState(string animationName)
		{
			animator.Play(animationName);
		}

		public void ToIdleState()
		{
			animator.Play(animIdle);
		}

		public void ToMeleeAttackState()
		{
			animator.Play(animAttackMelee);
		}

		public void ToRangeAttackState()
		{
			animator.Play(animAttackRange);
		}

		public void ToDieState()
		{
			enemyAnimationSpeedController.SetNormalSpeed();
			animator.Play(animDie);
		}

		public void ToSpecialAttackState()
		{
			animator.Play(animSpecialAttack);
		}

		public void TurnBack()
		{
			Vector3 localScale = base.EnemyModel.transform.localScale;
			Vector3 localScale2 = new Vector3(0f - localScale.x, localScale.y, 1f);
			base.EnemyModel.transform.localScale = localScale2;
		}

		public void TurnOnAnimator()
		{
			animator.enabled = true;
		}

		public void TurnOffAnimator()
		{
			animator.enabled = false;
		}
	}
}

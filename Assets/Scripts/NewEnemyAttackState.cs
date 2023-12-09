using Gameplay;
using Parameter;
using UnityEngine;

public class NewEnemyAttackState : NewEnemyState
{
	private float cooldownDur;

	private float countdownCooldown;

	private CharacterModel attackingHero;

	private Vector3 invertXVector = new Vector3(-1f, 1f, 1f);

	public NewEnemyAttackState(EnemyModel enemyModel, INewFSMController fSMController)
		: base(enemyModel, fSMController)
	{
		if ((bool)enemyModel.enemyAttackController)
		{
			cooldownDur = enemyModel.enemyAttackController.GetCooldownTime();
		}
	}

	public override void OnStartState()
	{
		base.OnStartState();
		attackingHero = enemyModel.EnemyFindTargetController.Target;
		countdownCooldown = cooldownDur * 0.5f;
		LookAtTarget(enemyModel.transform.position, attackingHero.transform.position);
	}

	public override void Update(float dt)
	{
		base.Update(dt);
		countdownCooldown -= dt;
		if (!(countdownCooldown <= 0f))
		{
			return;
		}
		countdownCooldown = cooldownDur;
		attackingHero = enemyModel.EnemyFindTargetController.Target;
		if (!GameTools.IsUnderTargetOfASpecificHero(enemyModel, attackingHero))
		{
			SetTransition(EntityStateEnum.EnemyMove);
		}
		else if ((bool)enemyModel.enemyAttackController)
		{
			Enemy originalParameter = enemyModel.OriginalParameter;
			if (originalParameter.attack_range_average < 0)
			{
				countdownCooldown = Mathf.Max(3f, cooldownDur);
				enemyModel.enemyAttackController.PrepareToRangeAttack();
			}
			else
			{
				enemyModel.enemyAttackController.PrepareToMeleeAttack();
			}
		}
	}

	private void LookAtTarget(Vector3 currentPosition, Vector3 targetPosition)
	{
		if (targetPosition.x - currentPosition.x > 0f)
		{
			enemyModel.transform.localScale = Vector3.one;
		}
		else
		{
			enemyModel.transform.localScale = invertXVector;
		}
	}
}

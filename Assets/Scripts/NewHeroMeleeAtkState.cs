using Gameplay;
using System.Collections.Generic;
using UnityEngine;

public class NewHeroMeleeAtkState : NewHeroState
{
	private float cooldownCountdown;

	private Vector3 invertXVector = new Vector3(-1f, 1f, 1f);

	public NewHeroMeleeAtkState(CharacterModel heroModel, INewFSMController fSMController)
		: base(heroModel, fSMController)
	{
	}

	public override void OnStartState()
	{
		base.OnStartState();
		cooldownCountdown = 0f;
	}

	public override void Update(float dt)
	{
		base.Update(dt);
		cooldownCountdown -= dt;
		if (cooldownCountdown <= 0f)
		{
			cooldownCountdown = heroModel.GetAtkCooldownDuration();
			DoMeleeAttack();
		}
	}

	public override void OnInput(StateInputType inputType, params object[] args)
	{
		base.OnInput(inputType, args);
		if (inputType != StateInputType.MonsterInAtkRange)
		{
			return;
		}
		EnemyModel enemyWithHighestScore = GameTools.GetEnemyWithHighestScore(heroModel);
		if (GameTools.IsValidEnemy(enemyWithHighestScore) && enemyWithHighestScore.GetInstanceID() != heroModel.GetCurrentTarget().GetInstanceID())
		{
			heroModel.AddTarget(enemyWithHighestScore);
			if (heroModel.IsInMeleeRange(enemyWithHighestScore))
			{
				SetTransition(EntityStateEnum.HeroMovetoTarget);
			}
			else
			{
				SetTransition(EntityStateEnum.HeroRangeAtk);
			}
		}
	}

	public bool IsEnemyUnderAttackAndBlockingOfAnotherAlly(EnemyModel enemy, CharacterModel hero)
	{
		if (!GameTools.IsValidEnemy(enemy))
		{
			return true;
		}
		if (enemy.EnemyFindTargetController == null)
		{
			return true;
		}
		if (enemy.EnemyFindTargetController.Target != null && enemy.EnemyFindTargetController.Target.GetInstanceID() != hero.GetInstanceID())
		{
			return true;
		}
		List<CharacterModel> listActiveAlly = SingletonMonoBehaviour<GameData>.Instance.ListActiveAlly;
		for (int num = listActiveAlly.Count - 1; num >= 0; num--)
		{
			if (listActiveAlly[num].GetInstanceID() != hero.GetInstanceID() && GameTools.IsValidEnemy(listActiveAlly[num].GetCurrentTarget()) && listActiveAlly[num].GetCurrentTarget().GetInstanceID() == enemy.GetInstanceID() && listActiveAlly[num].IsMeleeAttacking())
			{
				return true;
			}
		}
		return false;
	}

	public void DoMeleeAttack()
	{
		EnemyModel currentTarget = heroModel.GetCurrentTarget();
		if (!GameTools.IsValidEnemy(currentTarget) || !heroModel.IsInMeleeActionRange(currentTarget))
		{
			SetTransition(EntityStateEnum.HeroIdleForAWhile);
			return;
		}
		currentTarget.enemyFsmController.GetCurrentState().OnInput(StateInputType.HeroMeleeAttackEnemy, heroModel);
		heroModel.GetAnimationController().ToMeleeAttackState();
		heroModel.DoMeleeAttack();
		heroModel.LookAtEnemy();
	}
}

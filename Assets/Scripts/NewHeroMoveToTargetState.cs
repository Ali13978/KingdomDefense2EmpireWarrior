using DG.Tweening;
using Gameplay;
using UnityEngine;

public class NewHeroMoveToTargetState : NewHeroState
{
	private EnemyModel enemy;

	private Vector3 attackPosition;

	private float timeMovingToAtkPos;

	private Vector3 invertXVector = new Vector3(-1f, 1f, 1f);

	public NewHeroMoveToTargetState(CharacterModel heroModel, INewFSMController fSMController)
		: base(heroModel, fSMController)
	{
	}

	public override void OnStartState()
	{
		base.OnStartState();
		enemy = heroModel.GetCurrentTarget();
		if ((bool)enemy.EnemyFindTargetController)
		{
			enemy.EnemyFindTargetController.AddTarget(heroModel);
		}
		enemy.enemyFsmController.GetCurrentState().OnInput(StateInputType.SetEnemyIdleWaitForMeleeAtk, heroModel);
		heroModel.GetAnimationController().ToRunState();
		GameTools.MoveToAttackPosition(heroModel, enemy, heroModel.GetSpeed(), MoveToAttackPositionComplete);
	}

	public override void OnExitState()
	{
		base.OnExitState();
		CancelMoveToAtkPos();
	}

	public override void OnInput(StateInputType inputType, params object[] args)
	{
		base.OnInput(inputType, args);
		if (inputType == StateInputType.MonsterInAtkRange)
		{
			EnemyModel enemyWithHighestScore = GameTools.GetEnemyWithHighestScore(heroModel);
			if (enemyWithHighestScore.GetInstanceID() != heroModel.GetCurrentTarget().GetInstanceID())
			{
				CancelMoveToAtkPos();
				heroModel.AddTarget(enemyWithHighestScore);
				enemy = enemyWithHighestScore;
				GameTools.MoveToAttackPosition(heroModel, enemy, heroModel.GetSpeed(), MoveToAttackPositionComplete);
			}
		}
	}

	private void CancelMoveToAtkPos()
	{
		heroModel.transform.DOKill();
	}

	private void MoveToAttackPositionComplete()
	{
		SetTransition(EntityStateEnum.HeroMeleeAtk);
	}

	private bool IsEnemyUnderTargetOfAnother(EnemyModel enemy, CharacterModel hero)
	{
		return GameTools.IsUnderTargetOfAnyHero(enemy, hasExceptionHero: true, hero.GetInstanceID());
	}

	public override void Update(float dt)
	{
		base.Update(dt);
		if (!heroModel.IsInMeleeRange(enemy))
		{
			heroModel.AddTarget(null);
			SetTransition(EntityStateEnum.HeroIdleForAWhile);
		}
	}
}

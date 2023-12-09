using Gameplay;

public class NewHeroIdleState : NewHeroState
{
	public NewHeroIdleState(CharacterModel heroModel, INewFSMController fsmController)
		: base(heroModel, fsmController)
	{
	}

	public override void OnStartState()
	{
		base.OnStartState();
		heroModel.GetAnimationController().ToIdleState();
		EnemyModel enemyModel = FindTargetInRange();
		if (enemyModel != null)
		{
			OnInput(StateInputType.MonsterInAtkRange, enemyModel);
		}
	}

	public override void OnInput(StateInputType inputType, params object[] args)
	{
		base.OnInput(inputType, args);
		if (inputType == StateInputType.MonsterInAtkRange)
		{
			EnemyModel enemyWithHighestScore = GameTools.GetEnemyWithHighestScore(heroModel);
			if (heroModel.IsInMeleeRange(enemyWithHighestScore))
			{
				heroModel.AddTarget(enemyWithHighestScore);
				SetTransition(EntityStateEnum.HeroMovetoTarget);
			}
			else if (heroModel.IsInRangerRange(enemyWithHighestScore))
			{
				heroModel.AddTarget(enemyWithHighestScore);
				SetTransition(EntityStateEnum.HeroRangeAtk);
			}
		}
	}
}

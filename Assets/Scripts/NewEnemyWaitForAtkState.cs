using Gameplay;

public class NewEnemyWaitForAtkState : NewEnemyState
{
	private float countdown;

	private CharacterModel attackingHero;

	public NewEnemyWaitForAtkState(EnemyModel enemyModel, INewFSMController fSMController)
		: base(enemyModel, fSMController)
	{
	}

	public override void OnStartState()
	{
		base.OnStartState();
		if ((bool)enemyModel.EnemyFindTargetController)
		{
			attackingHero = enemyModel.EnemyFindTargetController.Target;
		}
		if (enemyModel.IsUnderground)
		{
			enemyModel.IsUnderground = false;
			enemyModel.EnemyAnimationController.ToSpawnFromGroundState();
		}
		else
		{
			enemyModel.EnemyAnimationController.ToIdleState();
		}
	}

	public override void OnInput(StateInputType inputType, params object[] args)
	{
		base.OnInput(inputType, args);
		if (inputType == StateInputType.HeroMeleeAttackEnemy)
		{
			CharacterModel target = (CharacterModel)args[0];
			enemyModel.EnemyFindTargetController.Target = target;
			SetTransition(EntityStateEnum.EnemyAttack);
		}
	}

	public override void Update(float dt)
	{
		base.Update(dt);
		if (!IsValidHeroAndReachingToAtk())
		{
			SetTransition(EntityStateEnum.EnemyMove);
		}
	}

	private bool IsValidHeroAndReachingToAtk()
	{
		if (!GameTools.IsValidCharacter(attackingHero))
		{
			return false;
		}
		if (!GameTools.IsValidEnemy(attackingHero.GetCurrentTarget()))
		{
			return false;
		}
		if (attackingHero.GetCurrentTarget().GetInstanceID() != enemyModel.GetInstanceID())
		{
			return false;
		}
		return true;
	}
}

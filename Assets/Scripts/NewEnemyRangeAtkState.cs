using Gameplay;

public class NewEnemyRangeAtkState : NewEnemyState
{
	private float cooldownDur;

	private float countdownCooldown;

	private CharacterModel attackingHero;

	private float atkRange;

	private float sqAtkRange;

	public NewEnemyRangeAtkState(EnemyModel enemyModel, INewFSMController fsmController)
		: base(enemyModel, fsmController)
	{
		if ((bool)enemyModel.enemyAttackController)
		{
			cooldownDur = enemyModel.enemyAttackController.GetCooldownTime();
			atkRange = enemyModel.enemyAttackController.GetRangerAtkRange();
			sqAtkRange = atkRange * atkRange;
		}
	}

	public override void OnStartState()
	{
		base.OnStartState();
		attackingHero = enemyModel.EnemyFindTargetController.Target;
		countdownCooldown = 0f;
	}

	public override void Update(float dt)
	{
		base.Update(dt);
		countdownCooldown -= dt;
		if (countdownCooldown <= 0f)
		{
			countdownCooldown = cooldownDur;
			if (!GameTools.IsValidCharacter(attackingHero) || (attackingHero.transform.position - enemyModel.transform.position).sqrMagnitude > sqAtkRange || !GameTools.IsCharacterVisible(attackingHero) || enemyModel.IsInTunnel)
			{
				SetTransition(EntityStateEnum.EnemyMove);
			}
			else
			{
				enemyModel.enemyAttackController.PrepareToRangeAttack();
			}
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
}

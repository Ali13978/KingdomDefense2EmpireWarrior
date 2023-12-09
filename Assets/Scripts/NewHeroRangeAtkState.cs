using Gameplay;

public class NewHeroRangeAtkState : NewHeroState
{
	private float cooldownCountdown;

	public NewHeroRangeAtkState(CharacterModel heroModel, INewFSMController fSMController)
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
			heroModel.LookAtEnemy();
			DoRangeAttack();
		}
	}

	public void DoRangeAttack()
	{
		if (!GameTools.IsValidEnemy(heroModel.GetCurrentTarget()) || !heroModel.IsInRangerRange(heroModel.GetCurrentTarget()))
		{
			SetTransition(EntityStateEnum.HeroIdleForAWhile);
			return;
		}
		if (heroModel.IsInMeleeRange(heroModel.GetCurrentTarget()))
		{
			SetTransition(EntityStateEnum.HeroMovetoTarget);
			return;
		}
		heroModel.GetAnimationController().ToRangeAttackState();
		heroModel.DoRangeAttack();
	}
}

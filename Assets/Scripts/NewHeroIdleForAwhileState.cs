using Gameplay;

public class NewHeroIdleForAwhileState : NewHeroIdleState
{
	private float idleCountdown;

	public NewHeroIdleForAwhileState(CharacterModel heroModel, INewFSMController fSMController)
		: base(heroModel, fSMController)
	{
	}

	public override void OnStartState()
	{
		base.OnStartState();
		idleCountdown = heroModel.GetShortIdleDuration();
	}

	public override void Update(float dt)
	{
		base.Update(dt);
		idleCountdown -= dt;
		if (idleCountdown <= 0f)
		{
			SetTransition(EntityStateEnum.HeroMove);
		}
	}
}

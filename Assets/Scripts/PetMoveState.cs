using Gameplay;

public class PetMoveState : NewHeroState
{
	public PetMoveState(CharacterModel heroModel, INewFSMController fSMController)
		: base(heroModel, fSMController)
	{
	}

	public override void OnStartState()
	{
		base.OnStartState();
		SetTransition(EntityStateEnum.HeroIdle);
	}
}

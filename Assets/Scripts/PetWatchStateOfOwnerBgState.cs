using Gameplay;

public class PetWatchStateOfOwnerBgState : NewEntityState
{
	private HeroModel petModel;

	private HeroModel ownerModel;

	private INewFSMController fsmController;

	public PetWatchStateOfOwnerBgState(CharacterModel heroModel, INewFSMController fsmController)
		: base(fsmController)
	{
		petModel = (heroModel as HeroModel);
		ownerModel = petModel.PetOwner;
		this.fsmController = fsmController;
	}

	public override void Update(float dt)
	{
		base.Update(dt);
		if (ownerModel.IsAlive && ownerModel.GetFSMController().GetCurrentState() is NewHeroMoveState && !(petModel.GetFSMController().GetCurrentState() is PetStayAroundState))
		{
			fsmController.GetCurrentState().OnInput(StateInputType.ThePetOwnerIsMoving);
		}
	}
}

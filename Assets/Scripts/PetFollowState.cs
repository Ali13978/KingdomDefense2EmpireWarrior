using Gameplay;
using UnityEngine;

public class PetFollowState : NewEntityState
{
	public PetStayAroundState parentState;

	private HeroModel allyModel;

	private float farThreshold = 3f;

	private float sqFarThreshold;

	private float sqIdleDisThreshold;

	private float moveSpeed;

	private Vector3 invertXVector = new Vector3(-1f, 1f, 1f);

	public PetFollowState(CharacterModel heroModel, INewFSMController fsmController, PetStayAroundState parentState)
		: base(fsmController)
	{
		this.parentState = parentState;
		allyModel = (heroModel as HeroModel);
		sqIdleDisThreshold = GameTools.sqPetDisToOwnerThreshold * 0.5f;
		moveSpeed = allyModel.GetSpeed() / GameData.PIXEL_PER_UNIT;
		sqFarThreshold = farThreshold * farThreshold;
	}

	public override void OnStartState()
	{
		base.OnStartState();
		allyModel.GetAnimationController().ToRunState();
	}

	public override void Update(float dt)
	{
		base.Update(dt);
		Vector3 vector = allyModel.PetOwner.transform.position + parentState.ownerToIdleSpotOffset - allyModel.transform.position;
		float sqrMagnitude = vector.sqrMagnitude;
		if (sqrMagnitude <= sqIdleDisThreshold)
		{
			parentState.OnInput(StateInputType.PetReachFollowSpot);
			return;
		}
		Vector3 normalized = vector.normalized;
		allyModel.transform.localScale = ((!(normalized.x > 0f)) ? invertXVector : Vector3.one);
		allyModel.transform.position += normalized * dt * moveSpeed * ((!(sqrMagnitude > sqFarThreshold)) ? 1 : 2);
	}
}

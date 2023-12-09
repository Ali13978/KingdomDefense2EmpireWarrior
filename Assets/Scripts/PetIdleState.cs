using Gameplay;
using UnityEngine;

public class PetIdleState : NewEntityState
{
	public PetStayAroundState parentState;

	private HeroModel allyModel;

	private float countdownToPlay;

	public PetIdleState(CharacterModel heroModel, INewFSMController fsmController, PetStayAroundState parentState)
		: base(fsmController)
	{
		this.parentState = parentState;
		allyModel = (heroModel as HeroModel);
	}

	public override void OnStartState()
	{
		base.OnStartState();
		allyModel.GetAnimationController().ToIdleState();
		allyModel.SetAssignedPosition(allyModel.PetOwner.transform.position + parentState.ownerToIdleSpotOffset);
		if (Random.Range(0f, 1f) < 0.5f)
		{
			countdownToPlay = Random.Range(0.8f, 2.1f);
		}
		else
		{
			countdownToPlay = -1f;
		}
	}

	public override void Update(float dt)
	{
		base.Update(dt);
		if ((allyModel.transform.position - (allyModel.PetOwner.transform.position + parentState.ownerToIdleSpotOffset)).sqrMagnitude > GameTools.sqPetDisToOwnerThreshold)
		{
			parentState.OnInput(StateInputType.PetFarFromOwner);
		}
		if (countdownToPlay >= 0f)
		{
			countdownToPlay -= dt;
			if (countdownToPlay < 0f)
			{
				allyModel.GetAnimationController().ToPlayState();
			}
		}
	}
}

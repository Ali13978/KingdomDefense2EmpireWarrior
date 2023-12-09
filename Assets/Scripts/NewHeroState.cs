using Gameplay;
using UnityEngine;

public class NewHeroState : NewEntityState
{
	public CharacterModel heroModel;

	public NewHeroState(CharacterModel heroModel, INewFSMController fsmController)
		: base(fsmController)
	{
		this.heroModel = heroModel;
	}

	public override void OnInput(StateInputType inputType, params object[] args)
	{
		base.OnInput(inputType, args);
		switch (inputType)
		{
		case StateInputType.UserAssignPosition:
		{
			Vector3 assignedPosition = (Vector3)args[0];
			heroModel.SetAssignedPosition(assignedPosition);
			SetTransition(EntityStateEnum.HeroMove);
			break;
		}
		case StateInputType.Die:
			SetTransition(EntityStateEnum.HeroDie);
			break;
		case StateInputType.SpecialState:
			SetTransition(EntityStateEnum.HeroSpecialState);
			break;
		case StateInputType.Disappear:
			SetTransition(EntityStateEnum.HeroDisappear);
			break;
		case StateInputType.ThePetOwnerIsMoving:
			SetTransition(EntityStateEnum.HeroMove);
			break;
		}
	}

	public EnemyModel FindTargetInRange()
	{
		return GameTools.GetEnemyWithHighestScore(heroModel);
	}
}

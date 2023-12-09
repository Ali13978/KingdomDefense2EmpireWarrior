using DG.Tweening;
using Gameplay;
using UnityEngine;

public class NewHeroMoveState : NewHeroState
{
	private Vector3 invertXVector = new Vector3(-1f, 1f, 1f);

	public NewHeroMoveState(CharacterModel heroModel, INewFSMController fSMController)
		: base(heroModel, fSMController)
	{
	}

	public override void OnStartState()
	{
		base.OnStartState();
		heroModel.AddTarget(null);
		heroModel.GetAnimationController().ToRunState();
		MoveToAssignedPosition();
	}

	public override void OnExitState()
	{
		base.OnExitState();
		heroModel.transform.DOKill();
	}

	private void MoveToAssignedPosition()
	{
		float duration = Vector2.Distance(heroModel.transform.position, heroModel.GetAssignedPosition()) / (heroModel.GetSpeed() / GameData.PIXEL_PER_UNIT);
		heroModel.transform.DOMove(heroModel.GetAssignedPosition(), duration).SetEase(Ease.Linear).OnComplete(MoveToAssignedPositionComplete);
		heroModel.GetAnimationController().ToRunState();
		ChangeAnimationRun(heroModel.transform.position, heroModel.GetAssignedPosition());
	}

	private void MoveToAssignedPositionComplete()
	{
		SetTransition(EntityStateEnum.HeroIdle);
	}

	private void ChangeAnimationRun(Vector3 currentPosition, Vector3 assignedPosition)
	{
		if (assignedPosition.x - currentPosition.x > 0f)
		{
			heroModel.transform.localScale = Vector3.one;
		}
		else
		{
			heroModel.transform.localScale = invertXVector;
		}
	}
}

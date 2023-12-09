using System.Collections.Generic;

public class NewEntityState : INewEntityState
{
	private INewFSMController fsmController;

	private Dictionary<EntityStateEnum, INewEntityState> targetEnumToTargetState = new Dictionary<EntityStateEnum, INewEntityState>();

	public EntityStateEnum entityStateEnum
	{
		get;
		set;
	}

	public NewEntityState(INewFSMController fsmController)
	{
		this.fsmController = fsmController;
	}

	public void InitTransition(EntityStateEnum stateEnum, INewEntityState state)
	{
		if (!targetEnumToTargetState.ContainsKey(stateEnum))
		{
			targetEnumToTargetState.Add(stateEnum, state);
		}
	}

	public void SetTransition(EntityStateEnum stateEnum)
	{
		if (targetEnumToTargetState.ContainsKey(stateEnum) && fsmController.GetCurrentState() == this)
		{
			OnExitState();
			fsmController.SetCurrentState(targetEnumToTargetState[stateEnum]);
		}
	}

	public virtual void Update(float dt)
	{
	}

	public virtual void OnInput(StateInputType inputType, params object[] args)
	{
	}

	public virtual void OnStartState()
	{
	}

	public virtual void OnExitState()
	{
	}
}

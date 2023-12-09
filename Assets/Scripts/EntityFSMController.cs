using System.Collections.Generic;

public class EntityFSMController : INewFSMController
{
	public List<INewEntityState> backgroundStates = new List<INewEntityState>();

	private INewEntityState currentState;

	public Dictionary<EntityStateEnum, INewEntityState> stateDictionary = new Dictionary<EntityStateEnum, INewEntityState>();

	public INewEntityState secondaryState
	{
		set
		{
			backgroundStates.Add(value);
		}
	}

	public void AddState(EntityStateEnum stateEnum, INewEntityState state)
	{
		stateDictionary.Add(stateEnum, state);
		state.entityStateEnum = stateEnum;
	}

	public void OnUpdate(float dt)
	{
		GetCurrentState().Update(dt);
		if (backgroundStates.Count > 0)
		{
			for (int num = backgroundStates.Count - 1; num >= 0; num--)
			{
				backgroundStates[num].Update(dt);
			}
		}
	}

	public void CreateTransition(EntityStateEnum fromState, EntityStateEnum toState)
	{
		stateDictionary[fromState].InitTransition(toState, stateDictionary[toState]);
	}

	public void CreateTransitionFromAllState(EntityStateEnum toState, params EntityStateEnum[] exceptionStates)
	{
		foreach (KeyValuePair<EntityStateEnum, INewEntityState> item in stateDictionary)
		{
			if (item.Key != toState && IsNotExceptionState(item.Key, exceptionStates))
			{
				CreateTransition(item.Key, toState);
			}
		}
	}

	private bool IsNotExceptionState(EntityStateEnum state, EntityStateEnum[] exceptionStates)
	{
		for (int i = 0; i < exceptionStates.Length; i++)
		{
			if (exceptionStates[i] == state)
			{
				return false;
			}
		}
		return true;
	}

	public INewEntityState GetCurrentState()
	{
		return currentState;
	}

	public void SetCurrentState(INewEntityState state)
	{
		currentState = state;
		state.OnStartState();
	}
}

public interface INewEntityState
{
	EntityStateEnum entityStateEnum
	{
		get;
		set;
	}

	void InitTransition(EntityStateEnum tostateEnum, INewEntityState tostate);

	void Update(float dt);

	void SetTransition(EntityStateEnum stateEnum);

	void OnInput(StateInputType inputType, params object[] args);

	void OnStartState();

	void OnExitState();
}

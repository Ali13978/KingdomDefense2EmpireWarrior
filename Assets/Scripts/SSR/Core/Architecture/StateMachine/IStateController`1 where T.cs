namespace SSR.Core.Architecture.StateMachine
{
	public interface IStateController<T> where T : IStateController<T>
	{
		IStateMachine<T> StateMachine
		{
			get;
			set;
		}

		void Initialize();

		void Activate(IStateController<T> previousStateController);

		void Deactivate(IStateController<T> nextStateController);

		void StateUpdate();
	}
}

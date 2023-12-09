namespace SSR.Core.Architecture.StateMachine
{
	public interface IStateMachine<T> where T : IStateController<T>
	{
		T currentStateController
		{
			get;
		}

		void SetStateController(T stateController);
	}
}

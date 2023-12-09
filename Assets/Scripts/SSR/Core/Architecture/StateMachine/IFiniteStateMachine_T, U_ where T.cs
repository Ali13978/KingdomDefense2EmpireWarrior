using System;

namespace SSR.Core.Architecture.StateMachine
{
	public interface IFiniteStateMachine<T, U> where T : IFiniteStateController<T, U> where U : IConvertible
	{
		U CurrentState
		{
			get;
		}

		void SetState(U state);
	}
}

namespace SSR.Core.Architecture
{
	public interface ICollabHub : ICollabHubRegister
	{
		bool IsFinished
		{
			get;
		}

		void OpenRegistration();

		void CloseRegistration();

		void StartWorking();
	}
}

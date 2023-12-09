namespace SSR.Core.Architecture
{
	public interface ICollabMember
	{
		bool IsFinished
		{
			get;
		}

		void OnStartWorking();
	}
}

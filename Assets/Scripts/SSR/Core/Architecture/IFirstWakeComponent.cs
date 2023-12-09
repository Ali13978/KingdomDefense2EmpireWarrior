namespace SSR.Core.Architecture
{
	public interface IFirstWakeComponent
	{
		bool Awoke
		{
			get;
			set;
		}

		void FirstAwake();
	}
}

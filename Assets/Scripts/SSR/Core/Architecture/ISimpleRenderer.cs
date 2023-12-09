namespace SSR.Core.Architecture
{
	public interface ISimpleRenderer
	{
		bool IsInitialized
		{
			get;
		}

		bool Visible
		{
			get;
		}

		bool SelfVisible
		{
			get;
			set;
		}

		void Initialize();
	}
}

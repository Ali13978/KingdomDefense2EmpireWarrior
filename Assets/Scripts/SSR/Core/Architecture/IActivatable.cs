namespace SSR.Core.Architecture
{
	public interface IActivatable
	{
		bool IsDestroyed
		{
			get;
		}

		bool ActiveSelf
		{
			get;
		}

		bool ActiveInHierarchy
		{
			get;
		}

		void Initialize();

		void SetActiveSelf(bool activeSelf);

		void Destroy();

		void DestroyImmediately();
	}
}

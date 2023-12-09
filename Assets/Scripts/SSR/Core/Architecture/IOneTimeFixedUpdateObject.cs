namespace SSR.Core.Architecture
{
	public interface IOneTimeFixedUpdateObject
	{
		bool Active
		{
			get;
		}

		void OneTimeFixedUpdate();
	}
}

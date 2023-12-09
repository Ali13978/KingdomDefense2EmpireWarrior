namespace SSR.Core.Architecture
{
	public interface IOneTimeFixedUpdateService
	{
		void AddFixedUpdateObject(IOneTimeFixedUpdateObject fixedUpdateObject);

		void RemoveFixedUpdateObject(IOneTimeFixedUpdateObject fixedUpdateObject);
	}
}

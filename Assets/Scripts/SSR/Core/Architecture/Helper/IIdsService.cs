namespace SSR.Core.Architecture.Helper
{
	public interface IIdsService
	{
		void Initialize(int[] ids);

		void FreeId(int Id);

		int GetNewId();
	}
}

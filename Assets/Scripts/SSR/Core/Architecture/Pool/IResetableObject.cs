namespace SSR.Core.Architecture.Pool
{
	public interface IResetableObject
	{
		void SaveCurrentState();

		void ResetToLastSavedState();
	}
}

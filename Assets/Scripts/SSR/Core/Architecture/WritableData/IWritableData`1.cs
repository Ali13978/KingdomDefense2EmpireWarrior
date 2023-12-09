namespace SSR.Core.Architecture.WritableData
{
	public interface IWritableData<T>
	{
		T Data
		{
			get;
			set;
		}

		void SaveData();
	}
}

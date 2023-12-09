namespace Services.PlatformSpecific
{
	public interface IDataSnapshot
	{
		string GetRawJsonValue();

		bool IsCompleted();

		bool IsFaulted();
	}
}

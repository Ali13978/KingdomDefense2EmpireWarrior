namespace Services.PlatformSpecific
{
	public class DataSnapshotEditor : IDataSnapshot
	{
		public string GetRawJsonValue()
		{
			return string.Empty;
		}

		public bool IsCompleted()
		{
			return true;
		}

		public bool IsFaulted()
		{
			return false;
		}
	}
}

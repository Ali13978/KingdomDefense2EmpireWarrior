using Firebase.Database;

namespace Services.PlatformSpecific
{
	public class DataSnapshotAndroid : IDataSnapshot
	{
		private DataSnapshot snapshot;

		private bool isTaskFaulted;

		private bool isTaskCompleted;

		public DataSnapshotAndroid(DataSnapshot snapshot, bool isTaskFaulted, bool isTaskCompleted)
		{
			this.snapshot = snapshot;
			this.isTaskFaulted = isTaskFaulted;
			this.isTaskCompleted = isTaskCompleted;
		}

		public string GetRawJsonValue()
		{
			if (snapshot == null)
			{
				return string.Empty;
			}
			return snapshot.GetRawJsonValue();
		}

		public bool IsCompleted()
		{
			return isTaskCompleted;
		}

		public bool IsFaulted()
		{
			return isTaskFaulted;
		}
	}
}

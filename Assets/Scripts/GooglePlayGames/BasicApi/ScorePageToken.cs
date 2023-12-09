namespace GooglePlayGames.BasicApi
{
	public class ScorePageToken
	{
		private string mId;

		private object mInternalObject;

		private LeaderboardCollection mCollection;

		private LeaderboardTimeSpan mTimespan;

		public LeaderboardCollection Collection => mCollection;

		public LeaderboardTimeSpan TimeSpan => mTimespan;

		public string LeaderboardId => mId;

		internal object InternalObject => mInternalObject;

		internal ScorePageToken(object internalObject, string id, LeaderboardCollection collection, LeaderboardTimeSpan timespan)
		{
			mInternalObject = internalObject;
			mId = id;
			mCollection = collection;
			mTimespan = timespan;
		}
	}
}

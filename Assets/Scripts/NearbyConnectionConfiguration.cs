using GooglePlayGames.OurUtils;
using System;

namespace GooglePlayGames.BasicApi.Nearby
{
	public struct NearbyConnectionConfiguration
	{
		public const int MaxUnreliableMessagePayloadLength = 1168;

		public const int MaxReliableMessagePayloadLength = 4096;

		private readonly Action<InitializationStatus> mInitializationCallback;

		private readonly long mLocalClientId;

		public long LocalClientId => mLocalClientId;

		public Action<InitializationStatus> InitializationCallback => mInitializationCallback;

		public NearbyConnectionConfiguration(Action<InitializationStatus> callback, long localClientId)
		{
			mInitializationCallback = Misc.CheckNotNull(callback);
			mLocalClientId = localClientId;
		}
	}
}

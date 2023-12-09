using System;

namespace Data
{
	[Serializable]
	public class FreeResourcesData
	{
		public bool isUserGetRewardLoggedInFacebook;

		public bool isUserGetRewardLikeFanpage;

		public bool isUserGetRewardJoinGroup;

		public int currentSharePerDay;

		public int currentWatchAdsPerDay;

		public int currentGemCollectedByInvite;
	}
}

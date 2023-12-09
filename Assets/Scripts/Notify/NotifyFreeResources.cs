using Data;

namespace Notify
{
	public class NotifyFreeResources : NotifyUnit
	{
		protected override bool ShouldShowNotify()
		{
			return isFreeResoucesAvailable();
		}

		private bool isFreeResoucesAvailable()
		{
			return !ReadWriteDataFreeResources.Instance.IsUserGetReward_LogInFacebook() || !ReadWriteDataFreeResources.Instance.IsUserGetReward_LikeFanpage() || !ReadWriteDataFreeResources.Instance.IsUserGetReward_JoinGroup() || ReadWriteDataFreeResources.Instance.GetCurrentSharePerDay() > 0 || ReadWriteDataFreeResources.Instance.GetCurrentWatchAdsPerDay() > 0 || ReadWriteDataFreeResources.Instance.GetCurrentGemCollectedByInvite() < 100;
		}
	}
}

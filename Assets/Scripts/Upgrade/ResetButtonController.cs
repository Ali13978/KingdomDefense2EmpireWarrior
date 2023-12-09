using Data;
using Services.PlatformSpecific;

namespace Upgrade
{
	public class ResetButtonController : ButtonController
	{
		public override void OnClick()
		{
			base.OnClick();
			DoReset();
		}

		private void DoReset()
		{
			SendEventResetUpgrade();
			UpgradePopupController.Instance.Reset();
			ReadWriteDataGlobalUpgrade.Instance.OnStarChange(isDispatchEventChange: true);
		}

		private void SendEventResetUpgrade()
		{
			int currentStar = UpgradePopupController.Instance.currentStar;
			int currentStar2 = ReadWriteDataPlayerCurrency.Instance.GetCurrentStar();
			PlatformSpecificServicesProvider.Services.Analytics.SendEvent_ResetGlobalUpgrade(currentStar, currentStar2);
		}
	}
}

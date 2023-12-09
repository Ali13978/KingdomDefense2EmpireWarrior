using Data;
using LifetimePopup;
using MyCustom;
using Services.PlatformSpecific;
using WorldMap;

namespace LinkGame
{
	public class LinkGameButtonController : ButtonController
	{
		private int rewardValue;

		public override void OnClick()
		{
			base.OnClick();
			rewardValue = PlatformSpecificServicesProvider.Services.FacebookServices.GetFreeResources("reward_id_install_goe");
			bool flag = StaticMethod.CheckPackageAppIsPresent(MarketingConfig.goe_packageName);
			bool flag2 = ReadWriteDataOffers.Instance.IsOfferProcessed(ReadWriteDataOffers.KEY_INSTALL_GOE);
			if (flag && !flag2)
			{
				ClaimReward();
			}
			else
			{
				InitPopupLink();
			}
		}

		private void InitPopupLink()
		{
			SingletonMonoBehaviour<UIRootController>.Instance.linkGamePopupController.Init();
		}

		private void ClaimReward()
		{
			ReadWriteDataPlayerCurrency.Instance.ChangeGem(rewardValue, isDispatchEventChange: true);
			RewardItem[] array = new RewardItem[1];
			RewardItem rewardItem = new RewardItem();
			rewardItem.rewardType = RewardType.Gem;
			rewardItem.value = rewardValue;
			rewardItem.isDisplayQuantity = true;
			array[0] = rewardItem;
			SingletonMonoBehaviour<LifetimeCanvas>.Instance.RewardPopupController.Init(array);
			ReadWriteDataOffers.Instance.SetOfferStatus(ReadWriteDataOffers.KEY_INSTALL_GOE, value: true);
			SingletonMonoBehaviour<UIRootController>.Instance.RefrestLinkGameButtonStatus();
		}
	}
}

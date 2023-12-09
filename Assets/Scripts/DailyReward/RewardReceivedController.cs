using Data;
using Gameplay;
using Services.PlatformSpecific;
using UnityEngine;

namespace DailyReward
{
	public class RewardReceivedController : GameplayPopupController
	{
		[SerializeField]
		private BonusReceivedController bonusReceivedController;

		[SerializeField]
		private DailyRewardItem rewardItem;

		[SerializeField]
		private DailyRewardItem bonusItem;

		private int currentDay;

		private DailyRewardConfigData data;

		private RewardType rewardType;

		private int rewardItemID;

		private int rewardItemQuantity;

		private BonusType bonusType;

		private int bonusItemID;

		private int bonusItemQuantity;

		private DailyRewardPopupController dailyRewardPopup;

		public void Init(DailyRewardConfigData data, DailyRewardPopupController dailyRewardPopup)
		{
			this.dailyRewardPopup = dailyRewardPopup;
			Show();
			this.data = data;
			currentDay = ReadWriteDataDailyReward.Instance.GetCurrentDay();
			rewardType = data.REWARDTYPE;
			rewardItemID = data.Rewardid;
			rewardItemQuantity = data.Rewardquantity;
			bonusType = data.BONUSTYPE;
			bonusItemID = data.Bonusid;
			bonusItemQuantity = data.Bonusquantity;
			rewardItem.SetView(0, rewardType, rewardItemID, rewardItemQuantity);
			bonusItem.SetView(0, bonusType, bonusItemID, bonusItemQuantity);
		}

		public void BonusItem()
		{
			bool flag = ReadWriteDataDailyReward.Instance.IsReceivedReward(currentDay);
			switch (rewardType)
			{
			case RewardType.Gem:
				if (!flag)
				{
					ReadWriteDataPlayerCurrency.Instance.ChangeGem(rewardItemQuantity, isDispatchEventChange: true);
					ReadWriteDataDailyReward.Instance.SetReceiveRewardStatus(currentDay);
				}
				break;
			case RewardType.Item:
				if (!flag)
				{
					ReadWriteDataPowerUpItem.Instance.ChangeItemQuantity(rewardItemID, rewardItemQuantity);
					ReadWriteDataDailyReward.Instance.SetReceiveRewardStatus(currentDay);
				}
				break;
			}
			PlatformSpecificServicesProvider.Services.Ad.ShowOfferVideo(OfferVideoCallback);
		}

		private void OfferVideoCallback(bool completed)
		{
			if (completed)
			{
				Hide();
				bonusReceivedController.Init(data, dailyRewardPopup);
			}
			else
			{
				dailyRewardPopup.CloseWithScaleAnimation();
			}
		}

		public void Claim()
		{
			bool flag = ReadWriteDataDailyReward.Instance.IsReceivedReward(currentDay);
			switch (rewardType)
			{
			case RewardType.Gem:
				if (!flag)
				{
					ReadWriteDataPlayerCurrency.Instance.ChangeGem(rewardItemQuantity, isDispatchEventChange: true);
					ReadWriteDataDailyReward.Instance.SetReceiveRewardStatus(currentDay);
				}
				break;
			case RewardType.Item:
				if (!flag)
				{
					ReadWriteDataPowerUpItem.Instance.ChangeItemQuantity(rewardItemID, rewardItemQuantity);
					ReadWriteDataDailyReward.Instance.SetReceiveRewardStatus(currentDay);
				}
				break;
			}
			ReadWriteDataDailyReward.Instance.SetReceiveRewardStatus(currentDay);
			Hide();
			dailyRewardPopup.RefreshRewardItemGroup();
		}

		private void Show()
		{
			OpenWithScaleAnimation();
		}

		public void Hide()
		{
			CloseWithScaleAnimation();
		}
	}
}

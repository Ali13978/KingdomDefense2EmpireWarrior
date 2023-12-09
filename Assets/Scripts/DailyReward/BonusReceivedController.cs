using Data;
using Gameplay;
using UnityEngine;

namespace DailyReward
{
	public class BonusReceivedController : GameplayPopupController
	{
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

		public void Claim()
		{
			bool flag = ReadWriteDataDailyReward.Instance.IsReceivedBonus(currentDay);
			switch (bonusType)
			{
			case BonusType.Gem:
				if (!flag)
				{
					ReadWriteDataPlayerCurrency.Instance.ChangeGem(bonusItemQuantity, isDispatchEventChange: true);
					ReadWriteDataDailyReward.Instance.SetReceiveBonusStatus(currentDay);
				}
				break;
			case BonusType.Item:
				if (!flag)
				{
					ReadWriteDataPowerUpItem.Instance.ChangeItemQuantity(bonusItemID, bonusItemQuantity);
					ReadWriteDataDailyReward.Instance.SetReceiveBonusStatus(currentDay);
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

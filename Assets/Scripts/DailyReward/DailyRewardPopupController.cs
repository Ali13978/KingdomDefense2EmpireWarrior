using Data;
using UnityEngine;

namespace DailyReward
{
	public class DailyRewardPopupController : GameplayPriorityPopupController
	{
		[SerializeField]
		private RewardItemGroupController rewardItemGroupController;

		[SerializeField]
		private RewardReceivedController rewardReceivedController;

		[SerializeField]
		private BonusReceivedController bonusReceivedController;

		public override void InitPriority(PopupPriorityEnum priority)
		{
			base.InitPriority(priority);
			Init();
		}

		public void Init()
		{
			rewardItemGroupController.gameObject.SetActive(value: false);
			rewardReceivedController.gameObject.SetActive(value: false);
			bonusReceivedController.gameObject.SetActive(value: false);
			int currentDay = ReadWriteDataDailyReward.Instance.GetCurrentDay();
			bool flag = ReadWriteDataDailyReward.Instance.IsReceivedReward(currentDay);
			DailyRewardConfigData data = CommonData.Instance.dailyRewardConfig.dataArray[currentDay];
			if (flag)
			{
				RefreshRewardItemGroup();
			}
			else
			{
				rewardReceivedController.Init(data, this);
			}
		}

		public void RefreshRewardItemGroup()
		{
			rewardItemGroupController.Init();
			rewardItemGroupController.RefreshStatus();
		}

		public override void Open()
		{
			base.Open();
			base.gameObject.SetActive(value: true);
		}
	}
}

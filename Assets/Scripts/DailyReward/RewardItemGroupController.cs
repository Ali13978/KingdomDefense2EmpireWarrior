using Gameplay;
using System.Collections.Generic;
using UnityEngine;

namespace DailyReward
{
	public class RewardItemGroupController : GameplayPopupController
	{
		[SerializeField]
		private List<DailyRewardItem> listDailyRewardItem = new List<DailyRewardItem>();

		public void Init()
		{
			Show();
			for (int i = 0; i < listDailyRewardItem.Count; i++)
			{
				DailyRewardConfigData data = CommonData.Instance.dailyRewardConfig.dataArray[i];
				listDailyRewardItem[i].Init(i, data);
			}
		}

		public void RefreshStatus()
		{
			Show();
			for (int i = 0; i < listDailyRewardItem.Count; i++)
			{
				listDailyRewardItem[i].RefreshStatus();
			}
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

using MyCustom;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay.EndGame.Reward
{
	public class ChestGroupController : CustomMonoBehaviour
	{
		[SerializeField]
		private List<ChestItem> listChestItems = new List<ChestItem>();

		public void AutoOpenChest()
		{
			int currentOpenChestTurn = SingletonMonoBehaviour<GameData>.Instance.CurrentOpenChestTurn;
			for (int i = 0; i < currentOpenChestTurn; i++)
			{
				foreach (ChestItem listChestItem in listChestItems)
				{
					if (!listChestItem.IsOpened)
					{
						listChestItem.OnClick();
						break;
					}
				}
			}
		}

		public bool isAvailableChestToOpen()
		{
			bool result = false;
			foreach (ChestItem listChestItem in listChestItems)
			{
				if (!listChestItem.IsOpened)
				{
					result = true;
				}
			}
			return result;
		}

		public void DisplayReward(int chestID, string rewardName, bool isDisplayRewardValue)
		{
			listChestItems[chestID].DisplayReward(rewardName, isDisplayRewardValue);
		}
	}
}

using Data;
using Parameter;
using UnityEngine;
using UnityEngine.UI;

namespace DailyReward
{
	public class DailyRewardItem : MonoBehaviour
	{
		[SerializeField]
		private Text titleDay;

		[SerializeField]
		private Image icon;

		[SerializeField]
		private Text itemQuantity;

		[SerializeField]
		private GameObject selected;

		[SerializeField]
		private GameObject received;

		private int day;

		private RewardType rewardType;

		private int rewardItemID;

		private int rewardItemQuantity;

		private BonusType bonusType;

		private int bonusItemID;

		private int bonusItemQuantity;

		private bool isReceived;

		public void Init(int day, DailyRewardConfigData data)
		{
			this.day = day;
			rewardType = data.REWARDTYPE;
			rewardItemID = data.Rewardid;
			rewardItemQuantity = data.Rewardquantity;
			bonusType = data.BONUSTYPE;
			bonusItemID = data.Bonusid;
			bonusItemQuantity = data.Bonusquantity;
			SetView(day, rewardType, rewardItemID, rewardItemQuantity);
		}

		public void SetView(int day, RewardType rewardType, int itemID, int itemAmount)
		{
			if ((bool)titleDay)
			{
				titleDay.text = Singleton<NotificationDescription>.Instance.GetNotiContent(53) + " " + (day + 1).ToString();
			}
			switch (rewardType)
			{
			case RewardType.Gem:
				icon.sprite = Resources.Load<Sprite>("LuckyChest/Items/lucky_item_gem");
				break;
			case RewardType.Item:
				icon.sprite = Resources.Load<Sprite>($"LuckyChest/Items/lucky_item_pw_{itemID}");
				break;
			}
			itemQuantity.text = itemAmount.ToString();
		}

		public void SetView(int day, BonusType bonusType, int itemID, int itemAmount)
		{
			if ((bool)titleDay)
			{
				titleDay.text = Singleton<NotificationDescription>.Instance.GetNotiContent(53) + " " + (day + 1).ToString();
			}
			switch (bonusType)
			{
			case BonusType.Gem:
				icon.sprite = Resources.Load<Sprite>("LuckyChest/Items/lucky_item_gem");
				break;
			case BonusType.Item:
				icon.sprite = Resources.Load<Sprite>($"LuckyChest/Items/lucky_item_pw_{itemID}");
				break;
			}
			itemQuantity.text = itemAmount.ToString();
		}

		public void RefreshStatus()
		{
			int currentDay = ReadWriteDataDailyReward.Instance.GetCurrentDay();
			isReceived = ReadWriteDataDailyReward.Instance.IsReceivedReward(day);
			ShowNotReceive();
			if (isReceived)
			{
				ShowReceiced();
				ShowHighlight();
			}
			else
			{
				ShowNotReceive();
			}
		}

		public void ShowHighlight()
		{
			if ((bool)selected)
			{
				selected.SetActive(value: true);
			}
		}

		public void ShowReceiced()
		{
			if ((bool)received)
			{
				received.SetActive(value: true);
			}
		}

		public void ShowNotReceive()
		{
			if ((bool)selected)
			{
				selected.SetActive(value: false);
			}
			if ((bool)received)
			{
				received.SetActive(value: false);
			}
		}
	}
}

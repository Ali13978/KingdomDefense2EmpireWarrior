using UnityEngine;

namespace RewardPopup
{
	public class SingleItemGroupController : MonoBehaviour
	{
		[SerializeField]
		private ItemQuantityController gemQuantity;

		[SerializeField]
		private ItemQuantityController lifeQuantity;

		[SerializeField]
		private ItemQuantityController moneyQuantity;

		private RewardType rewardType;

		private int value;

		public ItemQuantityController GemQuantity
		{
			get
			{
				return gemQuantity;
			}
			set
			{
				gemQuantity = value;
			}
		}

		public ItemQuantityController LifeQuantity
		{
			get
			{
				return lifeQuantity;
			}
			set
			{
				lifeQuantity = value;
			}
		}

		public ItemQuantityController MoneyQuantity
		{
			get
			{
				return moneyQuantity;
			}
			set
			{
				moneyQuantity = value;
			}
		}

		public void InitValue(RewardType rewardType, int value)
		{
			this.rewardType = rewardType;
			this.value = value;
			HideAllItems();
		}

		public void Show()
		{
			switch (rewardType)
			{
			case RewardType.Gem:
				GemQuantity.Init(value);
				break;
			case RewardType.Life:
				LifeQuantity.Init(value);
				break;
			case RewardType.Money:
				MoneyQuantity.Init(value);
				break;
			}
			base.gameObject.SetActive(value: true);
		}

		public void Hide()
		{
			base.gameObject.SetActive(value: false);
			HideAllItems();
		}

		private void HideAllItems()
		{
			gemQuantity.Hide();
			lifeQuantity.Hide();
			moneyQuantity.Hide();
		}
	}
}

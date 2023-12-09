using Data;
using LifetimePopup;
using Parameter;
using Services.PlatformSpecific;
using UnityEngine;
using UnityEngine.UI;

namespace Store
{
	public class ItemPowerUp : ButtonController
	{
		[SerializeField]
		private int itemID;

		[SerializeField]
		private Text itemName;

		[SerializeField]
		private Text itemQuantity;

		[SerializeField]
		private Text itemDescription;

		[SerializeField]
		private Text itemPrice;

		public void UpdateItemsQuantity()
		{
			itemQuantity.text = ReadWriteDataPowerUpItem.Instance.GetCurrentItemQuantity(itemID).ToString();
		}

		public void InitInfor()
		{
			itemName.text = Singleton<PowerupItemDescription>.Instance.GetName(itemID);
			UpdateItemsQuantity();
			itemDescription.text = Singleton<PowerupItemDescription>.Instance.GetDescription(itemID).Replace('@', '\n').Replace('#', '-');
			itemPrice.text = Singleton<PowerUpItemParameter>.Instance.GetPrice(itemID).ToString();
		}

		public override void OnClick()
		{
			base.OnClick();
			TryToBuyItem();
		}

		private void TryToBuyItem()
		{
			if (StoreCalculator.IsEnoughMoneyToBuy(itemID))
			{
				ProcessItem();
				return;
			}
			SingletonMonoBehaviour<LifetimeCanvas>.Instance.StorePopupController.PlayAnimationNotEnoughGem();
			string notiContent = Singleton<NotificationDescription>.Instance.GetNotiContent(20);
			SingletonMonoBehaviour<LifetimeCanvas>.Instance.NotifyPopupController.Init(notiContent, isShowButtonFreeResources: true, isShowButtonGoToStore: false);
		}

		private void ProcessItem()
		{
			int price = Singleton<PowerUpItemParameter>.Instance.GetPrice(itemID);
			ReadWriteDataPlayerCurrency.Instance.ChangeGem(-price, isDispatchEventChange: true);
			ReadWriteDataPowerUpItem.Instance.ChangeItemQuantity(itemID, 1);
			UnityEngine.Debug.Log("Mua thành công Item " + itemID);
			SingletonMonoBehaviour<LifetimeCanvas>.Instance.StorePopupController.ShowBuyEffect();
			SingletonMonoBehaviour<LifetimeCanvas>.Instance.StorePopupController.UpdateGemStatus();
			SendEventBuyPowerupItem();
			UISoundManager.Instance.PlayBuySuccess();
			UpdateItemsQuantity();
		}

		private void SendEventBuyPowerupItem()
		{
			int num = itemID;
			int price = Singleton<PowerUpItemParameter>.Instance.GetPrice(num);
			string name = Singleton<PowerupItemDescription>.Instance.GetName(num);
			PlatformSpecificServicesProvider.Services.Analytics.SendEvent_BuyPowerupItem(name, price);
		}
	}
}

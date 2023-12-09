using Data;
using LifetimePopup;
using Parameter;
using Services.PlatformSpecific;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace HeroCamp
{
	public class BuyButtonController : ButtonController
	{
		[SerializeField]
		private Text buyValue_Gem;

		[SerializeField]
		private Text buyValue_Money;

		[SerializeField]
		private GameObject miniGemIcon;

		private int currentHeroID;

		public override void OnClick()
		{
			base.OnClick();
			SendEventClickButtonBuy();
			currentHeroID = HeroCampPopupController.Instance.currentHeroID;
			if (Singleton<UnlockHeroParameter>.Instance.IsHeroUnlockByGem(currentHeroID))
			{
				TryToBuyHeroByGem();
			}
			if (Singleton<UnlockHeroParameter>.Instance.IsHeroUnlockByRealMoney(currentHeroID))
			{
				TryToBuyHeroByRealMoney();
			}
		}

		public void InitBuyPriceGem(int price)
		{
			miniGemIcon.SetActive(value: true);
			buyValue_Gem.gameObject.SetActive(value: true);
			buyValue_Money.gameObject.SetActive(value: false);
			buyValue_Gem.text = price.ToString();
		}

		private void TryToBuyHeroByGem()
		{
			int currentGem = ReadWriteDataPlayerCurrency.Instance.GetCurrentGem();
			int gemAmountToUnlockHero = Singleton<UnlockHeroParameter>.Instance.GetGemAmountToUnlockHero(currentHeroID);
			if (currentGem >= gemAmountToUnlockHero)
			{
				UnityEngine.Debug.Log("Đã đủ Gem để mua, Process Buy");
				SendEventBuyHeroByGemCompleted();
				ReadWriteDataHero.Instance.UnlockHero(currentHeroID);
				ReadWriteDataPlayerCurrency.Instance.ChangeGem(-gemAmountToUnlockHero, isDispatchEventChange: true);
				HeroCampPopupController.Instance.UpgradeNBuyGroupController.RefreshStatus();
				HeroCampPopupController.Instance.ShowUnlockEffect(currentHeroID);
				HeroCampPopupController.Instance.ShowLevelUpEffect();
				UISoundManager.Instance.PlayBuySuccess();
			}
			else
			{
				UnityEngine.Debug.Log("Không đủ Gem!");
				string notiContent = Singleton<NotificationDescription>.Instance.GetNotiContent(20);
				SingletonMonoBehaviour<LifetimeCanvas>.Instance.NotifyPopupController.Init(notiContent, isShowButtonFreeResources: true, isShowButtonGoToStore: true);
			}
		}

		public void InitBuyPriceMoney()
		{
			miniGemIcon.SetActive(value: false);
			buyValue_Money.gameObject.SetActive(value: true);
			buyValue_Gem.gameObject.SetActive(value: false);
			currentHeroID = HeroCampPopupController.Instance.currentHeroID;
			string heroItemID = SingletonMonoBehaviour<LifetimeCanvas>.Instance.StorePopupController.ReadDataShopItemAttribute.GetHeroItemID(currentHeroID);
			decimal localizedProductPrice = PlatformSpecificServicesProvider.Services.InApPurchase.GetLocalizedProductPrice(heroItemID);
			string iSOCurrencyCode = PlatformSpecificServicesProvider.Services.InApPurchase.GetISOCurrencyCode(heroItemID);
			int noDecimalFracment = BitConverter.GetBytes(decimal.GetBits(localizedProductPrice)[3])[2];
			buyValue_Money.text = PlatformSpecificServicesProvider.Services.InApPurchase.GetFormatedProductPrice(iSOCurrencyCode, localizedProductPrice, noDecimalFracment);
		}

		private void TryToBuyHeroByRealMoney()
		{
			currentHeroID = HeroCampPopupController.Instance.currentHeroID;
			string heroItemID = SingletonMonoBehaviour<LifetimeCanvas>.Instance.StorePopupController.ReadDataShopItemAttribute.GetHeroItemID(currentHeroID);
			PlatformSpecificServicesProvider.Services.InApPurchase.PurchaseHero(heroItemID);
		}

		private void SendEventClickButtonBuy()
		{
			PlatformSpecificServicesProvider.Services.Analytics.SendEvent_ClickButtonBuyHero();
		}

		private void SendEventBuyHeroByGemCompleted()
		{
			string heroName = Singleton<HeroDescription>.Instance.GetHeroName(currentHeroID);
			int currentGem = ReadWriteDataPlayerCurrency.Instance.GetCurrentGem();
			int gemAmountToUnlockHero = Singleton<UnlockHeroParameter>.Instance.GetGemAmountToUnlockHero(currentHeroID);
			int currentGem2 = currentGem - gemAmountToUnlockHero;
			int maxMapIDUnlocked = ReadWriteDataMap.Instance.GetMapIDUnlocked() + 1;
			int heroOwnedAmount = ReadWriteDataHero.Instance.GetHeroOwnedAmount();
			PlatformSpecificServicesProvider.Services.Analytics.SendEvent_BoughtHero(currentHeroID, heroName, currentGem, currentGem2, maxMapIDUnlocked, heroOwnedAmount);
		}
	}
}

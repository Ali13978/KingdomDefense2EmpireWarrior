using Data;
using HeroCamp;
using LifetimePopup;
using OfferPopup;
using Store;
using System;
using UnityEngine;

namespace Services.PlatformSpecific.Editor
{
	public class InappPurchaseEditor : MonoBehaviour, IInappPurchase
	{
		[SerializeField]
		private ReadDataShopItemAttribute readDataShopItemAttribute;

		[SerializeField]
		private ReadDataOfferBundle readDataOfferBundle;

		public string GetLocalizedProductDescription(string productID)
		{
			return readDataShopItemAttribute.GetGemPackDescription(productID);
		}

		public decimal GetLocalizedProductPrice(string productID)
		{
			return 0.01m;
		}

		public string GetLocalizedProductPriceString(string productID)
		{
			return readDataShopItemAttribute.GetGemPackPriceString(productID);
		}

		public string GetLocalizedProductTitle(string productID)
		{
			return readDataShopItemAttribute.GetGemPackTitle(productID);
		}

		public string GetFormatedProductPrice(string ISOCurrencyCode, decimal amount, int noDecimalFracment)
		{
			return amount.ToString("C" + noDecimalFracment);
		}

		public string GetISOCurrencyCode(string productID)
		{
			return "$";
		}

		public void PurchaseGem(string productID)
		{
			int gemPackValue = readDataShopItemAttribute.GetGemPackValue(productID);
			ReadWriteDataPlayerCurrency.Instance.ChangeGem(gemPackValue, isDispatchEventChange: true);
			UnityEngine.Debug.Log("Test Mode IAP: Buy " + gemPackValue + " Gem Success!");
			SingletonMonoBehaviour<LifetimeCanvas>.Instance.StorePopupController.UpdateGemStatus();
			PlatformSpecificServicesProvider.Services.Analytics.SendEvent_BuyItem(productID);
			RewardItem[] array = new RewardItem[1];
			RewardItem rewardItem = new RewardItem();
			rewardItem.rewardType = RewardType.Gem;
			rewardItem.value = gemPackValue;
			rewardItem.isDisplayQuantity = true;
			array[0] = rewardItem;
			SingletonMonoBehaviour<LifetimeCanvas>.Instance.RewardPopupController.Init(array);
			PlatformSpecificServicesProvider.Services.DataCloudSaver.AutoBackUpData();
		}

		public void PurchaseOfferBundle(string productID)
		{
			OfferBundleSingleHero offerBundleSingleHero = SingletonMonoBehaviour<LifetimeCanvas>.Instance.OfferPopupController.ReadDataOfferBundle.GetOfferBundleSingleHero(productID);
			int heroID = offerBundleSingleHero.heroID;
			if (!ReadWriteDataHero.Instance.IsHeroOwned(heroID))
			{
				ReadWriteDataHero.Instance.UnlockHero(heroID);
			}
			int[] itemsAmount = offerBundleSingleHero.itemsAmount;
			for (int i = 0; i < itemsAmount.Length; i++)
			{
				ReadWriteDataPowerUpItem.Instance.ChangeItemQuantity(i, itemsAmount[i]);
			}
			UnityEngine.Debug.Log("Test Mode IAP: Buy Hero Bundle " + productID + " Success!");
			RewardItem[] array = new RewardItem[5];
			RewardItem rewardItem = new RewardItem();
			rewardItem.rewardType = RewardType.SingleHero;
			rewardItem.itemID = heroID;
			rewardItem.isDisplayQuantity = false;
			array[0] = rewardItem;
			for (int j = 0; j < itemsAmount.Length; j++)
			{
				RewardItem rewardItem2 = new RewardItem();
				rewardItem2.rewardType = RewardType.Item;
				rewardItem2.itemID = j;
				rewardItem2.value = itemsAmount[j];
				rewardItem2.isDisplayQuantity = true;
				array[j + 1] = rewardItem2;
			}
			SingletonMonoBehaviour<LifetimeCanvas>.Instance.RewardPopupController.Init(array);
			PlatformSpecificServicesProvider.Services.DataCloudSaver.AutoBackUpData();
		}

		public void PurchaseHero(string productID)
		{
			int heroID = readDataShopItemAttribute.GetHeroID(productID);
			if (!ReadWriteDataHero.Instance.IsHeroOwned(heroID))
			{
				ReadWriteDataHero.Instance.UnlockHero(heroID);
			}
			RewardItem[] array = new RewardItem[1];
			RewardItem rewardItem = new RewardItem();
			rewardItem.rewardType = RewardType.SingleHero;
			rewardItem.itemID = heroID;
			rewardItem.isDisplayQuantity = false;
			array[0] = rewardItem;
			SingletonMonoBehaviour<LifetimeCanvas>.Instance.RewardPopupController.Init(array);
			HeroCampPopupController.Instance.UpgradeNBuyGroupController.RefreshStatus();
			PlatformSpecificServicesProvider.Services.DataCloudSaver.AutoBackUpData();
		}

		public void PurchaseSaleBundle(string productID)
		{
			SaleBundleConfigData dataSaleBundle = StoreBundleData.GetDataSaleBundle(productID);
			int[] heroid = dataSaleBundle.Heroid;
			int[] itemids = dataSaleBundle.Itemids;
			int[] itemquatities = dataSaleBundle.Itemquatities;
			int gembonus = dataSaleBundle.Gembonus;
			if (heroid.Length > 0)
			{
				for (int i = 0; i < heroid.Length; i++)
				{
					if (!ReadWriteDataHero.Instance.IsHeroOwned(heroid[i]))
					{
						ReadWriteDataHero.Instance.UnlockHero(heroid[i]);
						if (dataSaleBundle.Havepet)
						{
							ReadWriteDataHero.Instance.UnlockPet(heroid[i]);
						}
						if (dataSaleBundle.Herolevel > 0)
						{
							ReadWriteDataHero.Instance.LevelUpTo(heroid[i], dataSaleBundle.Herolevel);
						}
					}
				}
				if (heroid.Length == 1 && dataSaleBundle.Herolevel == 0)
				{
					SingletonMonoBehaviour<LifetimeCanvas>.Instance.AskToBuyPopupController.InitBuyHeroLevel(productID);
				}
			}
			if (gembonus > 0)
			{
				ReadWriteDataPlayerCurrency.Instance.ChangeGem(gembonus, isDispatchEventChange: true);
			}
			if (itemids.Length > 0)
			{
				for (int j = 0; j < itemids.Length; j++)
				{
					ReadWriteDataPowerUpItem.Instance.ChangeItemQuantity(j, itemquatities[j]);
				}
			}
			RewardItem[] array = new RewardItem[heroid.Length + itemids.Length + ((gembonus > 0) ? 1 : 0)];
			int num = 0;
			for (int k = 0; k < heroid.Length; k++)
			{
				RewardItem rewardItem = new RewardItem();
				rewardItem.rewardType = RewardType.SingleHero;
				rewardItem.itemID = heroid[k];
				rewardItem.isDisplayQuantity = false;
				array[k] = rewardItem;
				num++;
			}
			if (gembonus > 0)
			{
				RewardItem rewardItem2 = new RewardItem();
				rewardItem2.rewardType = RewardType.Gem;
				rewardItem2.value = gembonus;
				rewardItem2.isDisplayQuantity = true;
				array[num] = rewardItem2;
				num++;
			}
			for (int l = 0; l < itemids.Length; l++)
			{
				RewardItem rewardItem3 = new RewardItem();
				rewardItem3.rewardType = RewardType.Item;
				rewardItem3.itemID = itemids[l];
				rewardItem3.value = itemquatities[l];
				rewardItem3.isDisplayQuantity = true;
				array[num] = rewardItem3;
				num++;
			}
			if (dataSaleBundle.Bundletype.Equals(StoreBundleType.Starter.ToString()))
			{
				UnityEngine.Debug.Log("mua thành công bundle starter!");
				ReadWriteDataSaleBundle.Instance.SetSpecialPackBought(dataSaleBundle.Productid);
				ReadWriteDataSaleBundle.Instance.SetLastTimePlay();
			}
			if (dataSaleBundle.Bundletype.Equals(StoreBundleType.TimeLimited.ToString()))
			{
				UnityEngine.Debug.Log("mua thành công bundle limited!");
				ReadWriteDataSaleBundle.Instance.SetSpecialPackBought(dataSaleBundle.Productid);
				ReadWriteDataSaleBundle.Instance.SetLastTimePlay();
			}
			SingletonMonoBehaviour<LifetimeCanvas>.Instance.StorePopupController.SaleBundleGroupController.RefreshItemStatus();
			SingletonMonoBehaviour<LifetimeCanvas>.Instance.RewardPopupController.Init(array);
		}

		public void PurchaseHeroMaxLevel(string productID, string heroItemID)
		{
			SaleBundleConfigData dataSaleBundle = StoreBundleData.GetDataSaleBundle(heroItemID);
			int[] heroid = dataSaleBundle.Heroid;
			if (heroid.Length == 1)
			{
				ReadWriteDataHero.Instance.LevelUpTo(heroid[0], 9);
				SingletonMonoBehaviour<LifetimeCanvas>.Instance.AskToBuyPopupController.InitBuyHeroPet(heroItemID);
			}
		}

		public void PurchaseHeroPet(string productID, string heroItemID)
		{
			SaleBundleConfigData dataSaleBundle = StoreBundleData.GetDataSaleBundle(heroItemID);
			int[] heroid = dataSaleBundle.Heroid;
			if (heroid.Length == 1)
			{
				ReadWriteDataHero.Instance.UnlockPet(heroid[0]);
				string localization = GameTools.GetLocalization("CONFIRMED_BUY_PET_HERO");
				SingletonMonoBehaviour<LifetimeCanvas>.Instance.NotifyPopupController.Init(localization, isShowButtonFreeResources: false, isShowButtonGoToStore: false);
			}
		}

		public void PurchaseSubscription(string subscritionId, SubscriptionTypeEnum subType)
		{
			SaleBundleConfigData dataSaleBundle = StoreBundleData.GetDataSaleBundle(subscritionId);
			UnityEngine.Debug.Log("____ processing subscription " + subscritionId + " " + dataSaleBundle);
			switch (subType)
			{
			case SubscriptionTypeEnum.dailyBooster:
			{
				DateTime moment = GameTools.GetMoment0(GameTools.GetNow());
				GameTools.SetEndSubscriptionTime(subType, moment.AddDays(dataSaleBundle.Subcribedur).AddMinutes(-4.0));
				GameTools.SetLastTimeCheckInSubscription(subType, moment.AddDays(-1.0));
				DailyCheckinManager.Instance.CheckDailyBooster();
				break;
			}
			case SubscriptionTypeEnum.doubleAttack:
			case SubscriptionTypeEnum.fiftyPercentAtkBoost:
			{
				DateTime localTime = GameTools.GetNow().AddDays(dataSaleBundle.Subcribedur);
				GameTools.SetEndSubscriptionTime(subType, localTime);
				SingletonMonoBehaviour<LifetimeCanvas>.Instance.NotifyPopupController.Init(string.Format(GameTools.GetLocalization("ATTACK_BOOSTER_NOTI"), localTime.ToString("MM\\/dd\\/yyyy HH:mm"), dataSaleBundle.Itemquatities[0]), "OK", null);
				break;
			}
			}
			GameEventCenter.Instance.Trigger(GameEventType.OnCompletePurchase, null);
		}
	}
}

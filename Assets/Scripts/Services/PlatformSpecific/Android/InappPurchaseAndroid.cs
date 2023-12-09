using Data;
using HeroCamp;
using LifetimePopup;
using OfferPopup;
using Store;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Security;
using UnityEngine.SceneManagement;

namespace Services.PlatformSpecific.Android
{
	public class InappPurchaseAndroid : MonoBehaviour, IInappPurchase, IStoreListener
	{
		private static IStoreController m_StoreController;

		private static IExtensionProvider m_StoreExtensionProvider;

		private Dictionary<string, string> introductory_info_dict;

		[SerializeField]
		private ReadDataShopItemAttribute readDataShopItemAttribute;

		[SerializeField]
		private ReadDataOfferBundle readDataOfferBundle;

		private string tempHeroItemID = string.Empty;

		private void Start()
		{
			if (m_StoreController == null)
			{
				InitializePurchasing();
			}
		}

		public void InitializePurchasing()
		{
			if (!IsInitialized())
			{
				ConfigurationBuilder configurationBuilder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
				configurationBuilder.AddProduct("kd.v2.gem_pack_1", ProductType.Consumable);
				configurationBuilder.AddProduct("kd.v2.gem_pack_2", ProductType.Consumable);
				configurationBuilder.AddProduct("kd.v2.gem_pack_3", ProductType.Consumable);
				configurationBuilder.AddProduct("kd.v2.gem_pack_4", ProductType.Consumable);
				configurationBuilder.AddProduct("kd.v2.gem_pack_5", ProductType.Consumable);
				configurationBuilder.AddProduct(MarketingConfig.productIDHeroPackOffer[0], ProductType.NonConsumable);
				configurationBuilder.AddProduct(MarketingConfig.productIDHeroPackOffer[3], ProductType.NonConsumable);
				configurationBuilder.AddProduct(MarketingConfig.productIDHeroPackOffer[4], ProductType.NonConsumable);
				configurationBuilder.AddProduct(MarketingConfig.productIDHeroPackOffer[5], ProductType.NonConsumable);
				configurationBuilder.AddProduct(MarketingConfig.productIDHeroPackOffer[6], ProductType.NonConsumable);
				configurationBuilder.AddProduct(MarketingConfig.productIDHeroPackOffer[7], ProductType.NonConsumable);
				configurationBuilder.AddProduct(MarketingConfig.productIDHeroPackOffer[8], ProductType.NonConsumable);
				configurationBuilder.AddProduct(MarketingConfig.productIDHeroPackOffer[9], ProductType.NonConsumable);
				configurationBuilder.AddProduct(MarketingConfig.productIDHeroPack[0], ProductType.NonConsumable);
				configurationBuilder.AddProduct(MarketingConfig.productIDHeroPack[1], ProductType.NonConsumable);
				configurationBuilder.AddProduct(MarketingConfig.productIDHeroPackSale[0], ProductType.NonConsumable);
				configurationBuilder.AddProduct(MarketingConfig.productIDHeroPackSale[1], ProductType.NonConsumable);
				configurationBuilder.AddProduct(MarketingConfig.productIDHeroPackSale[2], ProductType.NonConsumable);
				configurationBuilder.AddProduct(MarketingConfig.productIDHeroPackSale[3], ProductType.NonConsumable);
				configurationBuilder.AddProduct(MarketingConfig.productIDHeroPackSale[4], ProductType.NonConsumable);
				configurationBuilder.AddProduct(MarketingConfig.productIDHeroPackSale[5], ProductType.NonConsumable);
				configurationBuilder.AddProduct(MarketingConfig.productIDHeroPackSale[6], ProductType.NonConsumable);
				configurationBuilder.AddProduct(MarketingConfig.productIDHeroPackSale[7], ProductType.NonConsumable);
				configurationBuilder.AddProduct(MarketingConfig.productIDHeroPackSale[8], ProductType.NonConsumable);
				configurationBuilder.AddProduct(MarketingConfig.productIDSpecialPack[0], ProductType.Consumable);
				configurationBuilder.AddProduct(MarketingConfig.productIDSpecialPack[1], ProductType.NonConsumable);
				configurationBuilder.AddProduct(MarketingConfig.productIDSpecialPack[2], ProductType.NonConsumable);
				configurationBuilder.AddProduct(MarketingConfig.productIDSpecialPack[3], ProductType.NonConsumable);
				configurationBuilder.AddProduct(MarketingConfig.productIDSpecialPack[4], ProductType.Subscription);
				configurationBuilder.AddProduct(MarketingConfig.productIDSpecialPack[5], ProductType.Consumable);
				configurationBuilder.AddProduct(MarketingConfig.productIDSpecialPack[6], ProductType.Consumable);
				configurationBuilder.AddProduct(MarketingConfig.productIDSpecialPack[7], ProductType.Consumable);
				configurationBuilder.AddProduct(MarketingConfig.productIDSpecialPack[8], ProductType.Subscription);
				configurationBuilder.AddProduct(MarketingConfig.productIDItemsPack[0], ProductType.Consumable);
				configurationBuilder.AddProduct(MarketingConfig.productIDComboHeroes[0], ProductType.NonConsumable);
				UnityPurchasing.Initialize(this, configurationBuilder);
				UnityEngine.Debug.Log("Unity purchase Init products completed!");
			}
		}

		private bool IsInitialized()
		{
			return m_StoreController != null && m_StoreExtensionProvider != null;
		}

		private void BuyProductID(string productId)
		{
			if (IsInitialized())
			{
				Product product = m_StoreController.products.WithID(productId);
				if (product != null && product.availableToPurchase)
				{
					UnityEngine.Debug.Log($"Purchasing product asychronously: '{product.definition.id}'");
					m_StoreController.InitiatePurchase(product);
					decimal localizedProductPrice = GetLocalizedProductPrice(productId);
					string iSOCurrencyCode = GetISOCurrencyCode(productId);
					PlatformSpecificServicesProvider.Services.Analytics.SendEvent_BeginCheckout(localizedProductPrice, iSOCurrencyCode);
				}
				else
				{
					UnityEngine.Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
				}
			}
			else
			{
				UnityEngine.Debug.Log("BuyProductID FAIL. Not initialized.");
			}
		}

		public void RestorePurchases()
		{
			if (!IsInitialized())
			{
				UnityEngine.Debug.Log("RestorePurchases FAIL. Not initialized.");
			}
			else if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXPlayer)
			{
				UnityEngine.Debug.Log("RestorePurchases started ...");
				IAppleExtensions extension = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
				extension.RestoreTransactions(delegate(bool result)
				{
					UnityEngine.Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
				});
			}
			else
			{
				UnityEngine.Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
			}
		}

		public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
		{
			UnityEngine.Debug.Log(">>>>>>>> OnInitialized: PASS");
			m_StoreController = controller;
			m_StoreExtensionProvider = extensions;
			ITransactionHistoryExtensions extension = extensions.GetExtension<ITransactionHistoryExtensions>();
			IGooglePlayStoreExtensions extension2 = extensions.GetExtension<IGooglePlayStoreExtensions>();
			introductory_info_dict = null;
			Product[] all = controller.products.all;
			foreach (Product product in all)
			{
				if (!product.availableToPurchase)
				{
					continue;
				}
				UnityEngine.Debug.Log(string.Join(" - ", new string[7]
				{
					product.metadata.localizedTitle,
					product.metadata.localizedDescription,
					product.metadata.isoCurrencyCode,
					product.metadata.localizedPrice.ToString(),
					product.metadata.localizedPriceString,
					product.transactionID,
					product.receipt
				}));
				if (product.receipt != null)
				{
					if (product.definition.type == ProductType.Subscription)
					{
						if (checkIfProductIsAvailableForSubscriptionManager(product.receipt))
						{
							string intro_json = (introductory_info_dict != null && introductory_info_dict.ContainsKey(product.definition.storeSpecificId)) ? introductory_info_dict[product.definition.storeSpecificId] : null;
							SubscriptionManager subscriptionManager = new SubscriptionManager(product, intro_json);
							SubscriptionInfo subscriptionInfo = subscriptionManager.getSubscriptionInfo();
							SubscriptionTypeEnum subscriptionTypeEnum = GameTools.productIdToSubscriptionEnum[subscriptionInfo.getProductId()];
							if (subscriptionInfo.isExpired() == Result.False)
							{
								UnityEngine.Debug.LogFormat(">>>>>>>>> set {0} expire date: {1}", subscriptionInfo.getProductId(), subscriptionInfo.getExpireDate().ToLocalTime());
								if (subscriptionTypeEnum == SubscriptionTypeEnum.dailyBooster)
								{
									GameTools.SetEndSubscriptionTime(subscriptionTypeEnum, GameTools.GetMoment0(subscriptionInfo.getExpireDate().ToLocalTime()));
									DateTime lastTimeCheckInSubscription = GameTools.GetLastTimeCheckInSubscription(SubscriptionTypeEnum.dailyBooster);
									if ((GameTools.GetNow() - lastTimeCheckInSubscription).Days > 14)
									{
										GameTools.SetLastTimeCheckInSubscription(SubscriptionTypeEnum.dailyBooster, GameTools.GetNow());
									}
								}
								else
								{
									GameTools.SetEndSubscriptionTime(subscriptionTypeEnum, subscriptionInfo.getExpireDate().ToLocalTime());
								}
							}
							else
							{
								GameTools.SetEndSubscriptionTime(subscriptionTypeEnum, GameTools.GetNow().AddDays(-1.0));
							}
						}
						else
						{
							UnityEngine.Debug.Log("This product is not available for SubscriptionManager class, only products that are purchase by 1.19+ SDK can use this class.");
						}
					}
					else
					{
						UnityEngine.Debug.Log("the product is not a subscription product");
					}
				}
				else
				{
					UnityEngine.Debug.Log("the product should have a valid receipt");
				}
			}
		}

		private bool checkIfProductIsAvailableForSubscriptionManager(string receipt)
		{
			Dictionary<string, object> dictionary = (Dictionary<string, object>)MiniJson.JsonDecode(receipt);
			if (!dictionary.ContainsKey("Store") || !dictionary.ContainsKey("Payload"))
			{
				UnityEngine.Debug.Log("The product receipt does not contain enough information");
				return false;
			}
			string text = (string)dictionary["Store"];
			string text2 = (string)dictionary["Payload"];
			if (text2 != null)
			{
				if (text != null)
				{
					if (text == "GooglePlay")
					{
						Dictionary<string, object> dictionary2 = (Dictionary<string, object>)MiniJson.JsonDecode(text2);
						if (!dictionary2.ContainsKey("json"))
						{
							UnityEngine.Debug.Log("The product receipt does not contain enough information, the 'json' field is missing");
							return false;
						}
						Dictionary<string, object> dictionary3 = (Dictionary<string, object>)MiniJson.JsonDecode((string)dictionary2["json"]);
						if (dictionary3 == null || !dictionary3.ContainsKey("developerPayload"))
						{
							UnityEngine.Debug.Log("The product receipt does not contain enough information, the 'developerPayload' field is missing");
							return false;
						}
						string json = (string)dictionary3["developerPayload"];
						Dictionary<string, object> dictionary4 = (Dictionary<string, object>)MiniJson.JsonDecode(json);
						if (dictionary4 == null || !dictionary4.ContainsKey("is_free_trial") || !dictionary4.ContainsKey("has_introductory_price_trial"))
						{
							UnityEngine.Debug.Log("The product receipt does not contain enough information, the product is not purchased using 1.19 or later");
							return false;
						}
						return true;
					}
					if (text == "AppleAppStore" || text == "AmazonApps" || text == "MacAppStore")
					{
						return true;
					}
				}
				return false;
			}
			return false;
		}

		public void OnInitializeFailed(InitializationFailureReason error)
		{
			UnityEngine.Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);
		}

		public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
		{
			bool flag = true;
			//CrossPlatformValidator crossPlatformValidator = new CrossPlatformValidator(GooglePlayTangle.Data(), AppleTangle.Data(), Application.identifier);
			//try
			//{
			//	IPurchaseReceipt[] array = crossPlatformValidator.Validate(args.purchasedProduct.receipt);
			//	UnityEngine.Debug.Log("Receipt is valid. Contents:");
			//	IPurchaseReceipt[] array2 = array;
			//	foreach (IPurchaseReceipt purchaseReceipt in array2)
			//	{
			//		UnityEngine.Debug.Log(purchaseReceipt.productID);
			//		UnityEngine.Debug.Log(purchaseReceipt.purchaseDate);
			//		UnityEngine.Debug.Log(purchaseReceipt.transactionID);
			//	}
			//}
			//catch (IAPSecurityException)
			//{
			//	UnityEngine.Debug.Log("Invalid receipt, not unlocking content");
			//	flag = false;
			//}
			if (!flag)
			{
				return PurchaseProcessingResult.Complete;
			}
			if (string.Equals(args.purchasedProduct.definition.id, "kd.v2.gem_pack_1", StringComparison.Ordinal))
			{
				UnityEngine.Debug.Log($"ProcessPurchase: PASS. Product: '{args.purchasedProduct.definition.id}'");
				int gemPackValue = readDataShopItemAttribute.GetGemPackValue("kd.v2.gem_pack_1");
				ReadWriteDataPlayerCurrency.Instance.ChangeGem(gemPackValue, isDispatchEventChange: true);
				PlatformSpecificServicesProvider.Services.Analytics.SendEvent_BuyItem("kd.v2.gem_pack_1");
				RewardItem[] array3 = new RewardItem[1];
				RewardItem rewardItem = new RewardItem();
				rewardItem.rewardType = RewardType.Gem;
				rewardItem.value = gemPackValue;
				rewardItem.isDisplayQuantity = true;
				array3[0] = rewardItem;
				SingletonMonoBehaviour<LifetimeCanvas>.Instance.RewardPopupController.Init(array3);
			}
			else if (string.Equals(args.purchasedProduct.definition.id, "kd.v2.gem_pack_2", StringComparison.Ordinal))
			{
				UnityEngine.Debug.Log($"ProcessPurchase: PASS. Product: '{args.purchasedProduct.definition.id}'");
				int gemPackValue2 = readDataShopItemAttribute.GetGemPackValue("kd.v2.gem_pack_2");
				ReadWriteDataPlayerCurrency.Instance.ChangeGem(gemPackValue2, isDispatchEventChange: true);
				PlatformSpecificServicesProvider.Services.Analytics.SendEvent_BuyItem("kd.v2.gem_pack_2");
				RewardItem[] array4 = new RewardItem[1];
				RewardItem rewardItem2 = new RewardItem();
				rewardItem2.rewardType = RewardType.Gem;
				rewardItem2.value = gemPackValue2;
				rewardItem2.isDisplayQuantity = true;
				array4[0] = rewardItem2;
				SingletonMonoBehaviour<LifetimeCanvas>.Instance.RewardPopupController.Init(array4);
			}
			else if (string.Equals(args.purchasedProduct.definition.id, "kd.v2.gem_pack_3", StringComparison.Ordinal))
			{
				UnityEngine.Debug.Log($"ProcessPurchase: PASS. Product: '{args.purchasedProduct.definition.id}'");
				int gemPackValue3 = readDataShopItemAttribute.GetGemPackValue("kd.v2.gem_pack_3");
				ReadWriteDataPlayerCurrency.Instance.ChangeGem(gemPackValue3, isDispatchEventChange: true);
				PlatformSpecificServicesProvider.Services.Analytics.SendEvent_BuyItem("kd.v2.gem_pack_3");
				RewardItem[] array5 = new RewardItem[1];
				RewardItem rewardItem3 = new RewardItem();
				rewardItem3.rewardType = RewardType.Gem;
				rewardItem3.value = gemPackValue3;
				rewardItem3.isDisplayQuantity = true;
				array5[0] = rewardItem3;
				SingletonMonoBehaviour<LifetimeCanvas>.Instance.RewardPopupController.Init(array5);
			}
			else if (string.Equals(args.purchasedProduct.definition.id, "kd.v2.gem_pack_4", StringComparison.Ordinal))
			{
				UnityEngine.Debug.Log($"ProcessPurchase: PASS. Product: '{args.purchasedProduct.definition.id}'");
				int gemPackValue4 = readDataShopItemAttribute.GetGemPackValue("kd.v2.gem_pack_4");
				ReadWriteDataPlayerCurrency.Instance.ChangeGem(gemPackValue4, isDispatchEventChange: true);
				PlatformSpecificServicesProvider.Services.Analytics.SendEvent_BuyItem("kd.v2.gem_pack_4");
				RewardItem[] array6 = new RewardItem[1];
				RewardItem rewardItem4 = new RewardItem();
				rewardItem4.rewardType = RewardType.Gem;
				rewardItem4.value = gemPackValue4;
				rewardItem4.isDisplayQuantity = true;
				array6[0] = rewardItem4;
				SingletonMonoBehaviour<LifetimeCanvas>.Instance.RewardPopupController.Init(array6);
			}
			else if (string.Equals(args.purchasedProduct.definition.id, "kd.v2.gem_pack_5", StringComparison.Ordinal))
			{
				UnityEngine.Debug.Log($"ProcessPurchase: PASS. Product: '{args.purchasedProduct.definition.id}'");
				int gemPackValue5 = readDataShopItemAttribute.GetGemPackValue("kd.v2.gem_pack_5");
				ReadWriteDataPlayerCurrency.Instance.ChangeGem(gemPackValue5, isDispatchEventChange: true);
				PlatformSpecificServicesProvider.Services.Analytics.SendEvent_BuyItem("kd.v2.gem_pack_5");
				RewardItem[] array7 = new RewardItem[1];
				RewardItem rewardItem5 = new RewardItem();
				rewardItem5.rewardType = RewardType.Gem;
				rewardItem5.value = gemPackValue5;
				rewardItem5.isDisplayQuantity = true;
				array7[0] = rewardItem5;
				SingletonMonoBehaviour<LifetimeCanvas>.Instance.RewardPopupController.Init(array7);
			}
			else if (string.Equals(args.purchasedProduct.definition.id, MarketingConfig.productIDHeroPackOffer[0], StringComparison.Ordinal))
			{
				OfferBundleSingleHero offerBundleSingleHero = readDataOfferBundle.GetOfferBundleSingleHero(MarketingConfig.productIDHeroPackOffer[0]);
				int heroID = offerBundleSingleHero.heroID;
				if (!ReadWriteDataHero.Instance.IsHeroOwned(heroID))
				{
					ReadWriteDataHero.Instance.UnlockHero(heroID);
				}
				int[] itemsAmount = offerBundleSingleHero.itemsAmount;
				for (int j = 0; j < itemsAmount.Length; j++)
				{
					ReadWriteDataPowerUpItem.Instance.ChangeItemQuantity(j, itemsAmount[j]);
				}
				RewardItem[] array8 = new RewardItem[5];
				RewardItem rewardItem6 = new RewardItem();
				rewardItem6.rewardType = RewardType.SingleHero;
				rewardItem6.itemID = heroID;
				rewardItem6.isDisplayQuantity = false;
				array8[0] = rewardItem6;
				for (int k = 0; k < itemsAmount.Length; k++)
				{
					RewardItem rewardItem7 = new RewardItem();
					rewardItem7.rewardType = RewardType.Item;
					rewardItem7.itemID = k;
					rewardItem7.value = itemsAmount[k];
					rewardItem7.isDisplayQuantity = true;
					array8[k + 1] = rewardItem7;
				}
				SingletonMonoBehaviour<LifetimeCanvas>.Instance.RewardPopupController.Init(array8);
				UnityEngine.Debug.Log($"ProcessPurchase: PASS. Product: '{args.purchasedProduct.definition.id}'");
				PlatformSpecificServicesProvider.Services.Analytics.SendEvent_BuyItem(MarketingConfig.productIDHeroPackOffer[0]);
			}
			else if (string.Equals(args.purchasedProduct.definition.id, MarketingConfig.productIDHeroPackOffer[3], StringComparison.Ordinal))
			{
				OfferBundleSingleHero offerBundleSingleHero2 = readDataOfferBundle.GetOfferBundleSingleHero(MarketingConfig.productIDHeroPackOffer[3]);
				int heroID2 = offerBundleSingleHero2.heroID;
				if (!ReadWriteDataHero.Instance.IsHeroOwned(heroID2))
				{
					ReadWriteDataHero.Instance.UnlockHero(heroID2);
				}
				int[] itemsAmount2 = offerBundleSingleHero2.itemsAmount;
				for (int l = 0; l < itemsAmount2.Length; l++)
				{
					ReadWriteDataPowerUpItem.Instance.ChangeItemQuantity(l, itemsAmount2[l]);
				}
				RewardItem[] array9 = new RewardItem[5];
				RewardItem rewardItem8 = new RewardItem();
				rewardItem8.rewardType = RewardType.SingleHero;
				rewardItem8.itemID = heroID2;
				rewardItem8.isDisplayQuantity = false;
				array9[0] = rewardItem8;
				for (int m = 0; m < itemsAmount2.Length; m++)
				{
					RewardItem rewardItem9 = new RewardItem();
					rewardItem9.rewardType = RewardType.Item;
					rewardItem9.itemID = m;
					rewardItem9.value = itemsAmount2[m];
					rewardItem9.isDisplayQuantity = true;
					array9[m + 1] = rewardItem9;
				}
				SingletonMonoBehaviour<LifetimeCanvas>.Instance.RewardPopupController.Init(array9);
				UnityEngine.Debug.Log($"ProcessPurchase: PASS. Product: '{args.purchasedProduct.definition.id}'");
				PlatformSpecificServicesProvider.Services.Analytics.SendEvent_BuyItem(MarketingConfig.productIDHeroPackOffer[3]);
			}
			else if (string.Equals(args.purchasedProduct.definition.id, MarketingConfig.productIDHeroPackOffer[4], StringComparison.Ordinal))
			{
				OfferBundleSingleHero offerBundleSingleHero3 = readDataOfferBundle.GetOfferBundleSingleHero(MarketingConfig.productIDHeroPackOffer[4]);
				int heroID3 = offerBundleSingleHero3.heroID;
				if (!ReadWriteDataHero.Instance.IsHeroOwned(heroID3))
				{
					ReadWriteDataHero.Instance.UnlockHero(heroID3);
				}
				int[] itemsAmount3 = offerBundleSingleHero3.itemsAmount;
				for (int n = 0; n < itemsAmount3.Length; n++)
				{
					ReadWriteDataPowerUpItem.Instance.ChangeItemQuantity(n, itemsAmount3[n]);
				}
				RewardItem[] array10 = new RewardItem[5];
				RewardItem rewardItem10 = new RewardItem();
				rewardItem10.rewardType = RewardType.SingleHero;
				rewardItem10.itemID = heroID3;
				rewardItem10.isDisplayQuantity = false;
				array10[0] = rewardItem10;
				for (int num = 0; num < itemsAmount3.Length; num++)
				{
					RewardItem rewardItem11 = new RewardItem();
					rewardItem11.rewardType = RewardType.Item;
					rewardItem11.itemID = num;
					rewardItem11.value = itemsAmount3[num];
					rewardItem11.isDisplayQuantity = true;
					array10[num + 1] = rewardItem11;
				}
				SingletonMonoBehaviour<LifetimeCanvas>.Instance.RewardPopupController.Init(array10);
				UnityEngine.Debug.Log($"ProcessPurchase: PASS. Product: '{args.purchasedProduct.definition.id}'");
				PlatformSpecificServicesProvider.Services.Analytics.SendEvent_BuyItem(MarketingConfig.productIDHeroPackOffer[4]);
			}
			else if (string.Equals(args.purchasedProduct.definition.id, MarketingConfig.productIDHeroPackOffer[5], StringComparison.Ordinal))
			{
				OfferBundleSingleHero offerBundleSingleHero4 = readDataOfferBundle.GetOfferBundleSingleHero(MarketingConfig.productIDHeroPackOffer[5]);
				int heroID4 = offerBundleSingleHero4.heroID;
				if (!ReadWriteDataHero.Instance.IsHeroOwned(heroID4))
				{
					ReadWriteDataHero.Instance.UnlockHero(heroID4);
				}
				int[] itemsAmount4 = offerBundleSingleHero4.itemsAmount;
				for (int num2 = 0; num2 < itemsAmount4.Length; num2++)
				{
					ReadWriteDataPowerUpItem.Instance.ChangeItemQuantity(num2, itemsAmount4[num2]);
				}
				RewardItem[] array11 = new RewardItem[5];
				RewardItem rewardItem12 = new RewardItem();
				rewardItem12.rewardType = RewardType.SingleHero;
				rewardItem12.itemID = heroID4;
				rewardItem12.isDisplayQuantity = false;
				array11[0] = rewardItem12;
				for (int num3 = 0; num3 < itemsAmount4.Length; num3++)
				{
					RewardItem rewardItem13 = new RewardItem();
					rewardItem13.rewardType = RewardType.Item;
					rewardItem13.itemID = num3;
					rewardItem13.value = itemsAmount4[num3];
					rewardItem13.isDisplayQuantity = true;
					array11[num3 + 1] = rewardItem13;
				}
				SingletonMonoBehaviour<LifetimeCanvas>.Instance.RewardPopupController.Init(array11);
				UnityEngine.Debug.Log($"ProcessPurchase: PASS. Product: '{args.purchasedProduct.definition.id}'");
				PlatformSpecificServicesProvider.Services.Analytics.SendEvent_BuyItem(MarketingConfig.productIDHeroPackOffer[5]);
			}
			else if (string.Equals(args.purchasedProduct.definition.id, MarketingConfig.productIDHeroPackOffer[6], StringComparison.Ordinal))
			{
				OfferBundleSingleHero offerBundleSingleHero5 = readDataOfferBundle.GetOfferBundleSingleHero(MarketingConfig.productIDHeroPackOffer[6]);
				int heroID5 = offerBundleSingleHero5.heroID;
				if (!ReadWriteDataHero.Instance.IsHeroOwned(heroID5))
				{
					ReadWriteDataHero.Instance.UnlockHero(heroID5);
				}
				int[] itemsAmount5 = offerBundleSingleHero5.itemsAmount;
				for (int num4 = 0; num4 < itemsAmount5.Length; num4++)
				{
					ReadWriteDataPowerUpItem.Instance.ChangeItemQuantity(num4, itemsAmount5[num4]);
				}
				RewardItem[] array12 = new RewardItem[5];
				RewardItem rewardItem14 = new RewardItem();
				rewardItem14.rewardType = RewardType.SingleHero;
				rewardItem14.itemID = heroID5;
				rewardItem14.isDisplayQuantity = false;
				array12[0] = rewardItem14;
				for (int num5 = 0; num5 < itemsAmount5.Length; num5++)
				{
					RewardItem rewardItem15 = new RewardItem();
					rewardItem15.rewardType = RewardType.Item;
					rewardItem15.itemID = num5;
					rewardItem15.value = itemsAmount5[num5];
					rewardItem15.isDisplayQuantity = true;
					array12[num5 + 1] = rewardItem15;
				}
				SingletonMonoBehaviour<LifetimeCanvas>.Instance.RewardPopupController.Init(array12);
				UnityEngine.Debug.Log($"ProcessPurchase: PASS. Product: '{args.purchasedProduct.definition.id}'");
				PlatformSpecificServicesProvider.Services.Analytics.SendEvent_BuyItem(MarketingConfig.productIDHeroPackOffer[6]);
			}
			else if (string.Equals(args.purchasedProduct.definition.id, MarketingConfig.productIDHeroPackOffer[7], StringComparison.Ordinal))
			{
				OfferBundleSingleHero offerBundleSingleHero6 = readDataOfferBundle.GetOfferBundleSingleHero(MarketingConfig.productIDHeroPackOffer[7]);
				int heroID6 = offerBundleSingleHero6.heroID;
				if (!ReadWriteDataHero.Instance.IsHeroOwned(heroID6))
				{
					ReadWriteDataHero.Instance.UnlockHero(heroID6);
				}
				int[] itemsAmount6 = offerBundleSingleHero6.itemsAmount;
				for (int num6 = 0; num6 < itemsAmount6.Length; num6++)
				{
					ReadWriteDataPowerUpItem.Instance.ChangeItemQuantity(num6, itemsAmount6[num6]);
				}
				RewardItem[] array13 = new RewardItem[5];
				RewardItem rewardItem16 = new RewardItem();
				rewardItem16.rewardType = RewardType.SingleHero;
				rewardItem16.itemID = heroID6;
				rewardItem16.isDisplayQuantity = false;
				array13[0] = rewardItem16;
				for (int num7 = 0; num7 < itemsAmount6.Length; num7++)
				{
					RewardItem rewardItem17 = new RewardItem();
					rewardItem17.rewardType = RewardType.Item;
					rewardItem17.itemID = num7;
					rewardItem17.value = itemsAmount6[num7];
					rewardItem17.isDisplayQuantity = true;
					array13[num7 + 1] = rewardItem17;
				}
				SingletonMonoBehaviour<LifetimeCanvas>.Instance.RewardPopupController.Init(array13);
				UnityEngine.Debug.Log($"ProcessPurchase: PASS. Product: '{args.purchasedProduct.definition.id}'");
				PlatformSpecificServicesProvider.Services.Analytics.SendEvent_BuyItem(MarketingConfig.productIDHeroPackOffer[7]);
			}
			else if (string.Equals(args.purchasedProduct.definition.id, MarketingConfig.productIDHeroPackOffer[8], StringComparison.Ordinal))
			{
				OfferBundleSingleHero offerBundleSingleHero7 = readDataOfferBundle.GetOfferBundleSingleHero(MarketingConfig.productIDHeroPackOffer[8]);
				int heroID7 = offerBundleSingleHero7.heroID;
				if (!ReadWriteDataHero.Instance.IsHeroOwned(heroID7))
				{
					ReadWriteDataHero.Instance.UnlockHero(heroID7);
				}
				int[] itemsAmount7 = offerBundleSingleHero7.itemsAmount;
				for (int num8 = 0; num8 < itemsAmount7.Length; num8++)
				{
					ReadWriteDataPowerUpItem.Instance.ChangeItemQuantity(num8, itemsAmount7[num8]);
				}
				RewardItem[] array14 = new RewardItem[5];
				RewardItem rewardItem18 = new RewardItem();
				rewardItem18.rewardType = RewardType.SingleHero;
				rewardItem18.itemID = heroID7;
				rewardItem18.isDisplayQuantity = false;
				array14[0] = rewardItem18;
				for (int num9 = 0; num9 < itemsAmount7.Length; num9++)
				{
					RewardItem rewardItem19 = new RewardItem();
					rewardItem19.rewardType = RewardType.Item;
					rewardItem19.itemID = num9;
					rewardItem19.value = itemsAmount7[num9];
					rewardItem19.isDisplayQuantity = true;
					array14[num9 + 1] = rewardItem19;
				}
				SingletonMonoBehaviour<LifetimeCanvas>.Instance.RewardPopupController.Init(array14);
				UnityEngine.Debug.Log($"ProcessPurchase: PASS. Product: '{args.purchasedProduct.definition.id}'");
				PlatformSpecificServicesProvider.Services.Analytics.SendEvent_BuyItem(MarketingConfig.productIDHeroPackOffer[8]);
			}
			else if (string.Equals(args.purchasedProduct.definition.id, MarketingConfig.productIDHeroPackOffer[9], StringComparison.Ordinal))
			{
				OfferBundleSingleHero offerBundleSingleHero8 = readDataOfferBundle.GetOfferBundleSingleHero(MarketingConfig.productIDHeroPackOffer[9]);
				int heroID8 = offerBundleSingleHero8.heroID;
				if (!ReadWriteDataHero.Instance.IsHeroOwned(heroID8))
				{
					ReadWriteDataHero.Instance.UnlockHero(heroID8);
				}
				int[] itemsAmount8 = offerBundleSingleHero8.itemsAmount;
				for (int num10 = 0; num10 < itemsAmount8.Length; num10++)
				{
					ReadWriteDataPowerUpItem.Instance.ChangeItemQuantity(num10, itemsAmount8[num10]);
				}
				RewardItem[] array15 = new RewardItem[5];
				RewardItem rewardItem20 = new RewardItem();
				rewardItem20.rewardType = RewardType.SingleHero;
				rewardItem20.itemID = heroID8;
				rewardItem20.isDisplayQuantity = false;
				array15[0] = rewardItem20;
				for (int num11 = 0; num11 < itemsAmount8.Length; num11++)
				{
					RewardItem rewardItem21 = new RewardItem();
					rewardItem21.rewardType = RewardType.Item;
					rewardItem21.itemID = num11;
					rewardItem21.value = itemsAmount8[num11];
					rewardItem21.isDisplayQuantity = true;
					array15[num11 + 1] = rewardItem21;
				}
				SingletonMonoBehaviour<LifetimeCanvas>.Instance.RewardPopupController.Init(array15);
				UnityEngine.Debug.Log($"ProcessPurchase: PASS. Product: '{args.purchasedProduct.definition.id}'");
				PlatformSpecificServicesProvider.Services.Analytics.SendEvent_BuyItem(MarketingConfig.productIDHeroPackOffer[9]);
			}
			else if (string.Equals(args.purchasedProduct.definition.id, MarketingConfig.productIDHeroPack[0], StringComparison.Ordinal))
			{
				int heroID9 = readDataShopItemAttribute.GetHeroID(MarketingConfig.productIDHeroPack[0]);
				if (!ReadWriteDataHero.Instance.IsHeroOwned(heroID9))
				{
					ReadWriteDataHero.Instance.UnlockHero(heroID9);
				}
				RewardItem[] array16 = new RewardItem[1];
				RewardItem rewardItem22 = new RewardItem();
				rewardItem22.rewardType = RewardType.SingleHero;
				rewardItem22.itemID = heroID9;
				rewardItem22.isDisplayQuantity = false;
				array16[0] = rewardItem22;
				SingletonMonoBehaviour<LifetimeCanvas>.Instance.RewardPopupController.Init(array16);
				if (SceneManager.GetActiveScene().name.Equals(GameApplication.WorldMapSceneName))
				{
					HeroCampPopupController.Instance.UpgradeNBuyGroupController.RefreshStatus();
				}
				UnityEngine.Debug.Log($"ProcessPurchase: PASS. Product: '{args.purchasedProduct.definition.id}'");
				PlatformSpecificServicesProvider.Services.Analytics.SendEvent_BuyItem(MarketingConfig.productIDHeroPack[0]);
			}
			else if (string.Equals(args.purchasedProduct.definition.id, MarketingConfig.productIDHeroPack[1], StringComparison.Ordinal))
			{
				int heroID10 = readDataShopItemAttribute.GetHeroID(MarketingConfig.productIDHeroPack[1]);
				if (!ReadWriteDataHero.Instance.IsHeroOwned(heroID10))
				{
					ReadWriteDataHero.Instance.UnlockHero(heroID10);
				}
				RewardItem[] array17 = new RewardItem[1];
				RewardItem rewardItem23 = new RewardItem();
				rewardItem23.rewardType = RewardType.SingleHero;
				rewardItem23.itemID = heroID10;
				rewardItem23.isDisplayQuantity = false;
				array17[0] = rewardItem23;
				SingletonMonoBehaviour<LifetimeCanvas>.Instance.RewardPopupController.Init(array17);
				if (SceneManager.GetActiveScene().name.Equals(GameApplication.WorldMapSceneName))
				{
					HeroCampPopupController.Instance.UpgradeNBuyGroupController.RefreshStatus();
				}
				UnityEngine.Debug.Log($"ProcessPurchase: PASS. Product: '{args.purchasedProduct.definition.id}'");
				PlatformSpecificServicesProvider.Services.Analytics.SendEvent_BuyItem(MarketingConfig.productIDHeroPack[1]);
			}
			else if (string.Equals(args.purchasedProduct.definition.id, MarketingConfig.productIDHeroPackSale[0], StringComparison.Ordinal))
			{
				ProcessDataAfterPurchase(MarketingConfig.productIDHeroPackSale[0]);
			}
			else if (string.Equals(args.purchasedProduct.definition.id, MarketingConfig.productIDHeroPackSale[1], StringComparison.Ordinal))
			{
				ProcessDataAfterPurchase(MarketingConfig.productIDHeroPackSale[1]);
			}
			else if (string.Equals(args.purchasedProduct.definition.id, MarketingConfig.productIDHeroPackSale[2], StringComparison.Ordinal))
			{
				ProcessDataAfterPurchase(MarketingConfig.productIDHeroPackSale[2]);
			}
			else if (string.Equals(args.purchasedProduct.definition.id, MarketingConfig.productIDHeroPackSale[3], StringComparison.Ordinal))
			{
				ProcessDataAfterPurchase(MarketingConfig.productIDHeroPackSale[3]);
			}
			else if (string.Equals(args.purchasedProduct.definition.id, MarketingConfig.productIDHeroPackSale[4], StringComparison.Ordinal))
			{
				ProcessDataAfterPurchase(MarketingConfig.productIDHeroPackSale[4]);
			}
			else if (string.Equals(args.purchasedProduct.definition.id, MarketingConfig.productIDHeroPackSale[5], StringComparison.Ordinal))
			{
				ProcessDataAfterPurchase(MarketingConfig.productIDHeroPackSale[5]);
			}
			else if (string.Equals(args.purchasedProduct.definition.id, MarketingConfig.productIDHeroPackSale[6], StringComparison.Ordinal))
			{
				ProcessDataAfterPurchase(MarketingConfig.productIDHeroPackSale[6]);
			}
			else if (string.Equals(args.purchasedProduct.definition.id, MarketingConfig.productIDHeroPackSale[7], StringComparison.Ordinal))
			{
				ProcessDataAfterPurchase(MarketingConfig.productIDHeroPackSale[7]);
			}
			else if (string.Equals(args.purchasedProduct.definition.id, MarketingConfig.productIDHeroPackSale[8], StringComparison.Ordinal))
			{
				ProcessDataAfterPurchase(MarketingConfig.productIDHeroPackSale[8]);
			}
			else if (string.Equals(args.purchasedProduct.definition.id, MarketingConfig.productIDSpecialPack[0], StringComparison.Ordinal))
			{
				ProcessDataAfterPurchase(MarketingConfig.productIDSpecialPack[0]);
			}
			else if (string.Equals(args.purchasedProduct.definition.id, MarketingConfig.productIDSpecialPack[1], StringComparison.Ordinal))
			{
				ProcessDataAfterPurchase(MarketingConfig.productIDSpecialPack[1]);
			}
			else if (string.Equals(args.purchasedProduct.definition.id, MarketingConfig.productIDSpecialPack[2], StringComparison.Ordinal))
			{
				ProcessDataAfterPurchase(MarketingConfig.productIDSpecialPack[2]);
			}
			else if (string.Equals(args.purchasedProduct.definition.id, MarketingConfig.productIDSpecialPack[3], StringComparison.Ordinal))
			{
				ProcessDataAfterPurchase(MarketingConfig.productIDSpecialPack[3]);
			}
			else if (string.Equals(args.purchasedProduct.definition.id, MarketingConfig.productIDSpecialPack[4], StringComparison.Ordinal))
			{
				string intro_json = (introductory_info_dict != null && introductory_info_dict.ContainsKey(args.purchasedProduct.definition.storeSpecificId)) ? introductory_info_dict[args.purchasedProduct.definition.storeSpecificId] : null;
				SubscriptionManager subscriptionManager = new SubscriptionManager(args.purchasedProduct, intro_json);
				SubscriptionInfo subscriptionInfo = subscriptionManager.getSubscriptionInfo();
				SaleBundleConfigData dataSaleBundle = StoreBundleData.GetDataSaleBundle(args.purchasedProduct.definition.id);
				SubscriptionTypeEnum subId = SubscriptionTypeEnum.dailyBooster;
				DateTime moment = GameTools.GetMoment0(GameTools.GetNow());
				GameTools.SetEndSubscriptionTime(subId, GameTools.GetMoment0(subscriptionInfo.getExpireDate().ToLocalTime()));
				GameTools.SetLastTimeCheckInSubscription(subId, moment.AddDays(-1.0));
				DailyCheckinManager.Instance.CheckDailyBooster();
				GameEventCenter.Instance.Trigger(GameEventType.OnCompletePurchase, null);
				PlatformSpecificServicesProvider.Services.Analytics.SendEvent_BuyItem(MarketingConfig.productIDSpecialPack[4]);
			}
			else if (string.Equals(args.purchasedProduct.definition.id, MarketingConfig.productIDSpecialPack[7], StringComparison.Ordinal))
			{
				SaleBundleConfigData dataSaleBundle2 = StoreBundleData.GetDataSaleBundle(args.purchasedProduct.definition.id);
				SubscriptionTypeEnum subId2 = GameTools.productIdToSubscriptionEnum[args.purchasedProduct.definition.id];
				DateTime localTime = GameTools.GetNow().AddDays(dataSaleBundle2.Subcribedur);
				GameTools.SetEndSubscriptionTime(subId2, localTime);
				SingletonMonoBehaviour<LifetimeCanvas>.Instance.NotifyPopupController.Init(string.Format(GameTools.GetLocalization("ATTACK_BOOSTER_NOTI"), localTime.ToString("MM\\/dd\\/yyyy HH:mm"), dataSaleBundle2.Itemquatities[0]), "OK", null);
				GameEventCenter.Instance.Trigger(GameEventType.OnCompletePurchase, null);
				PlatformSpecificServicesProvider.Services.Analytics.SendEvent_BuyItem(MarketingConfig.productIDSpecialPack[7]);
			}
			else if (string.Equals(args.purchasedProduct.definition.id, MarketingConfig.productIDSpecialPack[8], StringComparison.Ordinal))
			{
				string intro_json2 = (introductory_info_dict != null && introductory_info_dict.ContainsKey(args.purchasedProduct.definition.storeSpecificId)) ? introductory_info_dict[args.purchasedProduct.definition.storeSpecificId] : null;
				SubscriptionManager subscriptionManager2 = new SubscriptionManager(args.purchasedProduct, intro_json2);
				SubscriptionInfo subscriptionInfo2 = subscriptionManager2.getSubscriptionInfo();
				SaleBundleConfigData dataSaleBundle3 = StoreBundleData.GetDataSaleBundle(args.purchasedProduct.definition.id);
				SubscriptionTypeEnum subId3 = GameTools.productIdToSubscriptionEnum[args.purchasedProduct.definition.id];
				DateTime localTime2 = subscriptionInfo2.getExpireDate().ToLocalTime();
				GameTools.SetEndSubscriptionTime(subId3, localTime2);
				SingletonMonoBehaviour<LifetimeCanvas>.Instance.NotifyPopupController.Init(string.Format(GameTools.GetLocalization("ATTACK_BOOSTER_NOTI"), localTime2.ToString("MM\\/dd\\/yyyy HH:mm"), dataSaleBundle3.Itemquatities[0]), "OK", null);
				GameEventCenter.Instance.Trigger(GameEventType.OnCompletePurchase, null);
				PlatformSpecificServicesProvider.Services.Analytics.SendEvent_BuyItem(MarketingConfig.productIDSpecialPack[8]);
			}
			else if (string.Equals(args.purchasedProduct.definition.id, MarketingConfig.productIDSpecialPack[5], StringComparison.Ordinal))
			{
				ProcessDataAfterPurchaseHeroMaxLevel();
			}
			else if (string.Equals(args.purchasedProduct.definition.id, MarketingConfig.productIDSpecialPack[6], StringComparison.Ordinal))
			{
				ProcessDataAfterPurchaseHeroPet();
			}
			else
			{
				UnityEngine.Debug.Log($"ProcessPurchase: FAIL. Unrecognized product: '{args.purchasedProduct.definition.id}'");
			}
			return PurchaseProcessingResult.Complete;
		}

		public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
		{
			UnityEngine.Debug.Log($"OnPurchaseFailed: FAIL. Product: '{product.definition.storeSpecificId}', PurchaseFailureReason: {failureReason}");
		}

		private void ProcessDataAfterPurchase(string productID)
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
				if (SceneManager.GetActiveScene().name.Equals(GameApplication.WorldMapSceneName) && heroid.Length == 1 && dataSaleBundle.Herolevel == 0)
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
			UnityEngine.Debug.Log($"ProcessPurchase: PASS. Product: '{productID}'");
			PlatformSpecificServicesProvider.Services.Analytics.SendEvent_BuyItem(productID);
		}

		public void PurchaseHero(string productID)
		{
			BuyProductID(productID);
		}

		public void PurchaseGem(string productID)
		{
			BuyProductID(productID);
		}

		public void PurchaseOfferBundle(string productID)
		{
			BuyProductID(productID);
		}

		public void PurchaseSaleBundle(string productID)
		{
			BuyProductID(productID);
		}

		public void PurchaseHeroMaxLevel(string productID, string heroItemID)
		{
			tempHeroItemID = heroItemID;
			BuyProductID(productID);
		}

		private void ProcessDataAfterPurchaseHeroMaxLevel()
		{
			SaleBundleConfigData dataSaleBundle = StoreBundleData.GetDataSaleBundle(tempHeroItemID);
			int[] heroid = dataSaleBundle.Heroid;
			if (heroid.Length == 1)
			{
				ReadWriteDataHero.Instance.LevelUpTo(heroid[0], 9);
				if (SceneManager.GetActiveScene().name.Equals(GameApplication.WorldMapSceneName))
				{
					SingletonMonoBehaviour<LifetimeCanvas>.Instance.AskToBuyPopupController.InitBuyHeroPet(tempHeroItemID);
				}
			}
			PlatformSpecificServicesProvider.Services.Analytics.SendEvent_BuyItem(tempHeroItemID);
		}

		public void PurchaseHeroPet(string productID, string heroItemID)
		{
			tempHeroItemID = heroItemID;
			BuyProductID(productID);
		}

		private void ProcessDataAfterPurchaseHeroPet()
		{
			SaleBundleConfigData dataSaleBundle = StoreBundleData.GetDataSaleBundle(tempHeroItemID);
			int[] heroid = dataSaleBundle.Heroid;
			if (heroid.Length == 1)
			{
				ReadWriteDataHero.Instance.UnlockPet(heroid[0]);
				string localization = GameTools.GetLocalization("CONFIRMED_BUY_PET_HERO");
				SingletonMonoBehaviour<LifetimeCanvas>.Instance.NotifyPopupController.Init(localization, isShowButtonFreeResources: false, isShowButtonGoToStore: false);
			}
			PlatformSpecificServicesProvider.Services.Analytics.SendEvent_BuyItem(tempHeroItemID);
		}

		public void PurchaseSubscription(string subscritionId, SubscriptionTypeEnum subType)
		{
			BuyProductID(subscritionId);
		}

		public string GetLocalizedProductTitle(string productID)
		{
			string result = string.Empty;
			if (m_StoreController != null)
			{
				result = m_StoreController.products.WithID(productID).metadata.localizedTitle;
			}
			return result;
		}

		public string GetLocalizedProductDescription(string productID)
		{
			string result = string.Empty;
			if (m_StoreController != null)
			{
				result = m_StoreController.products.WithID(productID).metadata.localizedDescription;
			}
			return result;
		}

		public string GetLocalizedProductPriceString(string productID)
		{
			string result = string.Empty;
			if (m_StoreController != null)
			{
				result = m_StoreController.products.WithID(productID).metadata.localizedPriceString;
			}
			return result;
		}

		public decimal GetLocalizedProductPrice(string productID)
		{
			decimal result = 0m;
			if (m_StoreController != null)
			{
				result = m_StoreController.products.WithID(productID).metadata.localizedPrice;
			}
			return result;
		}

		public string GetISOCurrencyCode(string productID)
		{
			string result = string.Empty;
			if (m_StoreController != null)
			{
				result = m_StoreController.products.WithID(productID).metadata.isoCurrencyCode;
			}
			return result;
		}

		public string GetFormatedProductPrice(string ISOCurrencyCode, decimal amount, int noDecimalFracment)
		{
			string empty = string.Empty;
			noDecimalFracment = ((noDecimalFracment != 0) ? 2 : 0);
			string currencySymbol = GetCurrencySymbol(ISOCurrencyCode);
			return GetCurrencySymbol(ISOCurrencyCode) + " " + string.Format("{0:n" + noDecimalFracment + "}", amount);
		}

		private string GetCurrencySymbol(string ISOCurrencyCode)
		{
			if (ISOCurrencyCode == "USD" || ISOCurrencyCode == "AUD" || ISOCurrencyCode == "SGD")
			{
				return "$";
			}
			if (ISOCurrencyCode == "EUR")
			{
				return "€";
			}
			if (ISOCurrencyCode == "VND")
			{
				return "đ";
			}
			if (ISOCurrencyCode == "KRW")
			{
				return "₩";
			}
			if (ISOCurrencyCode == "GBP")
			{
				return "£";
			}
			if (ISOCurrencyCode == "CNY")
			{
				return "¥";
			}
			if (ISOCurrencyCode == "JPY")
			{
				return "¥";
			}
			if (ISOCurrencyCode == "MYR")
			{
				return "RM";
			}
			if (ISOCurrencyCode == "CHF")
			{
				return "CHF";
			}
			if (ISOCurrencyCode == "INR")
			{
				return "₹";
			}
			if (ISOCurrencyCode == "RUB")
			{
				return "₽";
			}
			if (ISOCurrencyCode == "THB")
			{
				return "฿";
			}
			if (ISOCurrencyCode == "HKD")
			{
				return "HK$";
			}
			if (ISOCurrencyCode == "BRL")
			{
				return "R$";
			}
			return ISOCurrencyCode;
		}
	}
}

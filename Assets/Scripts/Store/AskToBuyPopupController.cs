using ApplicationEntry;
using Data;
using Gameplay;
using LifetimePopup;
using MyCustom;
using Services.PlatformSpecific;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Store
{
	public class AskToBuyPopupController : GameplayPopupController
	{
		[Header("Mua Hero max level")]
		[SerializeField]
		private string productIDBuyHeroMaxLevel;

		[SerializeField]
		private Text priceBuyHero;

		[SerializeField]
		private GameObject buyHeroMaxLevelGroup;

		[Header("Mua Hero 1 level")]
		[SerializeField]
		private GameObject buyHero1LevelGroup;

		[Header("Mua Hero pet")]
		[SerializeField]
		private string productIDBuyHeroPet;

		[SerializeField]
		private Text priceBuyHeroPet;

		[SerializeField]
		private GameObject buyHeroPet;

		private string currentHeroProductID;

		public void InitBuyHeroLevel(string productID)
		{
			OpenWithScaleAnimation();
			base.transform.SetAsLastSibling();
			HideAllGroups();
			currentHeroProductID = productID;
			switch (ApplicationEntry.ApplicationEntry.Instance.ReadWriteRemoteSettingData.GetHeroBundleOption())
			{
			case 0:
			{
				buyHeroMaxLevelGroup.SetActive(value: true);
				SaleBundleConfigData dataSaleBundle = StoreBundleData.GetDataSaleBundle(productIDBuyHeroMaxLevel);
				decimal num = 0m;
				num = ((!StaticMethod.IsInternetConnectionAvailable()) ? ((decimal)dataSaleBundle.Defaultprice) : PlatformSpecificServicesProvider.Services.InApPurchase.GetLocalizedProductPrice(productIDBuyHeroMaxLevel));
				string iSOCurrencyCode = PlatformSpecificServicesProvider.Services.InApPurchase.GetISOCurrencyCode(productIDBuyHeroMaxLevel);
				int noDecimalFracment = BitConverter.GetBytes(decimal.GetBits(num)[3])[2];
				priceBuyHero.text = PlatformSpecificServicesProvider.Services.InApPurchase.GetFormatedProductPrice(iSOCurrencyCode, num, noDecimalFracment);
				break;
			}
			case 1:
				buyHero1LevelGroup.SetActive(value: true);
				break;
			}
		}

		public void PurchaseHeroMaxLevel()
		{
			Close();
			base.gameObject.SetActive(value: false);
			HideAllGroups();
			PlatformSpecificServicesProvider.Services.InApPurchase.PurchaseHeroMaxLevel(productIDBuyHeroMaxLevel, currentHeroProductID);
		}

		public void PurchaseHeroOneLevel()
		{
			PlatformSpecificServicesProvider.Services.Ad.ShowOfferVideo(OfferVideoCallback_HeroOneLevel);
		}

		private void OfferVideoCallback_HeroOneLevel(bool completed)
		{
			if (completed)
			{
				SaleBundleConfigData dataSaleBundle = StoreBundleData.GetDataSaleBundle(currentHeroProductID);
				int[] heroid = dataSaleBundle.Heroid;
				if (heroid.Length == 1)
				{
					ReadWriteDataHero.Instance.LevelUp(heroid[0]);
				}
				Close();
				base.gameObject.SetActive(value: false);
				HideAllGroups();
				string localization = GameTools.GetLocalization("CONFIRMED_BUY_ONE_LEVEL");
				SingletonMonoBehaviour<LifetimeCanvas>.Instance.NotifyPopupController.Init(localization, isShowButtonFreeResources: false, isShowButtonGoToStore: false);
			}
		}

		public void InitBuyHeroPet(string productID)
		{
			OpenWithScaleAnimation();
			base.transform.SetAsLastSibling();
			HideAllGroups();
			buyHeroPet.SetActive(value: true);
			currentHeroProductID = productID;
			SaleBundleConfigData dataSaleBundle = StoreBundleData.GetDataSaleBundle(productIDBuyHeroPet);
			decimal num = 0m;
			num = ((!StaticMethod.IsInternetConnectionAvailable()) ? ((decimal)dataSaleBundle.Defaultprice) : PlatformSpecificServicesProvider.Services.InApPurchase.GetLocalizedProductPrice(productIDBuyHeroPet));
			string iSOCurrencyCode = PlatformSpecificServicesProvider.Services.InApPurchase.GetISOCurrencyCode(productIDBuyHeroPet);
			int noDecimalFracment = BitConverter.GetBytes(decimal.GetBits(num)[3])[2];
			priceBuyHeroPet.text = PlatformSpecificServicesProvider.Services.InApPurchase.GetFormatedProductPrice(iSOCurrencyCode, num, noDecimalFracment);
		}

		public void PurchaseHeroPet()
		{
			Close();
			base.gameObject.SetActive(value: false);
			HideAllGroups();
			PlatformSpecificServicesProvider.Services.InApPurchase.PurchaseHeroPet(productIDBuyHeroPet, currentHeroProductID);
		}

		private void HideAllGroups()
		{
			buyHeroMaxLevelGroup.SetActive(value: false);
			buyHero1LevelGroup.SetActive(value: false);
			buyHeroPet.SetActive(value: false);
		}
	}
}

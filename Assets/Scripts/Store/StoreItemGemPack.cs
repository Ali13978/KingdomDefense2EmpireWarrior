using LifetimePopup;
using Services.PlatformSpecific;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Store
{
	public class StoreItemGemPack : ButtonController
	{
		[SerializeField]
		private string productID;

		[Space]
		[Header("UI")]
		[SerializeField]
		private Text titleText;

		[SerializeField]
		private Text descriptionText;

		[SerializeField]
		private Text priceText;

		[SerializeField]
		private Text valueText;

		private void Start()
		{
			UpdateText();
		}

		private void OnEnable()
		{
			UpdateText();
		}

		public override void OnClick()
		{
			base.OnClick();
			ProcessPurchase();
		}

		private void ProcessPurchase()
		{
			PlatformSpecificServicesProvider.Services.InApPurchase.PurchaseGem(productID);
		}

		private void UpdateText()
		{
			if (titleText != null)
			{
				titleText.text = PlatformSpecificServicesProvider.Services.InApPurchase.GetLocalizedProductTitle(productID);
			}
			if (descriptionText != null)
			{
				descriptionText.text = PlatformSpecificServicesProvider.Services.InApPurchase.GetLocalizedProductDescription(productID);
			}
			if (priceText != null)
			{
				decimal localizedProductPrice = PlatformSpecificServicesProvider.Services.InApPurchase.GetLocalizedProductPrice(productID);
				string iSOCurrencyCode = PlatformSpecificServicesProvider.Services.InApPurchase.GetISOCurrencyCode(productID);
				int noDecimalFracment = BitConverter.GetBytes(decimal.GetBits(localizedProductPrice)[3])[2];
				priceText.text = PlatformSpecificServicesProvider.Services.InApPurchase.GetFormatedProductPrice(iSOCurrencyCode, localizedProductPrice, noDecimalFracment);
			}
			if (valueText != null)
			{
				valueText.text = SingletonMonoBehaviour<LifetimeCanvas>.Instance.StorePopupController.ReadDataShopItemAttribute.GetGemPackValue(productID).ToString();
			}
		}
	}
}

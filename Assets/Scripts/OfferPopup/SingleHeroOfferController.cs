using DailyTrial;
using Gameplay;
using GeneralVariable;
using LifetimePopup;
using Middle;
using Services.PlatformSpecific;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace OfferPopup
{
	public class SingleHeroOfferController : GameplayPopupController
	{
		[Space]
		[Header("Title")]
		[SerializeField]
		private Text title;

		[SerializeField]
		private Image heroAvatar;

		[SerializeField]
		private Image heroName;

		[SerializeField]
		private Image[] heroSkills;

		[SerializeField]
		private GameObject timeCountDown;

		[SerializeField]
		private GameObject notiOneTime;

		[Space]
		[SerializeField]
		private Image[] itemsAvatar;

		[SerializeField]
		private Text[] itemsAmount;

		[Space]
		[SerializeField]
		private Text oldPrice;

		[SerializeField]
		private Text newPrice;

		[SerializeField]
		private Text saleRate;

		private string bundleID;

		public void Init(int heroID, OfferType type)
		{
			switch (type)
			{
			case OfferType.OneTime:
				if ((bool)timeCountDown && (bool)notiOneTime)
				{
					timeCountDown.SetActive(value: false);
					notiOneTime.SetActive(value: true);
				}
				break;
			case OfferType.TimeCountDown:
				if ((bool)timeCountDown && (bool)notiOneTime)
				{
					timeCountDown.SetActive(value: true);
					notiOneTime.SetActive(value: false);
				}
				break;
			}
			OpenWithScaleAnimation();
			InitAvatarHero(heroID);
			InitBundleInformation(heroID);
		}

		private void InitAvatarHero(int heroID)
		{
			heroAvatar.sprite = Resources.Load<Sprite>($"HeroesAvatar/avatar_hero_{heroID}");
			heroName.sprite = Resources.Load<Sprite>($"HeroesName/name_hero_{heroID}");
			for (int i = 0; i < heroSkills.Length; i++)
			{
				heroSkills[i].sprite = Resources.Load<Sprite>($"HeroCamp/SkillIcons/hero_{heroID}_skill_{i}");
			}
		}

		private void InitBundleInformation(int heroID)
		{
			bundleID = MarketingConfig.productIDHeroPackOffer[heroID];
			title.text = PlatformSpecificServicesProvider.Services.InApPurchase.GetLocalizedProductTitle(bundleID);
			OfferBundleSingleHero offerBundleSingleHero = SingletonMonoBehaviour<LifetimeCanvas>.Instance.OfferPopupController.ReadDataOfferBundle.GetOfferBundleSingleHero(bundleID);
			UnityEngine.Debug.Log(offerBundleSingleHero);
			for (int i = 0; i < itemsAvatar.Length; i++)
			{
				itemsAvatar[i].sprite = Resources.Load<Sprite>($"LuckyChest/Items/lucky_item_pw_{i}");
				itemsAmount[i].text = offerBundleSingleHero.itemsAmount[i].ToString();
			}
			int num = offerBundleSingleHero.saleRate;
			saleRate.text = num.ToString() + "%";
			decimal localizedProductPrice = PlatformSpecificServicesProvider.Services.InApPurchase.GetLocalizedProductPrice(bundleID);
			decimal d = (decimal)(1f - (float)num / 100f);
			decimal amount = decimal.Divide(localizedProductPrice, d);
			string iSOCurrencyCode = PlatformSpecificServicesProvider.Services.InApPurchase.GetISOCurrencyCode(bundleID);
			int noDecimalFracment = BitConverter.GetBytes(decimal.GetBits(localizedProductPrice)[3])[2];
			newPrice.text = PlatformSpecificServicesProvider.Services.InApPurchase.GetFormatedProductPrice(iSOCurrencyCode, localizedProductPrice, noDecimalFracment);
			oldPrice.text = PlatformSpecificServicesProvider.Services.InApPurchase.GetFormatedProductPrice(iSOCurrencyCode, amount, noDecimalFracment);
		}

		public void ProcessBuyItem()
		{
			PlatformSpecificServicesProvider.Services.InApPurchase.PurchaseOfferBundle(bundleID);
			switch (ModeManager.Instance.gameMode)
			{
			case GameMode.CampaignMode:
				CloseWithScaleAnimation();
				break;
			case GameMode.DailyTrialMode:
				CloseWithScaleAnimation();
				Continue();
				break;
			default:
				CloseWithScaleAnimation();
				break;
			}
			GameObject gameObject = GameObject.FindGameObjectWithTag(GeneralVariable.GeneralVariable.WATCH_OFFER_BUTTON);
			if ((bool)gameObject)
			{
				gameObject.GetComponent<WatchOfferButtonController>().TurnOffButton();
			}
		}

		public void NotNow()
		{
			switch (ModeManager.Instance.gameMode)
			{
			case GameMode.CampaignMode:
				CloseWithScaleAnimation();
				break;
			case GameMode.DailyTrialMode:
				CloseWithScaleAnimation();
				Continue();
				break;
			case GameMode.TournamentMode:
				CloseWithScaleAnimation();
				break;
			default:
				CloseWithScaleAnimation();
				break;
			}
		}

		public void Continue()
		{
			Loading.Instance.ShowLoading();
			Invoke("DoLoad", 1f);
			GameplayManager.Instance.gameSpeedController.UnPauseGame();
			ModeManager.Instance.gameMode = GameMode.CampaignMode;
		}

		private void DoLoad()
		{
			GameApplication.Instance.LoadScene(GameApplication.WorldMapSceneName);
		}
	}
}

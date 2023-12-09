using Data;
using LifetimePopup;
using Parameter;
using Services.PlatformSpecific;
using UnityEngine;
using UnityEngine.UI;

namespace HeroCamp
{
	public class UpgradeButtonController : ButtonController
	{
		[SerializeField]
		private Text statusText;

		[SerializeField]
		private Text maxLevelText;

		[SerializeField]
		private Text upgradePriceText;

		[SerializeField]
		private GameObject miniGem;

		[SerializeField]
		private Sprite normalSprite;

		[SerializeField]
		private Sprite maxLevelsprite;

		[SerializeField]
		private Button button;

		[SerializeField]
		private Image imageButton;

		private int currentHeroID;

		public void InitUpgradePrice(int price)
		{
			upgradePriceText.text = price.ToString();
		}

		public override void OnClick()
		{
			base.OnClick();
			currentHeroID = HeroCampPopupController.Instance.currentHeroID;
			if (ReadWriteDataHero.Instance.IsReachMaxLevel(currentHeroID))
			{
				UnityEngine.Debug.Log("Không thể nâng cấp thêm");
				return;
			}
			if (HeroesLevelGemCalculator.IsEnoughGemToUpgrade(currentHeroID))
			{
				LevelUpHero();
				return;
			}
			UnityEngine.Debug.Log("Không đủ Gem!");
			string notiContent = Singleton<NotificationDescription>.Instance.GetNotiContent(20);
			SingletonMonoBehaviour<LifetimeCanvas>.Instance.NotifyPopupController.Init(notiContent, isShowButtonFreeResources: true, isShowButtonGoToStore: true);
		}

		private void LevelUpHero()
		{
			SendEventUpgradeHeroLevel();
			int gemAmountToLevelUp = HeroesLevelGemCalculator.GetGemAmountToLevelUp(currentHeroID);
			ReadWriteDataPlayerCurrency.Instance.ChangeGem(-gemAmountToLevelUp, isDispatchEventChange: true);
			ReadWriteDataHero.Instance.LevelUp(currentHeroID);
			HeroCampPopupController.Instance.HeroLevelInformation.DisplayLevelUpHero();
			if (ReadWriteDataHero.Instance.IsReachMaxLevel(currentHeroID))
			{
				SetMaxLevel();
			}
			UISoundManager.Instance.PlayUpgradeSuccess();
			ReadWriteDataHero.Instance.OnLevelChange(isDispatchEventChange: true);
		}

		private void SendEventUpgradeHeroLevel()
		{
			string heroName = Singleton<HeroDescription>.Instance.GetHeroName(currentHeroID);
			int currentGem = ReadWriteDataPlayerCurrency.Instance.GetCurrentGem();
			int gemAmountToLevelUp = HeroesLevelGemCalculator.GetGemAmountToLevelUp(currentHeroID);
			int currentGem2 = currentGem - gemAmountToLevelUp;
			int currentHeroLevel = ReadWriteDataHero.Instance.GetCurrentHeroLevel(currentHeroID) + 2;
			int maxMapIDUnlocked = ReadWriteDataMap.Instance.GetMapIDUnlocked() + 1;
			int heroOwnedAmount = ReadWriteDataHero.Instance.GetHeroOwnedAmount();
			PlatformSpecificServicesProvider.Services.Analytics.SendEvent_UpgradeHeroLevel(heroName, currentGem, currentGem2, currentHeroLevel, maxMapIDUnlocked, heroOwnedAmount);
		}

		public void SetNormal()
		{
			button.enabled = true;
			upgradePriceText.enabled = true;
			statusText.gameObject.SetActive(value: true);
			maxLevelText.gameObject.SetActive(value: false);
			statusText.text = Singleton<NotificationDescription>.Instance.GetNotiContent(9);
			imageButton.sprite = normalSprite;
			miniGem.SetActive(value: true);
		}

		public void SetMaxLevel()
		{
			button.enabled = false;
			upgradePriceText.enabled = false;
			statusText.gameObject.SetActive(value: false);
			maxLevelText.gameObject.SetActive(value: true);
			maxLevelText.text = Singleton<NotificationDescription>.Instance.GetNotiContent(16);
			imageButton.sprite = maxLevelsprite;
			miniGem.SetActive(value: false);
		}
	}
}

using Data;
using LifetimePopup;
using Parameter;
using Services.PlatformSpecific;
using UnityEngine;
using UnityEngine.UI;

namespace HeroCamp.UltimateUpgrade
{
	public class UpgradeButtonController : ButtonController
	{
		[SerializeField]
		private Text upgradeValue;

		private UltimateUpgradePopupController ultimateUpgradePopupController;

		private int heroID;

		public void Init(UltimateUpgradePopupController ultimateUpgradePopupController, int heroID)
		{
			this.ultimateUpgradePopupController = ultimateUpgradePopupController;
			this.heroID = heroID;
			upgradeValue.text = HeroesLevelGemCalculator.GetGemAmountToUnlockPet(heroID).ToString();
		}

		public override void OnClick()
		{
			base.OnClick();
			TryToUnlockPet();
		}

		private void TryToUnlockPet()
		{
			if (ReadWriteDataHero.Instance.IsHeroOwned(heroID))
			{
				if (ReadWriteDataHero.Instance.IsReachMaxLevel(heroID))
				{
					if (HeroesLevelGemCalculator.IsEnoughGemToUnlockPet(heroID))
					{
						UnlockPet();
						return;
					}
					string notiContent = Singleton<NotificationDescription>.Instance.GetNotiContent(20);
					SingletonMonoBehaviour<LifetimeCanvas>.Instance.NotifyPopupController.Init(notiContent, isShowButtonFreeResources: true, isShowButtonGoToStore: true);
				}
				else
				{
					string notiContent2 = Singleton<NotificationDescription>.Instance.GetNotiContent(115);
					SingletonMonoBehaviour<LifetimeCanvas>.Instance.NotifyPopupController.Init(notiContent2, isShowButtonFreeResources: false, isShowButtonGoToStore: false);
				}
			}
			else
			{
				string notiContent3 = Singleton<NotificationDescription>.Instance.GetNotiContent(116);
				SingletonMonoBehaviour<LifetimeCanvas>.Instance.NotifyPopupController.Init(notiContent3, isShowButtonFreeResources: false, isShowButtonGoToStore: false);
			}
		}

		private void UnlockPet()
		{
			int gemAmountToUnlockPet = HeroesLevelGemCalculator.GetGemAmountToUnlockPet(heroID);
			ReadWriteDataPlayerCurrency.Instance.ChangeGem(-gemAmountToUnlockPet, isDispatchEventChange: true);
			ReadWriteDataHero.Instance.UnlockPet(heroID);
			ultimateUpgradePopupController.UpdateUpgradeButtonState();
			ultimateUpgradePopupController.CastEffectUpgrade();
			HeroCampPopupController.Instance.RefreshHeroInformation();
			SendEvent_UnlockPet();
		}

		private void SendEvent_UnlockPet()
		{
			int petID = HeroParameter.Instance.GetPetID(heroID);
			int heroOwnedAmount = ReadWriteDataHero.Instance.GetHeroOwnedAmount();
			int heroOwnPetAmount = ReadWriteDataHero.Instance.GetHeroOwnPetAmount();
			string petName = GameTools.GetPetName(petID);
			PlatformSpecificServicesProvider.Services.Analytics.SendEvent_UnlockPet(heroOwnedAmount, heroOwnPetAmount, petName);
		}

		public void Show()
		{
			base.gameObject.SetActive(value: true);
		}

		public void Hide()
		{
			base.gameObject.SetActive(value: false);
		}
	}
}

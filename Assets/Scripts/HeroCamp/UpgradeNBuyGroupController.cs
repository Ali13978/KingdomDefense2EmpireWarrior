using Data;
using Parameter;
using UnityEngine;
using UnityEngine.UI;

namespace HeroCamp
{
	public class UpgradeNBuyGroupController : MonoBehaviour
	{
		[Space]
		[Header("Upgrade Button")]
		[SerializeField]
		private UpgradeButtonController upgradeButtonController;

		[Space]
		[Header("Buy Button")]
		[SerializeField]
		private BuyButtonController buyButtonController;

		[Space]
		[Header("Lock Button")]
		[SerializeField]
		private GameObject lockButton;

		[Space]
		[Header("Total Gem")]
		[SerializeField]
		private Text totalGemText;

		private int currentHeroID;

		private void Awake()
		{
			ReadWriteDataPlayerCurrency.Instance.OnGemChangeEvent += Instance_OnGemChangeEvent;
		}

		private void Start()
		{
			UpdateGemAmount();
		}

		private void OnEnable()
		{
			UpdateGemAmount();
		}

		private void OnDestroy()
		{
			ReadWriteDataPlayerCurrency.Instance.OnGemChangeEvent -= Instance_OnGemChangeEvent;
		}

		private void Instance_OnGemChangeEvent()
		{
			UpdateGemAmount();
		}

		private void UpdateGemAmount()
		{
			totalGemText.text = ReadWriteDataPlayerCurrency.Instance.GetCurrentGem().ToString();
		}

		public void RefreshStatus()
		{
			currentHeroID = HeroCampPopupController.Instance.currentHeroID;
			HideAllButton();
			if (ReadWriteDataHero.Instance.IsHeroOwned(currentHeroID))
			{
				UpdateUpgradeButtonStatus();
				RefreshUpgradePrice();
			}
			else if (Singleton<UnlockHeroParameter>.Instance.IsHeroUnlockByGem(currentHeroID))
			{
				buyButtonController.gameObject.SetActive(value: true);
				buyButtonController.InitBuyPriceGem(Singleton<UnlockHeroParameter>.Instance.GetGemAmountToUnlockHero(currentHeroID));
			}
			else if (Singleton<UnlockHeroParameter>.Instance.IsHeroUnlockByRealMoney(currentHeroID))
			{
				buyButtonController.gameObject.SetActive(value: true);
				buyButtonController.InitBuyPriceMoney();
			}
			else
			{
				lockButton.SetActive(value: true);
			}
		}

		private void HideAllButton()
		{
			upgradeButtonController.gameObject.SetActive(value: false);
			buyButtonController.gameObject.SetActive(value: false);
			lockButton.SetActive(value: false);
		}

		private void RefreshUpgradePrice()
		{
			upgradeButtonController.InitUpgradePrice(HeroesLevelGemCalculator.GetGemAmountToLevelUp(currentHeroID));
		}

		private void UpdateUpgradeButtonStatus()
		{
			upgradeButtonController.gameObject.SetActive(value: true);
			if (ReadWriteDataHero.Instance.IsReachMaxLevel(currentHeroID))
			{
				upgradeButtonController.SetMaxLevel();
			}
			else
			{
				upgradeButtonController.SetNormal();
			}
		}
	}
}

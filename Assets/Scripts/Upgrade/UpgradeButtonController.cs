using Data;
using UnityEngine;
using UnityEngine.UI;

namespace Upgrade
{
	public class UpgradeButtonController : ButtonController
	{
		[SerializeField]
		private UpgradePopupController upgradePopupController;

		[SerializeField]
		private Image imageButton;

		[SerializeField]
		private Button button;

		private int starRequired;

		private RGBToGrayscale rGBToGrayscale;

		private void Awake()
		{
			rGBToGrayscale = GetComponent<RGBToGrayscale>();
		}

		public override void OnClick()
		{
			base.OnClick();
			int starRequiredForUpgrade = GetStarRequiredForUpgrade();
			if (isEnoughStarToUpgrade(starRequiredForUpgrade))
			{
				DoUpgrade(starRequiredForUpgrade);
			}
		}

		private void DoUpgrade(int starRequired)
		{
			if (canUpgrade())
			{
				upgradePopupController.upgradeGroupControllers[upgradePopupController.currentTowerIDSelected].currentUpgradeLevel++;
				upgradePopupController.currentStar -= starRequired;
				upgradePopupController.upgradeGroupControllers[upgradePopupController.currentTowerIDSelected].RefreshListTier();
				ReadWriteDataGlobalUpgrade.Instance.Save(upgradePopupController.currentTowerIDSelected, upgradePopupController.upgradeGroupControllers[upgradePopupController.currentTowerIDSelected].currentUpgradeLevel);
				upgradePopupController.TryShowButtonUpgrade(canUpgrade: false);
				upgradePopupController.CalculateCurrentStar();
				UpgradePopupController.Instance.ShowUpgradedEffect();
				UISoundManager.Instance.PlayUpgradeSuccess();
				ReadWriteDataGlobalUpgrade.Instance.OnStarChange(isDispatchEventChange: true);
			}
		}

		public void RefreshButtonStatus()
		{
			int starRequiredForUpgrade = GetStarRequiredForUpgrade();
			if (isEnoughStarToUpgrade(starRequiredForUpgrade))
			{
				ViewCanUpgrade();
			}
			else
			{
				ViewCannotUpgrade();
			}
		}

		private int GetStarRequiredForUpgrade()
		{
			return ReadWriteDataGlobalUpgrade.Instance.GetStarRequireForUpgrade(upgradePopupController.currentTowerIDSelected, upgradePopupController.currentTierSelected);
		}

		private bool canUpgrade()
		{
			bool result = true;
			if (upgradePopupController.currentTowerIDSelected < 0)
			{
				UnityEngine.Debug.Log("Chưa chọn tower!");
				result = false;
			}
			if (upgradePopupController.upgradeGroupControllers[upgradePopupController.currentTowerIDSelected].currentUpgradeLevel == 4)
			{
				UnityEngine.Debug.Log("Đã nâng cấp max!");
				result = false;
			}
			return result;
		}

		private bool isEnoughStarToUpgrade(int starRequired)
		{
			return upgradePopupController.currentStar >= starRequired;
		}

		private void ViewCanUpgrade()
		{
			button.enabled = true;
			if (!rGBToGrayscale)
			{
				rGBToGrayscale = GetComponent<RGBToGrayscale>();
			}
			rGBToGrayscale.SwitchToRGB();
		}

		private void ViewCannotUpgrade()
		{
			button.enabled = false;
			if (!rGBToGrayscale)
			{
				rGBToGrayscale = GetComponent<RGBToGrayscale>();
			}
			rGBToGrayscale.SwitchToGrayscale();
		}
	}
}

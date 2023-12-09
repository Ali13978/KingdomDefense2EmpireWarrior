using Data;
using UnityEngine;
using UnityEngine.UI;

namespace Upgrade
{
	public class TierUpgradeController : ButtonController
	{
		[Space]
		[Header("Parent")]
		[SerializeField]
		private UpgradeGroupController groupController;

		[Space]
		[SerializeField]
		private int towerID;

		[SerializeField]
		private int tierID;

		[SerializeField]
		private int upgradeAbilityID;

		[Space]
		[Header("Star Required")]
		[SerializeField]
		private Text starRequire;

		[SerializeField]
		private GameObject starHolder;

		[Space]
		[Header("Upgraded Image")]
		[SerializeField]
		private GameObject upgradedImage;

		private Image image;

		[Header("Image material")]
		[SerializeField]
		private Material material;

		private void Awake()
		{
			image = GetComponent<Image>();
		}

		private void Start()
		{
			starRequire.text = ReadWriteDataGlobalUpgrade.Instance.GetStarRequireForUpgrade(towerID, tierID).ToString();
		}

		public override void OnClick()
		{
			base.OnClick();
			UpgradePopupController.Instance.ShowSelectedUpgradeImage(base.transform.position);
			UpgradePopupController.Instance.upgradeInformationController.InitData(image.sprite, tierID, upgradeAbilityID, towerID);
			UpgradePopupController.Instance.currentTowerIDSelected = towerID;
			UpgradePopupController.Instance.currentTierSelected = tierID;
			if (tierID <= groupController.currentUpgradeLevel)
			{
				UpgradePopupController.Instance.TryShowButtonUpgrade(canUpgrade: false);
			}
			else if (tierID == groupController.currentUpgradeLevel + 1)
			{
				UpgradePopupController.Instance.TryShowButtonUpgrade(canUpgrade: true);
			}
			else
			{
				UpgradePopupController.Instance.TryShowButtonUpgrade(canUpgrade: false);
			}
		}

		public void InitDefaultData()
		{
			if (image == null)
			{
				image = GetComponent<Image>();
			}
			UpgradePopupController.Instance.upgradeInformationController.InitData(image.sprite, tierID, upgradeAbilityID, towerID);
		}

		public void ViewCanUpgrade()
		{
			material.SetFloat("_EffectAmount", 0f);
			upgradedImage.SetActive(value: false);
			starHolder.SetActive(value: true);
		}

		public void ViewCannotUpgrade()
		{
			material.SetFloat("_EffectAmount", 1f);
			upgradedImage.SetActive(value: false);
			starHolder.SetActive(value: true);
		}

		public void ViewUpgraded()
		{
			material.SetFloat("_EffectAmount", 0f);
			upgradedImage.SetActive(value: true);
			starHolder.SetActive(value: false);
		}
	}
}

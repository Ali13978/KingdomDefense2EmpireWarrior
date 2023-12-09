using Parameter;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
	public class TowerSkillUpgradeButtonController : ControllTowerButtonController
	{
		[SerializeField]
		private int skillID;

		private int ultimateBranch;

		private TowerModel towerModel;

		private TowerUltimateController towerUltimateController;

		private int maxUpgrade = 2;

		private bool canUpgrade;

		private bool isAllowedToUpgrade;

		[Space]
		[Header("General Variable")]
		[SerializeField]
		private Text currentLevelPrice;

		[SerializeField]
		private GameObject currentLevelPriceHolder;

		private int upgradeCost;

		[SerializeField]
		private Button button;

		[SerializeField]
		private Image imageButton;

		[Space]
		[Header("Tiers")]
		[SerializeField]
		private GameObject tiersHolder;

		[SerializeField]
		private GameObject[] ultimateTiers = new GameObject[3];

		[Space]
		[Header("Image material")]
		[SerializeField]
		private Material material;

		private void Update()
		{
			UpdateStatusButton();
		}

		public void Init(TowerModel towerModel)
		{
			this.towerModel = towerModel;
			towerUltimateController = towerModel.towerUltimateController;
			SetImageSprite();
			int num = towerUltimateController.currentLevelUpgrade[skillID];
			int ultimateBranchByLevel = TowerParameter.Instance.GetUltimateBranchByLevel(towerModel.Level);
			if (num < maxUpgrade)
			{
				upgradeCost = TowerSkillParameter.Instance.GetUltimateSkillUpgradeCost(towerModel.Id, ultimateBranchByLevel, skillID, num + 1);
				if (towerModel.Id == 4)
				{
					currentLevelPriceHolder.SetActive(value: false);
				}
				else
				{
					currentLevelPriceHolder.SetActive(value: true);
				}
				currentLevelPrice.text = upgradeCost.ToString();
			}
			else
			{
				currentLevelPriceHolder.SetActive(value: false);
			}
			HideAllTier();
			DisplayUpgradedTier();
		}

		public void SetImageSprite()
		{
			ultimateBranch = TowerParameter.Instance.GetUltimateBranchByLevel(towerModel.Level);
			imageButton.overrideSprite = Resources.Load<Sprite>($"TowerUltimateUpgradeIcon/ultimate_{towerModel.Id}_{ultimateBranch}_{skillID}");
			if (towerModel.Id == 4)
			{
				button.enabled = false;
				imageButton.enabled = false;
			}
			else
			{
				button.enabled = true;
				imageButton.enabled = true;
			}
		}

		public override void OnClick()
		{
			base.OnClick();
			if (canUpgrade)
			{
				if (towerUltimateController.currentLevelUpgrade[skillID] == maxUpgrade)
				{
					UnityEngine.Debug.Log("Khong the nang cap them");
				}
				else if (buttonStatus == ButtonStatus.Available)
				{
					OnClickAvailable();
				}
				else if (buttonStatus == ButtonStatus.Confirm)
				{
					OnConfirm();
				}
			}
		}

		protected override void OnClickAvailable()
		{
			base.OnClickAvailable();
		}

		protected override void OnConfirm()
		{
			base.OnClick();
			List<int> currentLevelUpgrade;
			int index;
			(currentLevelUpgrade = towerUltimateController.currentLevelUpgrade)[index = skillID] = currentLevelUpgrade[index] + 1;
			SingletonMonoBehaviour<GameData>.Instance.DecreaseMoney(upgradeCost);
			DisplayUpgradedTier();
			towerModel.towerUltimateController.listTowerUltimate[skillID].UnlockUltimate(towerUltimateController.currentLevelUpgrade[skillID]);
			SingletonMonoBehaviour<UIRootController>.Instance.UpgradeTowerPopupController.Close();
			GameplayManager.Instance.CurrentTowerRange.GetComponent<TowerRangeController>().HideRange();
		}

		private void HideAllTier()
		{
			if (towerModel.Id == 4)
			{
				tiersHolder.SetActive(value: false);
			}
			else
			{
				tiersHolder.SetActive(value: true);
			}
			for (int i = 0; i < ultimateTiers.Length; i++)
			{
				ultimateTiers[i].SetActive(value: false);
			}
		}

		private void DisplayUpgradedTier()
		{
			if (towerUltimateController.currentLevelUpgrade[skillID] >= 0)
			{
				for (int i = 0; i <= towerUltimateController.currentLevelUpgrade[skillID]; i++)
				{
					ultimateTiers[i].SetActive(value: true);
				}
			}
		}

		public void UpdateStatusButton()
		{
			if (upgradeCost <= SingletonMonoBehaviour<GameData>.Instance.Money)
			{
				CanUpgrade();
			}
			else
			{
				CannotUpgrade();
			}
		}

		private void CanUpgrade()
		{
			canUpgrade = true;
			material.SetFloat("_EffectAmount", 0f);
			currentLevelPrice.color = Color.yellow;
		}

		private void CannotUpgrade()
		{
			canUpgrade = false;
			material.SetFloat("_EffectAmount", 1f);
			currentLevelPrice.color = Color.white;
		}
	}
}

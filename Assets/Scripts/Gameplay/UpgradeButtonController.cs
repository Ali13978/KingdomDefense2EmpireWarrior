using Parameter;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
	public class UpgradeButtonController : ControllTowerButtonController
	{
		private Image imageButton;

		private Button button;

		private bool canUpgrade;

		private bool isAllowedToUpgrade;

		private TowerModel towerModel;

		[Space]
		[Header("General Variable")]
		[SerializeField]
		private bool isUltimateUpgrade;

		[SerializeField]
		private int ultimateBranch;

		[SerializeField]
		private Text currentLevelPrice;

		[SerializeField]
		private Sprite normalImage;

		[SerializeField]
		private Sprite lockImage;

		[Header("Image material")]
		[SerializeField]
		private Material material;

		public int UltimateBranch => ultimateBranch;

		private void Awake()
		{
			GetAllComponents();
		}

		private void GetAllComponents()
		{
			button = GetComponent<Button>();
			imageButton = GetComponent<Image>();
		}

		public void Init(TowerModel towerModel, bool _isAllowedToUpgrade, int price)
		{
			this.towerModel = towerModel;
			isAllowedToUpgrade = _isAllowedToUpgrade;
			currentLevelPrice.text = price.ToString();
			if (!isAllowedToUpgrade)
			{
				LockButton();
			}
			else
			{
				UnLockButton();
			}
		}

		public void SetImageSprite(int towerID, int towerLevel, int ultimateBranch, int skillID, bool isAllowedToUpgrade)
		{
		}

		public void LockButton()
		{
			if (imageButton == null)
			{
				GetAllComponents();
			}
			imageButton.sprite = lockImage;
			button.enabled = false;
			imageButton.SetNativeSize();
			currentLevelPrice.gameObject.SetActive(value: false);
		}

		public void UnLockButton()
		{
			if (imageButton == null)
			{
				GetAllComponents();
			}
			Tower originalParameter = towerModel.OriginalParameter;
			if (originalParameter.level < 2)
			{
				imageButton.sprite = normalImage;
			}
			Tower originalParameter2 = towerModel.OriginalParameter;
			if (originalParameter2.level == 2)
			{
				imageButton.sprite = Resources.Load<Sprite>($"TowerUltimateUpgradeIcon/ultimate_{towerModel.Id}_{ultimateBranch}");
			}
			button.enabled = true;
			imageButton.SetNativeSize();
			currentLevelPrice.gameObject.SetActive(value: true);
		}

		public override void OnClick()
		{
			base.OnClick();
			if (canUpgrade)
			{
				if (buttonStatus == ButtonStatus.Available)
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
			SingletonMonoBehaviour<UIRootController>.Instance.UpgradeTowerPopupController.nextLevelInfomationPopoup.Init(UltimateBranch, towerModel.Id, towerModel.Level, towerModel.transform);
			TowerRangeController component = GameplayManager.Instance.CurrentTowerRange.GetComponent<TowerRangeController>();
			component.target = towerModel.transform;
			if (towerModel != null)
			{
				if (isUltimateUpgrade)
				{
					component.SetRangeAttackMax((float)TowerParameter.Instance.GetRangeMax(towerModel.Id, towerModel.Level + 1 + ultimateBranch) / GameData.PIXEL_PER_UNIT);
				}
				else
				{
					component.SetRangeAttackMax((float)TowerParameter.Instance.GetRangeMax(towerModel.Id, towerModel.Level + 1) / GameData.PIXEL_PER_UNIT);
				}
			}
		}

		protected override void OnConfirm()
		{
			base.OnClick();
			SingletonMonoBehaviour<UIRootController>.Instance.UpgradeTowerPopupController.OnUpgrade(UltimateBranch);
			SingletonMonoBehaviour<UIRootController>.Instance.UpgradeTowerPopupController.nextLevelInfomationPopoup.Close();
		}

		public void UpdateStatusButton(CanUpgradeStatus canUpgradeStatus)
		{
			if (isAllowedToUpgrade)
			{
				switch (canUpgradeStatus)
				{
				case CanUpgradeStatus.CanUpgrade:
					CanUpgrade();
					break;
				case CanUpgradeStatus.CannotUpgrade:
					CannotUpgrade();
					break;
				case CanUpgradeStatus.MaxUpgrade:
					MaxUpgrade();
					break;
				}
			}
		}

		private void MaxUpgrade()
		{
			GetComponent<Button>().enabled = false;
		}

		private void CanUpgrade()
		{
			canUpgrade = true;
			button.enabled = true;
			material.SetFloat("_EffectAmount", 0f);
			currentLevelPrice.color = Color.yellow;
		}

		private void CannotUpgrade()
		{
			canUpgrade = false;
			button.enabled = false;
			material.SetFloat("_EffectAmount", 1f);
			currentLevelPrice.color = Color.white;
		}
	}
}

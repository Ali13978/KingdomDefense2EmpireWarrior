using Middle;
using Parameter;
using SSR.Core.Architecture;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
	public class TowerItem : ControllTowerButtonController
	{
		[Space]
		[SerializeField]
		private OrderedEventDispatcher onSelectToBuy;

		[SerializeField]
		private OrderedEventDispatcher onBuyTowerComplete;

		[Space]
		[SerializeField]
		private int towerID;

		[SerializeField]
		private Text textPrice;

		private Button button;

		private Image buttonImage;

		private bool isAllowedToUse;

		[Header("Image material")]
		[SerializeField]
		private Material material;

		private void Awake()
		{
			GetAllComponents();
		}

		private void GetAllComponents()
		{
			button = GetComponent<Button>();
			buttonImage = GetComponent<Image>();
		}

		public override void Init(bool _isAllowedToUse, Sprite spriteNormal, Sprite lockImage)
		{
			base.Init(_isAllowedToUse, spriteNormal, lockImage);
			isAllowedToUse = _isAllowedToUse;
			if (isAllowedToUse)
			{
				button.enabled = true;
				buttonImage.sprite = spriteNormal;
				buttonImage.SetNativeSize();
			}
			else
			{
				button.enabled = false;
				buttonImage.sprite = lockImage;
				buttonImage.SetNativeSize();
			}
			setPrice();
		}

		private void setPrice()
		{
			if (isAllowedToUse)
			{
				textPrice.text = TowerParameter.Instance.GetPrice(towerID, 0).ToString();
			}
			else
			{
				textPrice.text = string.Empty;
			}
		}

		public override void UpdateBuyState()
		{
			base.UpdateBuyState();
			bool flag = TowerParameter.Instance.GetPrice(towerID, 0) <= SingletonMonoBehaviour<GameData>.Instance.Money;
			if (!button || !buttonImage)
			{
				GetAllComponents();
			}
			if (isAllowedToUse)
			{
				if (flag)
				{
					button.enabled = true;
					material.SetFloat("_EffectAmount", 0f);
					textPrice.color = Color.yellow;
				}
				else
				{
					button.enabled = false;
					material.SetFloat("_EffectAmount", 1f);
					textPrice.color = Color.white;
				}
			}
		}

		public override void OnClick()
		{
			base.OnClick();
			if (buttonStatus == ButtonStatus.Available)
			{
				OnClickAvailable();
			}
			else if (buttonStatus == ButtonStatus.Confirm)
			{
				OnConfirm();
			}
		}

		protected override void OnClickAvailable()
		{
			base.OnClickAvailable();
			SingletonMonoBehaviour<UIRootController>.Instance.UpgradeTowerPopupController.nextLevelInfomationPopoup.Init(-1, towerID, 0, SingletonMonoBehaviour<BuildRegionManager>.Instance.listRegions[Config.Instance.currentTowerRegionIDSelected].transform);
			TowerRangeController component = GameplayManager.Instance.CurrentTowerRange.GetComponent<TowerRangeController>();
			component.target = SingletonMonoBehaviour<BuildRegionManager>.Instance.listRegions[Config.Instance.currentTowerRegionIDSelected].transform;
			component.SetRangeAttackMax((float)TowerParameter.Instance.GetRangeMax(towerID, 0) / GameData.PIXEL_PER_UNIT);
			onSelectToBuy.Dispatch();
		}

		protected override void OnConfirm()
		{
			base.OnClick();
			button.enabled = false;
			Invoke("doBuy", 0.01f);
		}

		private void doBuy()
		{
			TowerModel towerModel = SingletonMonoBehaviour<SpawnTower>.Instance.Get(towerID, 0);
			towerModel.StartBuild(towerID, 0, Config.Instance.currentTowerRegionIDSelected);
			towerModel.Appear();
			towerModel.transform.position = SingletonMonoBehaviour<BuildRegionManager>.Instance.listRegions[Config.Instance.currentTowerRegionIDSelected].transform.position;
			GameData instance = SingletonMonoBehaviour<GameData>.Instance;
			Tower originalParameter = towerModel.OriginalParameter;
			instance.DecreaseMoney(originalParameter.price);
			SingletonMonoBehaviour<TowerControlSoundController>.Instance.PlayBuild(towerID);
			SingletonMonoBehaviour<UIRootController>.Instance.BuyTowerPopupController.Close();
			SingletonMonoBehaviour<BuildRegionManager>.Instance.listRegions[Config.Instance.currentTowerRegionIDSelected].DisplayNotBuildable();
			onBuyTowerComplete.Dispatch();
		}
	}
}

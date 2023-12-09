using Data;
using DG.Tweening;
using Middle;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class UpgradeTowerPopupController : GameplayPopupController
	{
		[Space]
		[Header("Content Holder")]
		[SerializeField]
		private GameObject Content;

		[Space]
		[Header("List offset towers")]
		[SerializeField]
		public Vector3[] listOffsetTower;

		private Vector3 offset;

		[SerializeField]
		private GameObject buttonChangePosition;

		[Space]
		[Header("Controll tower group")]
		public GroupControllTowerButtonsForUpgrade groupControllTowerButtons;

		[SerializeField]
		private UpgradeButtonController upgradeButtonController;

		[SerializeField]
		private UpgradeButtonController[] ultimateUpgradeButtonController;

		[SerializeField]
		private TowerSkillUpgradeButtonController upgradeUltimate0ButtonController;

		[SerializeField]
		private TowerSkillUpgradeButtonController upgradeUltimate1ButtonController;

		[Space]
		[Header("Ultimate Upgrade")]
		[SerializeField]
		private UltimateInforButtonController ultimateInforButtonController;

		public UltimateInforGroup ultimateInforGroup;

		[Space]
		[Header("Pop-ups")]
		public CurrentLevelInformationPopup currentLevelInformationPopup;

		public NextLevelInformationPopup nextLevelInfomationPopoup;

		private CanUpgradeStatus canUpgrade;

		[HideInInspector]
		public TowerModel towerModel;

		private TowerAttackSingleTargetController towerAttackSingleTargetController;

		private TowerFindEnemyController towerFindEnemyController;

		private Transform target;

		private Vector3 PoolPos = new Vector3(2000f, 2000f, 0f);

		[Space]
		[SerializeField]
		private RectTransform rectTransform;

		[SerializeField]
		private RectTransform canvasHolder;

		private GameObject towerControllerCollider;

		public override void Update()
		{
			base.Update();
			if (null != towerModel)
			{
				UpdateUpgradeButtonsState();
			}
			if (base.gameObject.activeSelf && (bool)target)
			{
				UpdatePositionFollowTower();
			}
		}

		public void Init(TowerModel towerModel, Transform target)
		{
			this.towerModel = towerModel;
			this.target = target;
			CacheCurrentTowerController();
			SetUpgrateButtonFolowTower();
			groupControllTowerButtons.DisableConfirmAllButton();
			currentLevelInformationPopup.Init();
			nextLevelInfomationPopoup.Close();
			ultimateInforGroup.HideList();
			Open();
			ShowRange(towerModel);
			TryToFocusTowerPosition();
		}

		private void TryToFocusTowerPosition()
		{
			if (base.gameObject.activeSelf && (bool)target)
			{
				UpdatePositionFollowTower();
			}
			Vector3 localPosition = rectTransform.localPosition;
			float y = localPosition.y;
			Vector2 sizeDelta = canvasHolder.sizeDelta;
			float num = sizeDelta.y / 2f;
			Vector2 sizeDelta2 = rectTransform.sizeDelta;
			if (!(y > num - sizeDelta2.y / 2f))
			{
				Vector3 localPosition2 = rectTransform.localPosition;
				float y2 = localPosition2.y;
				Vector2 sizeDelta3 = canvasHolder.sizeDelta;
				float num2 = (0f - sizeDelta3.y) / 2f;
				Vector2 sizeDelta4 = rectTransform.sizeDelta;
				if (!(y2 < num2 + sizeDelta4.y / 2f))
				{
					return;
				}
			}
			if ((bool)target)
			{
				SingletonMonoBehaviour<CameraController>.Instance.PinchZoomFov.TryToMoveToBuildTowerPosition(target.position);
			}
		}

		private void ShowRange(TowerModel _towerModel)
		{
			TowerRangeController component = GameplayManager.Instance.CurrentTowerRange.GetComponent<TowerRangeController>();
			component.target = _towerModel.transform;
			if (towerFindEnemyController != null)
			{
				component.SetRangeAttackMax(towerFindEnemyController.BuffedAttackRangeMax);
				return;
			}
			TowerRangeController towerRangeController = component;
			Tower originalParameter = _towerModel.OriginalParameter;
			towerRangeController.SetRangeAttackMax((float)originalParameter.attackRangeMax / GameData.PIXEL_PER_UNIT);
		}

		private void CacheCurrentTowerController()
		{
			towerAttackSingleTargetController = towerModel.GetComponent<TowerAttackSingleTargetController>();
			towerFindEnemyController = towerModel.GetComponent<TowerFindEnemyController>();
		}

		private void UpdatePositionFollowTower()
		{
			base.transform.position = target.position + offset;
		}

		private void UpdateUpgradeButtonsState()
		{
			Tower originalParameter = towerModel.OriginalParameter;
			if (originalParameter.level < 2)
			{
				TowerModel obj = towerModel;
				Tower originalParameter2 = towerModel.OriginalParameter;
				canUpgrade = obj.GetUpgradeEnable(originalParameter2.level);
				upgradeButtonController.UpdateStatusButton(canUpgrade);
			}
			Tower originalParameter3 = towerModel.OriginalParameter;
			if (originalParameter3.level == 2)
			{
				TowerModel obj2 = towerModel;
				Tower originalParameter4 = towerModel.OriginalParameter;
				canUpgrade = obj2.GetUpgradeEnable(originalParameter4.level);
				ultimateUpgradeButtonController[0].UpdateStatusButton(canUpgrade);
				TowerModel obj3 = towerModel;
				Tower originalParameter5 = towerModel.OriginalParameter;
				canUpgrade = obj3.GetUpgradeEnable(originalParameter5.level + 2);
				ultimateUpgradeButtonController[1].UpdateStatusButton(canUpgrade);
			}
		}

		public void OnUpgrade(int ultimateBranch)
		{
			if (canUpgrade == CanUpgradeStatus.CanUpgrade)
			{
				towerModel.Upgrade(ultimateBranch);
				Close();
			}
		}

		private void SetUpgrateButtonFolowTower()
		{
			int num = 0;
			switch (ModeManager.Instance.gameMode)
			{
			case GameMode.CampaignMode:
				num = MapRuleParameter.Instance.GetMaxLevelTowerCanUpgrade_Campaign(SingletonMonoBehaviour<GameData>.Instance.MapID, towerModel.Id);
				break;
			case GameMode.DailyTrialMode:
			{
				int currentDayIndex = ReadWriteDataDailyTrial.Instance.GetCurrentDayIndex();
				int currentWave = SingletonMonoBehaviour<GameData>.Instance.CurrentWave;
				num = MapRuleParameter.Instance.GetMaxLevelTowerCanUpgrade_Daily(currentWave, towerModel.Id);
				break;
			}
			case GameMode.TournamentMode:
			{
				string currentSeasonID = MapRuleParameter.Instance.GetCurrentSeasonID();
				num = MapRuleParameter.Instance.GetMaxLevelTowerCanUpgrade_Tournament(currentSeasonID, towerModel.Id);
				break;
			}
			}
			Tower originalParameter = towerModel.OriginalParameter;
			if (originalParameter.level == num)
			{
			}
			Tower originalParameter2 = towerModel.OriginalParameter;
			if (originalParameter2.level < 2)
			{
				Tower originalParameter3 = towerModel.OriginalParameter;
				bool isAllowedToUpgrade = originalParameter3.level < num;
				TowerParameter instance = TowerParameter.Instance;
				Tower originalParameter4 = towerModel.OriginalParameter;
				int id = originalParameter4.id;
				Tower originalParameter5 = towerModel.OriginalParameter;
				int price = instance.GetPrice(id, originalParameter5.level + 1);
				ultimateInforButtonController.gameObject.SetActive(value: false);
				upgradeButtonController.gameObject.SetActive(value: true);
				upgradeButtonController.Init(towerModel, isAllowedToUpgrade, price);
				ultimateUpgradeButtonController[0].gameObject.SetActive(value: false);
				ultimateUpgradeButtonController[1].gameObject.SetActive(value: false);
				upgradeUltimate0ButtonController.gameObject.SetActive(value: false);
				upgradeUltimate1ButtonController.gameObject.SetActive(value: false);
				return;
			}
			Tower originalParameter6 = towerModel.OriginalParameter;
			if (originalParameter6.level == 2)
			{
				ultimateInforButtonController.gameObject.SetActive(value: false);
				upgradeButtonController.gameObject.SetActive(value: false);
				ultimateUpgradeButtonController[0].gameObject.SetActive(value: true);
				ultimateUpgradeButtonController[1].gameObject.SetActive(value: true);
				upgradeUltimate0ButtonController.gameObject.SetActive(value: false);
				upgradeUltimate1ButtonController.gameObject.SetActive(value: false);
				switch (num)
				{
				case 3:
				{
					UpgradeButtonController obj5 = ultimateUpgradeButtonController[0];
					TowerModel obj6 = towerModel;
					TowerParameter instance4 = TowerParameter.Instance;
					Tower originalParameter9 = towerModel.OriginalParameter;
					obj5.Init(obj6, _isAllowedToUpgrade: true, instance4.GetPrice(originalParameter9.id, 3));
					UpgradeButtonController obj7 = ultimateUpgradeButtonController[1];
					TowerModel obj8 = towerModel;
					TowerParameter instance5 = TowerParameter.Instance;
					Tower originalParameter10 = towerModel.OriginalParameter;
					obj7.Init(obj8, _isAllowedToUpgrade: false, instance5.GetPrice(originalParameter10.id, 4));
					break;
				}
				case 4:
				{
					UpgradeButtonController obj = ultimateUpgradeButtonController[0];
					TowerModel obj2 = towerModel;
					TowerParameter instance2 = TowerParameter.Instance;
					Tower originalParameter7 = towerModel.OriginalParameter;
					obj.Init(obj2, _isAllowedToUpgrade: true, instance2.GetPrice(originalParameter7.id, 3));
					UpgradeButtonController obj3 = ultimateUpgradeButtonController[1];
					TowerModel obj4 = towerModel;
					TowerParameter instance3 = TowerParameter.Instance;
					Tower originalParameter8 = towerModel.OriginalParameter;
					obj3.Init(obj4, _isAllowedToUpgrade: true, instance3.GetPrice(originalParameter8.id, 4));
					break;
				}
				default:
					ultimateUpgradeButtonController[0].Init(towerModel, _isAllowedToUpgrade: false, 0);
					ultimateUpgradeButtonController[1].Init(towerModel, _isAllowedToUpgrade: false, 0);
					break;
				}
			}
			else
			{
				ultimateInforButtonController.gameObject.SetActive(value: true);
				upgradeButtonController.gameObject.SetActive(value: false);
				ultimateUpgradeButtonController[0].gameObject.SetActive(value: false);
				ultimateUpgradeButtonController[1].gameObject.SetActive(value: false);
				upgradeUltimate0ButtonController.gameObject.SetActive(value: true);
				upgradeUltimate0ButtonController.Init(towerModel);
				upgradeUltimate1ButtonController.gameObject.SetActive(value: true);
				upgradeUltimate1ButtonController.Init(towerModel);
			}
		}

		public void OnSell()
		{
			towerModel.Sell();
			Close();
		}

		protected override void OnClickOutsideUp()
		{
			base.OnClickOutsideUp();
			Close();
			GameplayManager.Instance.CurrentTowerRange.GetComponent<TowerRangeController>().HideRange();
		}

		public override void Open()
		{
			base.Open();
			if (SingletonMonoBehaviour<UIRootController>.Instance.BuyTowerPopupController.isOpen)
			{
				SingletonMonoBehaviour<UIRootController>.Instance.BuyTowerPopupController.Close();
			}
			base.gameObject.SetActive(value: true);
			offset = listOffsetTower[towerModel.Id];
			if (towerModel.Id == 1)
			{
				buttonChangePosition.SetActive(value: true);
			}
			else
			{
				buttonChangePosition.SetActive(value: false);
			}
			tween.Kill();
			Content.transform.localScale = 0.5f * Vector3.one;
			tween = Content.transform.DOScale(1f, timeToOpen).SetEase(Ease.OutBack).OnComplete(OnOpenComplete);
			if (towerControllerCollider == null)
			{
				towerControllerCollider = (Object.Instantiate(Resources.Load("UI Gameplay/Popups/TowerControllerCollider")) as GameObject);
			}
			Vector3 position = target.position;
			position.z = -0.5f;
			position.y += 0.3f;
			towerControllerCollider.transform.position = position;
			towerControllerCollider.SetActive(value: true);
		}

		private void OnOpenComplete()
		{
		}

		public override void Close()
		{
			base.Close();
			target = null;
			nextLevelInfomationPopoup.Close();
			currentLevelInformationPopup.Close();
			ultimateInforGroup.HideList();
			tween.Kill();
			tween = Content.transform.DOScale(0f, timeToClose).SetEase(Ease.InBack).OnComplete(OnCloseComplete);
			if (towerControllerCollider != null)
			{
				towerControllerCollider.SetActive(value: false);
			}
		}

		private void OnCloseComplete()
		{
			base.transform.position = PoolPos;
			base.gameObject.SetActive(value: false);
		}
	}
}

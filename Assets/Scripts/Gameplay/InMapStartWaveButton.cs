using Middle;
using Parameter;
using Services.PlatformSpecific;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
	public class InMapStartWaveButton : ControlWaveButtonController
	{
		[SerializeField]
		private float offset = 40f;

		[SerializeField]
		private Image imageCooldown;

		[SerializeField]
		private Image indicator;

		[SerializeField]
		private GameObject flyEnemiesImage;

		private Vector3 StartWavePosition;

		private Vector3 OriginPos;

		private InMapStartWaveButtonsManager inMapStartWaveButtonsManager;

		private RectTransform rectTransform;

		private void Awake()
		{
			rectTransform = GetComponent<RectTransform>();
		}

		public override void Update()
		{
			base.Update();
			indicator.transform.right = base.transform.position - StartWavePosition;
		}

		public void Initialize(Vector3 position, float canvasHeight)
		{
			StartWavePosition = position;
			OriginPos = position * 100f;
			float num = Camera.main.orthographicSize * 2f;
			float num2 = num * (float)Screen.width / (float)Screen.height;
			OriginPos.x = Mathf.Clamp(OriginPos.x, 0f - num2 * 100f / 2f + offset, num2 * 100f / 2f - offset);
			OriginPos.y = Mathf.Clamp(OriginPos.y, 0f - num * 100f / 2f + offset, num * 100f / 2f - offset);
			if (OriginPos.y > canvasHeight / 2f - 100f)
			{
				OriginPos.x = Mathf.Clamp(OriginPos.x, -350f, 430f);
			}
			if (OriginPos.y < 0f - (canvasHeight / 2f - 100f) && OriginPos.x > 245f)
			{
				OriginPos.y = 0f - canvasHeight / 2f + 170f;
			}
			if (OriginPos.y < 0f - (canvasHeight / 2f - 100f) && OriginPos.x < -320f)
			{
				OriginPos.y = 0f - canvasHeight / 2f + 170f;
			}
			rectTransform.localPosition = OriginPos;
		}

		public void Initialize(InMapStartWaveButtonsManager inMapStartWaveButtonsManager)
		{
			this.inMapStartWaveButtonsManager = inMapStartWaveButtonsManager;
		}

		public void Show(bool isFlyEnemy)
		{
			base.gameObject.SetActive(value: true);
			flyEnemiesImage.SetActive(isFlyEnemy);
		}

		public void Hide()
		{
			base.gameObject.SetActive(value: false);
		}

		public void UpdateCountDownTime(float currentValue, float maxValue)
		{
			imageCooldown.fillAmount = currentValue / maxValue;
		}

		private void StartWave()
		{
			inMapStartWaveButtonsManager.BonusMoney(base.transform.position);
			GameplayManager.Instance.StartWave();
			Hide();
			SingletonMonoBehaviour<SpawnAlly>.Instance.RestoreHealthForAllAllies();
			SingletonMonoBehaviour<GameplayUIHeroManager>.Instance.RestoreAllCooldownSkill();
			UISoundManager.Instance.PlayCallEnemy();
			switch (ModeManager.Instance.gameMode)
			{
			case GameMode.CampaignMode:
				SendEventCallEnemies_Active();
				break;
			case GameMode.DailyTrialMode:
				if (MapRuleParameter.Instance.HaveEventNewTower(SingletonMonoBehaviour<GameData>.Instance.CurrentWave))
				{
					SingletonMonoBehaviour<UIRootController>.Instance.NewTowerInformationUIManager.InitNewTowerInformationPanel();
				}
				break;
			}
		}

		private void SendEventCallEnemies_Active()
		{
			int mapID = SingletonMonoBehaviour<GameData>.Instance.MapID;
			int currentWave = SingletonMonoBehaviour<GameData>.Instance.CurrentWave;
			string callType = "Active";
			PlatformSpecificServicesProvider.Services.Analytics.SendEvent_CallEarlyEnemies(mapID + 1, currentWave, callType);
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
			GameEventCenter.Instance.Trigger(GameEventType.OnClickButton, new ClickedObjectData(ClickedObjectType.NextWaveBtn));
		}

		protected override void OnClickAvailable()
		{
			base.OnClickAvailable();
			inMapStartWaveButtonsManager.DisableConfirmOtherButtons(this);
			SingletonMonoBehaviour<UIRootController>.Instance.incomingWavePopupController.Init(base.transform.localPosition, rectTransform.sizeDelta);
			inMapStartWaveButtonsManager.DispatchEvent_ClickButton();
		}

		protected override void OnConfirm()
		{
			base.OnClick();
			StartWave();
			SingletonMonoBehaviour<UIRootController>.Instance.incomingWavePopupController.Close();
			inMapStartWaveButtonsManager.DisableConfirmAllButton();
			inMapStartWaveButtonsManager.DispatchEvent_ConfirmButton();
		}

		protected override void OnClickOutsideDown()
		{
			base.OnClickOutsideDown();
			SingletonMonoBehaviour<UIRootController>.Instance.incomingWavePopupController.Close();
			DisableConfirm();
		}

		protected override void OnClickOutsideUp()
		{
			base.OnClickOutsideUp();
		}
	}
}

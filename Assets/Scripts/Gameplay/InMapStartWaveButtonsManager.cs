using Middle;
using Parameter;
using Services.PlatformSpecific;
using SSR.Core.Architecture;
using System.Collections.Generic;
using Tutorial;
using UnityEngine;

namespace Gameplay
{
	public class InMapStartWaveButtonsManager : MonoBehaviour
	{
		private static string STARTWAVE_BUTTON_NAME = "ButtonStartWave";

		private static string BONUSMONEY_OBJECT_NAME = "BonusMoney";

		[SerializeField]
		private OrderedEventDispatcher onClickCallEnemyButtonEvents;

		[SerializeField]
		private OrderedEventDispatcher onConfirmCallEnemyButtonEvents;

		[Space]
		[SerializeField]
		private InMapStartWaveButton prefabStartWaveButton;

		[SerializeField]
		private BonusMoneyAnimController bonusMoneyAnimController;

		private List<InMapStartWaveButton> buttons = new List<InMapStartWaveButton>();

		[SerializeField]
		private RectTransform canvas;

		[SerializeField]
		private Transform listButtonHolder;

		private float delayTime;

		private float countTime;

		private bool isCounting;

		private int freeTime;

		private int bonusMoney;

		public static InMapStartWaveButtonsManager Instance
		{
			get;
			set;
		}

		private void Awake()
		{
			if ((bool)Instance)
			{
				UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			Instance = this;
			SingletonMonoBehaviour<SpawnFX>.Instance.InitExtendObject(prefabStartWaveButton.gameObject, 0);
			SingletonMonoBehaviour<SpawnFX>.Instance.InitExtendObject(bonusMoneyAnimController.gameObject, 0);
		}

		private void Start()
		{
			if (GameplayTutorialManager.Instance.IsTutorialDone() || !GameplayTutorialManager.Instance.IsTutorialMap())
			{
				ShowListButtonHolder();
			}
			else
			{
				HideListButtonHolder();
			}
		}

		private void Update()
		{
			switch (ModeManager.Instance.gameMode)
			{
			case GameMode.CampaignMode:
				if (isCounting)
				{
					if (countTime == 0f)
					{
						countTime = 0f;
						isCounting = false;
						GameplayManager.Instance.StartWave();
						SendEventCallEnemies_Passive();
						Hide();
					}
					countTime = Mathf.MoveTowards(countTime, 0f, Time.deltaTime);
					UpdateCountDownButton();
				}
				break;
			case GameMode.TournamentMode:
				if (isCounting)
				{
					if (countTime == 0f)
					{
						countTime = 0f;
						isCounting = false;
						GameplayManager.Instance.StartWave();
						SendEventCallEnemies_Passive();
						Hide();
					}
					countTime = Mathf.MoveTowards(countTime, 0f, Time.deltaTime);
					UpdateCountDownButton();
				}
				break;
			}
		}

		public void InitListButtons()
		{
			CreateListButtons();
			GetListButtons();
			InitializeButtons();
		}

		private void SendEventCallEnemies_Passive()
		{
			switch (ModeManager.Instance.gameMode)
			{
			case GameMode.CampaignMode:
			{
				int mapID = SingletonMonoBehaviour<GameData>.Instance.MapID;
				int currentWave = SingletonMonoBehaviour<GameData>.Instance.CurrentWave;
				string callType = "Passive";
				PlatformSpecificServicesProvider.Services.Analytics.SendEvent_CallEarlyEnemies(mapID + 1, currentWave, callType);
				break;
			}
			}
		}

		private void CreateListButtons()
		{
			List<Transform> listStartPosition = SingletonMonoBehaviour<StartWavePositionManager>.Instance.listStartPosition;
			for (int i = 0; i < listStartPosition.Count; i++)
			{
				GameObject objectByName = SingletonMonoBehaviour<SpawnFX>.Instance.GetObjectByName(STARTWAVE_BUTTON_NAME);
				objectByName.transform.SetParent(listButtonHolder);
				objectByName.transform.localScale = Vector3.one;
				objectByName.SetActive(value: true);
				objectByName.GetComponent<InMapStartWaveButton>().Initialize(listStartPosition[i].position, canvas.rect.height);
			}
		}

		public void Show(int wave, List<int> listEnemyGate, float delayTime)
		{
			this.delayTime = delayTime;
			countTime = delayTime;
			isCounting = true;
			ShowButtons(wave, listEnemyGate);
		}

		public void Hide()
		{
			countTime = 0f;
			isCounting = false;
			HideButtons();
		}

		public void ShowListButtonHolder()
		{
			listButtonHolder.gameObject.SetActive(value: true);
		}

		public void HideListButtonHolder()
		{
			listButtonHolder.gameObject.SetActive(value: false);
		}

		public void DisableConfirmOtherButtons(ControlWaveButtonController NoDisableButton)
		{
			foreach (InMapStartWaveButton button in buttons)
			{
				if (!button.Equals(NoDisableButton))
				{
					button.DisableConfirm();
				}
			}
		}

		public void DisableConfirmAllButton()
		{
			foreach (InMapStartWaveButton button in buttons)
			{
				button.DisableConfirm();
			}
		}

		public void BonusMoney(Vector3 buttonPosition)
		{
			switch (ModeManager.Instance.gameMode)
			{
			case GameMode.CampaignMode:
				bonusMoney = Mathf.RoundToInt((float)Config.Instance.EarlyCallMoney * countTime);
				SingletonMonoBehaviour<GameData>.Instance.IncreaseMoney(bonusMoney);
				if (bonusMoney > 0)
				{
					GameObject objectByName = SingletonMonoBehaviour<SpawnFX>.Instance.GetObjectByName(BONUSMONEY_OBJECT_NAME);
					objectByName.transform.parent = base.gameObject.transform;
					objectByName.transform.localScale = Vector3.one;
					objectByName.SetActive(value: true);
					objectByName.transform.position = buttonPosition;
					objectByName.GetComponent<BonusMoneyAnimController>().Init(bonusMoney);
				}
				break;
			case GameMode.DailyTrialMode:
			{
				int goldBonusForCallEnemy = MapRuleParameter.Instance.GetGoldBonusForCallEnemy(SingletonMonoBehaviour<GameData>.Instance.CurrentWave);
				SingletonMonoBehaviour<GameData>.Instance.IncreaseMoney(goldBonusForCallEnemy);
				int healthBonusForCallEnemy = MapRuleParameter.Instance.GetHealthBonusForCallEnemy(SingletonMonoBehaviour<GameData>.Instance.CurrentWave);
				GameplayManager.Instance.gameLogicController.IncreaseHealth(healthBonusForCallEnemy);
				break;
			}
			}
		}

		public void ShowListButtonOnStart()
		{
			ShowButtons(0, EnemyParameterManager.Instance.getListEnemyGate(0));
		}

		private void GetListButtons()
		{
			buttons = new List<InMapStartWaveButton>(GetComponentsInChildren<InMapStartWaveButton>());
		}

		private void InitializeButtons()
		{
			foreach (InMapStartWaveButton button in buttons)
			{
				InitializeButton(button);
			}
		}

		private void InitializeButton(InMapStartWaveButton button)
		{
			button.Initialize(this);
		}

		private void ShowButtons(int wave, List<int> listGate)
		{
			HideButtons();
			for (int i = 0; i < listGate.Count; i++)
			{
				bool isFlyEnemy = EnemyParameterManager.Instance.IsFlyEnemyInGate(wave, listGate[i]);
				if (listGate[i] >= buttons.Count)
				{
					buttons[listGate[i] - buttons.Count].Show(isFlyEnemy);
				}
				else
				{
					buttons[listGate[i]].Show(isFlyEnemy);
				}
			}
			UISoundManager.Instance.PlayBeforeCallEnemy();
		}

		private void UpdateCountDownButton()
		{
			foreach (InMapStartWaveButton button in buttons)
			{
				button.UpdateCountDownTime(countTime, delayTime);
			}
		}

		private void HideButtons()
		{
			foreach (InMapStartWaveButton button in buttons)
			{
				button.Hide();
			}
			SingletonMonoBehaviour<UIRootController>.Instance.incomingWavePopupController.Close();
			DisableConfirmAllButton();
		}

		public void DispatchEvent_ClickButton()
		{
			onClickCallEnemyButtonEvents.Dispatch();
		}

		public void DispatchEvent_ConfirmButton()
		{
			onConfirmCallEnemyButtonEvents.Dispatch();
		}
	}
}

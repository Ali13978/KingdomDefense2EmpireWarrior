using DailyTrial;
using Gameplay.EndGame;
using IncomingWave;
using LifetimePopup;
using Middle;
using Tutorial;
using UnityEngine;

namespace Gameplay
{
	public class UIRootController : SingletonMonoBehaviour<UIRootController>
	{
		[Header("Game UI Controller")]
		[SerializeField]
		private BuyTowerPopupController buyTowerPopupController;

		[SerializeField]
		private UpgradeTowerPopupController upgradeTowerPopupController;

		public EnemyInformationPopup enemyInformationPopup;

		public AllyInformationPopup allyInformationPopup;

		public PetInformationPopup petInformationPopup;

		[Space]
		[Header("Game UI Elements")]
		[SerializeField]
		private Transform popupParent;

		[SerializeField]
		private Transform newTowerPopupParent;

		[SerializeField]
		private Transform incomingWavePopupParent;

		[Header("Setting Popup")]
		[SerializeField]
		private GameObject prefabSetting;

		private bool isSettingPopupExist;

		private SettingPopupController _settingPopupController;

		[Header("Endgame Popup")]
		[SerializeField]
		private GameObject prefabEndgame;

		private bool isEndGamePopupExist;

		private EndGamePopupController _endGamePopupController;

		[Header("Endgame Video Popup")]
		[SerializeField]
		private GameObject prefabEndGameVideo;

		private bool isEndGameVideoPopupExist;

		private EndGameVideoController _endGameVideoController;

		[Header("Gameplay free resources")]
		[SerializeField]
		private GameObject freeResourcesButton;

		[SerializeField]
		private GameObject prefabFreeResources;

		private bool isGameplayVideoPopupExist;

		private FreeResourcesPopupController _freeResourcesPopupController;

		[Header("Daily Trial Result")]
		[SerializeField]
		private GameObject dailyTrialResultPrefab;

		[HideInInspector]
		private bool isDailyTrialResultPopupExist;

		private DailyTrialResultPopupController _dailyTrialResultPopupController;

		[Header("Tournament Result")]
		[SerializeField]
		private GameObject tournamentResultPrefab;

		[HideInInspector]
		private bool isTournamentResultPopupExist;

		private TournamentResultPopupController _tournamentResultPopupController;

		[Header("Incoming Wave Popup")]
		[SerializeField]
		private GameObject incomingWavePopupPrefab;

		[HideInInspector]
		private IncomingWavePopupController _incomingWavePopupController;

		private InMapStartWaveButtonsManager inMapStartWaveButtonsManager;

		[Space]
		[Header("Game UI Infomation")]
		public MoneyController moneyController;

		public PlayerHealthController playerHealthController;

		public UICinematicEffectController uICinematicEffectController;

		private WaveMessageController waveMessageController;

		[Space]
		[Header("Wave message:")]
		[SerializeField]
		private WaveMessageController waveMessageCampaignPrefab;

		[SerializeField]
		private WaveMessageController waveMessageDailyTrialPrefab;

		[SerializeField]
		private WaveMessageController waveMessageTournamentPrefab;

		[SerializeField]
		private Transform waveHolderCampaign;

		[SerializeField]
		private Transform waveHolderDailyTrial;

		[SerializeField]
		private Transform waveHolderTournament;

		[Space]
		[SerializeField]
		private GameObject buttonSpeed;

		[Space]
		[Header("Game new tips")]
		[SerializeField]
		private NewTowerInformationUIManager newTowerInformationUIManager;

		[SerializeField]
		private Transform popupParentNewEnemy;

		[SerializeField]
		private GameObject enemyInforPopupPrefab;

		private bool isNewEnemyInformationPopupExist;

		private NewEnemyInformationPopup newEnemyInformationPopup;

		[SerializeField]
		private Transform popupParentGameplayTips;

		[SerializeField]
		private GameObject tipPopupPrefab;

		private bool isGameplayTipsInformationPopupExist;

		private GameplayTipInformationPopup gameplayTipPopup;

		public BuyTowerPopupController BuyTowerPopupController
		{
			get
			{
				return buyTowerPopupController;
			}
			set
			{
				buyTowerPopupController = value;
			}
		}

		public UpgradeTowerPopupController UpgradeTowerPopupController
		{
			get
			{
				return upgradeTowerPopupController;
			}
			set
			{
				upgradeTowerPopupController = value;
			}
		}

		public SettingPopupController settingPopupController
		{
			get
			{
				if (_settingPopupController == null)
				{
					GameObject gameObject = Object.Instantiate(prefabSetting);
					gameObject.transform.SetParent(popupParent);
					gameObject.transform.localScale = Vector3.one;
					gameObject.transform.localPosition = Vector3.zero;
					_settingPopupController = gameObject.GetComponent<SettingPopupController>();
					isSettingPopupExist = true;
				}
				return _settingPopupController;
			}
			set
			{
				_settingPopupController = value;
			}
		}

		public EndGamePopupController endGamePopupController
		{
			get
			{
				if (_endGamePopupController == null)
				{
					GameObject gameObject = Object.Instantiate(prefabEndgame);
					gameObject.transform.SetParent(popupParent);
					gameObject.transform.localScale = Vector3.one;
					gameObject.transform.localPosition = Vector3.zero;
					_endGamePopupController = gameObject.GetComponent<EndGamePopupController>();
					isEndGamePopupExist = true;
				}
				return _endGamePopupController;
			}
			set
			{
				_endGamePopupController = value;
			}
		}

		public EndGameVideoController endGameVideoController
		{
			get
			{
				if (_endGameVideoController == null)
				{
					GameObject gameObject = Object.Instantiate(prefabEndGameVideo);
					gameObject.transform.SetParent(popupParent);
					gameObject.transform.localScale = Vector3.one;
					gameObject.transform.localPosition = Vector3.zero;
					_endGameVideoController = gameObject.GetComponent<EndGameVideoController>();
					isEndGameVideoPopupExist = true;
				}
				return _endGameVideoController;
			}
			set
			{
				_endGameVideoController = value;
			}
		}

		public FreeResourcesPopupController freeResourcesPopupController
		{
			get
			{
				if (_freeResourcesPopupController == null)
				{
					GameObject gameObject = Object.Instantiate(prefabFreeResources);
					gameObject.transform.SetParent(popupParent);
					gameObject.transform.localScale = Vector3.one;
					gameObject.transform.localPosition = Vector3.zero;
					_freeResourcesPopupController = gameObject.GetComponent<FreeResourcesPopupController>();
					isGameplayVideoPopupExist = true;
				}
				return _freeResourcesPopupController;
			}
			set
			{
				_freeResourcesPopupController = value;
			}
		}

		public DailyTrialResultPopupController dailyTrialResultPopupController
		{
			get
			{
				if (_dailyTrialResultPopupController == null)
				{
					GameObject gameObject = Object.Instantiate(dailyTrialResultPrefab);
					gameObject.transform.SetParent(popupParent);
					gameObject.transform.localScale = Vector3.one;
					gameObject.transform.localPosition = Vector3.zero;
					_dailyTrialResultPopupController = gameObject.GetComponent<DailyTrialResultPopupController>();
					isDailyTrialResultPopupExist = true;
				}
				return _dailyTrialResultPopupController;
			}
			set
			{
				_dailyTrialResultPopupController = value;
			}
		}

		public TournamentResultPopupController tournamentResultPopupController
		{
			get
			{
				if (_tournamentResultPopupController == null)
				{
					GameObject gameObject = Object.Instantiate(tournamentResultPrefab);
					gameObject.transform.SetParent(popupParent);
					gameObject.transform.localScale = Vector3.one;
					gameObject.transform.localPosition = Vector3.zero;
					_tournamentResultPopupController = gameObject.GetComponent<TournamentResultPopupController>();
					isTournamentResultPopupExist = true;
				}
				return _tournamentResultPopupController;
			}
			set
			{
				_tournamentResultPopupController = value;
			}
		}

		public IncomingWavePopupController incomingWavePopupController
		{
			get
			{
				if (_incomingWavePopupController == null)
				{
					GameObject gameObject = Object.Instantiate(incomingWavePopupPrefab);
					gameObject.transform.SetParent(incomingWavePopupParent);
					gameObject.transform.localScale = Vector3.one;
					gameObject.transform.localPosition = Vector3.zero;
					_incomingWavePopupController = gameObject.GetComponent<IncomingWavePopupController>();
				}
				return _incomingWavePopupController;
			}
			set
			{
				_incomingWavePopupController = value;
			}
		}

		public InMapStartWaveButtonsManager InMapStartWaveButtonsManager
		{
			get
			{
				if (inMapStartWaveButtonsManager == null)
				{
					inMapStartWaveButtonsManager = Object.FindObjectOfType<InMapStartWaveButtonsManager>();
				}
				return inMapStartWaveButtonsManager;
			}
		}

		public WaveMessageController WaveMessageController
		{
			get
			{
				return waveMessageController;
			}
			set
			{
				waveMessageController = value;
			}
		}

		public NewTowerInformationUIManager NewTowerInformationUIManager
		{
			get
			{
				return newTowerInformationUIManager;
			}
			set
			{
				newTowerInformationUIManager = value;
			}
		}

		public NewEnemyInformationPopup NewEnemyInformationPopup
		{
			get
			{
				if (newEnemyInformationPopup == null)
				{
					GameObject gameObject = Object.Instantiate(enemyInforPopupPrefab);
					gameObject.transform.SetParent(popupParent);
					gameObject.transform.localScale = Vector3.one;
					gameObject.transform.localPosition = Vector3.zero;
					newEnemyInformationPopup = gameObject.GetComponent<NewEnemyInformationPopup>();
					isNewEnemyInformationPopupExist = true;
				}
				return newEnemyInformationPopup;
			}
			set
			{
				newEnemyInformationPopup = value;
			}
		}

		public GameplayTipInformationPopup GameplayTipPopup
		{
			get
			{
				if (gameplayTipPopup == null)
				{
					GameObject gameObject = Object.Instantiate(tipPopupPrefab);
					gameObject.transform.SetParent(popupParent);
					gameObject.transform.localScale = Vector3.one;
					gameObject.transform.localPosition = Vector3.zero;
					gameplayTipPopup = gameObject.GetComponent<GameplayTipInformationPopup>();
					isGameplayTipsInformationPopupExist = true;
				}
				return gameplayTipPopup;
			}
			set
			{
				gameplayTipPopup = value;
			}
		}

		private void Start()
		{
			if (GameplayTutorialManager.Instance.IsTutorialDone() || !GameplayTutorialManager.Instance.IsTutorialMap())
			{
				buttonSpeed.SetActive(value: true);
			}
			else
			{
				buttonSpeed.SetActive(value: false);
			}
			InitWaveMessage();
			enemyInformationPopup.Init();
			allyInformationPopup.Init();
			petInformationPopup.Init();
			InitFreeResourcesButton();
		}

		private void Update()
		{
			UpdateKeyBack();
		}

		private void InitWaveMessage()
		{
			switch (ModeManager.Instance.gameMode)
			{
			case GameMode.CampaignMode:
				waveMessageController = Object.Instantiate(waveMessageCampaignPrefab);
				waveMessageController.transform.SetParent(waveHolderCampaign);
				break;
			case GameMode.DailyTrialMode:
				waveMessageController = Object.Instantiate(waveMessageDailyTrialPrefab);
				waveMessageController.transform.SetParent(waveHolderDailyTrial);
				break;
			case GameMode.TournamentMode:
				waveMessageController = Object.Instantiate(waveMessageTournamentPrefab);
				waveMessageController.transform.SetParent(waveHolderTournament);
				break;
			}
			waveMessageController.transform.localScale = Vector3.one;
			waveMessageController.transform.localPosition = Vector3.zero;
		}

		public void ShowSpeedButton()
		{
			buttonSpeed.SetActive(value: true);
		}

		private void InitFreeResourcesButton()
		{
			switch (ModeManager.Instance.gameMode)
			{
			case GameMode.CampaignMode:
				freeResourcesButton.SetActive(value: true);
				break;
			case GameMode.DailyTrialMode:
				freeResourcesButton.SetActive(value: true);
				break;
			case GameMode.TournamentMode:
				freeResourcesButton.SetActive(value: false);
				break;
			}
		}

		public void RefreshStatusFreeResourcesButton()
		{
			if (freeResourcesPopupController.VideoLife.IsPlayed && freeResourcesPopupController.VideoMoney.IsPlayed)
			{
				freeResourcesButton.SetActive(value: false);
			}
		}

		private void UpdateKeyBack()
		{
			if (!Input.GetKeyUp(KeyCode.Escape) || SingletonMonoBehaviour<GameData>.Instance.IsPlayingVideoAds)
			{
				return;
			}
			if (SingletonMonoBehaviour<LifetimeCanvas>.Instance.NotifyPopupController.isOpen)
			{
				SingletonMonoBehaviour<LifetimeCanvas>.Instance.NotifyPopupController.Close();
			}
			else if (SingletonMonoBehaviour<LifetimeCanvas>.Instance.RewardPopupController.isOpen)
			{
				SingletonMonoBehaviour<LifetimeCanvas>.Instance.RewardPopupController.Close();
			}
			else if (isGameplayVideoPopupExist && freeResourcesPopupController.isOpen)
			{
				freeResourcesPopupController.Close();
			}
			else if ((!isEndGameVideoPopupExist || !endGameVideoController.isOpen) && !SingletonMonoBehaviour<GameData>.Instance.IsGameOver)
			{
				if ((bool)NewTowerInformationUIManager.NewTowerInforPanel && NewTowerInformationUIManager.NewTowerInforPanel.isOpen)
				{
					NewTowerInformationUIManager.NewTowerInforPanel.CloseWithScaleAnimation();
				}
				else if (isNewEnemyInformationPopupExist && NewEnemyInformationPopup.isOpen)
				{
					NewEnemyInformationPopup.CloseWithScaleAnimation();
				}
				else if (isGameplayTipsInformationPopupExist && GameplayTipPopup.isOpen)
				{
					GameplayTipPopup.CloseWithScaleAnimation();
				}
				else if (!isSettingPopupExist || !settingPopupController.isOpen)
				{
					settingPopupController.Open();
				}
				else if (isSettingPopupExist && settingPopupController.isOpen)
				{
					settingPopupController.Close();
				}
			}
		}

		private void OnApplicationPause(bool pause)
		{
			if (pause && !Loading.Instance.IsLoading && !SingletonMonoBehaviour<GameData>.Instance.IsGameOver && !SingletonMonoBehaviour<GameData>.Instance.IsPlayingVideoAds)
			{
				settingPopupController.Open();
			}
		}
	}
}

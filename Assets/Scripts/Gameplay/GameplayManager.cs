using Data;
using Middle;
using Parameter;
using Services.PlatformSpecific;
using SSR.Core.Architecture;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class GameplayManager : MonoBehaviour
	{
		[Header("Events")]
		[SerializeField]
		private OrderedEventDispatcher OnStartEvent;

		[Space]
		[SerializeField]
		private OrderedEventDispatcher OnEnemyDieEvent;

		[Space]
		public HeroesManager heroesManager;

		[SerializeField]
		private GameObject prefabTowerRange;

		private GameObject _currentTowerRange;

		[NonSerialized]
		public GameSpeedController gameSpeedController;

		[NonSerialized]
		public GameLogicController gameLogicController;

		[NonSerialized]
		public EndlessModeManager endlessModeManager;

		private int customWave;

		public static GameplayManager Instance
		{
			get;
			private set;
		}

		public GameObject CurrentTowerRange
		{
			get
			{
				if (_currentTowerRange == null)
				{
					_currentTowerRange = UnityEngine.Object.Instantiate(prefabTowerRange);
				}
				return _currentTowerRange;
			}
			set
			{
				_currentTowerRange = value;
			}
		}

		public void Awake()
		{
			Instance = this;
			GetAllComponents();
			SetDefaultGameData();
		}

		private void GetAllComponents()
		{
			gameSpeedController = GetComponentInChildren<GameSpeedController>();
			gameLogicController = GetComponentInChildren<GameLogicController>();
			endlessModeManager = GetComponentInChildren<EndlessModeManager>();
		}

		public void Start()
		{
			OnStartEvent.Dispatch();
			gameSpeedController.SetNormalSpeed();
			SendEventStartGame();
		}

		private void OnDestroy()
		{
			if (Instance == this)
			{
				Instance = null;
			}
		}

		private void SetDefaultGameData()
		{
			SingletonMonoBehaviour<GameData>.Instance.SetDefaultGameData();
			customWave = -1;
		}

		public void LoadMap()
		{
			string path = string.Empty;
			switch (ModeManager.Instance.gameMode)
			{
			case GameMode.CampaignMode:
				path = $"Maps/map_campaign_{SingletonMonoBehaviour<GameData>.Instance.MapID}";
				break;
			case GameMode.DailyTrialMode:
			{
				int currentDayIndex = ReadWriteDataDailyTrial.Instance.GetCurrentDayIndex();
				DailyTrialParam parameter = DailyTrialParameter.Instance.GetParameter(currentDayIndex);
				int map_id = parameter.map_id;
				path = $"Maps/map_daily_{map_id}";
				break;
			}
			case GameMode.TournamentMode:
			{
				int currentSeasonMapID = MapRuleParameter.Instance.GetCurrentSeasonMapID();
				path = $"Maps/map_tournament_{currentSeasonMapID}";
				break;
			}
			}
			GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load(path)) as GameObject;
			gameObject.transform.SetParent(base.transform);
			gameObject.transform.position = Vector3.zero;
		}

		private void SendEventStartGame()
		{
			if (ModeManager.Instance.gameMode == GameMode.CampaignMode)
			{
				int mapID = SingletonMonoBehaviour<GameData>.Instance.MapID;
				List<int> listHeroesIdsSelected = SingletonMonoBehaviour<GameData>.Instance.ListHeroesIdsSelected;
				string heroName = Singleton<HeroDescription>.Instance.GetHeroName(listHeroesIdsSelected[0]);
				int hero1Level = ReadWriteDataHero.Instance.GetCurrentHeroLevel(listHeroesIdsSelected[0]) + 1;
				string hero2Name = "Empty";
				int hero2Level = -1;
				if (listHeroesIdsSelected.Count > 1)
				{
					hero2Name = Singleton<HeroDescription>.Instance.GetHeroName(listHeroesIdsSelected[1]);
					hero2Level = ReadWriteDataHero.Instance.GetCurrentHeroLevel(listHeroesIdsSelected[1]) + 1;
				}
				string hero3Name = "Empty";
				int hero3Level = -1;
				if (listHeroesIdsSelected.Count > 2)
				{
					hero3Name = Singleton<HeroDescription>.Instance.GetHeroName(listHeroesIdsSelected[2]);
					hero3Level = ReadWriteDataHero.Instance.GetCurrentHeroLevel(listHeroesIdsSelected[2]) + 1;
				}
				int currentItemQuantity = ReadWriteDataPowerUpItem.Instance.GetCurrentItemQuantity(0);
				int currentItemQuantity2 = ReadWriteDataPowerUpItem.Instance.GetCurrentItemQuantity(1);
				int currentItemQuantity3 = ReadWriteDataPowerUpItem.Instance.GetCurrentItemQuantity(2);
				int currentItemQuantity4 = ReadWriteDataPowerUpItem.Instance.GetCurrentItemQuantity(3);
				PlatformSpecificServicesProvider.Services.Analytics.SendEvent_StartGame(mapID + 1, heroName, hero1Level, hero2Name, hero2Level, hero3Name, hero3Level, currentItemQuantity, currentItemQuantity2, currentItemQuantity3, currentItemQuantity4);
			}
		}

		public void OnEnemyDie()
		{
			OnEnemyDieEvent.Dispatch();
		}

		public void ReloadCurrentScene()
		{
			GameApplication.Instance.ReloadCurrentScene();
		}

		public void StartWave()
		{
			switch (ModeManager.Instance.gameMode)
			{
			case GameMode.CampaignMode:
				StartWave_CampaignMode();
				break;
			case GameMode.DailyTrialMode:
				StartWave_CampaignMode();
				break;
			case GameMode.TournamentMode:
				StartWave_TournamentMode();
				break;
			}
			SingletonMonoBehaviour<UIRootController>.Instance.InMapStartWaveButtonsManager.Hide();
			SingletonMonoBehaviour<GameData>.Instance.IsGameStart = true;
			SingletonMonoBehaviour<UIRootController>.Instance.WaveMessageController.SetWaveMessage();
		}

		private void StartWave_CampaignMode()
		{
			if (SingletonMonoBehaviour<GameData>.Instance.CurrentWave < SingletonMonoBehaviour<GameData>.Instance.TotalWave)
			{
				SingletonMonoBehaviour<GameData>.Instance.CurrentWave++;
				if (SingletonMonoBehaviour<GameData>.Instance.CurrentWave < customWave)
				{
					SingletonMonoBehaviour<GameData>.Instance.CurrentWave = customWave;
				}
				SingletonMonoBehaviour<SpawnEnemy>.Instance.StartWaveNormal(SingletonMonoBehaviour<GameData>.Instance.CurrentWave - 1);
			}
		}

		private void StartWave_TournamentMode()
		{
			endlessModeManager.IncreaseWavePassed();
			if (SingletonMonoBehaviour<GameData>.Instance.CurrentWave < SingletonMonoBehaviour<GameData>.Instance.TotalWave)
			{
				SingletonMonoBehaviour<GameData>.Instance.CurrentWave++;
				SingletonMonoBehaviour<SpawnEnemy>.Instance.StartWaveNormal(SingletonMonoBehaviour<GameData>.Instance.CurrentWave - 1);
			}
			if (endlessModeManager.IsLastEnemyInNormalWave)
			{
				endlessModeManager.FirstTimeIncreaseLoopAmount();
				SingletonMonoBehaviour<SpawnEnemy>.Instance.StartWaveNormal(endlessModeManager.CurrentWaveEndless);
				endlessModeManager.IncreaseCurrentWaveEndless();
			}
		}

		public void ShowStartWaveButton(int wave, List<int> listEnemyGate)
		{
			SingletonMonoBehaviour<UIRootController>.Instance.InMapStartWaveButtonsManager.Show(wave, listEnemyGate, (float)SingletonMonoBehaviour<GameData>.Instance.DeltaTimeWave / 1000f);
		}

		public void SetCustomWaveForTest(int customWave)
		{
			this.customWave = customWave;
		}

		public void GetEndingVideoReward()
		{
			int endGameVideoReward = SingletonMonoBehaviour<GameplayDataReader>.Instance.ReadDataEndGameVideo.GetEndGameVideoReward();
			gameLogicController.IncreaseHealth(endGameVideoReward);
		}
	}
}

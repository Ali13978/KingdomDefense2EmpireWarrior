using Data;
using Middle;
using MyCustom;
using Services.PlatformSpecific;

namespace Gameplay
{
	public class GameLogicController : CustomMonoBehaviour
	{
		private void Start()
		{
			SingletonMonoBehaviour<GameData>.Instance.OnMoneyChange += Instance_OnMoneyChange;
		}

		private void Instance_OnMoneyChange()
		{
			UpdateMoney();
		}

		private void Update()
		{
			UpdateMoney();
			UpdateHealth();
			UpdateVictory();
		}

		private void OnDestroy()
		{
			SingletonMonoBehaviour<GameData>.Instance.OnMoneyChange -= Instance_OnMoneyChange;
		}

		private void UpdateHealth()
		{
			SingletonMonoBehaviour<UIRootController>.Instance.playerHealthController.SetHealthMessage();
		}

		private void UpdateMoney()
		{
			SingletonMonoBehaviour<UIRootController>.Instance.moneyController.SetMoneyMessage();
		}

		private void UpdateVictory()
		{
			if (SingletonMonoBehaviour<GameData>.Instance.IsLastEnemyInBattle && SingletonMonoBehaviour<GameData>.Instance.TotalEnemy <= 0 && SingletonMonoBehaviour<GameData>.Instance.ListActiveEnemy.Count == 0 && SingletonMonoBehaviour<GameData>.Instance.CurrentHealth > 0 && !SingletonMonoBehaviour<GameData>.Instance.IsVictory)
			{
				switch (ModeManager.Instance.gameMode)
				{
				case GameMode.CampaignMode:
					SingletonMonoBehaviour<GameData>.Instance.IsVictory = true;
					CustomInvoke(OnCampaignVictory, 2f);
					break;
				case GameMode.DailyTrialMode:
					SingletonMonoBehaviour<GameData>.Instance.IsVictory = true;
					CustomInvoke(OnDailyTrialResult_Victory, 2f);
					break;
				case GameMode.TournamentMode:
					OnTournament_EndGame();
					break;
				}
			}
		}

		public void IncreaseHealth(int health)
		{
			SingletonMonoBehaviour<GameData>.Instance.CurrentHealth += health;
			if (SingletonMonoBehaviour<GameData>.Instance.CurrentHealth <= 0)
			{
				switch (ModeManager.Instance.gameMode)
				{
				case GameMode.CampaignMode:
					OnCampaign_Defeat();
					break;
				case GameMode.DailyTrialMode:
					OnDailyTrialResult_Defeat();
					break;
				case GameMode.TournamentMode:
					OnTournament_EndGame();
					break;
				}
			}
			UpdateHealth();
		}

		public void DecreaseHealth(int health)
		{
			if (SingletonMonoBehaviour<GameData>.Instance.CurrentHealth <= 0)
			{
				return;
			}
			SingletonMonoBehaviour<GameData>.Instance.CurrentHealth -= health;
			if (SingletonMonoBehaviour<GameData>.Instance.CurrentHealth <= 0)
			{
				switch (ModeManager.Instance.gameMode)
				{
				case GameMode.CampaignMode:
					OnCampaign_Defeat();
					break;
				case GameMode.DailyTrialMode:
					OnDailyTrialResult_Defeat();
					break;
				case GameMode.TournamentMode:
					OnTournament_EndGame();
					break;
				}
			}
			UpdateHealth();
		}

		private void OnCampaign_Defeat()
		{
			if (VideoPlayerManager.Instance.CheckIfVideoExits())
			{
				GameplayManager.Instance.gameSpeedController.PauseGame();
				if (!SingletonMonoBehaviour<GameData>.Instance.PlayedVideoEndGame)
				{
					SingletonMonoBehaviour<UIRootController>.Instance.endGameVideoController.Init();
					return;
				}
				GameplayManager.Instance.gameSpeedController.UnPauseGame();
				Defeated();
			}
			else
			{
				Defeated();
			}
		}

		public void Defeated()
		{
			SingletonMonoBehaviour<GameData>.Instance.CurrentHealth = 0;
			SingletonMonoBehaviour<UIRootController>.Instance.endGamePopupController.Init(BattleStatus.Defeat);
			ReadWriteDataMap.Instance.IncreaseMapPlaycount(SingletonMonoBehaviour<GameData>.Instance.MapID, BattleStatus.Defeat);
			UISoundManager.Instance.PlayDefeat();
		}

		private void OnCampaignVictory()
		{
			SingletonMonoBehaviour<UIRootController>.Instance.endGamePopupController.Init(BattleStatus.Victory);
			ReadWriteDataMap.Instance.IncreaseMapIdUnlock(SingletonMonoBehaviour<GameData>.Instance.MapID);
			ReadWriteDataMap.Instance.IncreaseModeResult(SingletonMonoBehaviour<GameData>.Instance.MapID);
			ReadWriteDataMap.Instance.IncreaseMapPlaycount(SingletonMonoBehaviour<GameData>.Instance.MapID);
			ReadWriteDataMap.Instance.IncreaseMapPlaycount(SingletonMonoBehaviour<GameData>.Instance.MapID, BattleStatus.Victory);
			int actuallyGemAmount = SingletonMonoBehaviour<GameData>.Instance.GetActuallyGemAmount();
			ReadWriteDataPlayerCurrency.Instance.ChangeGem(actuallyGemAmount, isDispatchEventChange: false);
			UISoundManager.Instance.PlayVictory();
		}

		public void EndGame()
		{
			SingletonMonoBehaviour<GameData>.Instance.IsGameOver = true;
			GameplayManager.Instance.gameSpeedController.SetNormalSpeed();
			SingletonMonoBehaviour<UIRootController>.Instance.uICinematicEffectController.MoveUIOut();
			PlatformSpecificServicesProvider.Services.DataCloudSaver.AutoBackUpData();
		}

		private void OnDailyTrialResult_Victory()
		{
			SingletonMonoBehaviour<GameData>.Instance.IsGameOver = true;
			GameplayManager.Instance.gameSpeedController.SetNormalSpeed();
			SingletonMonoBehaviour<UIRootController>.Instance.dailyTrialResultPopupController.Init(BattleStatus.Victory);
			SendEvent_EndGameDailyTrial(SingletonMonoBehaviour<GameData>.Instance.CurrentWave);
		}

		private void OnDailyTrialResult_Defeat()
		{
			SingletonMonoBehaviour<GameData>.Instance.IsGameOver = true;
			GameplayManager.Instance.gameSpeedController.SetNormalSpeed();
			SingletonMonoBehaviour<UIRootController>.Instance.dailyTrialResultPopupController.Init(BattleStatus.Defeat);
			SendEvent_EndGameDailyTrial(SingletonMonoBehaviour<GameData>.Instance.CurrentWave - 1);
		}

		private void SendEvent_EndGameDailyTrial(int currentWavePassed)
		{
			int currentDayIndex = ReadWriteDataDailyTrial.Instance.GetCurrentDayIndex();
			int playCount = 0;
			int mapIDUnlocked = ReadWriteDataMap.Instance.GetMapIDUnlocked();
			PlatformSpecificServicesProvider.Services.Analytics.SendEvent_EndGameDailyTrial(currentDayIndex, currentWavePassed, playCount, mapIDUnlocked);
		}

		private void OnTournament_EndGame()
		{
			SingletonMonoBehaviour<GameData>.Instance.IsGameOver = true;
			GameplayManager.Instance.gameSpeedController.SetNormalSpeed();
			SingletonMonoBehaviour<UIRootController>.Instance.tournamentResultPopupController.Init();
		}
	}
}

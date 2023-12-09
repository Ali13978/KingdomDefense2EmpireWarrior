using Data;
using Gameplay.EndGame.Reward;
using HeroLevelUp;
using LifetimePopup;
using Parameter;
using Services.PlatformSpecific;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.EndGame
{
	public class EndGamePopupController : GameplayPopupController
	{
		[Space]
		[Header("Attribute")]
		[SerializeField]
		private GameObject victoryGroup;

		[SerializeField]
		private GameObject defeatGroup;

		[Space]
		[Header("Victory elements")]
		[SerializeField]
		private EndGameRewardPopupController endGameRewardPopupController;

		[SerializeField]
		private HeroesLevelUpItemGroupController heroesLevelUpItemGroupController;

		[SerializeField]
		private Text gemAmountText;

		[SerializeField]
		private List<GameObject> listStar;

		[Space]
		[SerializeField]
		private float delayTimeToOpen;

		[SerializeField]
		private float delayTImeToInvokeStars;

		[SerializeField]
		private float timeBetweenStars;

		private BattleStatus battleStatus;

		private int numberStar;

		public EndGameRewardPopupController EndGameRewardPopupController
		{
			get
			{
				return endGameRewardPopupController;
			}
			set
			{
				endGameRewardPopupController = value;
			}
		}

		public BattleStatus BattleStatus
		{
			get
			{
				return battleStatus;
			}
			private set
			{
				battleStatus = value;
			}
		}

		public int NumberStar
		{
			get
			{
				return numberStar;
			}
			set
			{
				numberStar = value;
			}
		}

		public void Init(BattleStatus battleStatus)
		{
			BattleStatus = battleStatus;
			GameplayManager.Instance.gameLogicController.EndGame();
			SendEvent_EndGame();
			CustomInvoke(((PopupController)this).Open, delayTimeToOpen);
			StopBackgroundMusic();
			CloseAnyPopup();
		}

		private void CloseAnyPopup()
		{
		}

		private void StopBackgroundMusic()
		{
		}

		private void Victory()
		{
			for (int num = SingletonMonoBehaviour<GameData>.Instance.ListHeroesIdsSelected.Count - 1; num >= 0; num--)
			{
				GameEventCenter.Instance.Trigger(GameEventType.EventUseHero, new EventTriggerData(EventTriggerType.UseHeroWinCampaign, SingletonMonoBehaviour<GameData>.Instance.ListHeroesIdsSelected[num], 1, forceSaveProgress: true));
			}
			victoryGroup.SetActive(value: true);
			heroesLevelUpItemGroupController.InitData();
			PlaySoundVictory();
			StartCoroutine(PlayStarsEffect());
			DisplayGem();
			SendEventEndGame_Victory();
		}

		private void SendEvent_EndGame()
		{
			PlatformSpecificServicesProvider.Services.Analytics.SendEvent_EndGame();
		}

		private void SendEventEndGame_Victory()
		{
			int mapID = SingletonMonoBehaviour<GameData>.Instance.MapID + 1;
			int starEarned = NumberStar;
			int currentPlayCount = ReadWriteDataMap.Instance.GetCurrentPlayCount(SingletonMonoBehaviour<GameData>.Instance.MapID);
			int actuallyGemAmount = SingletonMonoBehaviour<GameData>.Instance.GetActuallyGemAmount();
			PlatformSpecificServicesProvider.Services.Analytics.SendEvent_EndGame(mapID, starEarned, actuallyGemAmount, currentPlayCount);
		}

		private void DisplayGem()
		{
			gemAmountText.text = SingletonMonoBehaviour<GameData>.Instance.GetActuallyGemAmount().ToString();
		}

		private void PlaySoundVictory()
		{
		}

		private void CalculatorStarCount()
		{
			NumberStar = StarParameter.Instance.GetStar(100 * SingletonMonoBehaviour<GameData>.Instance.CurrentHealth / SingletonMonoBehaviour<GameData>.Instance.TotalHealth);
			UnityEngine.Debug.Log("number star = " + NumberStar);
			ReadWriteDataMap.Instance.SaveStarEarned(SingletonMonoBehaviour<GameData>.Instance.MapID, NumberStar);
		}

		private IEnumerator PlayStarsEffect()
		{
			CalculatorStarCount();
			yield return new WaitForSeconds(delayTImeToInvokeStars);
			for (int i = 0; i < NumberStar; i++)
			{
				listStar[i].SetActive(value: true);
				yield return new WaitForSeconds(timeBetweenStars);
			}
		}

		public void PlayStarSound()
		{
		}

		public void TryInitEndGameRewardPopup()
		{
			int currentPlayCount = ReadWriteDataMap.Instance.GetCurrentPlayCount(SingletonMonoBehaviour<GameData>.Instance.MapID);
			UnityEngine.Debug.Log("current map play count = " + currentPlayCount);
			if (currentPlayCount > 1)
			{
				Loading.Instance.ShowLoading();
				Invoke("DoLoad", 1f);
				GameplayManager.Instance.gameSpeedController.UnPauseGame();
			}
			else
			{
				EndGameRewardPopupController.Init();
				victoryGroup.SetActive(value: false);
			}
		}

		private void DoLoad()
		{
			VideoPlayerManager.Instance.TryToShowInterstitialAds_EndGame();
			GameApplication.Instance.LoadScene(GameApplication.WorldMapSceneName);
			SingletonMonoBehaviour<LifetimeCanvas>.Instance.TryToOpenPopupAskRate(SingletonMonoBehaviour<GameData>.Instance.MapID);
		}

		private void Defeat()
		{
			defeatGroup.SetActive(value: true);
			PlaySoundDefeat();
		}

		private void PlaySoundDefeat()
		{
		}

		public override void Open()
		{
			base.Open();
			OpenWithScaleAnimation();
			switch (BattleStatus)
			{
			case BattleStatus.Victory:
				GameEventCenter.Instance.Trigger(GameEventType.EventCampaign, new EventTriggerData(EventTriggerType.WinCampaign, 1, forceSaveProgress: true));
				Victory();
				break;
			case BattleStatus.Defeat:
				GameEventCenter.Instance.Trigger(GameEventType.EventCampaign, new EventTriggerData(EventTriggerType.LoseCampaign, 1, forceSaveProgress: true));
				Defeat();
				break;
			}
		}

		public override void Close()
		{
			base.Close();
			base.gameObject.SetActive(value: false);
		}
	}
}

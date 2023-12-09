using ApplicationEntry;
using LifetimePopup;
using Services.PlatformSpecific;
using UnityEngine;

namespace Gameplay
{
	public class VideoPlayerManager
	{
		private static VideoPlayerManager instance;

		public static VideoPlayerManager Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new VideoPlayerManager();
				}
				return instance;
			}
			set
			{
				instance = value;
			}
		}

		public bool CheckIfVideoExits()
		{
			return PlatformSpecificServicesProvider.Services.Ad.IsOfferVideoAvailable;
		}

		public void playVideoGameplay_ForMoney()
		{
			PlatformSpecificServicesProvider.Services.Ad.ShowOfferVideo(OfferVideoGameplayCallback_Money);
			SingletonMonoBehaviour<GameData>.Instance.IsPlayingVideoAds = true;
		}

		private void OfferVideoGameplayCallback_Money(bool completed)
		{
			if (completed)
			{
				GetGameplayVideoReward_Money();
			}
			SingletonMonoBehaviour<GameData>.Instance.IsPlayingVideoAds = false;
		}

		private void GetGameplayVideoReward_Money()
		{
			int rewardValue = SingletonMonoBehaviour<UIRootController>.Instance.freeResourcesPopupController.ReadDataAdsReward.GetRewardValue("unity_vd_rd_money");
			SingletonMonoBehaviour<GameData>.Instance.IncreaseMoney(rewardValue);
			UnityEngine.Debug.Log("Get reward video: Money + " + rewardValue);
			SingletonMonoBehaviour<GameData>.Instance.PlayedGameplayVideo_ForMoney = true;
			RewardItem[] array = new RewardItem[1];
			RewardItem rewardItem = new RewardItem();
			rewardItem.rewardType = RewardType.Money;
			rewardItem.value = rewardValue;
			rewardItem.isDisplayQuantity = true;
			array[0] = rewardItem;
			SingletonMonoBehaviour<LifetimeCanvas>.Instance.RewardPopupController.Init(array);
			SendEvent_GetRewardMoneyComplete();
		}

		private void SendEvent_GetRewardMoneyComplete()
		{
			int currentMapID = SingletonMonoBehaviour<GameData>.Instance.MapID + 1;
			string rewardType = "Reward: Money";
			PlatformSpecificServicesProvider.Services.Analytics.SendEvent_WatchGameplayVideoRewardComplete(currentMapID, rewardType);
		}

		public void playVideoGameplay_ForLife()
		{
			PlatformSpecificServicesProvider.Services.Ad.ShowOfferVideo(OfferVideoGameplayCallback_Life);
			SingletonMonoBehaviour<GameData>.Instance.IsPlayingVideoAds = true;
		}

		private void OfferVideoGameplayCallback_Life(bool completed)
		{
			if (completed)
			{
				GetGameplayVideoReward_Life();
			}
			SingletonMonoBehaviour<GameData>.Instance.IsPlayingVideoAds = false;
		}

		private void GetGameplayVideoReward_Life()
		{
			int rewardValue = SingletonMonoBehaviour<UIRootController>.Instance.freeResourcesPopupController.ReadDataAdsReward.GetRewardValue("unity_vd_rd_life");
			GameplayManager.Instance.gameLogicController.IncreaseHealth(rewardValue);
			SingletonMonoBehaviour<GameData>.Instance.PlayedGameplayVideo_ForLife = true;
			UnityEngine.Debug.Log("Get reward video: Life + " + rewardValue);
			RewardItem[] array = new RewardItem[1];
			RewardItem rewardItem = new RewardItem();
			rewardItem.rewardType = RewardType.Life;
			rewardItem.value = rewardValue;
			rewardItem.isDisplayQuantity = true;
			array[0] = rewardItem;
			SingletonMonoBehaviour<LifetimeCanvas>.Instance.RewardPopupController.Init(array);
			SendEvent_GetRewardLifeComplete();
		}

		private void SendEvent_GetRewardLifeComplete()
		{
			string rewardType = "Reward: Life";
			int currentMapID = SingletonMonoBehaviour<GameData>.Instance.MapID + 1;
			PlatformSpecificServicesProvider.Services.Analytics.SendEvent_WatchGameplayVideoRewardComplete(currentMapID, rewardType);
		}

		public void playVideoGameplay_ForOpenChestOffer()
		{
			PlatformSpecificServicesProvider.Services.Ad.ShowOfferVideo(OfferVideoGameplayCallback_OpenChestOffer);
			SingletonMonoBehaviour<GameData>.Instance.IsPlayingVideoAds = true;
		}

		private void OfferVideoGameplayCallback_OpenChestOffer(bool completed)
		{
			if (completed)
			{
				GetGameplayVideoReward_OpenChestOffer();
				SingletonMonoBehaviour<GameData>.Instance.PlayedVideoLucky = true;
			}
			SingletonMonoBehaviour<GameData>.Instance.IsPlayingVideoAds = false;
		}

		private void GetGameplayVideoReward_OpenChestOffer()
		{
			UnityEngine.Debug.Log("Get reward video: Open Chest Offer + " + 3);
			SingletonMonoBehaviour<GameData>.Instance.ChangeOpenChestTurn(3);
			SingletonMonoBehaviour<GameData>.Instance.ChangeOpenChestOffer();
		}

		public void playVideoEndGame()
		{
			PlatformSpecificServicesProvider.Services.Ad.ShowOfferVideo(OfferVideoEndGameCallback);
			SingletonMonoBehaviour<GameData>.Instance.IsPlayingVideoAds = true;
		}

		private void OfferVideoEndGameCallback(bool completed)
		{
			SingletonMonoBehaviour<GameData>.Instance.PlayedVideoEndGame = completed;
			UnityEngine.Debug.Log("play video end game = " + completed);
			if (completed)
			{
				GameplayManager.Instance.GetEndingVideoReward();
				SingletonMonoBehaviour<GameData>.Instance.PlayedVideoEndGame = true;
				GameplayManager.Instance.gameSpeedController.UnPauseGame();
			}
			SingletonMonoBehaviour<GameData>.Instance.IsPlayingVideoAds = false;
		}

		public void TryToShowInterstitialAds_EndGame()
		{
			int num = ApplicationEntry.ApplicationEntry.Instance.ReadWriteRemoteSettingData.ChanceToShowInterAds_EndGame();
			if (Random.Range(0, 100) < num && SingletonMonoBehaviour<GameData>.Instance.MapID >= 2)
			{
				PlatformSpecificServicesProvider.Services.Ad.ShowInterstitial();
				UnityEngine.Debug.Log("Show Interstitial Ads");
			}
		}
	}
}

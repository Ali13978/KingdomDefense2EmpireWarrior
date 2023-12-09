using Data;
using Gameplay;
using LifetimePopup;
using Services.PlatformSpecific;
using UnityEngine;

namespace FreeResources
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

		public void PlayRewardVideo(OfferVideoCallback onCloseRewardVideoCallback)
		{
			PlatformSpecificServicesProvider.Services.Ad.ShowOfferVideo(onCloseRewardVideoCallback);
		}

		public void playVideoGameplay_ForGem()
		{
			PlatformSpecificServicesProvider.Services.Ad.ShowOfferVideo(OfferVideoGameplayCallback_Gem);
		}

		private void OfferVideoGameplayCallback_Gem(bool completed)
		{
			if (completed)
			{
				SingletonMonoBehaviour<GameData>.Instance.PlayedVideoGems = true;
				GetReward();
			}
		}

		private void GetReward()
		{
			int freeResources = PlatformSpecificServicesProvider.Services.FacebookServices.GetFreeResources("reward_id_watch_ad");
			ReadWriteDataPlayerCurrency.Instance.ChangeGem(freeResources, isDispatchEventChange: true);
			UnityEngine.Debug.Log("Get reward video: Gem + " + freeResources);
			RewardItem[] array = new RewardItem[1];
			RewardItem rewardItem = new RewardItem();
			rewardItem.rewardType = RewardType.Gem;
			rewardItem.value = freeResources;
			rewardItem.isDisplayQuantity = true;
			array[0] = rewardItem;
			SingletonMonoBehaviour<LifetimeCanvas>.Instance.RewardPopupController.Init(array);
			PlatformSpecificServicesProvider.Services.FacebookServices.SendEvent_GetFreeResourcesComplete(FreeResourcesType.WatchAds);
		}
	}
}

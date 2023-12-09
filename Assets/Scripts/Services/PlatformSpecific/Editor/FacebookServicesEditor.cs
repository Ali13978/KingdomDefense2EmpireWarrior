using Data;
using LifetimePopup;
using UnityEngine;

namespace Services.PlatformSpecific.Editor
{
	public class FacebookServicesEditor : MonoBehaviour, IFacebookServices
	{
		[SerializeField]
		private ReadDataFacebookServicesReward readDataFacebookServicesReward;

		public ReadDataFacebookServicesReward ReadDataFacebookServicesReward
		{
			get
			{
				return readDataFacebookServicesReward;
			}
			set
			{
				readDataFacebookServicesReward = value;
			}
		}

		public int GetFreeResources(string rewardID)
		{
			return ReadDataFacebookServicesReward.GetRewardAmount_Gem(rewardID);
		}

		public void InviteFriend()
		{
			int rewardAmount_Gem = ReadDataFacebookServicesReward.GetRewardAmount_Gem("reward_id_invite_friend");
			ReadWriteDataPlayerCurrency.Instance.ChangeGem(rewardAmount_Gem, isDispatchEventChange: true);
			int currentGemCollectedByInvite = ReadWriteDataFreeResources.Instance.GetCurrentGemCollectedByInvite();
			currentGemCollectedByInvite += rewardAmount_Gem;
			ReadWriteDataFreeResources.Instance.SetCurrentGemCollectedByInvite(currentGemCollectedByInvite);
			UnityEngine.Debug.Log("Test FB Invite Friend Success + reward: " + rewardAmount_Gem);
			RewardItem[] array = new RewardItem[1];
			RewardItem rewardItem = new RewardItem();
			rewardItem.rewardType = RewardType.Gem;
			rewardItem.value = rewardAmount_Gem;
			rewardItem.isDisplayQuantity = true;
			array[0] = rewardItem;
			SingletonMonoBehaviour<LifetimeCanvas>.Instance.RewardPopupController.Init(array);
			SendEvent_GetFreeResourcesComplete(FreeResourcesType.InviteFriend);
		}

		public void InviteToGroup()
		{
			if (!ReadWriteDataFreeResources.Instance.IsUserGetReward_JoinGroup())
			{
				int rewardAmount_Gem = ReadDataFacebookServicesReward.GetRewardAmount_Gem("reward_id_join_group");
				ReadWriteDataPlayerCurrency.Instance.ChangeGem(rewardAmount_Gem, isDispatchEventChange: true);
				UnityEngine.Debug.Log("Test FB Invite Group Success + reward:" + rewardAmount_Gem);
				ReadWriteDataFreeResources.Instance.SetOneTimeRewardStatus("one_time_join_group");
				RewardItem[] array = new RewardItem[1];
				RewardItem rewardItem = new RewardItem();
				rewardItem.rewardType = RewardType.Gem;
				rewardItem.value = rewardAmount_Gem;
				rewardItem.isDisplayQuantity = true;
				array[0] = rewardItem;
				SingletonMonoBehaviour<LifetimeCanvas>.Instance.RewardPopupController.Init(array);
				SendEvent_GetFreeResourcesComplete(FreeResourcesType.JoinGroup);
			}
		}

		public void LikeFanpage()
		{
			if (!ReadWriteDataFreeResources.Instance.IsUserGetReward_LikeFanpage())
			{
				int rewardAmount_Gem = ReadDataFacebookServicesReward.GetRewardAmount_Gem("reward_id_like_fanpage");
				ReadWriteDataPlayerCurrency.Instance.ChangeGem(rewardAmount_Gem, isDispatchEventChange: true);
				UnityEngine.Debug.Log("Test FB Like Fanpage Success + reward:" + rewardAmount_Gem);
				ReadWriteDataFreeResources.Instance.SetOneTimeRewardStatus("one_time_like_fanpage");
				RewardItem[] array = new RewardItem[1];
				RewardItem rewardItem = new RewardItem();
				rewardItem.rewardType = RewardType.Gem;
				rewardItem.value = rewardAmount_Gem;
				rewardItem.isDisplayQuantity = true;
				array[0] = rewardItem;
				SingletonMonoBehaviour<LifetimeCanvas>.Instance.RewardPopupController.Init(array);
				SendEvent_GetFreeResourcesComplete(FreeResourcesType.LikeFanpage);
			}
		}

		public void LogIn()
		{
			if (!ReadWriteDataFreeResources.Instance.IsUserGetReward_LogInFacebook())
			{
				int rewardAmount_Gem = ReadDataFacebookServicesReward.GetRewardAmount_Gem("reward_id_login");
				ReadWriteDataPlayerCurrency.Instance.ChangeGem(rewardAmount_Gem, isDispatchEventChange: true);
				UnityEngine.Debug.Log("Test FB Login Success + reward:" + rewardAmount_Gem);
				ReadWriteDataFreeResources.Instance.SetOneTimeRewardStatus("one_time_login");
				RewardItem[] array = new RewardItem[1];
				RewardItem rewardItem = new RewardItem();
				rewardItem.rewardType = RewardType.Gem;
				rewardItem.value = rewardAmount_Gem;
				rewardItem.isDisplayQuantity = true;
				array[0] = rewardItem;
				SingletonMonoBehaviour<LifetimeCanvas>.Instance.RewardPopupController.Init(array);
				SendEvent_GetFreeResourcesComplete(FreeResourcesType.Login);
			}
		}

		public void LogOut()
		{
			UnityEngine.Debug.Log("Test FB Logout Success ");
		}

		public void ShareFanpage()
		{
			int rewardAmount_Gem = ReadDataFacebookServicesReward.GetRewardAmount_Gem("reward_id_share_fanpage");
			ReadWriteDataPlayerCurrency.Instance.ChangeGem(rewardAmount_Gem, isDispatchEventChange: true);
			UnityEngine.Debug.Log("Test FB Share Fanpage Success + reward: " + rewardAmount_Gem);
			RewardItem[] array = new RewardItem[1];
			RewardItem rewardItem = new RewardItem();
			rewardItem.rewardType = RewardType.Gem;
			rewardItem.value = rewardAmount_Gem;
			rewardItem.isDisplayQuantity = true;
			array[0] = rewardItem;
			SingletonMonoBehaviour<LifetimeCanvas>.Instance.RewardPopupController.Init(array);
			SendEvent_GetFreeResourcesComplete(FreeResourcesType.ShareFanpage);
		}

		public void ShareLinkGame(SceneName sceneName, int currentMapID)
		{
			int rewardAmount_Gem = ReadDataFacebookServicesReward.GetRewardAmount_Gem("reward_id_share_link_game");
			ReadWriteDataPlayerCurrency.Instance.ChangeGem(rewardAmount_Gem, isDispatchEventChange: true);
			UnityEngine.Debug.Log("Test FB Share Link Game Success + reward: " + rewardAmount_Gem);
			RewardItem[] array = new RewardItem[1];
			RewardItem rewardItem = new RewardItem();
			rewardItem.rewardType = RewardType.Gem;
			rewardItem.value = rewardAmount_Gem;
			rewardItem.isDisplayQuantity = true;
			array[0] = rewardItem;
			SingletonMonoBehaviour<LifetimeCanvas>.Instance.RewardPopupController.Init(array);
			PlatformSpecificServicesProvider.Services.Analytics.SendEvent_ShareLinkGameComplete(sceneName, currentMapID);
		}

		public void SharePromotionImage(int imageID)
		{
			int rewardAmount_Gem = ReadDataFacebookServicesReward.GetRewardAmount_Gem("reward_id_share_promotion_image");
			ReadWriteDataPlayerCurrency.Instance.ChangeGem(rewardAmount_Gem, isDispatchEventChange: true);
			UnityEngine.Debug.Log("Test FB Share Promotion Image + reward: " + rewardAmount_Gem);
			RewardItem[] array = new RewardItem[1];
			RewardItem rewardItem = new RewardItem();
			rewardItem.rewardType = RewardType.Gem;
			rewardItem.value = rewardAmount_Gem;
			rewardItem.isDisplayQuantity = true;
			array[0] = rewardItem;
			SingletonMonoBehaviour<LifetimeCanvas>.Instance.RewardPopupController.Init(array);
		}

		public void ShareScreenShot()
		{
			int rewardAmount_Gem = ReadDataFacebookServicesReward.GetRewardAmount_Gem("reward_id_share_screenshot");
			ReadWriteDataPlayerCurrency.Instance.ChangeGem(rewardAmount_Gem, isDispatchEventChange: true);
			UnityEngine.Debug.Log("Test FB Shace Screenshot Success + reward: " + rewardAmount_Gem);
			RewardItem[] array = new RewardItem[1];
			RewardItem rewardItem = new RewardItem();
			rewardItem.rewardType = RewardType.Gem;
			rewardItem.value = rewardAmount_Gem;
			rewardItem.isDisplayQuantity = true;
			array[0] = rewardItem;
			SingletonMonoBehaviour<LifetimeCanvas>.Instance.RewardPopupController.Init(array);
		}

		public void SendEvent_GetFreeResourcesComplete(FreeResourcesType freeResourcesType)
		{
			int currentMapIDUnlocked = ReadWriteDataMap.Instance.GetMapIDUnlocked() + 1;
			PlatformSpecificServicesProvider.Services.Analytics.SendEvent_GetFreeResourcesComplete(freeResourcesType, currentMapIDUnlocked);
		}
	}
}

using ApplicationEntry;
using Data;
using Facebook.Unity;
using LifetimePopup;
using MyCustom;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Services.PlatformSpecific.Android
{
	public class FacebookServicesAndroid : MonoBehaviour, IFacebookServices
	{
		private List<string> listPermissionLogin = new List<string>
		{
			"public_profile",
			"email",
			"user_friends"
		};

		[SerializeField]
		private ReadDataFacebookServicesReward readDataFacebookServicesReward;

		private SceneName sceneName;

		private int currentMapID;

		private int currentImageID;

		public int GetFreeResources(string rewardID)
		{
			return readDataFacebookServicesReward.GetRewardAmount_Gem(rewardID);
		}

		public void LogIn()
		{
			if (!FB.IsLoggedIn)
			{
				FB.LogInWithReadPermissions(listPermissionLogin, LogIn0CallBack);
			}
			else
			{
				GetRewardLogin();
			}
		}

		private void LogIn0CallBack(ILoginResult result)
		{
			UnityEngine.Debug.Log(" Result callback login with read permission =  " + result.ToString());
			if (FB.IsLoggedIn)
			{
				GetRewardLogin();
			}
			else
			{
				UnityEngine.Debug.Log("User cancelled read permission login");
			}
		}

		private void GetRewardLogin()
		{
			if (!ReadWriteDataFreeResources.Instance.IsUserGetReward_LogInFacebook())
			{
				int rewardAmount_Gem = readDataFacebookServicesReward.GetRewardAmount_Gem("reward_id_login");
				ReadWriteDataPlayerCurrency.Instance.ChangeGem(rewardAmount_Gem, isDispatchEventChange: true);
				UnityEngine.Debug.Log("FB Login Success + reward:" + rewardAmount_Gem);
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

		public void InviteFriend()
		{
			FB.AppRequest("Let's play Kingdom Defense!", null, null, null, null, null, string.Empty, InviteFriendCallback);
		}

		private void InviteFriendCallback(IAppRequestResult result)
		{
			if (!string.IsNullOrEmpty(result.Error))
			{
				UnityEngine.Debug.Log("Invite Friend Error: " + result.Error);
			}
			else if (result.Cancelled)
			{
				UnityEngine.Debug.Log("User cancelled Invite!");
			}
			else
			{
				GetRewardInviteFriend(result);
			}
		}

		private void GetRewardInviteFriend(IAppRequestResult result)
		{
			int num = 0;
			if (result.ResultDictionary.TryGetValue("to", out object value))
			{
				UnityEngine.Debug.Log(value);
				char[] separator = new char[1]
				{
					','
				};
				string text = (string)value;
				string[] array = text.Split(separator);
				UnityEngine.Debug.Log(array.Length);
				if (array.Length > 0)
				{
					num = array.Length;
				}
			}
			UnityEngine.Debug.Log("requested count = " + num);
			GameEventCenter.Instance.Trigger(GameEventType.EventInviteFriend, new EventTriggerData(EventTriggerType.InviteFriend, num, forceSaveProgress: true));
			if (ApplicationEntry.ApplicationEntry.Instance.ReadWriteRemoteSettingData.isDisplayFreeGem())
			{
				int currentGemCollectedByInvite = ReadWriteDataFreeResources.Instance.GetCurrentGemCollectedByInvite();
				int num2 = 100;
				if (currentGemCollectedByInvite < num2)
				{
					int num3 = num * readDataFacebookServicesReward.GetRewardAmount_Gem("reward_id_invite_friend");
					ReadWriteDataPlayerCurrency.Instance.ChangeGem(num3, isDispatchEventChange: true);
					currentGemCollectedByInvite += num3;
					ReadWriteDataFreeResources.Instance.SetCurrentGemCollectedByInvite(currentGemCollectedByInvite);
					UnityEngine.Debug.Log("Invite Friend Success + reward: " + num3);
					RewardItem[] array2 = new RewardItem[1];
					RewardItem rewardItem = new RewardItem();
					rewardItem.rewardType = RewardType.Gem;
					rewardItem.value = num3;
					rewardItem.isDisplayQuantity = true;
					array2[0] = rewardItem;
					SingletonMonoBehaviour<LifetimeCanvas>.Instance.RewardPopupController.Init(array2);
					SendEvent_GetFreeResourcesComplete(FreeResourcesType.InviteFriend);
				}
			}
		}

		public void InviteToGroup()
		{
			if (StaticMethod.IsInternetConnectionAvailable())
			{
				if (StaticMethod.CheckPackageAppIsPresent("com.facebook.katana"))
				{
					Application.OpenURL(MarketingConfig.fbGroupLinkApp);
				}
				else
				{
					Application.OpenURL(MarketingConfig.fbGroupLinkWeb);
				}
				GetRewardJoinGroup();
			}
			else
			{
				UnityEngine.Debug.Log("No Internet Connection!");
			}
		}

		private void GetRewardJoinGroup()
		{
			if (ApplicationEntry.ApplicationEntry.Instance.ReadWriteRemoteSettingData.isDisplayFreeGem())
			{
				if (!ReadWriteDataFreeResources.Instance.IsUserGetReward_JoinGroup())
				{
					int rewardAmount_Gem = readDataFacebookServicesReward.GetRewardAmount_Gem("reward_id_join_group");
					ReadWriteDataPlayerCurrency.Instance.ChangeGem(rewardAmount_Gem, isDispatchEventChange: true);
					UnityEngine.Debug.Log("FB Invite Group Success + reward:" + rewardAmount_Gem);
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
				else
				{
					UnityEngine.Debug.Log("Đã nhận thưởng join group!");
				}
			}
		}

		public void LikeFanpage()
		{
			if (StaticMethod.IsInternetConnectionAvailable())
			{
				if (StaticMethod.CheckPackageAppIsPresent("com.facebook.katana"))
				{
					Application.OpenURL(MarketingConfig.fbFanpageLinkApp);
				}
				else
				{
					Application.OpenURL(MarketingConfig.fbFanpageLinkWeb);
				}
				GetRewardLikeFanpage();
			}
			else
			{
				UnityEngine.Debug.Log("No Internet Connection!");
			}
		}

		private void GetRewardLikeFanpage()
		{
			if (ApplicationEntry.ApplicationEntry.Instance.ReadWriteRemoteSettingData.isDisplayFreeGem())
			{
				if (!ReadWriteDataFreeResources.Instance.IsUserGetReward_LikeFanpage())
				{
					int rewardAmount_Gem = readDataFacebookServicesReward.GetRewardAmount_Gem("reward_id_like_fanpage");
					ReadWriteDataPlayerCurrency.Instance.ChangeGem(rewardAmount_Gem, isDispatchEventChange: true);
					UnityEngine.Debug.Log("FB Like Fanpage Success + reward:" + rewardAmount_Gem);
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
				else
				{
					UnityEngine.Debug.Log("Đã nhận thưởng like fanpage!");
				}
			}
		}

		public void ShareFanpage()
		{
			FB.ShareLink(new Uri(MarketingConfig.fbFanpageLinkWeb), string.Empty, string.Empty, null, ShareFanpageCallBack);
		}

		private void ShareFanpageCallBack(IShareResult result)
		{
			if (!string.IsNullOrEmpty(result.Error))
			{
				UnityEngine.Debug.Log("Share fanpage Error: " + result.Error);
			}
			else if (result.Cancelled)
			{
				UnityEngine.Debug.Log("User cancelled Share fanpage!");
			}
			else
			{
				GetRewardShareFanpage();
			}
		}

		private void GetRewardShareFanpage()
		{
			if (ApplicationEntry.ApplicationEntry.Instance.ReadWriteRemoteSettingData.isDisplayFreeGem())
			{
				int rewardAmount_Gem = readDataFacebookServicesReward.GetRewardAmount_Gem("reward_id_share_fanpage");
				ReadWriteDataPlayerCurrency.Instance.ChangeGem(rewardAmount_Gem, isDispatchEventChange: true);
				UnityEngine.Debug.Log("ShareLink Fanpage Success + reward: " + rewardAmount_Gem);
				RewardItem[] array = new RewardItem[1];
				RewardItem rewardItem = new RewardItem();
				rewardItem.rewardType = RewardType.Gem;
				rewardItem.value = rewardAmount_Gem;
				rewardItem.isDisplayQuantity = true;
				array[0] = rewardItem;
				SingletonMonoBehaviour<LifetimeCanvas>.Instance.RewardPopupController.Init(array);
				SendEvent_GetFreeResourcesComplete(FreeResourcesType.ShareFanpage);
			}
		}

		public void ShareLinkGame(SceneName sceneName, int currentMapID)
		{
			this.sceneName = sceneName;
			this.currentMapID = currentMapID;
			FB.ShareLink(new Uri(MarketingConfig.rateGameLink), string.Empty, string.Empty, null, ShareLinkGameCallback);
		}

		private void ShareLinkGameCallback(IShareResult result)
		{
			if (!string.IsNullOrEmpty(result.Error))
			{
				UnityEngine.Debug.Log("Share link game Error: " + result.Error);
			}
			else if (result.Cancelled)
			{
				UnityEngine.Debug.Log("User cancelled Share link game!");
			}
			else
			{
				GetRewardShareLinkGame();
			}
		}

		private void GetRewardShareLinkGame()
		{
			if (ApplicationEntry.ApplicationEntry.Instance.ReadWriteRemoteSettingData.isDisplayFreeGem())
			{
				int rewardAmount_Gem = readDataFacebookServicesReward.GetRewardAmount_Gem("reward_id_share_link_game");
				ReadWriteDataPlayerCurrency.Instance.ChangeGem(rewardAmount_Gem, isDispatchEventChange: true);
				UnityEngine.Debug.Log("ShareLink Game Success + reward: " + rewardAmount_Gem);
				RewardItem[] array = new RewardItem[1];
				RewardItem rewardItem = new RewardItem();
				rewardItem.rewardType = RewardType.Gem;
				rewardItem.value = rewardAmount_Gem;
				rewardItem.isDisplayQuantity = true;
				array[0] = rewardItem;
				SingletonMonoBehaviour<LifetimeCanvas>.Instance.RewardPopupController.Init(array);
				PlatformSpecificServicesProvider.Services.Analytics.SendEvent_ShareLinkGameComplete(sceneName, currentMapID);
			}
		}

		public void SharePromotionImage(int imageID)
		{
			currentImageID = imageID;
			if (!FB.IsLoggedIn)
			{
				LogIn_Addition_Promotion();
			}
			else
			{
				TryToSharePromotion();
			}
		}

		private void TryToSharePromotion()
		{
			if (AccessToken.CurrentAccessToken.Permissions.Contains("publish_actions"))
			{
				StartCoroutine(ISharePromotionImage(currentImageID));
				UnityEngine.Debug.Log("Share promotion image with publish actions");
			}
			else
			{
				ShareFanpage();
				UnityEngine.Debug.Log("no publish actions, share fanpage link");
			}
		}

		private IEnumerator ISharePromotionImage(int imageID)
		{
			yield return new WaitForEndOfFrame();
			Texture2D image = Resources.Load($"Publish/promotion_{imageID}", typeof(Texture2D)) as Texture2D;
			byte[] promotionImg = image.EncodeToPNG();
			WWWForm wwwForm = new WWWForm();
			wwwForm.AddBinaryData("image", promotionImg, "Promotion.png");
			FB.API("me/photos", HttpMethod.POST, SharePromotionImageCallback, wwwForm);
		}

		private void SharePromotionImageCallback(IGraphResult result)
		{
			if (!string.IsNullOrEmpty(result.Error))
			{
				UnityEngine.Debug.Log("Share Promotion Error: " + result.Error);
			}
			else if (result.Cancelled)
			{
				UnityEngine.Debug.Log("User cancelled share Promotion!");
			}
			else
			{
				GetRewardSharePromotion();
			}
		}

		private void GetRewardSharePromotion()
		{
			int rewardAmount_Gem = readDataFacebookServicesReward.GetRewardAmount_Gem("reward_id_share_promotion_image");
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

		public void LogIn_Addition_Promotion()
		{
			if (!FB.IsLoggedIn)
			{
				FB.LogInWithReadPermissions(listPermissionLogin, LogIn0CallBack_Addition_Promotion);
			}
		}

		private void LogIn0CallBack_Addition_Promotion(ILoginResult result)
		{
			UnityEngine.Debug.Log(" Result callback login with read permission =  " + result.ToString());
			if (FB.IsLoggedIn)
			{
				TryToSharePromotion();
			}
			else
			{
				UnityEngine.Debug.Log("User cancelled read permission login");
			}
		}

		public void ShareScreenShot()
		{
			if (!FB.IsLoggedIn)
			{
				LogIn_Addition_Screenshot();
			}
			else
			{
				TryToShareScreenshot();
			}
		}

		private void TryToShareScreenshot()
		{
			if (AccessToken.CurrentAccessToken.Permissions.Contains("publish_actions"))
			{
				StartCoroutine(TakeScreenshot());
				UnityEngine.Debug.Log("Share screenshot with publish actions");
			}
			else
			{
				ShareFanpage();
				UnityEngine.Debug.Log("no publish actions, share fanpage link");
			}
		}

		private IEnumerator TakeScreenshot()
		{
			yield return new WaitForEndOfFrame();
			int width = Screen.width;
			int height = Screen.height;
			Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, mipChain: false);
			tex.ReadPixels(new Rect(0f, 0f, width, height), 0, 0);
			tex.Apply();
			byte[] screenshot = tex.EncodeToPNG();
			WWWForm wwwForm = new WWWForm();
			wwwForm.AddBinaryData("image", screenshot, "Screenshot.png");
			FB.API("me/photos", HttpMethod.POST, ShareScreenshotCallback, wwwForm);
		}

		private void ShareScreenshotCallback(IGraphResult result)
		{
			if (!string.IsNullOrEmpty(result.Error))
			{
				UnityEngine.Debug.Log("Share Screenshot Error: " + result.Error);
			}
			else if (result.Cancelled)
			{
				UnityEngine.Debug.Log("User cancelled Share Screenshot!");
			}
			else
			{
				GetRewardShareScreenshot();
			}
		}

		private void GetRewardShareScreenshot()
		{
			int rewardAmount_Gem = readDataFacebookServicesReward.GetRewardAmount_Gem("reward_id_share_screenshot");
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

		public void LogIn_Addition_Screenshot()
		{
			if (!FB.IsLoggedIn)
			{
				FB.LogInWithReadPermissions(listPermissionLogin, LogIn0CallBack_Addition_Screenshot);
			}
		}

		private void LogIn0CallBack_Addition_Screenshot(ILoginResult result)
		{
			UnityEngine.Debug.Log(" Result callback login with read permission =  " + result.ToString());
			if (FB.IsLoggedIn)
			{
				TryToShareScreenshot();
			}
			else
			{
				UnityEngine.Debug.Log("User cancelled read permission login");
			}
		}

		public void SendEvent_GetFreeResourcesComplete(FreeResourcesType freeResourcesType)
		{
			int currentMapIDUnlocked = ReadWriteDataMap.Instance.GetMapIDUnlocked() + 1;
			PlatformSpecificServicesProvider.Services.Analytics.SendEvent_GetFreeResourcesComplete(freeResourcesType, currentMapIDUnlocked);
		}
	}
}

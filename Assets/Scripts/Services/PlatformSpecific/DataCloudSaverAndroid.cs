using Data;
using Firebase;
using Firebase.Database;
//using Firebase.Unity.Editor;
using LifetimePopup;
using MyCustom;
using Newtonsoft.Json;
using Parameter;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Services.PlatformSpecific
{
	public class DataCloudSaverAndroid : MonoBehaviour, IDataCloudSaver
	{
		private DatabaseReference reference;

		private string userID = string.Empty;

		private DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;

		private DataRestoreDeliver dataRestoreDeliver;

		private bool isDataCollected;

		private bool dataCollectFailedFlag;

		public event Action OnDataBackupCompletedEvent;

		public event Action OnDataRestoreCompletedEvent;

		private void Update()
		{
			if (isDataCollected)
			{
				dataRestoreDeliver.DispatchToAllDataWriter(dataRestoreDeliver);
				StartCoroutine(BackToMainMenu());
				SendEvent_DataRestoreComplete();
				isDataCollected = false;
			}
			if (dataCollectFailedFlag)
			{
				string notiContent = Singleton<NotificationDescription>.Instance.GetNotiContent(149);
				SingletonMonoBehaviour<LifetimeCanvas>.Instance.NotifyPopupController.Init(notiContent, isShowButtonFreeResources: false, isShowButtonGoToStore: false);
				dataCollectFailedFlag = false;
			}
		}

		private void SendEvent_DataBackupComplete()
		{
			if (this.OnDataBackupCompletedEvent != null)
			{
				this.OnDataBackupCompletedEvent();
			}
		}

		private void SendEvent_DataRestoreComplete()
		{
			if (this.OnDataRestoreCompletedEvent != null)
			{
				this.OnDataRestoreCompletedEvent();
			}
		}

		public void FirebaseInit()
		{
			//FirebaseApp.DefaultInstance.SetEditorDatabaseUrl(DataCloudSaverStaticVariable.FIREBASE_URL);
			reference = FirebaseDatabase.DefaultInstance.RootReference;
		}

		public void AutoBackUpData()
		{
			if (!StaticMethod.IsInternetConnectionAvailable())
			{
				return;
			}
			if (PlatformSpecificServicesProvider.Services.UserProfile.IsLoggedIn_Facebook() || PlatformSpecificServicesProvider.Services.UserProfile.IsLoggedIn_Google())
			{
				userID = PlatformSpecificServicesProvider.Services.UserProfile.GetFireseBaseUserID();
				if (string.IsNullOrEmpty(userID))
				{
					UnityEngine.Debug.Log("Auto Backup - User ID cannot be empty!");
					return;
				}
				int mapIDUnlocked = ReadWriteDataMap.Instance.GetMapIDUnlocked();
				if (mapIDUnlocked < 4)
				{
					UnityEngine.Debug.Log("Can mo khoa map id4 de su dung auto backup!");
					return;
				}
				BackupData_Hero();
				BackupData_GlobalUpgrade();
				BackupData_Map();
				BackupData_Theme();
				BackupData_UserProfile();
				BackupData_PowerUpItem();
				BackupData_Tutorial();
				BackupData_DailyTrial();
				BackupData_Offers();
				BackupData_FreeResources();
				BackupData_SaleBundle();
				BackupData_DailyReward();
				SendEvent_DataBackupComplete();
				UnityEngine.Debug.Log("Auto Backup - Done!");
			}
			else
			{
				UnityEngine.Debug.Log("Auto Backup - Failed - Require Login Auth!");
			}
		}

		public void BackupData()
		{
			userID = PlatformSpecificServicesProvider.Services.UserProfile.GetFireseBaseUserID();
			if (string.IsNullOrEmpty(userID))
			{
				UnityEngine.Debug.Log("Backup - User ID cannot be empty!");
				return;
			}
			SingletonMonoBehaviour<LifetimeCanvas>.Instance.LoadingProgressPopupController.Open();
			BackupData_Hero();
			BackupData_GlobalUpgrade();
			BackupData_Map();
			BackupData_Theme();
			BackupData_UserProfile();
			BackupData_PowerUpItem();
			BackupData_Tutorial();
			BackupData_DailyTrial();
			BackupData_Offers();
			BackupData_FreeResources();
			BackupData_SaleBundle();
			BackupData_DailyReward();
			SendEvent_DataBackupComplete();
			SingletonMonoBehaviour<LifetimeCanvas>.Instance.LoadingProgressPopupController.Close();
			string notiContent = Singleton<NotificationDescription>.Instance.GetNotiContent(132);
			SingletonMonoBehaviour<LifetimeCanvas>.Instance.NotifyPopupController.Init(notiContent, isShowButtonFreeResources: false, isShowButtonGoToStore: false);
		}

		private void BackupData_Hero()
		{
			UserData_Hero userData_Hero = new UserData_Hero();
			userData_Hero.listHeroData = new List<UserData_Hero_Unique>();
			List<int> listHeroIDOwned = ReadWriteDataHero.Instance.GetListHeroIDOwned();
			for (int i = 0; i < listHeroIDOwned.Count; i++)
			{
				UserData_Hero_Unique userData_Hero_Unique = new UserData_Hero_Unique();
				userData_Hero_Unique.id = listHeroIDOwned[i];
				userData_Hero_Unique.level = ReadWriteDataHero.Instance.GetCurrentHeroLevel(listHeroIDOwned[i]);
				userData_Hero_Unique.exp = ReadWriteDataHero.Instance.GetCurrentHeroTotalExp(listHeroIDOwned[i]);
				userData_Hero_Unique.isOwned = true;
				userData_Hero_Unique.ownedPet = ReadWriteDataHero.Instance.IsPetUnlocked(listHeroIDOwned[i]);
				userData_Hero_Unique.skillUpgraded = new List<int>();
				for (int j = 0; j < 4; j++)
				{
					userData_Hero_Unique.skillUpgraded.Add(ReadWriteDataHero.Instance.GetSkillPoint(listHeroIDOwned[i], j));
				}
				userData_Hero.listHeroData.Add(userData_Hero_Unique);
			}
			string rawJsonValueAsync = JsonConvert.SerializeObject(userData_Hero);
			reference.Child(userID).Child(DataCloudSaverStaticVariable.FIREBASE_NODE_USERDATA_HERO).SetRawJsonValueAsync(rawJsonValueAsync);
		}

		private void BackupData_GlobalUpgrade()
		{
			UserData_GlobalUpgrade userData_GlobalUpgrade = new UserData_GlobalUpgrade();
			userData_GlobalUpgrade.listUpgradedTower = new List<UserData_GlobalUpgrade_Unique>();
			for (int i = 0; i < 4; i++)
			{
				UserData_GlobalUpgrade_Unique userData_GlobalUpgrade_Unique = new UserData_GlobalUpgrade_Unique();
				userData_GlobalUpgrade_Unique.towerID = i;
				userData_GlobalUpgrade_Unique.towerUpgradedLevel = ReadWriteDataGlobalUpgrade.Instance.GetCurrentUpgradeLevel(i);
				userData_GlobalUpgrade.listUpgradedTower.Add(userData_GlobalUpgrade_Unique);
			}
			string rawJsonValueAsync = JsonConvert.SerializeObject(userData_GlobalUpgrade);
			reference.Child(userID).Child(DataCloudSaverStaticVariable.FIREBASE_NODE_USERDATA_GLOBALUPGRADE).SetRawJsonValueAsync(rawJsonValueAsync);
		}

		private void BackupData_Map()
		{
			UserData_Map userData_Map = new UserData_Map();
			userData_Map.mapIDUnlocked = ReadWriteDataMap.Instance.GetMapIDUnlocked();
			userData_Map.lastMapIDPlayed = ReadWriteDataMap.Instance.GetLastMapIDPlayed();
			userData_Map.lastMapModeChoose = ReadWriteDataMap.Instance.GetLastMapModeChoose();
			userData_Map.listDataMap = new List<UserData_Map_Unique>();
			for (int i = 0; i <= userData_Map.mapIDUnlocked; i++)
			{
				UserData_Map_Unique userData_Map_Unique = new UserData_Map_Unique();
				userData_Map_Unique.mapID = i;
				userData_Map_Unique.starEarned = ReadWriteDataMap.Instance.GetStarEarnedByMap(i);
				userData_Map_Unique.playCount = ReadWriteDataMap.Instance.GetCurrentPlayCount(i);
				userData_Map_Unique.playCount_victory = ReadWriteDataMap.Instance.GetCurrentPlayCount_Victory(i);
				userData_Map_Unique.playCount_defeat = ReadWriteDataMap.Instance.GetCurrentPlayCount_Defeat(i);
				userData_Map.listDataMap.Add(userData_Map_Unique);
			}
			string rawJsonValueAsync = JsonConvert.SerializeObject(userData_Map);
			reference.Child(userID).Child(DataCloudSaverStaticVariable.FIREBASE_NODE_USERDATA_MAP).SetRawJsonValueAsync(rawJsonValueAsync);
		}

		private void BackupData_Theme()
		{
			UserData_Theme userData_Theme = new UserData_Theme();
			userData_Theme.lastThemeIDPlayed = ReadWriteDataTheme.Instance.GetLastThemeIDPlayed();
			userData_Theme.listThemeIDUnlocked = new List<int>();
			int themeIDUnlocked = ReadWriteDataTheme.Instance.GetThemeIDUnlocked();
			for (int i = 0; i <= themeIDUnlocked; i++)
			{
				userData_Theme.listThemeIDUnlocked.Add(i);
			}
			string rawJsonValueAsync = JsonConvert.SerializeObject(userData_Theme);
			reference.Child(userID).Child(DataCloudSaverStaticVariable.FIREBASE_NODE_USERDATA_THEME).SetRawJsonValueAsync(rawJsonValueAsync);
		}

		private void BackupData_UserProfile()
		{
			UserData_UserProfile userData_UserProfile = new UserData_UserProfile();
			userData_UserProfile.userID = ReadWriteDataUserProfile.Instance.GetUserID();
			userData_UserProfile.userName = ReadWriteDataUserProfile.Instance.GetUserName();
			userData_UserProfile.renameCount = ReadWriteDataUserProfile.Instance.GetRenameCount();
			userData_UserProfile.renameItemQuantity = ReadWriteDataUserProfile.Instance.GetRenameItemQuantity();
			userData_UserProfile.rank = "empty";
			userData_UserProfile.league = ReadWriteDataUserProfile.Instance.GetLeagueValue();
			userData_UserProfile.exp = 0;
			userData_UserProfile.countryCode = ReadWriteDataUserProfile.Instance.GetUserRegionCode();
			userData_UserProfile.totalGem = ReadWriteDataPlayerCurrency.Instance.GetCurrentGem();
			userData_UserProfile.lastTimeBackup = ReadWriteDataUserProfile.Instance.GetLastTimeBackup();
			string rawJsonValueAsync = JsonConvert.SerializeObject(userData_UserProfile);
			reference.Child(userID).Child(DataCloudSaverStaticVariable.FIREBASE_NODE_USERDATA_USERPROFILE).SetRawJsonValueAsync(rawJsonValueAsync);
		}

		private void BackupData_PowerUpItem()
		{
			UserData_PowerupItem userData_PowerupItem = new UserData_PowerupItem();
			userData_PowerupItem.listDataPowerupItems = new List<UserData_PowerupItem_Unique>();
			for (int i = 0; i < 9; i++)
			{
				UserData_PowerupItem_Unique userData_PowerupItem_Unique = new UserData_PowerupItem_Unique();
				userData_PowerupItem_Unique.itemID = i;
				userData_PowerupItem_Unique.quantity = ReadWriteDataPowerUpItem.Instance.GetCurrentItemQuantity(i);
				userData_PowerupItem.listDataPowerupItems.Add(userData_PowerupItem_Unique);
			}
			string rawJsonValueAsync = JsonConvert.SerializeObject(userData_PowerupItem);
			reference.Child(userID).Child(DataCloudSaverStaticVariable.FIREBASE_NODE_USERDATA_POWERUPITEM).SetRawJsonValueAsync(rawJsonValueAsync);
		}

		private void BackupData_Tutorial()
		{
			UserData_Tutorial userData_Tutorial = new UserData_Tutorial();
			userData_Tutorial.ListTutorialData = new Dictionary<string, bool>();
			string[] allTutorialKeys = ReadWriteDataTutorial.Instance.AllTutorialKeys;
			string[] array = allTutorialKeys;
			foreach (string text in array)
			{
				bool tutorialStatus = ReadWriteDataTutorial.Instance.GetTutorialStatus(text);
				if (!userData_Tutorial.ListTutorialData.ContainsKey(text))
				{
					userData_Tutorial.ListTutorialData.Add(text, tutorialStatus);
				}
			}
			string rawJsonValueAsync = JsonConvert.SerializeObject(userData_Tutorial);
			reference.Child(userID).Child(DataCloudSaverStaticVariable.FIREBASE_NODE_USERDATA_TUTORIAL).SetRawJsonValueAsync(rawJsonValueAsync);
		}

		private void BackupData_Offers()
		{
			UserData_Offer userData_Offer = new UserData_Offer();
			userData_Offer.ListOfferData = new Dictionary<string, bool>();
			string[] allOfferKeys = ReadWriteDataOffers.Instance.AllOfferKeys;
			string[] array = allOfferKeys;
			foreach (string text in array)
			{
				bool value = ReadWriteDataOffers.Instance.IsOfferProcessed(text);
				if (!userData_Offer.ListOfferData.ContainsKey(text))
				{
					userData_Offer.ListOfferData.Add(text, value);
				}
			}
			string rawJsonValueAsync = JsonConvert.SerializeObject(userData_Offer);
			reference.Child(userID).Child(DataCloudSaverStaticVariable.FIREBASE_NODE_USERDATA_OFFER).SetRawJsonValueAsync(rawJsonValueAsync);
		}

		private void BackupData_FreeResources()
		{
			UserData_FreeResources userData_FreeResources = new UserData_FreeResources();
			userData_FreeResources.isUserGetRewardLoggedInFacebook = ReadWriteDataFreeResources.Instance.IsUserGetReward_LogInFacebook();
			userData_FreeResources.isUserGetRewardLikeFanpage = ReadWriteDataFreeResources.Instance.IsUserGetReward_LikeFanpage();
			userData_FreeResources.isUserGetRewardJoinGroup = ReadWriteDataFreeResources.Instance.IsUserGetReward_JoinGroup();
			userData_FreeResources.currentSharePerDay = ReadWriteDataFreeResources.Instance.GetCurrentSharePerDay();
			userData_FreeResources.currentWatchAdsPerDay = ReadWriteDataFreeResources.Instance.GetCurrentWatchAdsPerDay();
			userData_FreeResources.currentGemCollectedByInvite = ReadWriteDataFreeResources.Instance.GetCurrentGemCollectedByInvite();
			string rawJsonValueAsync = JsonConvert.SerializeObject(userData_FreeResources);
			reference.Child(userID).Child(DataCloudSaverStaticVariable.FIREBASE_NODE_USERDATA_FREERESOURCES).SetRawJsonValueAsync(rawJsonValueAsync);
		}

		private void BackupData_DailyTrial()
		{
			UserData_DailyTrial userData_DailyTrial = new UserData_DailyTrial();
			userData_DailyTrial.currentDay = ReadWriteDataDailyTrial.Instance.GetCurrentDayIndex();
			userData_DailyTrial.listDataDailyTrial = new List<UserData_DailyTrial_Unique>();
			for (int i = 0; i <= 6; i++)
			{
				UserData_DailyTrial_Unique userData_DailyTrial_Unique = new UserData_DailyTrial_Unique();
				userData_DailyTrial_Unique.day = i;
				userData_DailyTrial_Unique.missionDoneTier = ReadWriteDataDailyTrial.Instance.GetDoneMissionTier(i);
				userData_DailyTrial_Unique.playCount = ReadWriteDataDailyTrial.Instance.GetPlayCount(i);
				userData_DailyTrial.listDataDailyTrial.Add(userData_DailyTrial_Unique);
			}
			string rawJsonValueAsync = JsonConvert.SerializeObject(userData_DailyTrial);
			reference.Child(userID).Child(DataCloudSaverStaticVariable.FIREBASE_NODE_USERDATA_DAILYTRIAL).SetRawJsonValueAsync(rawJsonValueAsync);
		}

		private void BackupData_SaleBundle()
		{
			UserData_SaleBundle userData_SaleBundle = new UserData_SaleBundle();
			userData_SaleBundle.ListSaleBundleData = new List<UserData_SaleBundle_Unique>();
			string[] productIDSpecialPack = MarketingConfig.productIDSpecialPack;
			for (int i = 0; i < productIDSpecialPack.Length; i++)
			{
				UserData_SaleBundle_Unique userData_SaleBundle_Unique = new UserData_SaleBundle_Unique();
				userData_SaleBundle_Unique.bundleID = productIDSpecialPack[i];
				userData_SaleBundle_Unique.isBought = ReadWriteDataSaleBundle.Instance.GetSpecialPackBuyStatus(userData_SaleBundle_Unique.bundleID);
				userData_SaleBundle_Unique.isExpired = ReadWriteDataSaleBundle.Instance.GetSpecialPackExpireStatus(userData_SaleBundle_Unique.bundleID);
				userData_SaleBundle.ListSaleBundleData.Add(userData_SaleBundle_Unique);
			}
			string rawJsonValueAsync = JsonConvert.SerializeObject(userData_SaleBundle);
			reference.Child(userID).Child(DataCloudSaverStaticVariable.FIREBASE_NODE_USERDATA_SALEBUNDLE).SetRawJsonValueAsync(rawJsonValueAsync);
		}

		private void BackupData_DailyReward()
		{
			UserData_DailyReward userData_DailyReward = new UserData_DailyReward();
			userData_DailyReward.listDailyRewardData = new List<UserData_DailyReward_Unique>();
			userData_DailyReward.currentDay = ReadWriteDataDailyReward.Instance.GetCurrentDay();
			for (int i = 0; i <= 13; i++)
			{
				UserData_DailyReward_Unique userData_DailyReward_Unique = new UserData_DailyReward_Unique();
				userData_DailyReward_Unique.day = i;
				userData_DailyReward_Unique.isReceivedReward = ReadWriteDataDailyReward.Instance.IsReceivedReward(i);
				userData_DailyReward_Unique.isReceivedBonus = ReadWriteDataDailyReward.Instance.IsReceivedBonus(i);
				userData_DailyReward.listDailyRewardData.Insert(i, userData_DailyReward_Unique);
			}
			string rawJsonValueAsync = JsonConvert.SerializeObject(userData_DailyReward);
			reference.Child(userID).Child(DataCloudSaverStaticVariable.FIREBASE_NODE_USERDATA_DAILYREWARD).SetRawJsonValueAsync(rawJsonValueAsync);
		}

		public void RestoreData()
		{
			SingletonMonoBehaviour<LifetimeCanvas>.Instance.LoadingProgressPopupController.Open();
			userID = PlatformSpecificServicesProvider.Services.UserProfile.GetFireseBaseUserID();
			if (string.IsNullOrEmpty(userID))
			{
				UnityEngine.Debug.Log("Restore - User ID cannot be empty!");
			}
			else
			{
				StartCoroutine(_RestoreData());
			}
		}

		private IEnumerator _RestoreData()
		{
			TaskCompleteChecker checker = new TaskCompleteChecker();
			yield return new WaitForThreadedTaskWithChecker(delegate
			{
				FirebaseDatabase.DefaultInstance.GetReference(userID).GetValueAsync().ContinueWith(delegate(Task<DataSnapshot> task)
				{
					checker.isTaskCompleted = true;
					if (!task.IsFaulted && task.IsCompleted)
					{
						DataSnapshot result = task.Result;
						if (result != null && result.ChildrenCount > 0)
						{
							dataRestoreDeliver = new DataRestoreDeliver();
							isDataCollected = false;
							RestoreData_Hero();
							RestoreData_GlobalUpgrade();
							RestoreData_Map();
							RestoreData_Theme();
							RestoreData_UserProfile();
							RestoreData_PowerUpItem();
							RestoreData_Tutorial();
							RestoreData_Offers();
							RestoreData_FreeResources();
							RestoreData_SaleBundle();
							RestoreData_DailyReward();
							RestoreData_DailyTrial();
						}
						else
						{
							dataCollectFailedFlag = true;
						}
					}
				});
			}, checker, System.Threading.ThreadPriority.Lowest);
			SingletonMonoBehaviour<LifetimeCanvas>.Instance.LoadingProgressPopupController.Close();
		}

		private IEnumerator BackToMainMenu()
		{
			string notify = Singleton<NotificationDescription>.Instance.GetNotiContent(135);
			SingletonMonoBehaviour<LifetimeCanvas>.Instance.NotifyPopupController.Init(notify, isShowButtonFreeResources: false, isShowButtonGoToStore: false);
			yield return new WaitForSeconds(1f);
			Loading.Instance.ShowLoading();
			yield return new WaitForSeconds(1f);
			GameApplication.Instance.LoadScene(GameApplication.MainMenuSceneName);
		}

		private bool IsDataGetCompleted()
		{
			return isDataCollected;
		}

		private bool CheckIfUserHaveCloudData()
		{
			return false;
		}

		private void RestoreData_Hero()
		{
			FirebaseDatabase.DefaultInstance.GetReference(userID).Child(DataCloudSaverStaticVariable.FIREBASE_NODE_USERDATA_HERO).GetValueAsync()
				.ContinueWith(delegate(Task<DataSnapshot> task)
				{
					if (task.IsFaulted)
					{
						UnityEngine.Debug.LogError("Restore data hero failed!");
					}
					else if (task.IsCompleted)
					{
						DataSnapshot result = task.Result;
						string rawJsonValue = result.GetRawJsonValue();
						UserData_Hero userData_Hero = JsonConvert.DeserializeObject<UserData_Hero>(rawJsonValue);
						dataRestoreDeliver.userData_Hero = new UserData_Hero();
						dataRestoreDeliver.userData_Hero = userData_Hero;
					}
				});
		}

		private void RestoreData_GlobalUpgrade()
		{
			FirebaseDatabase.DefaultInstance.GetReference(userID).Child(DataCloudSaverStaticVariable.FIREBASE_NODE_USERDATA_GLOBALUPGRADE).GetValueAsync()
				.ContinueWith(delegate(Task<DataSnapshot> task)
				{
					if (task.IsFaulted)
					{
						UnityEngine.Debug.LogError("Restore data global upgrade failed!");
					}
					else if (task.IsCompleted)
					{
						DataSnapshot result = task.Result;
						string rawJsonValue = result.GetRawJsonValue();
						UserData_GlobalUpgrade userData_GlobalUpgrade = JsonConvert.DeserializeObject<UserData_GlobalUpgrade>(rawJsonValue);
						dataRestoreDeliver.userData_GlobalUpgrade = new UserData_GlobalUpgrade();
						dataRestoreDeliver.userData_GlobalUpgrade = userData_GlobalUpgrade;
					}
				});
		}

		private void RestoreData_Map()
		{
			FirebaseDatabase.DefaultInstance.GetReference(userID).Child(DataCloudSaverStaticVariable.FIREBASE_NODE_USERDATA_MAP).GetValueAsync()
				.ContinueWith(delegate(Task<DataSnapshot> task)
				{
					if (task.IsFaulted)
					{
						UnityEngine.Debug.LogError("Restore data map failed!");
					}
					else if (task.IsCompleted)
					{
						DataSnapshot result = task.Result;
						string rawJsonValue = result.GetRawJsonValue();
						UserData_Map userData_Map = JsonConvert.DeserializeObject<UserData_Map>(rawJsonValue);
						dataRestoreDeliver.userData_Map = new UserData_Map();
						dataRestoreDeliver.userData_Map = userData_Map;
					}
				});
		}

		private void RestoreData_Theme()
		{
			FirebaseDatabase.DefaultInstance.GetReference(userID).Child(DataCloudSaverStaticVariable.FIREBASE_NODE_USERDATA_THEME).GetValueAsync()
				.ContinueWith(delegate(Task<DataSnapshot> task)
				{
					if (task.IsFaulted)
					{
						UnityEngine.Debug.LogError("Restore data theme failed!");
					}
					else if (task.IsCompleted)
					{
						DataSnapshot result = task.Result;
						string rawJsonValue = result.GetRawJsonValue();
						UserData_Theme userData_Theme = JsonConvert.DeserializeObject<UserData_Theme>(rawJsonValue);
						dataRestoreDeliver.userData_Theme = new UserData_Theme();
						dataRestoreDeliver.userData_Theme = userData_Theme;
					}
				});
		}

		private void RestoreData_UserProfile()
		{
			FirebaseDatabase.DefaultInstance.GetReference(userID).Child(DataCloudSaverStaticVariable.FIREBASE_NODE_USERDATA_USERPROFILE).GetValueAsync()
				.ContinueWith(delegate(Task<DataSnapshot> task)
				{
					if (task.IsFaulted)
					{
						UnityEngine.Debug.LogError("Restore data user profile failed!");
					}
					else if (task.IsCompleted)
					{
						DataSnapshot result = task.Result;
						string rawJsonValue = result.GetRawJsonValue();
						UserData_UserProfile userData_UserProfile = JsonConvert.DeserializeObject<UserData_UserProfile>(rawJsonValue);
						dataRestoreDeliver.userData_UserProfile = new UserData_UserProfile();
						dataRestoreDeliver.userData_UserProfile = userData_UserProfile;
					}
				});
		}

		private void RestoreData_PowerUpItem()
		{
			FirebaseDatabase.DefaultInstance.GetReference(userID).Child(DataCloudSaverStaticVariable.FIREBASE_NODE_USERDATA_POWERUPITEM).GetValueAsync()
				.ContinueWith(delegate(Task<DataSnapshot> task)
				{
					if (task.IsFaulted)
					{
						UnityEngine.Debug.LogError("Restore data powerup item failed!");
					}
					else if (task.IsCompleted)
					{
						DataSnapshot result = task.Result;
						string rawJsonValue = result.GetRawJsonValue();
						UserData_PowerupItem userData_PowerupItem = JsonConvert.DeserializeObject<UserData_PowerupItem>(rawJsonValue);
						dataRestoreDeliver.userData_PowerupItem = new UserData_PowerupItem();
						dataRestoreDeliver.userData_PowerupItem = userData_PowerupItem;
					}
				});
		}

		private void RestoreData_Tutorial()
		{
			FirebaseDatabase.DefaultInstance.GetReference(userID).Child(DataCloudSaverStaticVariable.FIREBASE_NODE_USERDATA_TUTORIAL).GetValueAsync()
				.ContinueWith(delegate(Task<DataSnapshot> task)
				{
					if (task.IsFaulted)
					{
						UnityEngine.Debug.LogError("Restore data tutorial failed!");
					}
					else if (task.IsCompleted)
					{
						DataSnapshot result = task.Result;
						string rawJsonValue = result.GetRawJsonValue();
						UserData_Tutorial userData_Tutorial = new UserData_Tutorial
						{
							ListTutorialData = new Dictionary<string, bool>()
						};
						userData_Tutorial = JsonConvert.DeserializeObject<UserData_Tutorial>(rawJsonValue);
						dataRestoreDeliver.userData_Tutorial = new UserData_Tutorial();
						dataRestoreDeliver.userData_Tutorial = userData_Tutorial;
					}
				});
		}

		private void RestoreData_Offers()
		{
			FirebaseDatabase.DefaultInstance.GetReference(userID).Child(DataCloudSaverStaticVariable.FIREBASE_NODE_USERDATA_OFFER).GetValueAsync()
				.ContinueWith(delegate(Task<DataSnapshot> task)
				{
					if (task.IsFaulted)
					{
						UnityEngine.Debug.LogError("Restore data offer failed!");
					}
					else if (task.IsCompleted)
					{
						DataSnapshot result = task.Result;
						string rawJsonValue = result.GetRawJsonValue();
						UserData_Offer userData_Offer = JsonConvert.DeserializeObject<UserData_Offer>(rawJsonValue);
						dataRestoreDeliver.userData_Offer = new UserData_Offer();
						dataRestoreDeliver.userData_Offer = userData_Offer;
					}
				});
		}

		private void RestoreData_FreeResources()
		{
			FirebaseDatabase.DefaultInstance.GetReference(userID).Child(DataCloudSaverStaticVariable.FIREBASE_NODE_USERDATA_FREERESOURCES).GetValueAsync()
				.ContinueWith(delegate(Task<DataSnapshot> task)
				{
					if (task.IsFaulted)
					{
						UnityEngine.Debug.LogError("Restore data free resouces failed!");
					}
					else if (task.IsCompleted)
					{
						DataSnapshot result = task.Result;
						string rawJsonValue = result.GetRawJsonValue();
						UserData_FreeResources userData_FreeResources = JsonConvert.DeserializeObject<UserData_FreeResources>(rawJsonValue);
						dataRestoreDeliver.userData_FreeResources = new UserData_FreeResources();
						dataRestoreDeliver.userData_FreeResources = userData_FreeResources;
					}
				});
		}

		private void RestoreData_SaleBundle()
		{
			FirebaseDatabase.DefaultInstance.GetReference(userID).Child(DataCloudSaverStaticVariable.FIREBASE_NODE_USERDATA_SALEBUNDLE).GetValueAsync()
				.ContinueWith(delegate(Task<DataSnapshot> task)
				{
					if (task.IsFaulted)
					{
						UnityEngine.Debug.LogError("Restore data sale bundle failed!");
					}
					else if (task.IsCompleted)
					{
						DataSnapshot result = task.Result;
						string rawJsonValue = result.GetRawJsonValue();
						UserData_SaleBundle userData_SaleBundle = new UserData_SaleBundle();
						userData_SaleBundle = JsonConvert.DeserializeObject<UserData_SaleBundle>(rawJsonValue);
						dataRestoreDeliver.userData_SaleBundle = new UserData_SaleBundle();
						dataRestoreDeliver.userData_SaleBundle = userData_SaleBundle;
					}
				});
		}

		private void RestoreData_DailyReward()
		{
			FirebaseDatabase.DefaultInstance.GetReference(userID).Child(DataCloudSaverStaticVariable.FIREBASE_NODE_USERDATA_DAILYREWARD).GetValueAsync()
				.ContinueWith(delegate(Task<DataSnapshot> task)
				{
					if (task.IsFaulted)
					{
						UnityEngine.Debug.LogError("Restore data daily reward failed!");
					}
					else if (task.IsCompleted)
					{
						DataSnapshot result = task.Result;
						string rawJsonValue = result.GetRawJsonValue();
						UserData_DailyReward userData_DailyReward = new UserData_DailyReward();
						userData_DailyReward = JsonConvert.DeserializeObject<UserData_DailyReward>(rawJsonValue);
						dataRestoreDeliver.userData_DailyReward = new UserData_DailyReward();
						dataRestoreDeliver.userData_DailyReward = userData_DailyReward;
					}
				});
		}

		private void RestoreData_DailyTrial()
		{
			FirebaseDatabase.DefaultInstance.GetReference(userID).Child(DataCloudSaverStaticVariable.FIREBASE_NODE_USERDATA_DAILYTRIAL).GetValueAsync()
				.ContinueWith(delegate(Task<DataSnapshot> task)
				{
					if (task.IsFaulted)
					{
						UnityEngine.Debug.LogError("Restore data daily trial item failed!");
					}
					else if (task.IsCompleted)
					{
						DataSnapshot result = task.Result;
						string rawJsonValue = result.GetRawJsonValue();
						UserData_DailyTrial userData_DailyTrial = JsonConvert.DeserializeObject<UserData_DailyTrial>(rawJsonValue);
						dataRestoreDeliver.userData_DailyTrial = new UserData_DailyTrial();
						dataRestoreDeliver.userData_DailyTrial = userData_DailyTrial;
						isDataCollected = true;
					}
				});
		}

		public void RetrieveData(string dbRefPath, Action<IDataSnapshot> callback)
		{
			RetrieveDataWithMainThreadCallback(dbRefPath, callback);
		}

		public void RetrieveDataWithMainThreadCallback(string dbRef, Action<IDataSnapshot> callback)
		{
			StartCoroutine(_RetrieveDataWithMainThreadCallback(dbRef, callback));
		}

		private IEnumerator _RetrieveDataWithMainThreadCallback(string dbRef, Action<IDataSnapshot> callback)
		{
			TaskCompleteChecker checker = new TaskCompleteChecker();
			Task<DataSnapshot> mTask = null;
			yield return new WaitForThreadedTaskWithChecker(delegate
			{
				FirebaseDatabase.DefaultInstance.GetReference(dbRef).GetValueAsync().ContinueWith(delegate(Task<DataSnapshot> task)
				{
					mTask = task;
					checker.isTaskCompleted = true;
				});
			}, checker, System.Threading.ThreadPriority.Lowest);
			if (callback != null && mTask != null)
			{
				callback(new DataSnapshotAndroid(mTask.Result, mTask.IsFaulted, mTask.IsCompleted));
			}
		}

		public void UpdateData(Dictionary<string, object> updateList, string dbRefPath = null)
		{
			DatabaseReference databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
			if (!string.IsNullOrEmpty(dbRefPath))
			{
				databaseReference = databaseReference.Child(dbRefPath);
			}
			databaseReference.UpdateChildrenAsync(updateList);
		}

		public void WriteData(object data, string dbRefPath)
		{
			WriteDataWithMainThreadCallback(data, dbRefPath, null);
		}

		public void WriteDataWithMainThreadCallback(object data, string dbRefPath, Action<IDataSnapshot> callback)
		{
			StartCoroutine(_WriteDataWithMainThreadCallback(data, dbRefPath, callback));
		}

		private IEnumerator _WriteDataWithMainThreadCallback(object data, string dbRefPath, Action<IDataSnapshot> callback)
		{
			TaskCompleteChecker checker = new TaskCompleteChecker();
			Task mTask = null;
			string senderJSON = JsonConvert.SerializeObject(data);
			yield return new WaitForThreadedTaskWithChecker(delegate
			{
				reference.Child(dbRefPath).SetRawJsonValueAsync(senderJSON).ContinueWith(delegate(Task task)
				{
					mTask = task;
					checker.isTaskCompleted = true;
				});
			}, checker, System.Threading.ThreadPriority.Lowest);
			callback?.Invoke(new DataSnapshotAndroid(null, mTask.IsFaulted, mTask.IsCompleted));
		}

		public void WriteGroupInfoTransaction(string groupInfoPath, bool isUserPremium, int tier = -1)
		{
			reference.Child(groupInfoPath).RunTransaction(delegate(MutableData mutableData)
			{
				Dictionary<string, object> dictionary = mutableData.Value as Dictionary<string, object>;
				if (dictionary != null)
				{
					WriteGroupInfo_UpdateGroupData(dictionary, isUserPremium);
					mutableData.Value = dictionary;
				}
				return TransactionResult.Success(mutableData);
			}).ContinueWith(delegate(Task<DataSnapshot> task)
			{
				if (task.Exception != null)
				{
					UnityEngine.Debug.Log(task.Exception.ToString());
				}
			});
		}

		public void WriteNewGroupInfoTransaction(int newGroupId, bool isUserPremium, int tier = -1)
		{
			string groupId = newGroupId.ToString();
			reference.Child("Tournament/Groupinfo/").RunTransaction(delegate(MutableData mutableData)
			{
				Dictionary<string, object> dictionary = mutableData.Value as Dictionary<string, object>;
				if (dictionary != null)
				{
					if (!dictionary.ContainsKey(groupId))
					{
						dictionary.Add(groupId, new Dictionary<string, object>());
					}
					Dictionary<string, object> groupInfo = dictionary[groupId] as Dictionary<string, object>;
					WriteGroupInfo_UpdateGroupData(groupInfo, isUserPremium);
					mutableData.Value = dictionary;
				}
				return TransactionResult.Success(mutableData);
			}).ContinueWith(delegate(Task<DataSnapshot> task)
			{
				if (task.Exception != null)
				{
					UnityEngine.Debug.Log(task.Exception.ToString());
				}
			});
		}

		private void WriteGroupInfo_UpdateGroupData(Dictionary<string, object> groupInfo, bool isUserPremium)
		{
			if (groupInfo.ContainsKey("quantity"))
			{
				long num = (long)groupInfo["quantity"];
				groupInfo["quantity"] = num + 1;
			}
			else
			{
				groupInfo["quantity"] = 1;
			}
			if (isUserPremium)
			{
				if (groupInfo.ContainsKey("premiumCount"))
				{
					long num2 = (long)groupInfo["premiumCount"];
					groupInfo["premiumCount"] = num2 + 1;
				}
				else
				{
					groupInfo["premiumCount"] = 1;
				}
			}
		}

		public void ClaimPremiumUserInfor(string userID, string userName, string userEmail, string userPhoneNumber)
		{
			UserData_PremiumInfor userData_PremiumInfor = new UserData_PremiumInfor();
			userData_PremiumInfor.userID = userID;
			userData_PremiumInfor.userName = userName;
			userData_PremiumInfor.userEmail = userEmail;
			userData_PremiumInfor.userPhoneNUmber = userPhoneNumber;
			string rawJsonValueAsync = JsonConvert.SerializeObject(userData_PremiumInfor);
			reference.Child(DataCloudSaverStaticVariable.FIREBASE_NODE_USERDATA_PREMIUMINFOR).Child(userID).SetRawJsonValueAsync(rawJsonValueAsync);
		}
	}
}

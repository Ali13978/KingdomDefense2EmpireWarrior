using Data;
using Gameplay;
using Parameter;
using Services.PlatformSpecific;
using UnityEngine;
using UnityEngine.UI;

namespace UserProfile
{
	public class UserProfilePopupController : GameplayPopupController
	{
		[Space]
		[Header("References")]
		[SerializeField]
		private ButtonController[] logInNOutButtons;

		[Space]
		[Header("Profile")]
		[SerializeField]
		private Text userDatabaseID;

		[SerializeField]
		private Text userName;

		[SerializeField]
		private Image userAvatar;

		[SerializeField]
		private Image userCountryAvatar;

		[SerializeField]
		private Text userLeague;

		[SerializeField]
		private Text lastBackupTime;

		[Space]
		[Header("Progress")]
		[SerializeField]
		private Text worldUnlockProgress;

		[SerializeField]
		private Text mapUnlockProgress;

		[SerializeField]
		private Text dailyTrialProgress;

		[SerializeField]
		private Text heroOwnedProgress;

		[Space]
		[Header("Confirm Backup/Restore")]
		[SerializeField]
		private ConfirmPopupController confirmPopup;

		[Space]
		[Header("User name")]
		[SerializeField]
		private ChangeNamePopupController changeNamePopupController;

		[Space]
		[Header("User Region")]
		[SerializeField]
		private ChangeRegionPopupController changeRegionPopupController;

		[SerializeField]
		private ChangeRegionButtonController changeRegionButtonController;

		public ConfirmPopupController ConfirmPopup
		{
			get
			{
				return confirmPopup;
			}
			set
			{
				confirmPopup = value;
			}
		}

		public ChangeNamePopupController ChangeNamePopupController
		{
			get
			{
				return changeNamePopupController;
			}
			set
			{
				changeNamePopupController = value;
			}
		}

		public ChangeRegionPopupController ChangeRegionPopupController
		{
			get
			{
				return changeRegionPopupController;
			}
			set
			{
				changeRegionPopupController = value;
			}
		}

		public ChangeRegionButtonController ChangeRegionButtonController
		{
			get
			{
				return changeRegionButtonController;
			}
			set
			{
				changeRegionButtonController = value;
			}
		}

		private void Awake()
		{
			PlatformSpecificServicesProvider.Services.UserProfile.OnLogStatusChangeEvent += UserProfile_OnLogStatusChangeEvent;
			PlatformSpecificServicesProvider.Services.DataCloudSaver.OnDataRestoreCompletedEvent += DataCloudSaver_OnDataRestoreCompletedEvent;
			ReadWriteDataUserProfile.Instance.OnUserInforChangeEvent += Instance_OnUserInforChangeEvent;
		}

		private void OnDestroy()
		{
			PlatformSpecificServicesProvider.Services.UserProfile.OnLogStatusChangeEvent -= UserProfile_OnLogStatusChangeEvent;
			PlatformSpecificServicesProvider.Services.DataCloudSaver.OnDataRestoreCompletedEvent -= DataCloudSaver_OnDataRestoreCompletedEvent;
			ReadWriteDataUserProfile.Instance.OnUserInforChangeEvent -= Instance_OnUserInforChangeEvent;
		}

		private void UserProfile_OnLogStatusChangeEvent()
		{
			RefreshButtonsStatus();
			InitUserBasicInformation();
		}

		private void DataCloudSaver_OnDataRestoreCompletedEvent()
		{
			InitUserBasicInformation();
			InitUserProgress();
		}

		private void Instance_OnUserInforChangeEvent()
		{
			InitUserBasicInformation();
			InitUserProgress();
		}

		public void Init()
		{
			OpenWithScaleAnimation();
			RefreshButtonsStatus();
			InitUserBasicInformation();
			InitUserProgress();
		}

		private void RefreshButtonsStatus()
		{
			ButtonController[] array = logInNOutButtons;
			foreach (ButtonController buttonController in array)
			{
				buttonController.UpdateButtonStatus();
			}
		}

		private void InitUserBasicInformation()
		{
			userDatabaseID.text = ReadWriteDataUserProfile.Instance.GetUserID();
			userName.text = ReadWriteDataUserProfile.Instance.GetUserName();
			SetUserAvatar();
			ChangeRegionButtonController.UpdateImage();
			userLeague.text = ReadWriteDataUserProfile.Instance.GetLeagueName();
			lastBackupTime.text = ReadWriteDataUserProfile.Instance.GetLastTimeBackup();
		}

		private void InitUserProgress()
		{
			int themeIDUnlocked = ReadWriteDataTheme.Instance.GetThemeIDUnlocked();
			int totalTheme = ReadWriteDataTheme.Instance.GetTotalTheme();
			worldUnlockProgress.text = themeIDUnlocked + 1 + "/" + totalTheme;
			int mapIDPassed = ReadWriteDataMap.Instance.GetMapIDPassed();
			int totalMap = ReadWriteDataMap.Instance.GetTotalMap();
			mapUnlockProgress.text = mapIDPassed + 1 + "/" + totalMap;
			int currentDayIndex = ReadWriteDataDailyTrial.Instance.GetCurrentDayIndex();
			int num = 7;
			if (currentDayIndex + 1 == num)
			{
				dailyTrialProgress.text = num + "/" + num;
			}
			else
			{
				dailyTrialProgress.text = currentDayIndex + 1 + "/" + num;
			}
			int count = ReadWriteDataHero.Instance.GetListHeroIDOwned().Count;
			int count2 = HeroParameter.Instance.GetListHeroID().Count;
			heroOwnedProgress.text = count + "/" + count2;
		}

		private void SetUserAvatar()
		{
			Sprite sprite = PlatformSpecificServicesProvider.Services.UserProfile.GetUserAvatar();
			if ((bool)sprite)
			{
				userAvatar.sprite = sprite;
			}
		}

		public void InitConfirmPopup(CloudDataInteraction cloudDataInteraction)
		{
			ConfirmPopup.Init(cloudDataInteraction);
		}

		public override void OpenWithScaleAnimation()
		{
			base.OpenWithScaleAnimation();
		}

		public override void CloseWithScaleAnimation()
		{
			base.CloseWithScaleAnimation();
		}
	}
}

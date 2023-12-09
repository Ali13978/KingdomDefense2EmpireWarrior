using Data;
using LifetimePopup;
using MyCustom;
using Parameter;
using Services.PlatformSpecific;
using UnityEngine;
using UnityEngine.UI;
using WorldMap;

namespace UserProfile
{
	public class BackupButtonController : ButtonController
	{
		[SerializeField]
		private Button button;

		[SerializeField]
		private Image image;

		private void Start()
		{
			if (PassSpecificCondition())
			{
				SetClickable();
			}
			else
			{
				SetUnClickable();
			}
		}

		private bool PassSpecificCondition()
		{
			bool flag = false;
			return ReadWriteDataMap.Instance.GetMapIDPassed() >= 0;
		}

		private void SetClickable()
		{
			button.enabled = true;
			image.color = Color.white;
		}

		private void SetUnClickable()
		{
			button.enabled = false;
			image.color = Color.gray;
		}

		public override void OnClick()
		{
			base.OnClick();
			if (!StaticMethod.IsInternetConnectionAvailable())
			{
				string notiContent = Singleton<NotificationDescription>.Instance.GetNotiContent(134);
				SingletonMonoBehaviour<LifetimeCanvas>.Instance.NotifyPopupController.Init(notiContent, isShowButtonFreeResources: false, isShowButtonGoToStore: false);
			}
			else if (PlatformSpecificServicesProvider.Services.UserProfile.IsLoggedIn_Google() || PlatformSpecificServicesProvider.Services.UserProfile.IsLoggedIn_Facebook())
			{
				SingletonMonoBehaviour<UIRootController>.Instance.userProfilePopupController.InitConfirmPopup(CloudDataInteraction.Backup);
			}
			else
			{
				string notiContent2 = Singleton<NotificationDescription>.Instance.GetNotiContent(133);
				SingletonMonoBehaviour<LifetimeCanvas>.Instance.NotifyPopupController.Init(notiContent2, isShowButtonFreeResources: false, isShowButtonGoToStore: false);
			}
		}
	}
}

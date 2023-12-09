using LifetimePopup;
using MyCustom;
using Parameter;
using Services.PlatformSpecific;
using UnityEngine;
using UnityEngine.UI;

namespace UserProfile
{
	public class LogoutButtonController : ButtonController
	{
		[SerializeField]
		private Button button;

		[SerializeField]
		private Image image;

		public override void OnClick()
		{
			base.OnClick();
			if (!StaticMethod.IsInternetConnectionAvailable())
			{
				string notiContent = Singleton<NotificationDescription>.Instance.GetNotiContent(134);
				SingletonMonoBehaviour<LifetimeCanvas>.Instance.NotifyPopupController.Init(notiContent, isShowButtonFreeResources: false, isShowButtonGoToStore: false);
			}
			else
			{
				PlatformSpecificServicesProvider.Services.UserProfile.LogOut_Google();
				PlatformSpecificServicesProvider.Services.UserProfile.LogOut_Facebook();
				PlatformSpecificServicesProvider.Services.UserProfile.FirebaseSignOut();
			}
		}

		public override void UpdateButtonStatus()
		{
			base.UpdateButtonStatus();
			if (PlatformSpecificServicesProvider.Services.UserProfile.IsLoggedIn_Google() || PlatformSpecificServicesProvider.Services.UserProfile.IsLoggedIn_Facebook())
			{
				Show();
			}
			else
			{
				Hide();
			}
		}

		private void Hide()
		{
			button.enabled = false;
			image.color = Color.gray;
		}

		private void Show()
		{
			button.enabled = true;
			image.color = Color.white;
		}
	}
}

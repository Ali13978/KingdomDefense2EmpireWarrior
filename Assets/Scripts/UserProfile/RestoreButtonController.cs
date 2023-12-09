using LifetimePopup;
using MyCustom;
using Parameter;
using Services.PlatformSpecific;
using WorldMap;

namespace UserProfile
{
	public class RestoreButtonController : ButtonController
	{
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
				SingletonMonoBehaviour<UIRootController>.Instance.userProfilePopupController.InitConfirmPopup(CloudDataInteraction.Restore);
			}
			else
			{
				string notiContent2 = Singleton<NotificationDescription>.Instance.GetNotiContent(133);
				SingletonMonoBehaviour<LifetimeCanvas>.Instance.NotifyPopupController.Init(notiContent2, isShowButtonFreeResources: false, isShowButtonGoToStore: false);
			}
		}
	}
}

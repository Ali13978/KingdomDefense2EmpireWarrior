using LifetimePopup;
using MyCustom;
using Parameter;
using WorldMap;

namespace GiftcodeSystem
{
	public class GiftCodeButtonController : ButtonController
	{
		public override void OnClick()
		{
			base.OnClick();
			TryOpenPopupGiftCode();
		}

		private void TryOpenPopupGiftCode()
		{
			if (StaticMethod.IsInternetConnectionAvailable())
			{
				SingletonMonoBehaviour<UIRootController>.Instance.giftCodePopupController.Init();
				return;
			}
			string notiContent = Singleton<NotificationDescription>.Instance.GetNotiContent(118);
			SingletonMonoBehaviour<LifetimeCanvas>.Instance.NotifyPopupController.Init(notiContent, isShowButtonFreeResources: true, isShowButtonGoToStore: false);
		}
	}
}

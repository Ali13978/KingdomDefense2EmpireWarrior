using Data;
using LifetimePopup;
using MyCustom;
using Parameter;
using WorldMap;

namespace Tournament
{
	public class TournamentButtonController : ButtonController
	{
		public override void OnClick()
		{
			base.OnClick();
			TryToOpenTournamentPopup();
		}

		private void TryToOpenTournamentPopup()
		{
			if (StaticMethod.IsInternetConnectionAvailable())
			{
				string userID = ReadWriteDataUserProfile.Instance.GetUserID();
				if (string.IsNullOrEmpty(userID) || userID == "empty")
				{
					string notiContent = Singleton<NotificationDescription>.Instance.GetNotiContent(133);
					SingletonMonoBehaviour<LifetimeCanvas>.Instance.NotifyPopupController.Init(notiContent, "OK", null, delegate
					{
						SingletonMonoBehaviour<UIRootController>.Instance.userProfilePopupController.Init();
					});
				}
				else
				{
					OpenTournamentPopup();
				}
			}
			else
			{
				string notiContent2 = Singleton<NotificationDescription>.Instance.GetNotiContent(119);
				SingletonMonoBehaviour<LifetimeCanvas>.Instance.NotifyPopupController.Init(notiContent2, isShowButtonFreeResources: false, isShowButtonGoToStore: false);
			}
		}

		private void OpenTournamentPopup()
		{
			SingletonMonoBehaviour<UIRootController>.Instance.tournamentPopupController.Init();
		}
	}
}

using WorldMap;

namespace UserProfile
{
	public class UserProfileButtonController : ButtonController
	{
		public override void OnClick()
		{
			base.OnClick();
			InitUserProfilePopup();
		}

		private void InitUserProfilePopup()
		{
			SingletonMonoBehaviour<UIRootController>.Instance.userProfilePopupController.Init();
		}
	}
}

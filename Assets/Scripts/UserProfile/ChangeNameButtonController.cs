using WorldMap;

namespace UserProfile
{
	public class ChangeNameButtonController : ButtonController
	{
		public override void OnClick()
		{
			base.OnClick();
			OpenPopupChangeName();
		}

		private void OpenPopupChangeName()
		{
			SingletonMonoBehaviour<UIRootController>.Instance.userProfilePopupController.ChangeNamePopupController.Init();
		}
	}
}

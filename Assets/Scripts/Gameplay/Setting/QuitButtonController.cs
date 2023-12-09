namespace Gameplay.Setting
{
	public class QuitButtonController : ButtonController
	{
		public override void OnClick()
		{
			base.OnClick();
			SingletonMonoBehaviour<UIRootController>.Instance.settingPopupController.InitConfirmGroup(CancelGameType.Quit);
		}
	}
}

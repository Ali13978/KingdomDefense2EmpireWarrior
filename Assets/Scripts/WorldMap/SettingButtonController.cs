namespace WorldMap
{
	public class SettingButtonController : ButtonController
	{
		public override void OnClick()
		{
			base.OnClick();
			SingletonMonoBehaviour<UIRootController>.Instance.settingPopupController.Init();
		}
	}
}

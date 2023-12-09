namespace Gameplay
{
	public class PauseButtonController : ButtonController
	{
		public override void OnClick()
		{
			base.OnClick();
			if (!SingletonMonoBehaviour<GameData>.Instance.IsGameOver)
			{
				SingletonMonoBehaviour<UIRootController>.Instance.settingPopupController.Open();
			}
		}
	}
}

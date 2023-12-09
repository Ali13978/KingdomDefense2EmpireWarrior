namespace Gameplay
{
	public class FreeResourcesButtonController : ButtonController
	{
		public override void OnClick()
		{
			base.OnClick();
			OpenPopup();
		}

		private void OpenPopup()
		{
			if (!SingletonMonoBehaviour<GameData>.Instance.IsGameOver)
			{
				SingletonMonoBehaviour<UIRootController>.Instance.freeResourcesPopupController.Init();
			}
		}
	}
}

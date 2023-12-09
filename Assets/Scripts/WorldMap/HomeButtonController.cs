namespace WorldMap
{
	public class HomeButtonController : ButtonController
	{
		public override void OnClick()
		{
			base.OnClick();
			OpenSceneMainMenu();
		}

		private void OpenSceneMainMenu()
		{
			Loading.Instance.ShowLoading();
			Invoke("DoLoadSceneMainMenu", 1f);
		}

		private void DoLoadSceneMainMenu()
		{
			GameApplication.Instance.LoadScene(GameApplication.MainMenuSceneName);
		}
	}
}

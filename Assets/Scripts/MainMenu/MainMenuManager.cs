using Services.PlatformSpecific;

namespace MainMenu
{
	public class MainMenuManager : SingletonMonoBehaviour<MainMenuManager>
	{
		private IAnalytics analytics;

		private void Start()
		{
			analytics = PlatformSpecificServicesProvider.Services.Analytics;
			SendEventOpenScene();
		}

		private void SendEventOpenScene()
		{
			analytics.SendEvent_OpenSceneMainMenu();
		}
	}
}

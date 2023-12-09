using Middle;

namespace DailyTrial
{
	public class StartButtonController : ButtonController
	{
		public override void OnClick()
		{
			base.OnClick();
			Invoke("OnPrepareToLoad", 0.1f);
			UISoundManager.Instance.PlayStartGameAtMapLevel();
		}

		private void OnPrepareToLoad()
		{
			Loading.Instance.ShowLoading();
			Invoke("DoLoadSceneGameplay", 0.3f);
			ModeManager.Instance.gameMode = GameMode.DailyTrialMode;
		}

		private void DoLoadSceneGameplay()
		{
			GameApplication.Instance.LoadScene(GameApplication.GameplaySceneName);
		}
	}
}

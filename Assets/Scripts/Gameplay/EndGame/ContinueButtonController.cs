using LifetimePopup;
using Middle;

namespace Gameplay.EndGame
{
	public class ContinueButtonController : ButtonController
	{
		public override void OnClick()
		{
			base.OnClick();
			Loading.Instance.ShowLoading();
			Invoke("DoLoad", 1f);
			GameplayManager.Instance.gameSpeedController.UnPauseGame();
		}

		private void DoLoad()
		{
			VideoPlayerManager.Instance.TryToShowInterstitialAds_EndGame();
			ModeManager.Instance.gameMode = GameMode.CampaignMode;
			GameApplication.Instance.LoadScene(GameApplication.WorldMapSceneName);
			SingletonMonoBehaviour<LifetimeCanvas>.Instance.TryToOpenPopupAskRate(SingletonMonoBehaviour<GameData>.Instance.MapID);
		}
	}
}

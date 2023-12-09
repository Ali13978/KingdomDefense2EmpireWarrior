using Data;
using Services.PlatformSpecific;

namespace Gameplay.EndGame
{
	public class RestartButtonController : ButtonController
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
			SendEventRestartGame();
			GameplayManager.Instance.ReloadCurrentScene();
		}

		private void SendEventRestartGame()
		{
			int mapID = SingletonMonoBehaviour<GameData>.Instance.MapID;
			int starEarnedByMap = ReadWriteDataMap.Instance.GetStarEarnedByMap(mapID);
			PlatformSpecificServicesProvider.Services.Analytics.SendEvent_RestartGame_EndGame(mapID + 1, starEarnedByMap);
		}
	}
}

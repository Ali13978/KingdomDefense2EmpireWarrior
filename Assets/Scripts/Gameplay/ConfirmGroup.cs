using Data;
using Middle;
using Parameter;
using Services.PlatformSpecific;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
	public class ConfirmGroup : MonoBehaviour
	{
		[SerializeField]
		private Text warning;

		[SerializeField]
		private int warningQuitID;

		[SerializeField]
		private int warningRestartID;

		private CancelGameType cancelGameType;

		public void Init(CancelGameType cancelGameType)
		{
			this.cancelGameType = cancelGameType;
			switch (cancelGameType)
			{
			case CancelGameType.Quit:
				warning.text = Singleton<NotificationDescription>.Instance.GetNotiContent(warningQuitID);
				break;
			case CancelGameType.Restart:
				warning.text = Singleton<NotificationDescription>.Instance.GetNotiContent(warningRestartID);
				break;
			}
		}

		public void Click_YES()
		{
			Loading.Instance.ShowLoading();
			switch (cancelGameType)
			{
			case CancelGameType.Quit:
				Invoke("DoQuit", 1f);
				break;
			case CancelGameType.Restart:
				Invoke("DoRestart", 1f);
				break;
			}
			GameplayManager.Instance.gameSpeedController.UnPauseGame();
		}

		public void Click_NO()
		{
			SingletonMonoBehaviour<UIRootController>.Instance.settingPopupController.Close();
		}

		private void DoQuit()
		{
			ModeManager.Instance.gameMode = GameMode.CampaignMode;
			GameApplication.Instance.LoadScene(GameApplication.WorldMapSceneName);
		}

		private void DoRestart()
		{
			SendEventRestartGame();
			GameplayManager.Instance.ReloadCurrentScene();
		}

		private void SendEventRestartGame()
		{
			int mapID = SingletonMonoBehaviour<GameData>.Instance.MapID;
			int starEarnedByMap = ReadWriteDataMap.Instance.GetStarEarnedByMap(mapID);
			PlatformSpecificServicesProvider.Services.Analytics.SendEvent_RestartGame_Setting(mapID + 1, starEarnedByMap);
		}
	}
}

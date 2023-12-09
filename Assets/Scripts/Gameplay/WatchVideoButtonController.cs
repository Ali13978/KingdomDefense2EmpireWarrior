using UnityEngine;

namespace Gameplay
{
	public class WatchVideoButtonController : ButtonController
	{
		[SerializeField]
		private EndGameVideoController endGameVideoController;

		public static bool changedVideoStatus;

		public override void OnClick()
		{
			base.OnClick();
			VideoPlayerManager.Instance.playVideoEndGame();
		}

		private void Update()
		{
			if (!changedVideoStatus && SingletonMonoBehaviour<GameData>.Instance.PlayedVideoEndGame)
			{
				endGameVideoController.StopCountDown();
				endGameVideoController.Close();
				changedVideoStatus = true;
			}
		}
	}
}

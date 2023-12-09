using UnityEngine;

namespace Gameplay
{
	public class SkipButtonController : ButtonController
	{
		[SerializeField]
		private EndGameVideoController endGameVideoController;

		public override void OnClick()
		{
			base.OnClick();
			endGameVideoController.StopCountDown();
			endGameVideoController.Close();
			GameplayManager.Instance.gameSpeedController.UnPauseGame();
			GameplayManager.Instance.gameLogicController.Defeated();
		}
	}
}

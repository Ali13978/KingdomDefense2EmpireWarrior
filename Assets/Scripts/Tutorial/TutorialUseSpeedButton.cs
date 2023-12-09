using Data;
using Gameplay;
using UnityEngine;

namespace Tutorial
{
	public class TutorialUseSpeedButton : TutorialUnit
	{
		private string TutorialID = ReadWriteDataTutorial.TUTORIAL_ID_USE_SPEED_UP;

		protected override void SaveTutorialPassed()
		{
			UnityEngine.Debug.Log("Hoàn thành tut sử dụng X2!");
			ReadWriteDataTutorial.Instance.SetTutorialStatus(TutorialID, value: true);
		}

		protected override bool ShouldShowTutorial()
		{
			return !ReadWriteDataTutorial.Instance.GetTutorialStatus(TutorialID) && SingletonMonoBehaviour<GameData>.Instance.MapID == 0;
		}
	}
}

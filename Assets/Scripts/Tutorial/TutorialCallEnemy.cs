using Data;
using Gameplay;
using UnityEngine;

namespace Tutorial
{
	public class TutorialCallEnemy : TutorialUnit
	{
		private string TutorialID = ReadWriteDataTutorial.TUTORIAL_ID_CALL_ENEMY;

		protected override void SaveTutorialPassed()
		{
			UnityEngine.Debug.Log("Click gọi quái, hoàn thành gọi quái!");
			ReadWriteDataTutorial.Instance.SetTutorialStatus(TutorialID, value: true);
		}

		protected override bool ShouldShowTutorial()
		{
			return !ReadWriteDataTutorial.Instance.GetTutorialStatus(TutorialID) && SingletonMonoBehaviour<GameData>.Instance.MapID == 0;
		}
	}
}

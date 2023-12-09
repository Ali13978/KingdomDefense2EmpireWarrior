using Data;
using UnityEngine;

namespace Tutorial
{
	public class TutorialToTournamentPanel : TutorialUnit
	{
		private string TutorialID = ReadWriteDataTutorial.TUTORIAL_ID_GO_TOURNAMENT;

		protected override void SaveTutorialPassed()
		{
			UnityEngine.Debug.Log("Mở panel tournament, hoàn thành tut!");
			ReadWriteDataTutorial.Instance.SetTutorialStatus(TutorialID, value: true);
		}

		protected override bool ShouldShowTutorial()
		{
			bool flag = false;
			return !ReadWriteDataTutorial.Instance.GetTutorialStatus(TutorialID) && ReadWriteDataMap.Instance.GetMapIDUnlocked() >= 4;
		}
	}
}

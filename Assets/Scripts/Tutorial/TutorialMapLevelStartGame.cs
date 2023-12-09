using Data;
using UnityEngine;

namespace Tutorial
{
	public class TutorialMapLevelStartGame : TutorialUnit
	{
		private string TutorialID = ReadWriteDataTutorial.TUTORIAL_ID_START_GAME_MAP_LEVEL;

		protected override void SaveTutorialPassed()
		{
			UnityEngine.Debug.Log("Hoàn thành bước chọn StartGame tại MapLevelSelect");
			ReadWriteDataTutorial.Instance.SetTutorialStatus(TutorialID, value: true);
		}

		protected override bool ShouldShowTutorial()
		{
			return !ReadWriteDataTutorial.Instance.GetTutorialStatus(TutorialID);
		}

		public void TryToCheckCondition()
		{
			if (ReadWriteDataTutorial.Instance.GetTutorialStatus(ReadWriteDataTutorial.TUTORIAL_ID_BRING_FIRST_HERO))
			{
				CheckCondition();
			}
		}
	}
}

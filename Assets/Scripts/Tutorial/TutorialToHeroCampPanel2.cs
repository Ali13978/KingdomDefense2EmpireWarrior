using Data;
using UnityEngine;

namespace Tutorial
{
	public class TutorialToHeroCampPanel2 : TutorialUnit
	{
		private string TutorialID = ReadWriteDataTutorial.TUTORIAL_ID_GO_HERO_CAMP_2ND;

		protected override void SaveTutorialPassed()
		{
			UnityEngine.Debug.Log("Mở panel hero camp, hoàn thành tut!");
			ReadWriteDataTutorial.Instance.SetTutorialStatus(TutorialID, value: true);
		}

		protected override bool ShouldShowTutorial()
		{
			bool result = false;
			if (PlayerPrefs.GetInt(TutorialID, 0) == 1)
			{
				SaveTutorialPassed();
			}
			else
			{
				result = (!ReadWriteDataTutorial.Instance.GetTutorialStatus(TutorialID) && ReadWriteDataMap.Instance.GetMapIDUnlocked() >= 2);
			}
			return result;
		}
	}
}

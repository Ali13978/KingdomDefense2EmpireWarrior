using Data;
using UnityEngine;

namespace Tutorial
{
	public class TutorialToHeroCampPanel : TutorialUnit
	{
		private string TutorialID = ReadWriteDataTutorial.TUTORIAL_ID_GO_HERO_CAMP_1ST;

		protected override void SaveTutorialPassed()
		{
			UnityEngine.Debug.Log("Mở panel hero camp, hoàn thành tut!");
			ReadWriteDataTutorial.Instance.SetTutorialStatus(TutorialID, value: true);
		}

		protected override bool ShouldShowTutorial()
		{
			bool result = false;
			if (ReadWriteDataMap.Instance.GetMapIDUnlocked() >= 2 || ReadWriteDataHero.Instance.GetCurrentHeroLevel(1) >= 1)
			{
				SaveTutorialPassed();
			}
			else
			{
				result = (!ReadWriteDataTutorial.Instance.GetTutorialStatus(TutorialID) && ReadWriteDataHero.Instance.IsHeroOwned(1));
			}
			return result;
		}
	}
}

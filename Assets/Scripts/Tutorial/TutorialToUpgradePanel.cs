using Data;
using UnityEngine;

namespace Tutorial
{
	public class TutorialToUpgradePanel : TutorialUnit
	{
		private string TutorialID = ReadWriteDataTutorial.TUTORIAL_ID_GO_GLOBAL_UPGRADE;

		protected override void SaveTutorialPassed()
		{
			UnityEngine.Debug.Log("Mở panel upgrade, hoàn thành tut!");
			ReadWriteDataTutorial.Instance.SetTutorialStatus(TutorialID, value: true);
		}

		protected override bool ShouldShowTutorial()
		{
			bool result = false;
			if (ReadWriteDataMap.Instance.GetMapIDUnlocked() >= 2)
			{
				SaveTutorialPassed();
			}
			else
			{
				result = (!ReadWriteDataTutorial.Instance.GetTutorialStatus(TutorialID) && ReadWriteDataPlayerCurrency.Instance.GetCurrentStar() >= 1);
			}
			return result;
		}
	}
}

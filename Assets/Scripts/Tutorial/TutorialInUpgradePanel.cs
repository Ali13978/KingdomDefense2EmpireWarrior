using Data;
using UnityEngine;

namespace Tutorial
{
	public class TutorialInUpgradePanel : TutorialUnit
	{
		private string TutorialID = ReadWriteDataTutorial.TUTORIAL_ID_GLOBAL_UPGRADE;

		protected override void SaveTutorialPassed()
		{
			UnityEngine.Debug.Log("Đã upgrade thành công 1 tier, hoàn thành tut!");
			ReadWriteDataTutorial.Instance.SetTutorialStatus(TutorialID, value: true);
		}

		protected override bool ShouldShowTutorial()
		{
			if (ReadWriteDataGlobalUpgrade.Instance.GetCurrentUpgradeLevel(2) >= 0)
			{
				SaveTutorialPassed();
			}
			return !ReadWriteDataTutorial.Instance.GetTutorialStatus(TutorialID) && ReadWriteDataPlayerCurrency.Instance.GetCurrentStar() >= 1 && ReadWriteDataGlobalUpgrade.Instance.GetCurrentUpgradeLevel(2) < 1;
		}
	}
}

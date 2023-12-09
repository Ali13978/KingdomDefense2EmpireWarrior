using Data;
using UnityEngine;

namespace Tutorial
{
	public class TutorialUpgradeHeroByGem : TutorialUnit
	{
		private string TutorialID = ReadWriteDataTutorial.TUTORIAL_ID_UPGRADE_HERO_LEVEL;

		protected override void SaveTutorialPassed()
		{
			UnityEngine.Debug.Log("Upgrade level cho hero, hoàn thành tut!");
			ReadWriteDataTutorial.Instance.SetTutorialStatus(TutorialID, value: true);
		}

		protected override bool ShouldShowTutorial()
		{
			if (ReadWriteDataMap.Instance.GetMapIDUnlocked() >= 2)
			{
				SaveTutorialPassed();
			}
			return !ReadWriteDataTutorial.Instance.GetTutorialStatus(TutorialID) && (ReadWriteDataHero.Instance.IsHeroOwned(1) & (ReadWriteDataHero.Instance.GetCurrentHeroLevel(1) <= 1));
		}
	}
}

using Data;
using UnityEngine;

namespace Tutorial
{
	public class TutorialUseHeroSkillPoint : TutorialUnit
	{
		private string TutorialID = ReadWriteDataTutorial.TUTORIAL_ID_UPGRADE_HERO_SKILL;

		protected override void SaveTutorialPassed()
		{
			UnityEngine.Debug.Log("Upgrade skill level cho hero, hoàn thành tut!");
			ReadWriteDataTutorial.Instance.SetTutorialStatus(TutorialID, value: true);
		}

		protected override bool ShouldShowTutorial()
		{
			if (ReadWriteDataHero.Instance.GetSkillPoint(2, 0) > 1)
			{
				SaveTutorialPassed();
			}
			return !ReadWriteDataTutorial.Instance.GetTutorialStatus(TutorialID) && ReadWriteDataHero.Instance.GetCurrentSkillPoint(2) >= 1 && ReadWriteDataHero.Instance.GetSkillPoint(2, 0) == 1 && ReadWriteDataMap.Instance.GetMapIDUnlocked() >= 2;
		}
	}
}

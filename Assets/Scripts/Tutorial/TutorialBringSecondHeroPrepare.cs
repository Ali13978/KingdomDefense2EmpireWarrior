using Data;
using UnityEngine;

namespace Tutorial
{
	public class TutorialBringSecondHeroPrepare : TutorialUnit
	{
		private string TutorialID = ReadWriteDataTutorial.TUTORIAL_ID_BRING_SECOND_HERO;

		protected override void SaveTutorialPassed()
		{
			UnityEngine.Debug.Log("Hoàn thành bước chọn Second Hero");
			ReadWriteDataTutorial.Instance.SetTutorialStatus(TutorialID, value: true);
		}

		protected override bool ShouldShowTutorial()
		{
			return !ReadWriteDataTutorial.Instance.GetTutorialStatus(TutorialID) && ReadWriteDataHero.Instance.IsHeroOwned(1);
		}

		public new void TryToSetTutorialPassed()
		{
			if (ShouldShowTutorial())
			{
				SetTutorialPassed();
			}
		}
	}
}

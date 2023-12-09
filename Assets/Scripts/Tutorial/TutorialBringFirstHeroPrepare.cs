using Data;
using UnityEngine;

namespace Tutorial
{
	public class TutorialBringFirstHeroPrepare : TutorialUnit
	{
		private string TutorialID = ReadWriteDataTutorial.TUTORIAL_ID_BRING_FIRST_HERO;

		protected override void SaveTutorialPassed()
		{
			UnityEngine.Debug.Log("Hoàn thành bước chọn First Hero!");
			ReadWriteDataTutorial.Instance.SetTutorialStatus(TutorialID, value: true);
		}

		protected override bool ShouldShowTutorial()
		{
			return !ReadWriteDataTutorial.Instance.GetTutorialStatus(TutorialID);
		}
	}
}

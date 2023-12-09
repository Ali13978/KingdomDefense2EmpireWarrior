using Data;
using Gameplay;
using UnityEngine;

namespace Tutorial
{
	public class TutorialGetFreeLuckyChestByVideo : TutorialUnit
	{
		private string TutorialID = ReadWriteDataTutorial.TUTORIAL_ID_GET_LUCKY_CHEST_BY_VIDEO;

		protected override void SaveTutorialPassed()
		{
			UnityEngine.Debug.Log("Click button watch video, hoàn thành Tut!");
			ReadWriteDataTutorial.Instance.SetTutorialStatus(TutorialID, value: true);
		}

		protected override bool ShouldShowTutorial()
		{
			return !ReadWriteDataTutorial.Instance.GetTutorialStatus(TutorialID) && SingletonMonoBehaviour<GameData>.Instance.MapID == 1;
		}
	}
}

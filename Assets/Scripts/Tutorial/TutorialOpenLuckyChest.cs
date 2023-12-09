using Data;
using Gameplay;
using UnityEngine;

namespace Tutorial
{
	public class TutorialOpenLuckyChest : TutorialUnit
	{
		private string TutorialID = ReadWriteDataTutorial.TUTORIAL_ID_LUCKY_CHEST;

		protected override void SaveTutorialPassed()
		{
			UnityEngine.Debug.Log("Click gọi quái, hoàn thành gọi quái!");
			ReadWriteDataTutorial.Instance.SetTutorialStatus(TutorialID, value: true);
		}

		public bool IsTutorialDone()
		{
			return ReadWriteDataTutorial.Instance.GetTutorialStatus(TutorialID);
		}

		protected override bool ShouldShowTutorial()
		{
			if (ReadWriteDataMap.Instance.GetMapIDUnlocked() >= 2)
			{
				SaveTutorialPassed();
			}
			return !ReadWriteDataTutorial.Instance.GetTutorialStatus(TutorialID) && SingletonMonoBehaviour<GameData>.Instance.MapID == 0;
		}
	}
}

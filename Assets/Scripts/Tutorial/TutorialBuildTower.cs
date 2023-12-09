using Data;
using Gameplay;
using UnityEngine;

namespace Tutorial
{
	public class TutorialBuildTower : TutorialUnit
	{
		private string TutorialID = ReadWriteDataTutorial.TUTORIAL_ID_BUILD_TOWER;

		private void Start()
		{
			CheckCondition();
		}

		public void InvokeBuildRegionClick(int regionID)
		{
			SingletonMonoBehaviour<BuildRegionManager>.Instance.InvokeClickk(regionID);
		}

		protected override void SaveTutorialPassed()
		{
			UnityEngine.Debug.Log("Xây trụ thành công hoàn thành tut!");
			ReadWriteDataTutorial.Instance.SetTutorialStatus(TutorialID, value: true);
		}

		protected override bool ShouldShowTutorial()
		{
			return !ReadWriteDataTutorial.Instance.GetTutorialStatus(TutorialID) && SingletonMonoBehaviour<GameData>.Instance.MapID == 0;
		}
	}
}

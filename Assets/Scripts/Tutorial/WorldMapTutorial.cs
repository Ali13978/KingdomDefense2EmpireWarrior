using Data;
using UnityEngine;

namespace Tutorial
{
	public class WorldMapTutorial : TutorialUnit
	{
		private string TutorialID = ReadWriteDataTutorial.TUTORIAL_ID_WORLD_MAP;

		protected override void SaveTutorialPassed()
		{
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
				result = (!ReadWriteDataTutorial.Instance.GetTutorialStatus(TutorialID) && ReadWriteDataMap.Instance.GetMapIDUnlocked() >= 1);
			}
			return result;
		}

		public void TryInvokeTutorialSelectSecondMap()
		{
			GameObject gameObject = GameObject.Find("Map_1");
			if ((bool)gameObject)
			{
				TutorialSelectSecondMap component = gameObject.GetComponent<TutorialSelectSecondMap>();
				if ((bool)component)
				{
					component.CheckCondition();
				}
			}
		}

		public bool IsShowingTutorial()
		{
			return ShouldShowTutorial();
		}
	}
}

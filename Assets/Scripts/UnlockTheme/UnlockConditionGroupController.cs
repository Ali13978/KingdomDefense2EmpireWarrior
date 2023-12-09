using UnityEngine;

namespace UnlockTheme
{
	public class UnlockConditionGroupController : MonoBehaviour
	{
		[SerializeField]
		private UnlockConditionContent[] listUnlockConditionContents;

		public void InitConditionContent(int index, int conditionType, int themeID, bool isPassedCondition)
		{
			listUnlockConditionContents[index].InitContent(conditionType, themeID, isPassedCondition);
		}

		public void HideAll()
		{
			UnlockConditionContent[] array = listUnlockConditionContents;
			foreach (UnlockConditionContent unlockConditionContent in array)
			{
				unlockConditionContent.Hide();
			}
		}
	}
}

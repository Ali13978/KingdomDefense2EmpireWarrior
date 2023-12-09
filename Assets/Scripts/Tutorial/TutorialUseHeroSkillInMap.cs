using UnityEngine;

namespace Tutorial
{
	public class TutorialUseHeroSkillInMap : MonoBehaviour
	{
		[SerializeField]
		private GameObject tutorialContent;

		public void Show()
		{
			tutorialContent.SetActive(value: true);
		}

		public void Hide()
		{
			tutorialContent.SetActive(value: false);
		}
	}
}

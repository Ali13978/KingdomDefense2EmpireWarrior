using UnityEngine;

namespace Tutorial
{
	public class TutorialMoveHeroInMap : MonoBehaviour
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

using MyCustom;
using Parameter;
using UnityEngine;
using UnityEngine.UI;

namespace Tutorial
{
	public class TutorialDescriptionSetter : CustomMonoBehaviour
	{
		[SerializeField]
		private string tutorialID;

		[SerializeField]
		private Text tutorialDescription;

		[SerializeField]
		private TextMesh textMesh;

		private void Start()
		{
			SetTutorialDescription();
		}

		private void SetTutorialDescription()
		{
			if ((bool)tutorialDescription)
			{
				tutorialDescription.text = Singleton<TutorialDescription>.Instance.GetDescription(tutorialID).Replace('@', '\n').Replace('#', '-');
			}
			if ((bool)textMesh)
			{
				textMesh.text = Singleton<TutorialDescription>.Instance.GetDescription(tutorialID).Replace('@', '\n').Replace('#', '-');
			}
		}
	}
}

using Middle;
using UnityEngine;

namespace MainMenu
{
	public class LanguageChooseButtonController : ButtonController
	{
		[SerializeField]
		private string languageID;

		[SerializeField]
		private GameObject selectedImage;

		private void OnEnable()
		{
			if (Config.Instance.LanguageID == languageID)
			{
				selectedImage.SetActive(value: true);
			}
			else
			{
				selectedImage.SetActive(value: false);
			}
		}

		public override void OnClick()
		{
			base.OnClick();
			Config.Instance.LanguageID = languageID;
			SingletonMonoBehaviour<UIRootController>.Instance.languageChoosePanelController.Close();
			SingletonMonoBehaviour<UIRootController>.Instance.LanguageButtonController.UpdateButtonImage();
			CommonData.Instance.multiLanguaguageDataReader.ReloadParameters();
		}
	}
}

using Middle;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenu
{
	public class LanguageButtonController : ButtonController
	{
		private Image imageButton;

		private void Awake()
		{
			imageButton = GetComponent<Image>();
		}

		private void Start()
		{
			UpdateButtonImage();
		}

		public void UpdateButtonImage()
		{
			imageButton.sprite = Resources.Load<Sprite>($"CountryFlags/flag_{Config.Instance.LanguageID}");
		}

		public override void OnClick()
		{
			base.OnClick();
			SingletonMonoBehaviour<UIRootController>.Instance.languageChoosePanelController.Init();
		}
	}
}

using Data;
using UnityEngine;
using UnityEngine.UI;

namespace WorldMap
{
	public class SelectThemeButtonController : ButtonController
	{
		[SerializeField]
		private SelectThemeButtonGroupController selectThemeButtonGroupController;

		[SerializeField]
		private int themeID;

		private Button button;

		private void Awake()
		{
			GetAllComponents();
		}

		private void GetAllComponents()
		{
			button = GetComponent<Button>();
		}

		public void ViewUnlock()
		{
			button.interactable = true;
		}

		public void ViewLock()
		{
			button.interactable = false;
		}

		public override void OnClick()
		{
			base.OnClick();
			SelectTheme();
		}

		private void SelectTheme()
		{
			ReadWriteDataTheme.Instance.SaveLastThemePlayed(themeID);
			SingletonMonoBehaviour<UIRootController>.Instance.MapThemesController.SpawnTheme(themeID);
			selectThemeButtonGroupController.SetSelectedImage();
		}
	}
}

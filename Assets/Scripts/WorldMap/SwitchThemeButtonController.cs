using Data;
using UnityEngine;

namespace WorldMap
{
	public class SwitchThemeButtonController : ButtonController
	{
		[SerializeField]
		private SelectThemeButtonGroupController selectThemeButtonGroupController;

		[SerializeField]
		private bool isNext;

		[SerializeField]
		private bool isPrevious;

		private int currentThemeSelected;

		private int targetThemeSelected;

		private int maxThemeIDUnlocked;

		public override void OnClick()
		{
			base.OnClick();
			currentThemeSelected = ReadWriteDataTheme.Instance.GetLastThemeIDPlayed();
			maxThemeIDUnlocked = ReadWriteDataTheme.Instance.GetThemeIDUnlocked();
			if (isNext)
			{
				currentThemeSelected++;
			}
			if (isPrevious)
			{
				currentThemeSelected--;
			}
			currentThemeSelected = Mathf.Clamp(currentThemeSelected, 0, maxThemeIDUnlocked);
			ReadWriteDataTheme.Instance.SaveLastThemePlayed(currentThemeSelected);
			SingletonMonoBehaviour<UIRootController>.Instance.MapThemesController.SpawnTheme(currentThemeSelected);
			selectThemeButtonGroupController.SetSelectedImage();
		}
	}
}

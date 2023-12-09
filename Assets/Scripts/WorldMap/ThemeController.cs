using Data;
using UnityEngine;

namespace WorldMap
{
	public class ThemeController : MonoBehaviour
	{
		[SerializeField]
		private MapButtonsController mapButtonsController;

		[Space]
		[SerializeField]
		private ThemeStatueController themeStatueController;

		[Space]
		[SerializeField]
		private UnlockThemeButtonController unlockThemeButtonController;

		[Space]
		[SerializeField]
		private int themeID;

		public void Init()
		{
			Show();
			RefreshUnlockThemeButton();
		}

		public void RefreshUnlockThemeButton()
		{
			bool flag = ReadWriteDataTheme.Instance.IsNextThemeUnlock(themeID);
			bool flag2 = ReadWriteDataTheme.Instance.IsReachMaxTheme(themeID);
			if (flag)
			{
				unlockThemeButtonController.Hide();
			}
			else
			{
				unlockThemeButtonController.Show();
			}
			if (flag2)
			{
				unlockThemeButtonController.Hide();
			}
		}

		private void RefreshStatue()
		{
			themeStatueController.InitStatue();
		}

		public void RefreshMapButtons(int themeID)
		{
			mapButtonsController.RefreshListMapButtons(themeID);
		}

		private void Show()
		{
			base.gameObject.SetActive(value: true);
		}

		public void Hide()
		{
			base.gameObject.SetActive(value: false);
		}
	}
}

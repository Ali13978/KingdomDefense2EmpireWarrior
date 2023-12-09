using Data;
using System.Collections.Generic;
using UnityEngine;

namespace WorldMap
{
	public class SelectThemeButtonGroupController : MonoBehaviour
	{
		[SerializeField]
		private GameObject selectedImage;

		[Space]
		[SerializeField]
		private List<SelectThemeButtonController> listSelectThemeButtons = new List<SelectThemeButtonController>();

		public void RefreshListThemesButton()
		{
			int themeIDUnlocked = ReadWriteDataTheme.Instance.GetThemeIDUnlocked();
			UnlockToTheme(themeIDUnlocked);
		}

		public void SetSelectedImage()
		{
			Invoke("DoSelect", 0.2f);
		}

		private void DoSelect()
		{
			int lastThemeIDPlayed = ReadWriteDataTheme.Instance.GetLastThemeIDPlayed();
			selectedImage.transform.position = listSelectThemeButtons[lastThemeIDPlayed].transform.position;
		}

		private void UnlockToTheme(int currentThemeIDUnlocked)
		{
			for (int i = 0; i < listSelectThemeButtons.Count; i++)
			{
				if (i <= currentThemeIDUnlocked)
				{
					listSelectThemeButtons[i].ViewUnlock();
				}
				else
				{
					listSelectThemeButtons[i].ViewLock();
				}
			}
		}
	}
}

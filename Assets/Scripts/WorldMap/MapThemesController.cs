using Data;
using UnityEngine;

namespace WorldMap
{
	public class MapThemesController : MonoBehaviour
	{
		[Space]
		[Header("Select Themes Button Group")]
		[SerializeField]
		private SelectThemeButtonGroupController selectThemeButtonGroupController;

		[Space]
		[Header("Prefab Themes")]
		[SerializeField]
		private GameObject prefab_theme0;

		[SerializeField]
		private GameObject prefab_theme1;

		[SerializeField]
		private GameObject prefab_theme2;

		[Space]
		[SerializeField]
		private Transform themeHolder;

		private bool theme0Exist;

		private ThemeController _theme0Controller;

		private bool theme1Exist;

		private ThemeController _theme1Controller;

		private bool theme2Exist;

		private ThemeController _theme2Controller;

		public ThemeController theme0Controller
		{
			get
			{
				if (_theme0Controller == null)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate(prefab_theme0);
					gameObject.transform.SetParent(themeHolder);
					gameObject.transform.localPosition = Vector3.zero;
					gameObject.transform.localScale = Vector3.one;
					_theme0Controller = gameObject.GetComponent<ThemeController>();
					theme0Exist = true;
				}
				return _theme0Controller;
			}
			set
			{
				_theme0Controller = value;
			}
		}

		public ThemeController theme1Controller
		{
			get
			{
				if (_theme1Controller == null)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate(prefab_theme1);
					gameObject.transform.SetParent(themeHolder);
					gameObject.transform.localPosition = Vector3.zero;
					gameObject.transform.localScale = Vector3.one;
					_theme1Controller = gameObject.GetComponent<ThemeController>();
					theme1Exist = true;
				}
				return _theme1Controller;
			}
			set
			{
				_theme1Controller = value;
			}
		}

		public ThemeController theme2Controller
		{
			get
			{
				if (_theme2Controller == null)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate(prefab_theme2);
					gameObject.transform.SetParent(themeHolder);
					gameObject.transform.localPosition = Vector3.zero;
					gameObject.transform.localScale = Vector3.one;
					_theme2Controller = gameObject.GetComponent<ThemeController>();
					theme2Exist = true;
				}
				return _theme2Controller;
			}
			set
			{
				_theme2Controller = value;
			}
		}

		private void Start()
		{
			SpawnDefaultTheme();
			RefreshListMapButtons();
		}

		private void SpawnDefaultTheme()
		{
			int lastThemeIDPlayed = ReadWriteDataTheme.Instance.GetLastThemeIDPlayed();
			SpawnTheme(lastThemeIDPlayed);
			RefreshSelectThemesButtonStatus();
		}

		public void SpawnTheme(int themeID)
		{
			HideAll();
			switch (themeID)
			{
			case 0:
			{
				ThemeController theme0Controller = this.theme0Controller;
				theme0Controller.Init();
				break;
			}
			case 1:
			{
				ThemeController theme1Controller = this.theme1Controller;
				theme1Controller.Init();
				break;
			}
			case 2:
			{
				ThemeController theme2Controller = this.theme2Controller;
				theme2Controller.Init();
				break;
			}
			}
			RefreshListMapButtons();
		}

		private void HideAll()
		{
			if (theme0Exist && (bool)theme0Controller)
			{
				theme0Controller.Hide();
			}
			if (theme1Exist && (bool)theme1Controller)
			{
				theme1Controller.Hide();
			}
			if (theme2Exist && (bool)theme2Controller)
			{
				theme2Controller.Hide();
			}
		}

		public void RefreshUnlockThemesStatus()
		{
			if (theme0Exist && (bool)theme0Controller)
			{
				theme0Controller.RefreshUnlockThemeButton();
			}
			if (theme1Exist && (bool)theme1Controller)
			{
				theme1Controller.RefreshUnlockThemeButton();
			}
			if (theme2Exist && (bool)theme2Controller)
			{
				theme2Controller.RefreshUnlockThemeButton();
			}
		}

		public void RefreshSelectThemesButtonStatus()
		{
			selectThemeButtonGroupController.RefreshListThemesButton();
			selectThemeButtonGroupController.SetSelectedImage();
		}

		private void RefreshListMapButtons()
		{
			if (theme0Exist && (bool)theme0Controller)
			{
				theme0Controller.RefreshMapButtons(0);
			}
			if (theme1Exist && (bool)theme1Controller)
			{
				theme1Controller.RefreshMapButtons(1);
			}
			if (theme2Exist && (bool)theme2Controller)
			{
				theme2Controller.RefreshMapButtons(2);
			}
		}
	}
}

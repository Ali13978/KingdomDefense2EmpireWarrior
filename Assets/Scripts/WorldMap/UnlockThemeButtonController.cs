using UnityEngine;

namespace WorldMap
{
	public class UnlockThemeButtonController : ButtonController
	{
		[SerializeField]
		private int themeIDToUnlock;

		public override void OnClick()
		{
			base.OnClick();
			InitUnlockThemePanel();
		}

		private void InitUnlockThemePanel()
		{
			SingletonMonoBehaviour<UIRootController>.Instance.unlockThemePopupController.Init(themeIDToUnlock);
		}

		public void Show()
		{
			base.gameObject.SetActive(value: true);
		}

		public void Hide()
		{
			base.gameObject.SetActive(value: false);
		}
	}
}

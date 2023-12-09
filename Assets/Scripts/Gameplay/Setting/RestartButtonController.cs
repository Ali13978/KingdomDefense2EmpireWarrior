using UnityEngine;
using UnityEngine.UI;

namespace Gameplay.Setting
{
	public class RestartButtonController : ButtonController
	{
		[SerializeField]
		private Button button;

		[SerializeField]
		private Image image;

		public override void OnClick()
		{
			base.OnClick();
			SingletonMonoBehaviour<UIRootController>.Instance.settingPopupController.InitConfirmGroup(CancelGameType.Restart);
		}

		public void SetClickable()
		{
			button.enabled = true;
			image.color = Color.white;
		}

		public void SetUnClickable()
		{
			button.enabled = false;
			image.color = Color.gray;
		}
	}
}

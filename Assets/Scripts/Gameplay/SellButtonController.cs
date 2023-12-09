using UnityEngine;

namespace Gameplay
{
	public class SellButtonController : ControllTowerButtonController
	{
		public override void OnClick()
		{
			base.OnClick();
			if (buttonStatus == ButtonStatus.Available)
			{
				OnClickAvailable();
			}
			else if (buttonStatus == ButtonStatus.Confirm)
			{
				OnConfirm();
			}
		}

		protected override void OnConfirm()
		{
			base.OnConfirm();
			SingletonMonoBehaviour<UIRootController>.Instance.UpgradeTowerPopupController.OnSell();
			UISoundManager.Instance.PlayClick();
			UnityEngine.Debug.Log("Confirm sell!");
		}
	}
}

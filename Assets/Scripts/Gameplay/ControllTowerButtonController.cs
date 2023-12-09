using UnityEngine;

namespace Gameplay
{
	public class ControllTowerButtonController : GameplayButtonController
	{
		[SerializeField]
		protected GameObject confirmImage;

		protected ButtonStatus buttonStatus;

		public virtual void Init(bool _isAllowedToUse, Sprite spriteNormal, Sprite lockImage)
		{
		}

		public override void OnClick()
		{
			base.OnClick();
			GameEventCenter.Instance.Trigger(GameEventType.OnClickButton, new ClickedObjectData(ClickedObjectType.TowerControlBtn));
		}

		protected virtual void OnClickAvailable()
		{
			buttonStatus = ButtonStatus.Confirm;
			confirmImage.SetActive(value: true);
			SingletonMonoBehaviour<UIRootController>.Instance.UpgradeTowerPopupController.groupControllTowerButtons.DisableConfirmOtherButtons(this);
			SingletonMonoBehaviour<UIRootController>.Instance.BuyTowerPopupController.groupControllTowerButtons.DisableConfirmOtherButtons(this);
		}

		protected virtual void OnConfirm()
		{
			confirmImage.SetActive(value: false);
			SingletonMonoBehaviour<UIRootController>.Instance.UpgradeTowerPopupController.Close();
		}

		public virtual void DisableConfirm()
		{
			if (buttonStatus == ButtonStatus.Confirm)
			{
				buttonStatus = ButtonStatus.Available;
				confirmImage.SetActive(value: false);
			}
		}

		public virtual void UpdateBuyState()
		{
		}
	}
}

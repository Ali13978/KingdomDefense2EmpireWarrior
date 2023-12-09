using UnityEngine;

namespace Gameplay
{
	public class ControlWaveButtonController : GameplayButtonController
	{
		[SerializeField]
		protected GameObject confirmImage;

		protected ButtonStatus buttonStatus;

		public virtual void Update()
		{
			if (!SingletonMonoBehaviour<GameData>.Instance.IsAnyTutorialPopupOpen)
			{
				UpdateHideIfClickedOutside();
			}
		}

		private void UpdateHideIfClickedOutside()
		{
			if (Input.GetMouseButtonDown(0) && base.gameObject.activeSelf && !RectTransformUtility.RectangleContainsScreenPoint(base.gameObject.GetComponent<RectTransform>(), UnityEngine.Input.mousePosition, Camera.main))
			{
				OnClickOutsideDown();
			}
			if (Input.GetMouseButtonUp(0) && base.gameObject.activeSelf && !RectTransformUtility.RectangleContainsScreenPoint(base.gameObject.GetComponent<RectTransform>(), UnityEngine.Input.mousePosition, Camera.main))
			{
				OnClickOutsideUp();
			}
		}

		protected virtual void OnClickOutsideUp()
		{
		}

		protected virtual void OnClickOutsideDown()
		{
		}

		public virtual void Init(bool _isAllowedToUse, Sprite spriteNormal, Sprite lockImage)
		{
		}

		protected virtual void OnClickAvailable()
		{
			buttonStatus = ButtonStatus.Confirm;
			confirmImage.SetActive(value: true);
		}

		protected virtual void OnConfirm()
		{
			confirmImage.SetActive(value: false);
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

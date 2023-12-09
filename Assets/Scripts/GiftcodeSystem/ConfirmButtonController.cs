using UnityEngine;

namespace GiftcodeSystem
{
	public class ConfirmButtonController : ButtonController
	{
		[SerializeField]
		private GiftCodePopupController giftCodePopupController;

		public override void OnClick()
		{
			base.OnClick();
			Confirm();
		}

		private void Confirm()
		{
			giftCodePopupController.TryToSendGiftCode();
		}
	}
}

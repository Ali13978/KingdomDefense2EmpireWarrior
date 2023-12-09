using UnityEngine;

namespace GiftcodeSystem
{
	public class CancelButtonController : ButtonController
	{
		[SerializeField]
		private GiftCodePopupController giftCodePopupController;

		public override void OnClick()
		{
			base.OnClick();
			Cancel();
		}

		private void Cancel()
		{
			giftCodePopupController.CancelGiftCode();
		}
	}
}

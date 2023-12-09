using LifetimePopup;
using UnityEngine;
using UnityEngine.UI;

namespace UserProfile
{
	public class CopyButtonController : ButtonController
	{
		[SerializeField]
		private Text userID;

		public override void OnClick()
		{
			base.OnClick();
			CopyUserID();
		}

		private void CopyUserID()
		{
			if (string.IsNullOrEmpty(userID.text))
			{
				string localization = GameTools.GetLocalization("COPY_USERID_NOT_SUCCESS");
				SingletonMonoBehaviour<LifetimeCanvas>.Instance.ToastPopupController.Init(localization);
			}
			else
			{
				UniClipboard.SetText(userID.text);
				string localization2 = GameTools.GetLocalization("COPY_USERID_SUCCESS");
				SingletonMonoBehaviour<LifetimeCanvas>.Instance.ToastPopupController.Init(localization2);
			}
		}
	}
}

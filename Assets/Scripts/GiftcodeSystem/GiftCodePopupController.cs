using Gameplay;
using LifetimePopup;
using MyCustom;
using Parameter;
using UnityEngine;
using UnityEngine.UI;
using WorldMap;

namespace GiftcodeSystem
{
	public class GiftCodePopupController : GameplayPopupController
	{
		[SerializeField]
		private InputField inputField;

		public void Init()
		{
			OpenWithScaleAnimation();
		}

		public void TryToSendGiftCode()
		{
			if (StaticMethod.IsInternetConnectionAvailable())
			{
				if (inputField.text.Length == 0)
				{
					string notiContent = Singleton<NotificationDescription>.Instance.GetNotiContent(120);
					SingletonMonoBehaviour<LifetimeCanvas>.Instance.NotifyPopupController.Init(notiContent, isShowButtonFreeResources: false, isShowButtonGoToStore: false);
				}
				else
				{
					SendGiftCode();
					CloseWithScaleAnimation();
				}
			}
			else
			{
				string notiContent2 = Singleton<NotificationDescription>.Instance.GetNotiContent(119);
				SingletonMonoBehaviour<LifetimeCanvas>.Instance.NotifyPopupController.Init(notiContent2, isShowButtonFreeResources: false, isShowButtonGoToStore: false);
			}
		}

		private void SendGiftCode()
		{
			string deviceUniqueID = StaticMethod.GetDeviceUniqueID();
			string text = inputField.text;
			SingletonMonoBehaviour<WorldMapManager>.Instance.GiftCodeManager.SubmitGiftCode(text, deviceUniqueID);
		}

		public void CancelGiftCode()
		{
			CloseWithScaleAnimation();
		}

		public override void OpenWithScaleAnimation()
		{
			base.OpenWithScaleAnimation();
		}

		public override void CloseWithScaleAnimation()
		{
			base.CloseWithScaleAnimation();
			StaticMethod.ClearInputField(inputField);
		}
	}
}

using Data;
using Gameplay;
using LifetimePopup;
using MyCustom;
using Parameter;
using UnityEngine;
using UnityEngine.UI;

namespace UserProfile
{
	public class ChangeNamePopupController : GameplayPopupController
	{
		[SerializeField]
		private InputField inputField;

		[SerializeField]
		private ConfirmRenamePopupController confirmRenamePopupController;

		public void Init()
		{
			StaticMethod.ClearInputField(inputField);
			OpenWithScaleAnimation();
		}

		public void TryConfirmChangeName()
		{
			int renameCount = ReadWriteDataUserProfile.Instance.GetRenameCount();
			if (CheckIfInputFieldEmpty())
			{
				string notiContent = Singleton<NotificationDescription>.Instance.GetNotiContent(145);
				SingletonMonoBehaviour<LifetimeCanvas>.Instance.NotifyPopupController.Init(notiContent, isShowButtonFreeResources: false, isShowButtonGoToStore: false);
			}
			else if (renameCount == 0)
			{
				Rename();
			}
			else
			{
				confirmRenamePopupController.Init();
			}
		}

		public void Rename()
		{
			ReadWriteDataUserProfile.Instance.SetUserName(inputField.text);
			ReadWriteDataUserProfile.Instance.IncreaseRenameCount();
			CloseWithScaleAnimation();
		}

		private bool CheckIfInputFieldEmpty()
		{
			bool result = false;
			string text = inputField.text;
			if (text.Length == 0)
			{
				result = true;
			}
			return result;
		}
	}
}

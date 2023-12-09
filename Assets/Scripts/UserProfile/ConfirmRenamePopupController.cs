using Data;
using Gameplay;
using LifetimePopup;
using Parameter;
using UnityEngine;

namespace UserProfile
{
	public class ConfirmRenamePopupController : GameplayPopupController
	{
		[SerializeField]
		private ChangeNamePopupController changeNamePopupController;

		public void Init()
		{
			OpenWithScaleAnimation();
		}

		public void ConfirmRename()
		{
			int renameCost = ReadWriteDataUserProfile.Instance.GetRenameCost();
			int currentGem = ReadWriteDataPlayerCurrency.Instance.GetCurrentGem();
			if (currentGem >= renameCost)
			{
				changeNamePopupController.Rename();
				ReadWriteDataPlayerCurrency.Instance.ChangeGem(-renameCost, isDispatchEventChange: true);
				CloseWithScaleAnimation();
			}
			else
			{
				string notiContent = Singleton<NotificationDescription>.Instance.GetNotiContent(20);
				SingletonMonoBehaviour<LifetimeCanvas>.Instance.NotifyPopupController.Init(notiContent, isShowButtonFreeResources: true, isShowButtonGoToStore: true);
			}
		}
	}
}

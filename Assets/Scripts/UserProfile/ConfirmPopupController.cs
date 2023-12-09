using Gameplay;
using Parameter;
using UnityEngine;
using UnityEngine.UI;

namespace UserProfile
{
	public class ConfirmPopupController : GameplayPopupController
	{
		[SerializeField]
		private ConfirmButtonController confirmButtonController;

		[Space]
		[SerializeField]
		private Text noti;

		[SerializeField]
		private int notiID_backup;

		[SerializeField]
		private int notiID_restore;

		public void Init(CloudDataInteraction cloudDataInteraction)
		{
			OpenWithScaleAnimation();
			confirmButtonController.Init(cloudDataInteraction);
			switch (cloudDataInteraction)
			{
			case CloudDataInteraction.Backup:
			{
				string notiContent2 = Singleton<NotificationDescription>.Instance.GetNotiContent(notiID_backup);
				noti.text = notiContent2.Replace('@', '\n').Replace('#', '-');
				break;
			}
			case CloudDataInteraction.Restore:
			{
				string notiContent = Singleton<NotificationDescription>.Instance.GetNotiContent(notiID_restore);
				noti.text = notiContent.Replace('@', '\n').Replace('#', '-');
				break;
			}
			}
		}
	}
}

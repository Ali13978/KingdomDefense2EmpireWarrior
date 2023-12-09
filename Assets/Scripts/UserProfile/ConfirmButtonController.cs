using Data;
using Services.PlatformSpecific;
using WorldMap;

namespace UserProfile
{
	public class ConfirmButtonController : ButtonController
	{
		private CloudDataInteraction cloudDataInteraction;

		public void Init(CloudDataInteraction cloudDataInteraction)
		{
			this.cloudDataInteraction = cloudDataInteraction;
		}

		public override void OnClick()
		{
			base.OnClick();
			Confirm();
		}

		private void Confirm()
		{
			switch (cloudDataInteraction)
			{
			case CloudDataInteraction.Backup:
				PlatformSpecificServicesProvider.Services.UserProfile.BackupData();
				ReadWriteDataUserProfile.Instance.SaveLastTimeBackup();
				break;
			case CloudDataInteraction.Restore:
				PlatformSpecificServicesProvider.Services.UserProfile.RestoreData();
				break;
			}
			SingletonMonoBehaviour<UIRootController>.Instance.userProfilePopupController.ConfirmPopup.CloseWithScaleAnimation();
		}
	}
}

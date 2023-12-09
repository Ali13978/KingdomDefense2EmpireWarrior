using ApplicationEntry;
using Data;
using Services.PlatformSpecific;

namespace FreeResources
{
	public class JoinGroupButtonController : FreeResourcesButtonController
	{
		public override void InitData()
		{
			base.InitData();
			SetGemData();
			SetDisplayByRemoteSetting();
		}

		private void SetGemData()
		{
			if (oneTimeOnlyReward)
			{
				if (ReadWriteDataFreeResources.Instance.IsUserGetReward_JoinGroup())
				{
					titleReceived.SetActive(value: true);
					gemAmount.gameObject.SetActive(value: false);
				}
				else
				{
					titleReceived.SetActive(value: false);
					gemAmount.gameObject.SetActive(value: true);
				}
			}
			else
			{
				titleReceived.SetActive(value: false);
				gemAmount.gameObject.SetActive(value: true);
			}
			gemAmount.text = "+ " + PlatformSpecificServicesProvider.Services.FacebookServices.GetFreeResources("reward_id_join_group").ToString();
		}

		public override void OnClick()
		{
			base.OnClick();
			PlatformSpecificServicesProvider.Services.FacebookServices.InviteToGroup();
			InitData();
		}

		private void SetDisplayByRemoteSetting()
		{
			if (visualDependOnRemoteSetting)
			{
				if (ApplicationEntry.ApplicationEntry.Instance.ReadWriteRemoteSettingData.isDisplayFreeGem())
				{
					gemAmount.gameObject.SetActive(value: true);
					notification.gameObject.SetActive(value: true);
					icon.sprite = sprite_gem_chest;
					icon.SetNativeSize();
				}
				else
				{
					gemAmount.gameObject.SetActive(value: false);
					notification.gameObject.SetActive(value: false);
					icon.sprite = sprite_normal;
					titleReceived.SetActive(value: false);
				}
			}
		}
	}
}

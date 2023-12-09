using ApplicationEntry;
using Data;
using Services.PlatformSpecific;
using UnityEngine;
using UnityEngine.UI;

namespace FreeResources
{
	public class InviteFriendsButtonController : FreeResourcesButtonController
	{
		[SerializeField]
		private Text currentGemRewardProgress;

		private int currentGem;

		private int maxGem;

		public override void InitData()
		{
			base.InitData();
			SetGemData();
			SetCurrentGemRewardProgress();
			SetDisplayByRemoteSetting();
		}

		private void SetCurrentGemRewardProgress()
		{
			currentGem = ReadWriteDataFreeResources.Instance.GetCurrentGemCollectedByInvite();
			maxGem = 100;
			currentGemRewardProgress.text = currentGem + "/" + maxGem;
		}

		private void SetGemData()
		{
			if (!oneTimeOnlyReward)
			{
				titleReceived.SetActive(value: false);
			}
			gemAmount.text = "+ " + PlatformSpecificServicesProvider.Services.FacebookServices.GetFreeResources("reward_id_invite_friend").ToString();
		}

		public override void OnClick()
		{
			base.OnClick();
			if (currentGem < maxGem)
			{
				PlatformSpecificServicesProvider.Services.FacebookServices.InviteFriend();
				InitData();
			}
			else
			{
				UnityEngine.Debug.Log("Đã đạt max số lượng nhận thưởng invite friends!");
			}
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

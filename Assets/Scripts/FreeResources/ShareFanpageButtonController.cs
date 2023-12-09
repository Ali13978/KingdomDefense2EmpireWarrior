using ApplicationEntry;
using Data;
using Services.PlatformSpecific;
using UnityEngine;

namespace FreeResources
{
	public class ShareFanpageButtonController : FreeResourcesButtonController
	{
		private int currentShareAmount;

		public override void InitData()
		{
			base.InitData();
			SetGemData();
			SetShareCountData();
			SetDisplayByRemoteSetting();
		}

		private void SetGemData()
		{
			if (!oneTimeOnlyReward)
			{
				titleReceived.SetActive(value: false);
			}
			gemAmount.text = "+ " + PlatformSpecificServicesProvider.Services.FacebookServices.GetFreeResources("reward_id_share_fanpage").ToString();
		}

		private void SetShareCountData()
		{
			currentShareAmount = ReadWriteDataFreeResources.Instance.GetCurrentSharePerDay();
			if (currentShareAmount > 0)
			{
				titleReceived.SetActive(value: false);
				gemAmount.gameObject.SetActive(value: true);
				HideCountdownTime();
			}
			else
			{
				titleReceived.SetActive(value: true);
				gemAmount.gameObject.SetActive(value: false);
				DisplayCountdownTime();
			}
		}

		public override void OnClick()
		{
			base.OnClick();
			if (currentShareAmount > 0)
			{
				PlatformSpecificServicesProvider.Services.FacebookServices.ShareFanpage();
				currentShareAmount--;
				ReadWriteDataFreeResources.Instance.SetCurrentSharePerDay(currentShareAmount);
				if (currentShareAmount == 0)
				{
					DisplayCountdownTime();
				}
				InitData();
			}
			else
			{
				UnityEngine.Debug.Log("Da het luot share trong ngay!");
			}
		}

		private void DisplayCountdownTime()
		{
			timeCountDown.gameObject.SetActive(value: true);
		}

		private void HideCountdownTime()
		{
			timeCountDown.gameObject.SetActive(value: false);
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

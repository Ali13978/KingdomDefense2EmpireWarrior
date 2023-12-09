using ApplicationEntry;
using Data;
using Gameplay;
using LifetimePopup;
using Parameter;
using Services.PlatformSpecific;
using UnityEngine;

namespace FreeResources
{
	public class WatchAdButtonController : FreeResourcesButtonController
	{
		private int currentWatchAmount;

		public static bool changedVideoStatus;

		public override void InitData()
		{
			base.InitData();
			SetGemData();
			SetWatchCountData();
			SetDisplayByRemoteSetting();
		}

		private void SetGemData()
		{
			if (!oneTimeOnlyReward)
			{
				titleReceived.SetActive(value: false);
			}
			gemAmount.text = "+ " + PlatformSpecificServicesProvider.Services.FacebookServices.GetFreeResources("reward_id_watch_ad").ToString();
		}

		private void SetWatchCountData()
		{
			currentWatchAmount = ReadWriteDataFreeResources.Instance.GetCurrentWatchAdsPerDay();
			if (currentWatchAmount > 0)
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
			if (currentWatchAmount > 0)
			{
				if (VideoPlayerManager.Instance.CheckIfVideoExits())
				{
					VideoPlayerManager.Instance.playVideoGameplay_ForGem();
					return;
				}
				string notiContent = Singleton<NotificationDescription>.Instance.GetNotiContent(19);
				SingletonMonoBehaviour<LifetimeCanvas>.Instance.NotifyPopupController.Init(notiContent, isShowButtonFreeResources: false, isShowButtonGoToStore: false);
			}
			else
			{
				UnityEngine.Debug.Log("Da het luot xem video trong ngay!");
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

		private void Update()
		{
			if (!changedVideoStatus && SingletonMonoBehaviour<GameData>.Instance.PlayedVideoGems)
			{
				changedVideoStatus = true;
				currentWatchAmount--;
				ReadWriteDataFreeResources.Instance.SetCurrentWatchAdsPerDay(currentWatchAmount);
				if (currentWatchAmount == 0)
				{
					DisplayCountdownTime();
				}
				InitData();
			}
		}
	}
}

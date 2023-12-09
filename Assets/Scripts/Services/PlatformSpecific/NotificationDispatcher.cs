using System;
using Tournament;
using UnityEngine;

namespace Services.PlatformSpecific
{
	public class NotificationDispatcher : SingletonMonoBehaviour<NotificationDispatcher>
	{
		private void Start()
		{
			PlatformSpecificServicesProvider.Services.GameNotification.CancelAllNotify();
			SendNotify_ComeBackGame();
			SendNotify_Tournament();
		}

		private void SendNotify_ComeBackGame()
		{
			string key = "NOTIFY_COME_BACK_GAME";
			string localization = GameTools.GetLocalization(key);
			PlatformSpecificServicesProvider.Services.GameNotification.PushNotify(localization, 172800);
			PlatformSpecificServicesProvider.Services.GameNotification.PushNotify(localization, 259200);
			PlatformSpecificServicesProvider.Services.GameNotification.PushNotify(localization, 432000);
		}

		private void SendNotify_Tournament()
		{
			DateTime d = GameTools.ReadTimeStamp(TournamentPopupController.NEXT_FREE_TIME_KEY);
			TimeSpan timeSpan = d - TournamentPopupController.GetNow();
			if (timeSpan.TotalMinutes > 1.0)
			{
				string key = "NOTIFY_FREE_TOUR";
				string localization = GameTools.GetLocalization(key);
				PlatformSpecificServicesProvider.Services.GameNotification.PushNotify(localization, (int)timeSpan.TotalSeconds);
				UnityEngine.Debug.Log("noti next free tourplay in (hour)" + timeSpan.TotalHours);
			}
			DateTime d2 = GameTools.ReadTimeStamp(TournamentPopupController.END_SEASON_TIME_KEY).AddMinutes(15.0);
			timeSpan = d2 - TournamentPopupController.GetNow();
			if (timeSpan.TotalMinutes > 1.0)
			{
				string key2 = "NOTIFY_END_TOUR_SEASON";
				string localization2 = GameTools.GetLocalization(key2);
				PlatformSpecificServicesProvider.Services.GameNotification.PushNotify(localization2, (int)timeSpan.TotalSeconds);
				UnityEngine.Debug.Log("noti end season in (hour)" + timeSpan.TotalHours);
			}
		}
	}
}

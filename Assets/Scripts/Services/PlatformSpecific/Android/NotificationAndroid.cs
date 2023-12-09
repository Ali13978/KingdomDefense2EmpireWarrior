using Assets.SimpleAndroidNotifications;
using Middle;
using System;
using UnityEngine;

namespace Services.PlatformSpecific.Android
{
	public class NotificationAndroid : MonoBehaviour, INotification
	{
		private string topic = "KingdomDefense";

		public void FirebaseInit()
		{
		}

		private void OnDestroy()
		{
		}

		public void PushNotify(string content, int delayTimeBySecond)
		{
			if (Config.Instance.AllowPushNoti)
			{
				UnityEngine.Debug.Log("User allow push notify: Editor Notify with  content: " + content + " delay time: " + delayTimeBySecond);
				NotificationParams notificationParams = new NotificationParams();
				notificationParams.Id = NotificationIdHandler.GetNotificationId();
				notificationParams.Delay = TimeSpan.FromSeconds(delayTimeBySecond);
				notificationParams.Title = "Kingdom Defense";
				notificationParams.Message = content;
				notificationParams.Ticker = content;
				notificationParams.Sound = true;
				notificationParams.Vibrate = true;
				notificationParams.Vibration = new int[6]
				{
					500,
					500,
					500,
					500,
					500,
					500
				};
				notificationParams.Light = true;
				notificationParams.LightOnMs = 1000;
				notificationParams.LightOffMs = 1000;
				notificationParams.LightColor = Color.green;
				notificationParams.SmallIcon = NotificationIcon.Event;
				notificationParams.SmallIconColor = new Color(0f, 0.5f, 0f);
				notificationParams.LargeIcon = "anp_licon";
				notificationParams.ExecuteMode = NotificationExecuteMode.Inexact;
				notificationParams.CallbackData = "notification created at " + DateTime.Now;
				NotificationParams notificationParams2 = notificationParams;
				NotificationManager.SendCustom(notificationParams2);
			}
			else
			{
				UnityEngine.Debug.Log("User not allow push notify");
			}
		}

		public void CancelAllNotify()
		{
			UnityEngine.Debug.Log("Cancel all notifications");
			NotificationManager.CancelAll();
		}
	}
}

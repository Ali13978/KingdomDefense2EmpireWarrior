using System;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.SimpleAndroidNotifications
{
	public class NotificationExample : MonoBehaviour
	{
		public Toggle Toggle;

		public void Awake()
		{
			Toggle.isOn = (NotificationManager.GetNotificationCallback() != null);
		}

		public void OnApplicationPause(bool pause)
		{
			if (!pause)
			{
				Toggle.isOn = (NotificationManager.GetNotificationCallback() != null);
			}
		}

		public void Rate()
		{
			Application.OpenURL("http://u3d.as/A6d");
		}

		public void OpenWiki()
		{
			Application.OpenURL("https://github.com/hippogamesunity/SimpleAndroidNotificationsPublic/wiki");
		}

		public void CancelAll()
		{
			NotificationManager.CancelAll();
		}

		public void ScheduleSimple(int seconds)
		{
			NotificationManager.Send(TimeSpan.FromSeconds(seconds), "Simple notification", "Please rate the asset on the Asset Store!", new Color(1f, 0.3f, 0.15f));
		}

		public void ScheduleNormal()
		{
			NotificationManager.SendWithAppIcon(TimeSpan.FromSeconds(5.0), "Notification", "Notification with app icon", new Color(0f, 0.6f, 1f), NotificationIcon.Message);
		}

		public void ScheduleRepeated()
		{
			NotificationParams notificationParams = new NotificationParams();
			notificationParams.Id = NotificationIdHandler.GetNotificationId();
			notificationParams.Delay = TimeSpan.FromSeconds(5.0);
			notificationParams.Title = "Repeated notification";
			notificationParams.Message = "Please rate the asset on the Asset Store!";
			notificationParams.Ticker = "This is repeated message ticker!";
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
			notificationParams.LightColor = Color.magenta;
			notificationParams.SmallIcon = NotificationIcon.Skull;
			notificationParams.SmallIconColor = new Color(0f, 0.5f, 0f);
			notificationParams.LargeIcon = "app_icon";
			notificationParams.ExecuteMode = NotificationExecuteMode.Inexact;
			notificationParams.Repeat = true;
			notificationParams.RepeatInterval = TimeSpan.FromSeconds(30.0);
			NotificationParams notificationParams2 = notificationParams;
			NotificationManager.SendCustom(notificationParams2);
		}

		public void ScheduleMultiline()
		{
			NotificationParams notificationParams = new NotificationParams();
			notificationParams.Id = NotificationIdHandler.GetNotificationId();
			notificationParams.Delay = TimeSpan.FromSeconds(5.0);
			notificationParams.Title = "Multiline notification";
			notificationParams.Message = "Line#1\nLine#2\nLine#3\nLine#4";
			notificationParams.Ticker = "This is multiline message ticker!";
			notificationParams.Multiline = true;
			NotificationParams notificationParams2 = notificationParams;
			NotificationManager.SendCustom(notificationParams2);
		}

		public void ScheduleGrouped()
		{
			int notificationId = NotificationIdHandler.GetNotificationId();
			NotificationParams notificationParams = new NotificationParams();
			notificationParams.Id = notificationId;
			notificationParams.GroupName = "Group";
			notificationParams.GroupSummary = "{0} new messages";
			notificationParams.Delay = TimeSpan.FromSeconds(5.0);
			notificationParams.Title = "Grouped notification";
			notificationParams.Message = "Message " + notificationId;
			notificationParams.Ticker = "Please rate the asset on the Asset Store!";
			NotificationParams notificationParams2 = notificationParams;
			NotificationManager.SendCustom(notificationParams2);
		}

		public void ScheduleCustom()
		{
			NotificationParams notificationParams = new NotificationParams();
			notificationParams.Id = NotificationIdHandler.GetNotificationId();
			notificationParams.Delay = TimeSpan.FromSeconds(5.0);
			notificationParams.Title = "Notification with callback";
			notificationParams.Message = "Open app and check the checkbox!";
			notificationParams.Ticker = "Notification with callback";
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
			notificationParams.LightColor = Color.red;
			notificationParams.SmallIcon = NotificationIcon.Sync;
			notificationParams.SmallIconColor = new Color(0f, 0.5f, 0f);
			notificationParams.LargeIcon = "app_icon";
			notificationParams.ExecuteMode = NotificationExecuteMode.Inexact;
			notificationParams.CallbackData = "notification created at " + DateTime.Now;
			NotificationParams notificationParams2 = notificationParams;
			NotificationManager.SendCustom(notificationParams2);
		}

		public void ScheduleWithChannel()
		{
			NotificationParams notificationParams = new NotificationParams();
			notificationParams.Id = NotificationIdHandler.GetNotificationId();
			notificationParams.Delay = TimeSpan.FromSeconds(5.0);
			notificationParams.Title = "Notification with news channel";
			notificationParams.Message = "Check the channel in your app settings!";
			notificationParams.Ticker = "Notification with news channel";
			notificationParams.ChannelId = "com.company.app.news";
			notificationParams.ChannelName = "News";
			NotificationParams notificationParams2 = notificationParams;
			NotificationManager.SendCustom(notificationParams2);
		}
	}
}

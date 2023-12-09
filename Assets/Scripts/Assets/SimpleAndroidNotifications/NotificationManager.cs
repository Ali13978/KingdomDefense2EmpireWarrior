using System;
using System.Linq;
using UnityEngine;

namespace Assets.SimpleAndroidNotifications
{
	public static class NotificationManager
	{
		public const string FullClassName = "com.hippogames.simpleandroidnotifications.Controller";

		public const string MainActivityClassName = "com.unity3d.player.UnityPlayerActivity";

		public static int Send(TimeSpan delay, string title, string message, Color smallIconColor, NotificationIcon smallIcon = NotificationIcon.Bell, bool silent = false)
		{
			NotificationParams notificationParams = new NotificationParams();
			notificationParams.Id = NotificationIdHandler.GetNotificationId();
			notificationParams.Delay = delay;
			notificationParams.Title = title;
			notificationParams.Message = message;
			notificationParams.Ticker = message;
			notificationParams.Sound = !silent;
			notificationParams.Vibrate = !silent;
			notificationParams.Light = true;
			notificationParams.SmallIcon = smallIcon;
			notificationParams.SmallIconColor = smallIconColor;
			notificationParams.LargeIcon = string.Empty;
			notificationParams.ExecuteMode = NotificationExecuteMode.Inexact;
			return SendCustom(notificationParams);
		}

		public static int SendWithAppIcon(TimeSpan delay, string title, string message, Color smallIconColor, NotificationIcon smallIcon = NotificationIcon.Bell, bool silent = false)
		{
			NotificationParams notificationParams = new NotificationParams();
			notificationParams.Id = NotificationIdHandler.GetNotificationId();
			notificationParams.Delay = delay;
			notificationParams.Title = title;
			notificationParams.Message = message;
			notificationParams.Ticker = message;
			notificationParams.Sound = !silent;
			notificationParams.Vibrate = !silent;
			notificationParams.Light = true;
			notificationParams.SmallIcon = smallIcon;
			notificationParams.SmallIconColor = smallIconColor;
			notificationParams.LargeIcon = "app_icon";
			notificationParams.ExecuteMode = NotificationExecuteMode.Inexact;
			return SendCustom(notificationParams);
		}

		public static int SendCustom(NotificationParams notificationParams)
		{
			long num = (long)notificationParams.Delay.TotalMilliseconds;
			long num2 = (!notificationParams.Repeat) ? 0 : ((long)notificationParams.RepeatInterval.TotalMilliseconds);
			string text = string.Join(",", (from i in notificationParams.Vibration
				select i.ToString()).ToArray());
			new AndroidJavaClass("com.hippogames.simpleandroidnotifications.Controller").CallStatic("SetNotification", notificationParams.Id, notificationParams.GroupName ?? string.Empty, notificationParams.GroupSummary ?? string.Empty, notificationParams.ChannelId, notificationParams.ChannelName, num, Convert.ToInt32(notificationParams.Repeat), num2, notificationParams.Title, notificationParams.Message, notificationParams.Ticker, Convert.ToInt32(notificationParams.Multiline), Convert.ToInt32(notificationParams.Sound), notificationParams.CustomSound ?? string.Empty, Convert.ToInt32(notificationParams.Vibrate), text, Convert.ToInt32(notificationParams.Light), notificationParams.LightOnMs, notificationParams.LightOffMs, ColotToInt(notificationParams.LightColor), notificationParams.LargeIcon ?? string.Empty, GetSmallIconName(notificationParams.SmallIcon), ColotToInt(notificationParams.SmallIconColor), (int)notificationParams.ExecuteMode, notificationParams.CallbackData, "com.unity3d.player.UnityPlayerActivity");
			NotificationIdHandler.AddScheduledNotificaion(notificationParams.Id);
			return notificationParams.Id;
		}

		public static void Cancel(int id)
		{
			new AndroidJavaClass("com.hippogames.simpleandroidnotifications.Controller").CallStatic("CancelNotification", id);
			NotificationIdHandler.RemoveScheduledNotificaion(id);
		}

		public static void CancelAll()
		{
			new AndroidJavaClass("com.hippogames.simpleandroidnotifications.Controller").CallStatic("CancelAllNotifications");
			NotificationIdHandler.RemoveAllScheduledNotificaions();
		}

		public static void CancelAllDisplayed()
		{
			new AndroidJavaClass("com.hippogames.simpleandroidnotifications.Controller").CallStatic("CancelAllDisplayedNotifications");
			NotificationIdHandler.RemoveAllScheduledNotificaions();
		}

		public static NotificationCallback GetNotificationCallback()
		{
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
			AndroidJavaObject androidJavaObject = @static.Call<AndroidJavaObject>("getIntent", new object[0]);
			if (androidJavaObject.Call<bool>("hasExtra", new object[1]
			{
				"Notification.Id"
			}))
			{
				AndroidJavaObject androidJavaObject2 = androidJavaObject.Call<AndroidJavaObject>("getExtras", new object[0]);
				NotificationCallback notificationCallback = new NotificationCallback();
				notificationCallback.Id = androidJavaObject2.Call<int>("getInt", new object[1]
				{
					"Notification.Id"
				});
				notificationCallback.Data = androidJavaObject2.Call<string>("getString", new object[1]
				{
					"Notification.CallbackData"
				});
				return notificationCallback;
			}
			return null;
		}

		private static int ColotToInt(Color color)
		{
			Color32 color2 = color;
			return color2.r * 65536 + color2.g * 256 + color2.b;
		}

		private static string GetSmallIconName(NotificationIcon icon)
		{
			return "anp_" + icon.ToString().ToLower();
		}
	}
}

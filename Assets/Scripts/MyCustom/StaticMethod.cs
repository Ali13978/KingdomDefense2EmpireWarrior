using System;
using UnityEngine;
using UnityEngine.UI;

namespace MyCustom
{
	public static class StaticMethod
	{
		public static bool IsInternetConnectionAvailable()
		{
			if (Application.internetReachability == NetworkReachability.NotReachable)
			{
				return false;
			}
			return true;
		}

		public static bool CheckIfDayPassed()
		{
			bool result = false;
			string text = GameTools.GetNow().ToString();
			DateTime dateTime = Convert.ToDateTime(PlayerPrefs.GetString("savedDateTime", text));
			PlayerPrefs.SetString("savedDateTime", text);
			if (DateTime.Today.Year > dateTime.Year)
			{
				result = true;
			}
			else if (DateTime.Today.Month > dateTime.Month)
			{
				result = true;
			}
			else if (DateTime.Today.Day > dateTime.Day)
			{
				result = true;
			}
			return result;
		}

		public static TimeSpan getTimeSpanFromSecond(float value)
		{
			return TimeSpan.FromSeconds(value);
		}

		public static string GetFormatedTimeSpan(TimeSpan timeSpan)
		{
			string empty = string.Empty;
			return $"{timeSpan.Hours:00}:{timeSpan.Minutes:00}:{timeSpan.Seconds:00}";
		}

		public static bool CheckPackageAppIsPresent(string package)
		{
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
			AndroidJavaObject androidJavaObject = @static.Call<AndroidJavaObject>("getPackageManager", new object[0]);
			AndroidJavaObject androidJavaObject2 = androidJavaObject.Call<AndroidJavaObject>("getInstalledPackages", new object[1]
			{
				0
			});
			int num = androidJavaObject2.Call<int>("size", new object[0]);
			for (int i = 0; i < num; i++)
			{
				AndroidJavaObject androidJavaObject3 = androidJavaObject2.Call<AndroidJavaObject>("get", new object[1]
				{
					i
				});
				string text = androidJavaObject3.Get<string>("packageName");
				if (text.CompareTo(package) == 0)
				{
					return true;
				}
			}
			return false;
		}

		public static string GetHighLightTextByLevel(string text0, string text1, string text2, int level)
		{
			string result = string.Empty;
			switch (level)
			{
			case 0:
				result = "<color=lime>" + text0 + "</color>/" + text1 + "/" + text2;
				break;
			case 1:
				result = "<color=lime>" + text0 + "</color>/" + text1 + "/" + text2;
				break;
			case 2:
				result = text0 + "/<color=lime>" + text1 + "</color>/" + text2;
				break;
			case 3:
				result = text0 + "/" + text1 + "/<color=lime>" + text2 + "</color>";
				break;
			}
			return result;
		}

		public static string GetDeviceUniqueID()
		{
			return SystemInfo.deviceUniqueIdentifier;
		}

		public static void ClearInputField(InputField inputField)
		{
			inputField.Select();
			inputField.text = string.Empty;
		}
	}
}

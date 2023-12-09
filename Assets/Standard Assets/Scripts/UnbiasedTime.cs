using System;
using UnityEngine;

public class UnbiasedTime : MonoBehaviour
{
	private static UnbiasedTime instance;

	[HideInInspector]
	public long timeOffset;

	public static UnbiasedTime Instance
	{
		get
		{
			if (instance == null)
			{
				GameObject gameObject = new GameObject("UnbiasedTimeSingleton");
				instance = gameObject.AddComponent<UnbiasedTime>();
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
			}
			return instance;
		}
	}

	private void Awake()
	{
		SessionStart();
	}

	private void OnApplicationPause(bool pause)
	{
		if (pause)
		{
			SessionEnd();
		}
		else
		{
			SessionStart();
		}
	}

	private void OnApplicationQuit()
	{
		SessionEnd();
	}

	public DateTime Now()
	{
		return DateTime.Now.AddSeconds(-1f * (float)timeOffset);
	}

	public void UpdateTimeOffset()
	{
		UpdateTimeOffsetAndroid();
	}

	public bool IsUsingSystemTime()
	{
		return UsingSystemTimeAndroid();
	}

	private void SessionStart()
	{
		StartAndroid();
	}

	private void SessionEnd()
	{
		EndAndroid();
	}

	private void UpdateTimeOffsetAndroid()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("com.vasilij.unbiasedtime.UnbiasedTime"))
				{
					AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
					if (@static != null && androidJavaClass2 != null)
					{
						timeOffset = androidJavaClass2.CallStatic<long>("vtcTimestampOffset", new object[1]
						{
							@static
						});
					}
				}
			}
		}
	}

	private void StartAndroid()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("com.vasilij.unbiasedtime.UnbiasedTime"))
				{
					AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
					if (@static != null && androidJavaClass2 != null)
					{
						androidJavaClass2.CallStatic("vtcOnSessionStart", @static);
						timeOffset = androidJavaClass2.CallStatic<long>("vtcTimestampOffset", new object[0]);
					}
				}
			}
		}
	}

	private void EndAndroid()
	{
		if (Application.platform == RuntimePlatform.Android)
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
			{
				using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("com.vasilij.unbiasedtime.UnbiasedTime"))
				{
					AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
					if (@static != null)
					{
						androidJavaClass2?.CallStatic("vtcOnSessionEnd", @static);
					}
				}
			}
		}
	}

	private bool UsingSystemTimeAndroid()
	{
		if (Application.platform != RuntimePlatform.Android)
		{
			return true;
		}
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("com.vasilij.unbiasedtime.UnbiasedTime"))
			{
				AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
				if (@static != null && androidJavaClass2 != null)
				{
					return androidJavaClass2.CallStatic<bool>("vtcUsingDeviceTime", new object[0]);
				}
			}
		}
		return true;
	}
}

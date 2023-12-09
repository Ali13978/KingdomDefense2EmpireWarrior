using IronSourceJSON;
using System.Collections.Generic;
using UnityEngine;

public class IronSourceConfig
{
	private const string unsupportedPlatformStr = "Unsupported Platform";

	private static IronSourceConfig _instance;

	private static AndroidJavaObject _androidBridge;

	private static readonly string AndroidBridge = "com.ironsource.unity.androidbridge.AndroidBridge";

	public static IronSourceConfig Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new IronSourceConfig();
			}
			return _instance;
		}
	}

	public IronSourceConfig()
	{
		using (AndroidJavaClass androidJavaClass = new AndroidJavaClass(AndroidBridge))
		{
			_androidBridge = androidJavaClass.CallStatic<AndroidJavaObject>("getInstance", new object[0]);
		}
	}

	public void setLanguage(string language)
	{
		_androidBridge.Call("setLanguage", language);
	}

	public void setClientSideCallbacks(bool status)
	{
		_androidBridge.Call("setClientSideCallbacks", status);
	}

	public void setRewardedVideoCustomParams(Dictionary<string, string> rewardedVideoCustomParams)
	{
		string text = Json.Serialize(rewardedVideoCustomParams);
		_androidBridge.Call("setRewardedVideoCustomParams", text);
	}

	public void setOfferwallCustomParams(Dictionary<string, string> offerwallCustomParams)
	{
		string text = Json.Serialize(offerwallCustomParams);
		_androidBridge.Call("setOfferwallCustomParams", text);
	}
}

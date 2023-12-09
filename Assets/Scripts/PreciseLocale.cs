using UnityEngine;

public class PreciseLocale
{
	private interface PlatformBridge
	{
		string GetRegion();

		string GetLanguage();

		string GetLanguageID();

		string GetCurrencyCode();

		string GetCurrencySymbol();
	}

	private class EditorBridge : PlatformBridge
	{
		public string GetRegion()
		{
			return "US";
		}

		public string GetLanguage()
		{
			return "en";
		}

		public string GetLanguageID()
		{
			return "en_US";
		}

		public string GetCurrencyCode()
		{
			return "USD";
		}

		public string GetCurrencySymbol()
		{
			return "$";
		}
	}

	private class PreciseLocaleAndroid : PlatformBridge
	{
		private static AndroidJavaClass _preciseLocale = new AndroidJavaClass("com.kokosoft.preciselocale.PreciseLocale");

		public string GetRegion()
		{
			return _preciseLocale.CallStatic<string>("getRegion", new object[0]);
		}

		public string GetLanguage()
		{
			return _preciseLocale.CallStatic<string>("getLanguage", new object[0]);
		}

		public string GetLanguageID()
		{
			return _preciseLocale.CallStatic<string>("getLanguageID", new object[0]);
		}

		public string GetCurrencyCode()
		{
			return _preciseLocale.CallStatic<string>("getCurrencyCode", new object[0]);
		}

		public string GetCurrencySymbol()
		{
			return _preciseLocale.CallStatic<string>("getCurrencySymbol", new object[0]);
		}
	}

	private static PlatformBridge _platform;

	private static PlatformBridge platform
	{
		get
		{
			if (_platform == null)
			{
				_platform = new PreciseLocaleAndroid();
			}
			return _platform;
		}
	}

	public static string GetRegion()
	{
		return platform.GetRegion();
	}

	public static string GetLanguageID()
	{
		return platform.GetLanguageID();
	}

	public static string GetLanguage()
	{
		return platform.GetLanguage();
	}

	public static string GetCurrencyCode()
	{
		return platform.GetCurrencyCode();
	}

	public static string GetCurrencySymbol()
	{
		return platform.GetCurrencySymbol();
	}
}

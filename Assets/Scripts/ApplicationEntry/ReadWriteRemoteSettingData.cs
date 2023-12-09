using UnityEngine;

namespace ApplicationEntry
{
	public class ReadWriteRemoteSettingData : MonoBehaviour
	{
		private static string REMOTE_CONFIG_KEY_GEM = "free_gem_config";

		private static string CURRENT_REMOTE_SETTING_VALUE_GEM = "current_remote_setting_value";

		private int defaultRemoteSettingValue_gem;

		private int remoteSettingValue_gem;

		private static string REMOTE_VALUE_ADS_END_GAME = "remote_value_ads_endGame";

		private static string REMOTE_VALUE_ADS_LOADING = "remote_value_ads_loading";

		private int defaultRemoteSettingValue_ads;

		private int remoteSettingValue_ads;

		private static string REMOTE_VALUE_SHOW_ASK_RATING = "remote_chance_to_show_ask_rating";

		private int defaultRemoteSettingValue_askRating;

		private int remoteSettingValue_askRating;

		private static string REMOTE_VALUE_RATING_BEHAVIOR = "remote_rating_behavior";

		private string defaultRemoteValueRatingBehavior = RatingBehavior.thanknhide.ToString();

		private string remoteValueRatingBehavior = string.Empty;

		private static string REMOTE_VALUE_HERO_BUNDLE_OPTION = "remote_sale_hero_bundle_option";

		private int defaultRemoteSettingValue_heroBundleOption;

		private int remoteSettingValue_heroBundleOption;

		private string HOLIDAY_EVENT_ID_KEY = "holiday_event_id";

		private string HOLIDAY_EVENT_START_DAY = "holiday_event_start_day";

		private int defaultHolidayEventId = -1;

		private string defaultHolidayEventStartDay = "0";

		private int holidayEventId;

		private string holidayEventStartDay;

		private void Awake()
		{
			WriteDefaultRemoteSettingValue_Gem();
			WriteDefaultRemoteSettingValue_Ads();
			WriteDefaultRemoteSettingValue_AskRating();
			WriteDefaultRemoteSettingValue_RatingBehavior();
			WriteDefaultRemoteSettingValue_HeroBundleOption();
		}

		private void Start()
		{
			RemoteSettings.Updated += RemoteSettingHandleUpdate;
		}

		public void WriteDefaultRemoteSettingValue_Gem()
		{
			remoteSettingValue_gem = RemoteSettings.GetInt(REMOTE_CONFIG_KEY_GEM, defaultRemoteSettingValue_gem);
			if (UnityEngine.Random.Range(0, 100) < remoteSettingValue_gem)
			{
				PlayerPrefs.SetInt(CURRENT_REMOTE_SETTING_VALUE_GEM, 1);
			}
			else
			{
				PlayerPrefs.SetInt(CURRENT_REMOTE_SETTING_VALUE_GEM, 0);
			}
		}

		public bool isDisplayFreeGem()
		{
			return PlayerPrefs.GetInt(CURRENT_REMOTE_SETTING_VALUE_GEM) == 1;
		}

		private void RemoteSettingHandleUpdate()
		{
			WriteDefaultRemoteSettingValue_Gem();
			WriteDefaultRemoteSettingValue_Ads();
			WriteDefaultRemoteSettingValue_AskRating();
			WriteDefaultRemoteSettingValue_RatingBehavior();
			GetHolidayEvent();
			WriteDefaultRemoteSettingValue_HeroBundleOption();
		}

		public void WriteDefaultRemoteSettingValue_Ads()
		{
			remoteSettingValue_ads = RemoteSettings.GetInt(MarketingConfig.REMOTE_KEY_ADS_END_GAME, defaultRemoteSettingValue_ads);
			PlayerPrefs.SetInt(REMOTE_VALUE_ADS_END_GAME, remoteSettingValue_ads);
			remoteSettingValue_ads = RemoteSettings.GetInt(MarketingConfig.REMOTE_KEY_ADS_LOADING, defaultRemoteSettingValue_ads);
			PlayerPrefs.SetInt(REMOTE_VALUE_ADS_LOADING, remoteSettingValue_ads);
			UnityEngine.Debug.Log("Chance to show ad loading = " + PlayerPrefs.GetInt(REMOTE_VALUE_ADS_LOADING));
			UnityEngine.Debug.Log("Chance to show ad end game = " + PlayerPrefs.GetInt(REMOTE_VALUE_ADS_END_GAME));
		}

		public int ChanceToShowInterAds_EndGame()
		{
			return PlayerPrefs.GetInt(REMOTE_VALUE_ADS_END_GAME);
		}

		public int ChanceToShowInterAds_Loading()
		{
			return PlayerPrefs.GetInt(REMOTE_VALUE_ADS_LOADING);
		}

		private void WriteDefaultRemoteSettingValue_AskRating()
		{
			remoteSettingValue_askRating = RemoteSettings.GetInt(MarketingConfig.REMOTE_KEY_ASK_RATING, defaultRemoteSettingValue_askRating);
			PlayerPrefs.SetInt(REMOTE_VALUE_SHOW_ASK_RATING, remoteSettingValue_askRating);
			UnityEngine.Debug.Log("chance to show ask rating = " + PlayerPrefs.GetInt(REMOTE_VALUE_SHOW_ASK_RATING));
		}

		public int GetChanceToShowAskRating()
		{
			return PlayerPrefs.GetInt(REMOTE_VALUE_SHOW_ASK_RATING);
		}

		private void WriteDefaultRemoteSettingValue_RatingBehavior()
		{
			remoteValueRatingBehavior = RemoteSettings.GetString(MarketingConfig.REMOTE_KEY_RATING_BEHAVIOR, defaultRemoteValueRatingBehavior);
			PlayerPrefs.SetString(REMOTE_VALUE_RATING_BEHAVIOR, remoteValueRatingBehavior);
			UnityEngine.Debug.Log("rating behavior = " + PlayerPrefs.GetString(REMOTE_VALUE_RATING_BEHAVIOR));
		}

		public string GetRatingBehavior()
		{
			return PlayerPrefs.GetString(REMOTE_VALUE_RATING_BEHAVIOR);
		}

		private void GetHolidayEvent()
		{
			holidayEventId = RemoteSettings.GetInt(HOLIDAY_EVENT_ID_KEY, defaultHolidayEventId);
			PlayerPrefs.SetInt(HOLIDAY_EVENT_ID_KEY, holidayEventId);
			holidayEventStartDay = RemoteSettings.GetString(HOLIDAY_EVENT_START_DAY, defaultHolidayEventStartDay);
			PlayerPrefs.SetString(HOLIDAY_EVENT_START_DAY, holidayEventStartDay);
			UnityEngine.Debug.LogFormat("_____ set remote config holiday event {0} start day {1}", holidayEventId, holidayEventStartDay);
		}

		public int GetHolidayEventId()
		{
			return PlayerPrefs.GetInt(HOLIDAY_EVENT_ID_KEY, defaultHolidayEventId);
		}

		public long GetHolidayStartDay()
		{
			string @string = PlayerPrefs.GetString(HOLIDAY_EVENT_START_DAY, holidayEventStartDay);
			long result = 0L;
			long.TryParse(@string, out result);
			return result;
		}

		private void WriteDefaultRemoteSettingValue_HeroBundleOption()
		{
			remoteSettingValue_heroBundleOption = RemoteSettings.GetInt(MarketingConfig.REMOTE_KEY_HERO_BUNDLE_OPTION, defaultRemoteSettingValue_heroBundleOption);
			PlayerPrefs.SetInt(REMOTE_VALUE_HERO_BUNDLE_OPTION, remoteSettingValue_heroBundleOption);
			UnityEngine.Debug.Log("hero bundle option = " + PlayerPrefs.GetInt(REMOTE_VALUE_HERO_BUNDLE_OPTION));
		}

		public int GetHeroBundleOption()
		{
			return PlayerPrefs.GetInt(REMOTE_VALUE_HERO_BUNDLE_OPTION);
		}
	}
}

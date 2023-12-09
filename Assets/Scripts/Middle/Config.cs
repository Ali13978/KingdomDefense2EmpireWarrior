using Data;

namespace Middle
{
	public class Config
	{
		public const string LANGUAGE_KEY_ENGLISH = "lg_en";

		public const string LANGUAGE_KEY_VIETNAMESE = "lg_vi";

		public const string LANGUAGE_KEY_FRENCH = "lg_french";

		public const string LANGUAGE_KEY_SPANISH = "lg_spanish";

		public const string LANGUAGE_KEY_RUSSIAN = "lg_russian";

		public const string LANGUAGE_KEY_BRAZIL = "lg_brazil";

		public const string LANGUAGE_KEY_GERMAN = "lg_german";

		public const string LANGUAGE_KEY_POLISH = "lg_polish";

		public const string LANGUAGE_KEY_KOREAN = "lg_korean";

		public const string LANGUAGE_KEY_CHINESE = "lg_chinese";

		public const string LANGUAGE_KEY_JAPANESE = "lg_japanese";

		private bool isSoundOn;

		private bool isMusicOn;

		private bool isVibrationOn;

		private bool isAllowPushNoti;

		public int currentTowerRegionIDSelected = -1;

		private static Config instance;

		public int LineCount
		{
			get;
			set;
		}

		public int EarlyCallMoney
		{
			get;
			set;
		}

		public int LifePercent2Star
		{
			get;
			set;
		}

		public int LifePercent3Star
		{
			get;
			set;
		}

		public int FirstTimeGemTakenPercentage
		{
			get;
			set;
		}

		public int SecondTimeGemTakenPercentage
		{
			get;
			set;
		}

		public int ThirdTimeGemTakenPercentage
		{
			get;
			set;
		}

		public static Config Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new Config();
				}
				return instance;
			}
		}

		public bool Sound
		{
			get
			{
				return isSoundOn;
			}
			set
			{
				isSoundOn = value;
				ReadWriteData.Instance.WriteSound(value);
			}
		}

		public bool Music
		{
			get
			{
				return isMusicOn;
			}
			set
			{
				isMusicOn = value;
				ReadWriteData.Instance.WriteMusic(isMusicOn);
			}
		}

		public bool Vibration
		{
			get
			{
				return isVibrationOn;
			}
			set
			{
				isVibrationOn = value;
				ReadWriteData.Instance.WriteVibration(isVibrationOn);
			}
		}

		public bool AllowPushNoti
		{
			get
			{
				return isAllowPushNoti;
			}
			set
			{
				isAllowPushNoti = value;
				ReadWriteData.Instance.WriteNotification(isAllowPushNoti);
			}
		}

		public string LanguageID
		{
			get
			{
				return ReadWriteData.Instance.ReadLanguage();
			}
			set
			{
				ReadWriteData.Instance.WriteLanguage(value);
				GameTools.SetCurrentLanguage(value);
			}
		}

		public Config()
		{
			isSoundOn = ReadWriteData.Instance.ReadSound();
			isMusicOn = ReadWriteData.Instance.ReadMusic();
			isVibrationOn = ReadWriteData.Instance.ReadVibration();
			isAllowPushNoti = ReadWriteData.Instance.ReadNotification();
		}
	}
}

using System;
using UnityEngine;

namespace Data
{
	public class ReadWriteData
	{
		private string KEY_DEFAULT_DATA_0 = "setting";

		private string KEY_SOUND = "sound";

		private string KEY_MUSIC = "music";

		private string KEY_VIBRATE = "vibrate";

		private string KEY_NOTIFIATION = "notification";

		private string KEY_LANGUAGE = "language_";

		public const string KEY_SAVED_DATETIME = "savedDateTime";

		private string KEY_RATE = "rate_game";

		private string PLAY_COUNT = "play_count";

		private string TOURNAMENT_PLAY_COUNT = "tournament_play_count";

		private bool isFirstTimeSession = true;

		private static ReadWriteData instance;

		public static ReadWriteData Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new ReadWriteData();
				}
				return instance;
			}
		}

		public ReadWriteData()
		{
			isFirstTimeSession = true;
			IncreasePlayCount();
			UnityEngine.Debug.Log("current play count = " + GetPlayCount());
		}

		public void WriteDefaultSetting()
		{
			if (PlayerPrefs.GetInt(KEY_DEFAULT_DATA_0) == 0)
			{
				WriteSound(sound: true);
				WriteMusic(music: true);
				WriteVibration(vibration: true);
				WriteNotification(isAllowPushNoti: true);
				WriteDateTime();
				WriteDefaultLanguage();
				PlayerPrefs.SetInt(KEY_DEFAULT_DATA_0, 1);
			}
		}

		public void WriteDateTime()
		{
			PlayerPrefs.SetString("savedDateTime", DateTime.Today.ToString());
		}

		public void WriteSound(bool sound)
		{
			PlayerPrefs.SetInt(KEY_SOUND, sound ? 1 : 0);
		}

		public bool ReadSound()
		{
			if (PlayerPrefs.HasKey(KEY_SOUND))
			{
				int @int = PlayerPrefs.GetInt(KEY_SOUND);
				return (@int > 0) ? true : false;
			}
			return true;
		}

		public void WriteMusic(bool music)
		{
			PlayerPrefs.SetInt(KEY_MUSIC, music ? 1 : 0);
		}

		public bool ReadMusic()
		{
			if (PlayerPrefs.HasKey(KEY_MUSIC))
			{
				int @int = PlayerPrefs.GetInt(KEY_MUSIC);
				return (@int > 0) ? true : false;
			}
			return true;
		}

		public void WriteVibration(bool vibration)
		{
			PlayerPrefs.SetInt(KEY_VIBRATE, vibration ? 1 : 0);
		}

		public bool ReadVibration()
		{
			if (PlayerPrefs.HasKey(KEY_VIBRATE))
			{
				int @int = PlayerPrefs.GetInt(KEY_VIBRATE);
				return (@int > 0) ? true : false;
			}
			return true;
		}

		public void WriteNotification(bool isAllowPushNoti)
		{
			PlayerPrefs.SetInt(KEY_NOTIFIATION, isAllowPushNoti ? 1 : 0);
		}

		public bool ReadNotification()
		{
			if (PlayerPrefs.HasKey(KEY_NOTIFIATION))
			{
				int @int = PlayerPrefs.GetInt(KEY_NOTIFIATION);
				return (@int > 0) ? true : false;
			}
			return true;
		}

		private void WriteDefaultLanguage()
		{
			UnityEngine.Debug.Log("System language = " + Application.systemLanguage);
			switch (Application.systemLanguage)
			{
			case SystemLanguage.English:
				WriteLanguage("lg_en");
				break;
			case SystemLanguage.Vietnamese:
				WriteLanguage("lg_vi");
				break;
			case SystemLanguage.French:
				WriteLanguage("lg_french");
				break;
			case SystemLanguage.Spanish:
				WriteLanguage("lg_spanish");
				break;
			case SystemLanguage.Russian:
				WriteLanguage("lg_russian");
				break;
			case SystemLanguage.German:
				WriteLanguage("lg_german");
				break;
			case SystemLanguage.Portuguese:
				WriteLanguage("lg_brazil");
				break;
			case SystemLanguage.Polish:
				WriteLanguage("lg_polish");
				break;
			case SystemLanguage.Korean:
				WriteLanguage("lg_korean");
				break;
			case SystemLanguage.Chinese:
				WriteLanguage("lg_chinese");
				break;
			case SystemLanguage.Japanese:
				WriteLanguage("lg_japanese");
				break;
			default:
				WriteLanguage("lg_en");
				break;
			}
			CommonData.Instance.multiLanguaguageDataReader.ReloadParameters();
		}

		public void WriteLanguage(string value)
		{
			PlayerPrefs.SetString(KEY_LANGUAGE, value);
			PlayerPrefs.Save();
		}

		public string ReadLanguage()
		{
			string result = "lg_en";
			if (PlayerPrefs.HasKey(KEY_LANGUAGE))
			{
				result = PlayerPrefs.GetString(KEY_LANGUAGE);
			}
			return result;
		}

		private void OnApplicationQuit()
		{
			PlayerPrefs.Save();
		}

		public bool IsFirstTimeSession()
		{
			bool result = false;
			if (isFirstTimeSession)
			{
				result = true;
				isFirstTimeSession = false;
			}
			return result;
		}

		private void IncreasePlayCount()
		{
			int @int = PlayerPrefs.GetInt(PLAY_COUNT, 0);
			@int++;
			PlayerPrefs.SetInt(PLAY_COUNT, @int);
			PlayerPrefs.Save();
		}

		public int GetPlayCount()
		{
			return PlayerPrefs.GetInt(PLAY_COUNT, 0);
		}

		private void IncreaseTournamentPlayCount()
		{
			int @int = PlayerPrefs.GetInt(TOURNAMENT_PLAY_COUNT, 0);
			@int++;
			PlayerPrefs.SetInt(TOURNAMENT_PLAY_COUNT, @int);
			PlayerPrefs.Save();
		}

		public int GetTournamentPlayCount()
		{
			return PlayerPrefs.GetInt(TOURNAMENT_PLAY_COUNT, 0);
		}

		public void SetDataRated()
		{
			PlayerPrefs.SetInt(KEY_RATE, 1);
		}

		public bool IsUserRated()
		{
			return PlayerPrefs.GetInt(KEY_RATE, 0) == 1;
		}
	}
}

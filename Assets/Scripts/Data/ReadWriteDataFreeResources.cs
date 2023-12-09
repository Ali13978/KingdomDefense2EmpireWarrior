using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Data
{
	public class ReadWriteDataFreeResources : MonoBehaviour
	{
		public const string KEY_ONCE_LOGIN = "one_time_login";

		public const string KEY_ONCE_LIKE_FANPAGE = "one_time_like_fanpage";

		public const string KEY_ONCE_JOIN_GROUP = "one_time_join_group";

		public const string KEY_SHARE_FANPAGE = "key_share_friend";

		public const string KEY_WATCH_AD = "key_watch_ad";

		public const string KEY_CURRENT_GEM_INVITE_FRIEND = "key_current_gem_invite_friend";

		public const int MAX_SHARE_PER_DAY = 1;

		public const int MAX_WATCH_AD_PER_DAY = 1;

		public const int MAX_INVITE_GEM = 100;

		private static string DB_NAME = "/freeResourcesInfor.dat";

		private FreeResourcesData data = new FreeResourcesData();

		private static ReadWriteDataFreeResources instance;

		public static ReadWriteDataFreeResources Instance => instance;

		private void Awake()
		{
			instance = this;
			SaveDefaultData();
			Load();
		}

		private void SaveDefaultData()
		{
			if (!File.Exists(Application.persistentDataPath + DB_NAME))
			{
				FileStream fileStream = File.Create(Application.persistentDataPath + DB_NAME);
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				data.isUserGetRewardLoggedInFacebook = (PlayerPrefs.GetInt("one_time_login", 0) == 1);
				data.isUserGetRewardLikeFanpage = (PlayerPrefs.GetInt("one_time_like_fanpage", 0) == 1);
				data.isUserGetRewardJoinGroup = (PlayerPrefs.GetInt("one_time_join_group", 0) == 1);
				data.currentSharePerDay = PlayerPrefs.GetInt("key_share_friend", 1);
				data.currentWatchAdsPerDay = PlayerPrefs.GetInt("key_watch_ad", 1);
				data.currentGemCollectedByInvite = PlayerPrefs.GetInt("key_current_gem_invite_friend", 0);
				UnityEngine.Debug.Log(data);
				binaryFormatter.Serialize(fileStream, data);
				fileStream.Close();
			}
		}

		public void SaveAll()
		{
			FileStream fileStream = File.Create(Application.persistentDataPath + DB_NAME);
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			binaryFormatter.Serialize(fileStream, data);
			fileStream.Close();
		}

		private void Load()
		{
			FileStream fileStream = File.Open(Application.persistentDataPath + DB_NAME, FileMode.Open);
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			data = (FreeResourcesData)binaryFormatter.Deserialize(fileStream);
			fileStream.Close();
		}

		public void ResetFreeResourcesDailyData()
		{
			SetCurrentSharePerDay(1);
			SetCurrentWatchAdsPerDay(1);
			UnityEngine.Debug.Log("Reset daily data!");
		}

		public bool IsUserGetReward_LogInFacebook()
		{
			Load();
			return data.isUserGetRewardLoggedInFacebook;
		}

		public bool IsUserGetReward_LikeFanpage()
		{
			Load();
			return data.isUserGetRewardLikeFanpage;
		}

		public bool IsUserGetReward_JoinGroup()
		{
			Load();
			return data.isUserGetRewardJoinGroup;
		}

		public void SetOneTimeRewardStatus(string rewardID)
		{
			if (rewardID != null)
			{
				if (!(rewardID == "one_time_login"))
				{
					if (!(rewardID == "one_time_like_fanpage"))
					{
						if (rewardID == "one_time_join_group")
						{
							data.isUserGetRewardJoinGroup = true;
						}
					}
					else
					{
						data.isUserGetRewardLikeFanpage = true;
					}
				}
				else
				{
					data.isUserGetRewardLoggedInFacebook = true;
				}
			}
			SaveAll();
		}

		public int GetCurrentSharePerDay()
		{
			Load();
			return data.currentSharePerDay;
		}

		public void SetCurrentSharePerDay(int value)
		{
			data.currentSharePerDay = value;
			SaveAll();
		}

		public int GetCurrentWatchAdsPerDay()
		{
			Load();
			return data.currentWatchAdsPerDay;
		}

		public void SetCurrentWatchAdsPerDay(int value)
		{
			data.currentWatchAdsPerDay = value;
			SaveAll();
		}

		public int GetCurrentGemCollectedByInvite()
		{
			Load();
			return data.currentGemCollectedByInvite;
		}

		public void SetCurrentGemCollectedByInvite(int value)
		{
			data.currentGemCollectedByInvite = value;
			SaveAll();
		}

		public void RestoreDataFromCloud(UserData_FreeResources restoredData)
		{
			if (restoredData == null)
			{
				data.isUserGetRewardLoggedInFacebook = false;
				data.isUserGetRewardLikeFanpage = false;
				data.isUserGetRewardJoinGroup = false;
				data.currentSharePerDay = 0;
				data.currentWatchAdsPerDay = 0;
				data.currentGemCollectedByInvite = 0;
			}
			else
			{
				data.isUserGetRewardLoggedInFacebook = restoredData.isUserGetRewardLoggedInFacebook;
				data.isUserGetRewardLikeFanpage = restoredData.isUserGetRewardLikeFanpage;
				data.isUserGetRewardJoinGroup = restoredData.isUserGetRewardJoinGroup;
				data.currentSharePerDay = restoredData.currentSharePerDay;
				data.currentWatchAdsPerDay = restoredData.currentWatchAdsPerDay;
				data.currentGemCollectedByInvite = restoredData.currentGemCollectedByInvite;
			}
			UnityEngine.Debug.Log(data);
			SaveAll();
		}
	}
}

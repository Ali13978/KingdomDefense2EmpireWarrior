using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Data
{
	public class ReadWriteDataUserProfile : MonoBehaviour
	{
		private static string DB_NAME = "/userProfile.dat";

		private static string LEAGUE_PREFIX = "LEAGUE_";

		private UserProfileData data = new UserProfileData();

		private static ReadWriteDataUserProfile instance;

		public static ReadWriteDataUserProfile Instance => instance;

		public event Action OnUserInforChangeEvent;

		private void Awake()
		{
			instance = this;
			SaveDefaultData();
			Load();
		}

		private void Start()
		{
			UnityEngine.Debug.Log("+++subscribe league");
			GameEventCenter.Instance.Subscribe(GameEventType.OnTournamentTierUp, new SimpleSubscriberData(GameTools.GetUniqueId(), OnLeagueChange));
		}

		private void SaveDefaultData()
		{
			if (!File.Exists(Application.persistentDataPath + DB_NAME))
			{
				FileStream fileStream = File.Create(Application.persistentDataPath + DB_NAME);
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				data.userID = "empty";
				data.userName = "Nameless hero";
				data.renameCount = 0;
				data.renameItemQuantity = 1;
				data.userCountryCode = "gb";
				data.league = 0;
				data.lastTimeBackup = DateTime.Now.ToString();
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
			data = (UserProfileData)binaryFormatter.Deserialize(fileStream);
			fileStream.Close();
		}

		public void SetUserID(string value)
		{
			data.userID = value;
			SaveAll();
			if (this.OnUserInforChangeEvent != null)
			{
				this.OnUserInforChangeEvent();
			}
		}

		public void ClearUserID()
		{
			SetUserID(string.Empty);
		}

		public string GetUserID()
		{
			Load();
			return data.userID;
		}

		public void SaveLastTimeBackup()
		{
			data.lastTimeBackup = DateTime.Now.ToString();
			SaveAll();
		}

		public string GetLastTimeBackup()
		{
			Load();
			return data.lastTimeBackup;
		}

		private void OnLeagueChange()
		{
			int leagueValue = GetLeagueValue();
			int curtier = GameTools.tourUserSelfInfo.curtier;
			UnityEngine.Debug.LogFormat("_____read write data, curLeague from server {0}, league from user info {1}", leagueValue, curtier);
			if (curtier != leagueValue)
			{
				SetLeague(curtier);
			}
		}

		public void SetLeague(int value)
		{
			data.league = value;
			SaveAll();
		}

		public int GetLeagueValue()
		{
			Load();
			return data.league;
		}

		public string GetLeagueName()
		{
			Load();
			string empty = string.Empty;
			if (data.league < 0)
			{
				return string.Empty;
			}
			string key = LEAGUE_PREFIX + data.league;
			return GameTools.GetLocalization(key);
		}

		public void SetRegionCode(string value)
		{
			data.userCountryCode = value;
			SaveAll();
			if (this.OnUserInforChangeEvent != null)
			{
				this.OnUserInforChangeEvent();
			}
		}

		public string GetUserRegionCode()
		{
			Load();
			return data.userCountryCode;
		}

		public string GetUserName()
		{
			Load();
			UnityEngine.Debug.Log("Get user name " + data.userName);
			return data.userName;
		}

		public void SetUserName(string name)
		{
			data.userName = name;
			SaveAll();
			UnityEngine.Debug.Log("Set user name " + data.userName);
			if (this.OnUserInforChangeEvent != null)
			{
				this.OnUserInforChangeEvent();
			}
		}

		public int GetRenameCount()
		{
			Load();
			return data.renameCount;
		}

		public void IncreaseRenameCount()
		{
			Load();
			data.renameCount++;
			SaveAll();
		}

		public int GetRenameItemQuantity()
		{
			Load();
			return data.renameItemQuantity;
		}

		public void RenameItemQuantityChange(int value)
		{
			Load();
			data.renameItemQuantity += value;
			SaveAll();
		}

		public int GetRenameCost()
		{
			return 50;
		}

		public void RestoreDataFromCloud(UserData_UserProfile restoredData)
		{
			data.userID = restoredData.userID;
			data.userName = restoredData.userName;
			data.renameCount = restoredData.renameCount;
			data.renameItemQuantity = restoredData.renameItemQuantity;
			data.userCountryCode = restoredData.countryCode;
			data.league = restoredData.league;
			data.lastTimeBackup = restoredData.lastTimeBackup;
			UnityEngine.Debug.Log(data);
			SaveAll();
		}
	}
}

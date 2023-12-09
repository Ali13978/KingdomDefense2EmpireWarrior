using System;
using System.Collections.Generic;
using UnityEngine;

namespace Services.PlatformSpecific
{
	public class UserProfileEditor : MonoBehaviour, IUserProfile
	{
		private static string USER_LOGIN_GOOGLE = "user_login_google";

		private static string USER_LOGIN_FACEBOOK = "user_login_facebook";

		private static string USER_NAME = "user_name";

		public event Action OnLogStatusChangeEvent;

		public void LogIn_Facebook()
		{
			PlayerPrefs.SetInt(USER_LOGIN_FACEBOOK, 1);
			if (this.OnLogStatusChangeEvent != null)
			{
				this.OnLogStatusChangeEvent();
			}
			UnityEngine.Debug.Log("Log in FB!");
		}

		public void LogIn_Google()
		{
			PlayerPrefs.SetInt(USER_LOGIN_GOOGLE, 1);
			if (this.OnLogStatusChangeEvent != null)
			{
				this.OnLogStatusChangeEvent();
			}
			UnityEngine.Debug.Log("Log in GG!");
		}

		public void LogOut_Google()
		{
			PlayerPrefs.SetInt(USER_LOGIN_GOOGLE, 0);
			if (this.OnLogStatusChangeEvent != null)
			{
				this.OnLogStatusChangeEvent();
			}
			UnityEngine.Debug.Log("Log out GG!");
		}

		public void LogOut_Facebook()
		{
			PlayerPrefs.SetInt(USER_LOGIN_FACEBOOK, 0);
			if (this.OnLogStatusChangeEvent != null)
			{
				this.OnLogStatusChangeEvent();
			}
			UnityEngine.Debug.Log("Log out FB!");
		}

		public void BackupData()
		{
			PlatformSpecificServicesProvider.Services.DataCloudSaver.BackupData();
		}

		public void RestoreData()
		{
			PlatformSpecificServicesProvider.Services.DataCloudSaver.RestoreData();
		}

		public bool IsLoggedIn_Google()
		{
			return PlayerPrefs.GetInt(USER_LOGIN_GOOGLE, 0) == 1;
		}

		public bool IsLoggedIn_Facebook()
		{
			return PlayerPrefs.GetInt(USER_LOGIN_FACEBOOK, 0) == 1;
		}

		public string GetFireseBaseUserID()
		{
			return "userIDTest";
		}

		public Sprite GetUserAvatar()
		{
			return null;
		}

		public string GetUidOfUser()
		{
			throw new NotImplementedException();
		}

		public void GetUidsOfUserFriends(Action<List<string>> callback)
		{
			throw new NotImplementedException();
		}

		public void FirebaseSignOut()
		{
			UnityEngine.Debug.Log("Sign out firebase!");
		}

		public void InviteFriend_Facebook()
		{
		}

		public void TakePremiumUserInfor()
		{
			throw new NotImplementedException();
		}
	}
}

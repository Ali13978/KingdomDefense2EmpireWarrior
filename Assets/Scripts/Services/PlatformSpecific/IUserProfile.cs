using System;
using System.Collections.Generic;
using UnityEngine;

namespace Services.PlatformSpecific
{
	public interface IUserProfile
	{
		event Action OnLogStatusChangeEvent;

		void LogIn_Google();

		void LogOut_Google();

		bool IsLoggedIn_Google();

		void LogIn_Facebook();

		void LogOut_Facebook();

		bool IsLoggedIn_Facebook();

		void InviteFriend_Facebook();

		string GetUidOfUser();

		void GetUidsOfUserFriends(Action<List<string>> callback);

		Sprite GetUserAvatar();

		void BackupData();

		void RestoreData();

		string GetFireseBaseUserID();

		void FirebaseSignOut();

		void TakePremiumUserInfor();
	}
}

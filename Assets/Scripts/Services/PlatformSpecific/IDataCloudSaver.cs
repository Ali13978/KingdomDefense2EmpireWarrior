using System;
using System.Collections.Generic;

namespace Services.PlatformSpecific
{
	public interface IDataCloudSaver
	{
		event Action OnDataBackupCompletedEvent;

		event Action OnDataRestoreCompletedEvent;

		void BackupData();

		void RestoreData();

		void AutoBackUpData();

		void RetrieveData(string dbRef, Action<IDataSnapshot> callback);

		void RetrieveDataWithMainThreadCallback(string dbRef, Action<IDataSnapshot> callback);

		void UpdateData(Dictionary<string, object> updateList, string dbRefPath = null);

		void WriteData(object data, string dbRefPath);

		void WriteDataWithMainThreadCallback(object data, string dbRefPath, Action<IDataSnapshot> callback);

		void WriteGroupInfoTransaction(string groupInfoPath, bool isUserPremium, int tier = -1);

		void WriteNewGroupInfoTransaction(int newGroupId, bool isUserPremium, int tier = -1);

		void ClaimPremiumUserInfor(string userID, string userName, string userEmail, string userPhoneNumber);
	}
}

using LifetimePopup;
using Parameter;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Services.PlatformSpecific
{
	public class DataCloudSaverEditor : MonoBehaviour, IDataCloudSaver
	{
		public event Action OnDataBackupCompletedEvent;

		public event Action OnDataRestoreCompletedEvent;

		public void AutoBackUpData()
		{
			UnityEngine.Debug.Log("Auto Backup data!");
		}

		public void BackupData()
		{
			UnityEngine.Debug.Log("Backup data!");
			string notiContent = Singleton<NotificationDescription>.Instance.GetNotiContent(132);
			SingletonMonoBehaviour<LifetimeCanvas>.Instance.NotifyPopupController.Init(notiContent, isShowButtonFreeResources: false, isShowButtonGoToStore: false);
			if (this.OnDataBackupCompletedEvent != null)
			{
				this.OnDataBackupCompletedEvent();
			}
		}

		public void ClaimPremiumUserInfor(string userID, string userName, string userEmail, string userPhoneNumber)
		{
			throw new NotImplementedException();
		}

		public void RestoreData()
		{
			UnityEngine.Debug.Log("Restore data!");
			string notiContent = Singleton<NotificationDescription>.Instance.GetNotiContent(135);
			SingletonMonoBehaviour<LifetimeCanvas>.Instance.NotifyPopupController.Init(notiContent, isShowButtonFreeResources: false, isShowButtonGoToStore: false);
			if (this.OnDataRestoreCompletedEvent != null)
			{
				this.OnDataRestoreCompletedEvent();
			}
		}

		public void RetrieveData(string dbRef, Action<IDataSnapshot> callback)
		{
			throw new NotImplementedException();
		}

		public void RetrieveDataWithMainThreadCallback(string dbRef, Action<IDataSnapshot> callback)
		{
			callback(new DataSnapshotEditor());
		}

		public void UpdateData(Dictionary<string, object> updateList, string dbRefPath = null)
		{
		}

		public void WriteData(object data, string dbRefPath)
		{
		}

		public void WriteDataWithMainThreadCallback(object data, string dbRefPath, Action<IDataSnapshot> callback)
		{
		}

		public void WriteGroupInfoTransaction(string groupInfoPath, bool isUserPremium, int tier = -1)
		{
		}

		public void WriteNewGroupInfoTransaction(int newGroupId, bool isUserPremium, int tier = -1)
		{
		}
	}
}

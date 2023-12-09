using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Data
{
	public class ReadWriteDataDailyReward : MonoBehaviour
	{
		private static string DB_NAME = "/dailyRewardData.dat";

		private DailyRewardSerializeData data = new DailyRewardSerializeData();

		private int minDay;

		private int maxDay = 13;

		private static ReadWriteDataDailyReward instance;

		public static ReadWriteDataDailyReward Instance => instance;

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
				data.ListDailyRewarData = new List<DailyRewardData>();
				data.currentDay = 0;
				for (int i = 0; i <= maxDay; i++)
				{
					DailyRewardData dailyRewardData = new DailyRewardData();
					dailyRewardData.day = i;
					dailyRewardData.isReceivedReward = false;
					dailyRewardData.isReceivedBonus = false;
					data.ListDailyRewarData.Insert(i, dailyRewardData);
				}
				binaryFormatter.Serialize(fileStream, data);
				fileStream.Close();
			}
		}

		private void Load()
		{
			FileStream fileStream = File.Open(Application.persistentDataPath + DB_NAME, FileMode.Open);
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			data = (DailyRewardSerializeData)binaryFormatter.Deserialize(fileStream);
			fileStream.Close();
		}

		private void SaveAll()
		{
			FileStream fileStream = File.Create(Application.persistentDataPath + DB_NAME);
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			binaryFormatter.Serialize(fileStream, data);
			fileStream.Close();
		}

		public void TryIncreaseDay()
		{
			Load();
			if (data.ListDailyRewarData[data.currentDay].isReceivedReward)
			{
				UnityEngine.Debug.Log("đã nhận reward, tăng ngày!");
				data.currentDay++;
				if (data.currentDay > maxDay)
				{
					UnityEngine.Debug.Log("reset ngày nhận dailyreward");
					data.currentDay = minDay;
					ResetData();
				}
				SaveAll();
			}
			else
			{
				UnityEngine.Debug.Log("chưa nhận reward, chưa tăng ngày!");
			}
		}

		public int GetCurrentDay()
		{
			Load();
			return data.currentDay;
		}

		public bool IsReceivedReward(int dayIndex)
		{
			Load();
			return data.ListDailyRewarData[dayIndex].isReceivedReward;
		}

		public void SetReceiveRewardStatus(int dayIndex)
		{
			data.ListDailyRewarData[dayIndex].isReceivedReward = true;
			SaveAll();
		}

		public bool IsReceivedBonus(int dayIndex)
		{
			Load();
			return data.ListDailyRewarData[dayIndex].isReceivedBonus;
		}

		public void SetReceiveBonusStatus(int dayIndex)
		{
			data.ListDailyRewarData[dayIndex].isReceivedBonus = true;
			SaveAll();
		}

		public void ResetData()
		{
			for (int i = 0; i <= maxDay; i++)
			{
				data.ListDailyRewarData[i].isReceivedReward = false;
				data.ListDailyRewarData[i].isReceivedBonus = false;
			}
			SaveAll();
		}

		public void RestoreDataFromCloud(UserData_DailyReward restoredData)
		{
			data.ListDailyRewarData = new List<DailyRewardData>();
			if (restoredData != null)
			{
				data.currentDay = restoredData.currentDay;
			}
			for (int i = 0; i <= maxDay; i++)
			{
				if (restoredData == null)
				{
					DailyRewardData dailyRewardData = new DailyRewardData();
					dailyRewardData.day = i;
					dailyRewardData.isReceivedReward = false;
					dailyRewardData.isReceivedBonus = false;
					data.ListDailyRewarData.Insert(i, dailyRewardData);
					continue;
				}
				DailyRewardData dailyRewardData2 = new DailyRewardData();
				dailyRewardData2.day = i;
				if (restoredData.listDailyRewardData == null)
				{
					dailyRewardData2.isReceivedReward = false;
					dailyRewardData2.isReceivedBonus = false;
				}
				else if (restoredData.listDailyRewardData[i] != null)
				{
					dailyRewardData2.isReceivedReward = restoredData.listDailyRewardData[i].isReceivedReward;
					dailyRewardData2.isReceivedBonus = restoredData.listDailyRewardData[i].isReceivedBonus;
				}
				else
				{
					dailyRewardData2.isReceivedReward = false;
					dailyRewardData2.isReceivedBonus = false;
				}
				data.ListDailyRewarData.Insert(i, dailyRewardData2);
			}
			UnityEngine.Debug.Log(data);
			SaveAll();
		}
	}
}

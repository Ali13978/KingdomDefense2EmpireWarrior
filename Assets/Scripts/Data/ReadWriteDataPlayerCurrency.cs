using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Data
{
	public class ReadWriteDataPlayerCurrency : MonoBehaviour
	{
		private static string DB_NAME = "/playerCurrencyInfor.dat";

		private PlayerCurrencyData data = new PlayerCurrencyData();

		private static ReadWriteDataPlayerCurrency instance;

		public static ReadWriteDataPlayerCurrency Instance => instance;

		public event Action OnGemChangeEvent;

		private void Awake()
		{
			instance = this;
			SaveDefaultData();
			Load();
			LoadTestData();
		}

		private void LoadTestData()
		{
			if (PlayerPrefs.GetInt("TESTgemNumber", 0) > 0)
			{
				ChangeGem(Singleton<TestConfig>.Instance.gemNumber, isDispatchEventChange: true);
			}
		}

		private void SaveDefaultData()
		{
			if (!File.Exists(Application.persistentDataPath + DB_NAME))
			{
				FileStream fileStream = File.Create(Application.persistentDataPath + DB_NAME);
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				data.totalGem = 20;
				binaryFormatter.Serialize(fileStream, data);
				fileStream.Close();
			}
		}

		public void Save(int gem)
		{
			FileStream fileStream = File.Create(Application.persistentDataPath + DB_NAME);
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			data.totalGem = gem;
			binaryFormatter.Serialize(fileStream, data);
			fileStream.Close();
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
			data = (PlayerCurrencyData)binaryFormatter.Deserialize(fileStream);
			fileStream.Close();
		}

		public int GetCurrentGem()
		{
			Load();
			return data.totalGem;
		}

		public int GetCurrentStar()
		{
			int num = 0;
			if (PlayerPrefs.GetInt("TESTstarsNumber", 0) > 0)
			{
				return Singleton<TestConfig>.Instance.starsNumber;
			}
			return ReadWriteDataMap.Instance.GetTotalStarEarned();
		}

		public void ChangeGem(int gemAmount, bool isDispatchEventChange)
		{
			if (gemAmount < 0)
			{
				GameEventCenter.Instance.Trigger(GameEventType.EventUseGem, new EventTriggerData(EventTriggerType.UseGem, Mathf.Abs(gemAmount), forceSaveProgress: true));
			}
			data.totalGem += gemAmount;
			Save(data.totalGem);
			DispatchGemChangeEvent(isDispatchEventChange);
		}

		private void DispatchGemChangeEvent(bool isDispatchEventChange)
		{
			if (isDispatchEventChange && this.OnGemChangeEvent != null)
			{
				this.OnGemChangeEvent();
			}
		}

		public void RestoreDataFromCloud(UserData_UserProfile restoredData)
		{
			data.totalGem = restoredData.totalGem;
			UnityEngine.Debug.Log(data);
			SaveAll();
			DispatchGemChangeEvent(isDispatchEventChange: true);
		}
	}
}

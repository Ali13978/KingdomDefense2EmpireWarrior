using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Data
{
	public class ReadWriteDataSaleBundle : MonoBehaviour
	{
		private static string DB_NAME = "/saleBundleInfor.dat";

		private SaleBundleData data;

		private string[] listSpecialBundleID = MarketingConfig.productIDSpecialPack;

		private static ReadWriteDataSaleBundle instance;

		public static ReadWriteDataSaleBundle Instance => instance;

		private void Awake()
		{
			instance = this;
			SaveDefaultData();
			Load();
		}

		private void OnApplicationQuit()
		{
			SetLastTimePlay();
		}

		private void SaveDefaultData()
		{
			if (!File.Exists(Application.persistentDataPath + DB_NAME))
			{
				FileStream fileStream = File.Create(Application.persistentDataPath + DB_NAME);
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				data = new SaleBundleData();
				data.ListSpecialBundleData = new List<SerializeBundleItem>();
				for (int i = 0; i < listSpecialBundleID.Length; i++)
				{
					SerializeBundleItem serializeBundleItem = new SerializeBundleItem();
					SaleBundleConfigData dataSaleBundle = StoreBundleData.GetDataSaleBundle(listSpecialBundleID[i]);
					serializeBundleItem.bundleID = dataSaleBundle.Productid;
					serializeBundleItem.isExpired = false;
					serializeBundleItem.isBought = false;
					data.ListSpecialBundleData.Insert(i, serializeBundleItem);
				}
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
			data = (SaleBundleData)binaryFormatter.Deserialize(fileStream);
			fileStream.Close();
		}

		public void SetLastTimePlay()
		{
			data.lastTimePlayed = GameTools.GetNow().ToBinary().ToString();
			SaveAll();
		}

		public DateTime GetLastTimePlay()
		{
			Load();
			string lastTimePlayed = data.lastTimePlayed;
			long dateData = Convert.ToInt64(lastTimePlayed);
			return DateTime.FromBinary(dateData);
		}

		public int GetCurrentSpecialPackIndex()
		{
			int result = -1;
			if (data == null || data.ListSpecialBundleData == null)
			{
				Load();
			}
			for (int i = 0; i < listSpecialBundleID.Length; i++)
			{
				if (data == null)
				{
					UnityEngine.Debug.LogError("NULL DATA, PLS CHECK");
					return -1;
				}
				if (data.ListSpecialBundleData == null)
				{
					UnityEngine.Debug.LogError("NULL DATA.ListSpecialBundleData, PLS CHECK");
					return -1;
				}
				SerializeBundleItem serializeBundleItem = GetSerializeBundleItem(listSpecialBundleID[i]);
				if (!serializeBundleItem.isBought && !serializeBundleItem.isExpired)
				{
					result = i;
					break;
				}
			}
			return result;
		}

		private SerializeBundleItem GetSerializeBundleItem(string bundleID)
		{
			SerializeBundleItem result = null;
			foreach (SerializeBundleItem listSpecialBundleDatum in data.ListSpecialBundleData)
			{
				if (listSpecialBundleDatum.bundleID.Equals(bundleID))
				{
					result = listSpecialBundleDatum;
				}
			}
			return result;
		}

		public int GetCurrentAvailableSpeciakPackIndex()
		{
			Load();
			int result = -1;
			int currentSpecialPackIndex = GetCurrentSpecialPackIndex();
			if (currentSpecialPackIndex >= 0)
			{
				SerializeBundleItem serializeBundleItem = GetSerializeBundleItem(listSpecialBundleID[currentSpecialPackIndex]);
				if (StoreBundleData.GetDataSaleBundle(listSpecialBundleID[currentSpecialPackIndex]).Bundletype.Equals(StoreBundleType.Starter.ToString()))
				{
					result = GetCurrentSpecialPackIndex();
				}
				if (StoreBundleData.GetDataSaleBundle(listSpecialBundleID[currentSpecialPackIndex]).Bundletype.Equals(StoreBundleType.TimeLimited.ToString()))
				{
					bool isExpired = serializeBundleItem.isExpired;
					bool isBought = serializeBundleItem.isBought;
					int condition = StoreBundleData.GetDataSaleBundle(listSpecialBundleID[currentSpecialPackIndex]).Condition;
					int mapIDUnlocked = ReadWriteDataMap.Instance.GetMapIDUnlocked();
					bool flag = mapIDUnlocked > condition;
					if (!isExpired && !isBought && flag)
					{
						result = GetCurrentSpecialPackIndex();
					}
				}
			}
			return result;
		}

		public TimeSpan getCountdownTime(string bundleID)
		{
			TimeSpan timeSpan = default(TimeSpan);
			DateTime lastTimePlay = GetLastTimePlay();
			TimeSpan t = GameTools.GetNow().Subtract(lastTimePlay);
			int timecountdown = StoreBundleData.GetDataSaleBundle(bundleID).Timecountdown;
			TimeSpan result = new TimeSpan(timecountdown, 0, 0).Add(-t);
			if (result.TotalSeconds > 0.0)
			{
				return result;
			}
			return TimeSpan.MinValue;
		}

		public bool GetSpecialPackExpireStatus(string bundleID)
		{
			Load();
			return GetSerializeBundleItem(bundleID).isExpired;
		}

		public void SetSpecialPackExpired(string bundleID)
		{
			SerializeBundleItem serializeBundleItem = GetSerializeBundleItem(bundleID);
			serializeBundleItem.isExpired = true;
			SaveAll();
		}

		public bool GetSpecialPackBuyStatus(string bundleID)
		{
			Load();
			return GetSerializeBundleItem(bundleID).isBought;
		}

		public void SetSpecialPackBought(string bundleID)
		{
			SerializeBundleItem serializeBundleItem = GetSerializeBundleItem(bundleID);
			serializeBundleItem.isBought = true;
			SaveAll();
		}

		public void RestoreDataFromCloud(UserData_SaleBundle restoredData)
		{
			data.ListSpecialBundleData = new List<SerializeBundleItem>();
			for (int i = 0; i < listSpecialBundleID.Length; i++)
			{
				if (restoredData == null)
				{
					SerializeBundleItem serializeBundleItem = new SerializeBundleItem();
					serializeBundleItem.bundleID = listSpecialBundleID[i];
					serializeBundleItem.isExpired = false;
					serializeBundleItem.isBought = false;
					data.ListSpecialBundleData.Insert(i, serializeBundleItem);
					continue;
				}
				SerializeBundleItem serializeBundleItem2 = new SerializeBundleItem();
				serializeBundleItem2.bundleID = listSpecialBundleID[i];
				if (restoredData.ListSaleBundleData == null)
				{
					serializeBundleItem2.isExpired = false;
					serializeBundleItem2.isBought = false;
				}
				else if (restoredData.ListSaleBundleData[i] != null)
				{
					serializeBundleItem2.isBought = restoredData.ListSaleBundleData[i].isBought;
					serializeBundleItem2.isExpired = restoredData.ListSaleBundleData[i].isExpired;
				}
				else
				{
					serializeBundleItem2.isExpired = false;
					serializeBundleItem2.isBought = false;
				}
				data.ListSpecialBundleData.Insert(i, serializeBundleItem2);
			}
			UnityEngine.Debug.Log(data);
			SaveAll();
		}
	}
}

using CodeStage.AntiCheat.ObscuredTypes;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Data
{
	public class ReadWriteDataPowerUpItem : MonoBehaviour
	{
		private ObscuredInt[] encodedItemQuantity;

		private static string DB_NAME = "/powerUpItemQuantity.dat";

		private int totalPowerupItem = 9;

		private static ReadWriteDataPowerUpItem instance;

		public static ReadWriteDataPowerUpItem Instance => instance;

		public event Action OnItemQuantityChangeEvent;

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
				PowerUpItemData powerUpItemData = new PowerUpItemData();
				powerUpItemData.itemQuantity = new int[9];
				binaryFormatter.Serialize(fileStream, powerUpItemData);
				fileStream.Close();
			}
		}

		public void Save(int powerUpItemID, int quantity)
		{
			SetEncodedItemQuantity(powerUpItemID, quantity);
			SaveAll();
		}

		public void SaveAll()
		{
			FileStream fileStream = File.Create(Application.persistentDataPath + DB_NAME);
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			PowerUpItemData powerUpItemData = new PowerUpItemData();
			powerUpItemData.itemQuantity = GetDecodedItemQuantities();
			binaryFormatter.Serialize(fileStream, powerUpItemData);
			fileStream.Close();
		}

		private void Load()
		{
			FileStream fileStream = File.Open(Application.persistentDataPath + DB_NAME, FileMode.Open);
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			PowerUpItemData powerUpItemData = new PowerUpItemData();
			powerUpItemData = (PowerUpItemData)binaryFormatter.Deserialize(fileStream);
			fileStream.Close();
			encodedItemQuantity = new ObscuredInt[9];
			for (int i = 0; i < totalPowerupItem; i++)
			{
				if (i < powerUpItemData.itemQuantity.Length)
				{
					SetEncodedItemQuantity(i, powerUpItemData.itemQuantity[i]);
				}
				else
				{
					SetEncodedItemQuantity(i, 0);
				}
			}
		}

		public int GetCurrentItemQuantity(int itemID)
		{
			return GetDecodedItemQuanity(itemID);
		}

		public void ChangeItemQuantity(int itemID, int addedQuantity)
		{
			Save(itemID, GetDecodedItemQuanity(itemID) + addedQuantity);
			if (this.OnItemQuantityChangeEvent != null)
			{
				this.OnItemQuantityChangeEvent();
			}
		}

		public void RestoreDataFromCloud(UserData_PowerupItem restoredData)
		{
			encodedItemQuantity = new ObscuredInt[9]
			{
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0,
				0
			};
			for (int i = 0; i < totalPowerupItem; i++)
			{
				if (i < restoredData.listDataPowerupItems.Count)
				{
					SetEncodedItemQuantity(i, restoredData.listDataPowerupItems[i].quantity);
				}
				else
				{
					SetEncodedItemQuantity(i, 0);
				}
			}
			SaveAll();
		}

		private void SetEncodedItemQuantity(int itemId, int quantity)
		{
			encodedItemQuantity[itemId] = quantity + GameTools.deltaValue;
		}

		private int GetDecodedItemQuanity(int itemId)
		{
			return (int)encodedItemQuantity[itemId] - GameTools.deltaValue;
		}

		private int[] GetDecodedItemQuantities()
		{
			int[] array = new int[encodedItemQuantity.Length];
			for (int num = encodedItemQuantity.Length - 1; num >= 0; num--)
			{
				array[num] = GetDecodedItemQuanity(num);
			}
			return array;
		}
	}
}

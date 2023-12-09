using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Data
{
	public class ReadWriteDataOffers : MonoBehaviour
	{
		private string[] allOfferKeys = new string[3]
		{
			KEY_OFFER_STARTER,
			KEY_OFFER_SPECIAL,
			KEY_INSTALL_GOE
		};

		public static string KEY_OFFER_STARTER = "key_offer_starter";

		public static string KEY_OFFER_SPECIAL = "key_offer_special";

		public static string KEY_INSTALL_GOE = "key_install_goe";

		private static string DB_NAME = "/offerInfor.dat";

		private OfferData data = new OfferData();

		private static ReadWriteDataOffers instance;

		public static ReadWriteDataOffers Instance => instance;

		public string[] AllOfferKeys
		{
			get
			{
				return allOfferKeys;
			}
			set
			{
				allOfferKeys = value;
			}
		}

		private void Awake()
		{
			instance = this;
			SaveDefaultData();
			Load();
		}

		private void SaveDefaultData()
		{
			if (File.Exists(Application.persistentDataPath + DB_NAME))
			{
				return;
			}
			FileStream fileStream = File.Create(Application.persistentDataPath + DB_NAME);
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			data.ListOfferData = new Dictionary<string, bool>();
			string[] array = AllOfferKeys;
			foreach (string key in array)
			{
				if (!data.ListOfferData.ContainsKey(key))
				{
					data.ListOfferData.Add(key, PlayerPrefs.GetInt(key, 0) == 1);
				}
			}
			UnityEngine.Debug.Log(data);
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
			data = (OfferData)binaryFormatter.Deserialize(fileStream);
			fileStream.Close();
		}

		public bool IsOfferProcessed(string offerID)
		{
			Load();
			bool flag = false;
			if (data.ListOfferData.ContainsKey(offerID))
			{
				return data.ListOfferData[offerID];
			}
			data.ListOfferData.Add(offerID, value: false);
			SaveAll();
			return data.ListOfferData[offerID];
		}

		public void SetOfferStatus(string offerID, bool value)
		{
			Load();
			data.ListOfferData[offerID] = value;
			SaveAll();
		}

		public void RestoreDataFromCloud(UserData_Offer restoredData)
		{
			data.ListOfferData = new Dictionary<string, bool>();
			string[] array = AllOfferKeys;
			foreach (string key in array)
			{
				if (!data.ListOfferData.ContainsKey(key))
				{
					if (restoredData == null)
					{
						data.ListOfferData.Add(key, value: false);
					}
					else if (restoredData.ListOfferData.ContainsKey(key))
					{
						data.ListOfferData.Add(key, restoredData.ListOfferData[key]);
					}
					else
					{
						data.ListOfferData.Add(key, value: false);
					}
				}
			}
			UnityEngine.Debug.Log(data);
			SaveAll();
		}
	}
}

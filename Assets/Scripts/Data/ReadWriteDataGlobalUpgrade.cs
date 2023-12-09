using MyCustom;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Data
{
	public class ReadWriteDataGlobalUpgrade : CustomMonoBehaviour
	{
		[Serializable]
		public class UpgradeData
		{
			public int towerID;

			public int currentUpgradeLevel;
		}

		[Serializable]
		public class UpgradeSerializeData
		{
			private Dictionary<int, UpgradeData> listUpgradeData;

			public Dictionary<int, UpgradeData> ListUpgradeData
			{
				get
				{
					return listUpgradeData;
				}
				set
				{
					listUpgradeData = value;
				}
			}
		}

		private List<TierOptionParameter> tierParameters = new List<TierOptionParameter>();

		private List<StarUpgradeParameter> starParameters = new List<StarUpgradeParameter>();

		private static ReadWriteDataGlobalUpgrade instance;

		private static string DB_NAME = "/upgradeInfor.dat";

		private UpgradeSerializeData data = new UpgradeSerializeData();

		public static ReadWriteDataGlobalUpgrade Instance => instance;

		public event Action OnStarChangeEvent;

		private void Awake()
		{
			instance = this;
			ReadParameterUpgrade();
			ReadDataStarUpgradeRequire();
			SaveDefaultData();
			LoadData();
		}

		public void OnStarChange(bool isDispatchEventChange)
		{
			if (isDispatchEventChange && this.OnStarChangeEvent != null)
			{
				this.OnStarChangeEvent();
			}
		}

		private void ReadParameterUpgrade()
		{
			string file = "Parameters/global_upgrade_parameter";
			List<Dictionary<string, object>> list = CSVReader.Read(file);
			for (int i = 0; i < list.Count; i++)
			{
				List<int> list2 = new List<int>();
				list2.Add((int)list[i]["tower_0"]);
				list2.Add((int)list[i]["tower_1"]);
				list2.Add((int)list[i]["tower_2"]);
				list2.Add((int)list[i]["tower_3"]);
				tierParameters.Add(new TierOptionParameter(list2));
			}
		}

		private void ReadDataStarUpgradeRequire()
		{
			string file = "Parameters/global_upgrade_star_require";
			List<Dictionary<string, object>> list = CSVReader.Read(file);
			for (int i = 0; i < list.Count; i++)
			{
				int num = (int)list[i]["tower_id"];
				List<int> list2 = new List<int>();
				list2.Add((int)list[i]["star_tier_0"]);
				list2.Add((int)list[i]["star_tier_1"]);
				list2.Add((int)list[i]["star_tier_2"]);
				list2.Add((int)list[i]["star_tier_3"]);
				list2.Add((int)list[i]["star_tier_4"]);
				starParameters.Add(new StarUpgradeParameter(list2));
			}
		}

		public int GetStarRequireForUpgrade(int towerID, int tier)
		{
			int num = 0;
			if (towerID < 0)
			{
				num = -1;
			}
			else
			{
				num = ((tier >= 0) ? starParameters[towerID].Value[tier] : starParameters[towerID].Value[tier + 1]);
				if (tier > 4)
				{
					num = 0;
				}
			}
			return num;
		}

		public int GetUpgradeValue(int upgradeID, int towerID)
		{
			return tierParameters[upgradeID].Value[towerID];
		}

		private void SaveDefaultData()
		{
			if (!File.Exists(Application.persistentDataPath + DB_NAME))
			{
				FileStream fileStream = File.Create(Application.persistentDataPath + DB_NAME);
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				UpgradeData upgradeData = new UpgradeData();
				upgradeData.towerID = 0;
				upgradeData.currentUpgradeLevel = -1;
				UpgradeData upgradeData2 = new UpgradeData();
				upgradeData2.towerID = 1;
				upgradeData2.currentUpgradeLevel = -1;
				UpgradeData upgradeData3 = new UpgradeData();
				upgradeData3.towerID = 2;
				upgradeData3.currentUpgradeLevel = -1;
				UpgradeData upgradeData4 = new UpgradeData();
				upgradeData4.towerID = 2;
				upgradeData4.currentUpgradeLevel = -1;
				data.ListUpgradeData = new Dictionary<int, UpgradeData>();
				data.ListUpgradeData.Add(0, upgradeData);
				data.ListUpgradeData.Add(1, upgradeData2);
				data.ListUpgradeData.Add(2, upgradeData3);
				data.ListUpgradeData.Add(3, upgradeData4);
				binaryFormatter.Serialize(fileStream, data);
				fileStream.Close();
			}
		}

		public void Save(int _towerID, int _currentUpgradeLevel)
		{
			FileStream fileStream = File.Create(Application.persistentDataPath + DB_NAME);
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			data.ListUpgradeData[_towerID].currentUpgradeLevel = _currentUpgradeLevel;
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

		private void LoadData()
		{
			FileStream fileStream = File.Open(Application.persistentDataPath + DB_NAME, FileMode.Open);
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			data = (UpgradeSerializeData)binaryFormatter.Deserialize(fileStream);
			fileStream.Close();
		}

		public int GetCurrentUpgradeLevel(int _towerID)
		{
			LoadData();
			return data.ListUpgradeData[_towerID].currentUpgradeLevel;
		}

		public void RestoreDataFromCloud(UserData_GlobalUpgrade restoredData)
		{
			data.ListUpgradeData = new Dictionary<int, UpgradeData>();
			foreach (UserData_GlobalUpgrade_Unique item in restoredData.listUpgradedTower)
			{
				UpgradeData upgradeData = new UpgradeData();
				upgradeData.towerID = item.towerID;
				upgradeData.currentUpgradeLevel = item.towerUpgradedLevel;
				data.ListUpgradeData.Add(upgradeData.towerID, upgradeData);
			}
			UnityEngine.Debug.Log(data);
			SaveAll();
		}
	}
}

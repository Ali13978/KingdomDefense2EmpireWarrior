using Middle;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Data
{
	public class ReadWriteDataMap : MonoBehaviour
	{
		private static string DB_NAME = "/mapInfor.dat";

		private MapSerializeData data = new MapSerializeData();

		private static int mapIDMax = 100;

		private static int currentMapIDMax = 17;

		private static int modeResultMax = 3;

		private static ReadWriteDataMap instance;

		public static ReadWriteDataMap Instance => instance;

		private void Awake()
		{
			instance = this;
			SaveDefaultData();
			Load();
			UpdateDataVersionFrom105To106();
		}

		private void UpdateDataVersionFrom105To106()
		{
			string key = "UpdateDataVersionFrom105To106";
			if (PlayerPrefs.GetInt(key, 0) == 0 && data.ListMapsData.Count <= 12)
			{
				data.mapIDUnlocked = 12;
				for (int i = 12; i < 18; i++)
				{
					MapData value = new MapData();
					data.ListMapsData.Add(i, value);
				}
				SaveAll();
				PlayerPrefs.SetInt(key, 1);
			}
		}

		private void SaveDefaultData()
		{
			if (!File.Exists(Application.persistentDataPath + DB_NAME))
			{
				FileStream fileStream = File.Create(Application.persistentDataPath + DB_NAME);
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				data.mapIDUnlocked = 0;
				data.lastMapIDPlayed = 0;
				data.ListMapsData = new Dictionary<int, MapData>();
				for (int i = 0; i < mapIDMax + 1; i++)
				{
					MapData value = new MapData();
					data.ListMapsData.Add(i, value);
				}
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

		public void SaveLastMapPlayed(int mapIDPlayed)
		{
			data.lastMapIDPlayed = mapIDPlayed;
			SaveAll();
		}

		public void SaveLastMapModeChoose(int mapModeChoose)
		{
			data.lastMapModeChoose = mapModeChoose;
			SaveAll();
		}

		private void SaveMapModeResult(int _mapID, int result)
		{
			GetMapDataByID(_mapID).modePassed = result;
			SaveAll();
		}

		private void Load()
		{
			FileStream fileStream = File.Open(Application.persistentDataPath + DB_NAME, FileMode.Open);
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			data = (MapSerializeData)binaryFormatter.Deserialize(fileStream);
			fileStream.Close();
		}

		private MapData GetMapDataByID(int _mapID)
		{
			MapData result = new MapData();
			if (data.ListMapsData.ContainsKey(_mapID))
			{
				result = data.ListMapsData[_mapID];
			}
			return result;
		}

		public int GetCurrentPlayCount(int _mapID)
		{
			int num = -1;
			return GetMapDataByID(_mapID).playCount;
		}

		public void IncreaseMapPlaycount(int _mapID)
		{
			GetMapDataByID(_mapID).playCount++;
			SaveAll();
		}

		public void IncreaseMapPlaycount(int _mapID, BattleStatus status)
		{
			switch (status)
			{
			case BattleStatus.Victory:
				GetMapDataByID(_mapID).playCount_victory++;
				SaveAll();
				break;
			case BattleStatus.Defeat:
				GetMapDataByID(_mapID).playCount_defeat++;
				SaveAll();
				break;
			}
		}

		public int GetCurrentPlayCount_Victory(int _mapID)
		{
			int num = -1;
			return GetMapDataByID(_mapID).playCount_victory;
		}

		public int GetCurrentPlayCount_Defeat(int _mapID)
		{
			int num = -1;
			return GetMapDataByID(_mapID).playCount_defeat;
		}

		public void SaveStarEarned(int _mapID, int _starEarned)
		{
			int starEarnedByMap = GetStarEarnedByMap(_mapID);
			if (_starEarned > starEarnedByMap)
			{
				GetMapDataByID(_mapID).starEarned = _starEarned;
				SaveAll();
			}
		}

		public int GetStarEarnedByMap(int _mapID)
		{
			int num = -1;
			return GetMapDataByID(_mapID).starEarned;
		}

		public int GetTotalStarEarned()
		{
			int num = 0;
			for (int i = 0; i <= currentMapIDMax; i++)
			{
				num += GetMapDataByID(i).starEarned;
			}
			return num;
		}

		public int GetMapIDPassed()
		{
			int result = -1;
			for (int i = 0; i < currentMapIDMax; i++)
			{
				if (GetStarEarnedByMap(i) > 0)
				{
					result = i;
				}
			}
			return result;
		}

		public int GetMapIDUnlocked()
		{
			int num = -1;
			if (PlayerPrefs.GetInt("TESTunlockAllMap", 0) > 0)
			{
				return currentMapIDMax;
			}
			return data.mapIDUnlocked;
		}

		public bool IsMapUnlocked(int mapID)
		{
			bool flag = false;
			return GetMapIDUnlocked() == mapID;
		}

		public void IncreaseMapIdUnlock(int currentMapID)
		{
			int mapIDUnlocked = GetMapIDUnlocked();
			if (currentMapID == mapIDUnlocked)
			{
				data.mapIDUnlocked = currentMapID + 1;
				SaveAll();
			}
		}

		public int GetTotalMap()
		{
			return currentMapIDMax + 1;
		}

		public int GetLastMapIDPlayed()
		{
			return data.lastMapIDPlayed;
		}

		public int GetMapModeResult(int mapID)
		{
			return GetMapDataByID(mapID).modePassed;
		}

		public void IncreaseModeResult(int mapID)
		{
			int battleLevel = (int)MiddleDelivery.Instance.BattleLevel;
			battleLevel++;
			if (battleLevel > GetMapDataByID(mapID).modePassed)
			{
				battleLevel = Mathf.Clamp(battleLevel, 0, modeResultMax);
				SaveMapModeResult(mapID, battleLevel);
			}
		}

		public int GetLastMapModeChoose()
		{
			return data.lastMapModeChoose;
		}

		public void RestoreDataFromCloud(UserData_Map restoredData)
		{
			data.mapIDUnlocked = restoredData.mapIDUnlocked;
			data.lastMapIDPlayed = restoredData.lastMapIDPlayed;
			data.lastMapModeChoose = restoredData.lastMapModeChoose;
			for (int i = 0; i < restoredData.listDataMap.Count; i++)
			{
				MapData mapData = new MapData();
				mapData.starEarned = restoredData.listDataMap[i].starEarned;
				mapData.playCount = restoredData.listDataMap[i].playCount;
				mapData.playCount_victory = restoredData.listDataMap[i].playCount_victory;
				mapData.playCount_defeat = restoredData.listDataMap[i].playCount_defeat;
				data.ListMapsData[restoredData.listDataMap[i].mapID] = mapData;
			}
			UnityEngine.Debug.Log(data);
			SaveAll();
		}
	}
}

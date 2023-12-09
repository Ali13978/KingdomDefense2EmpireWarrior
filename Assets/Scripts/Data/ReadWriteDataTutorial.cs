using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Tutorial;
using UnityEngine;

namespace Data
{
	public class ReadWriteDataTutorial : MonoBehaviour
	{
		private string[] allTutorialKeys = new string[20]
		{
			TUTORIAL_ID_SELECT_MAP,
			TUTORIAL_ID_START_GAME_MAP_LEVEL,
			TUTORIAL_ID_BRING_FIRST_HERO,
			TUTORIAL_ID_BUILD_TOWER,
			TUTORIAL_ID_HERO_MOVE,
			TUTORIAL_ID_CALL_ENEMY,
			TUTORIAL_ID_USE_SPEED_UP,
			TUTORIAL_ID_HERO_SKILL,
			TUTORIAL_ID_LUCKY_CHEST,
			TUTORIAL_ID_GET_LUCKY_CHEST_BY_VIDEO,
			TUTORIAL_ID_WORLD_MAP,
			TUTORIAL_ID_GO_HERO_CAMP_1ST,
			TUTORIAL_ID_UPGRADE_HERO_LEVEL,
			TUTORIAL_ID_GO_GLOBAL_UPGRADE,
			TUTORIAL_ID_GLOBAL_UPGRADE,
			TUTORIAL_ID_SELECT_SECOND_MAP,
			TUTORIAL_ID_BRING_SECOND_HERO,
			TUTORIAL_ID_GO_HERO_CAMP_2ND,
			TUTORIAL_ID_UPGRADE_HERO_SKILL,
			TUTORIAL_ID_GO_TOURNAMENT
		};

		public static string TUTORIAL_ID_SELECT_MAP = "TutorialSelectMapPassed";

		public static string TUTORIAL_ID_START_GAME_MAP_LEVEL = "TutorialMapLevelStartGame";

		public static string TUTORIAL_ID_BRING_FIRST_HERO = "TutorialBringFirstHeroPassed";

		public static string TUTORIAL_ID_BUILD_TOWER = "TutorialBuildtowerPassed";

		public static string TUTORIAL_ID_HERO_MOVE = "TutorialBuildtowerPassed";

		public static string TUTORIAL_ID_CALL_ENEMY = "TutorialBuildtowerPassed";

		public static string TUTORIAL_ID_USE_SPEED_UP = "TutorialBuildtowerPassed";

		public static string TUTORIAL_ID_HERO_SKILL = "TutorialUseHeroSkillPassed";

		public static string TUTORIAL_ID_LUCKY_CHEST = "TutorialOpenLuckyChest";

		public static string TUTORIAL_ID_GET_LUCKY_CHEST_BY_VIDEO = "TutorialGetLuckyChestByVideo";

		public static string TUTORIAL_ID_WORLD_MAP = "WorldMapTutorialPassed";

		public static string TUTORIAL_ID_GO_HERO_CAMP_1ST = "TutorialToHeroCampPanelPassed";

		public static string TUTORIAL_ID_UPGRADE_HERO_LEVEL = "TutorialUpgradeHeroByGemPassed";

		public static string TUTORIAL_ID_GO_GLOBAL_UPGRADE = "TutorialToUpgradePanelPassed";

		public static string TUTORIAL_ID_GLOBAL_UPGRADE = "TutorialInUpgradePanelPassed";

		public static string TUTORIAL_ID_SELECT_SECOND_MAP = "TutorialSelectSecondMapPassed";

		public static string TUTORIAL_ID_BRING_SECOND_HERO = "TutorialBringSecondHeroPassed";

		public static string TUTORIAL_ID_GO_HERO_CAMP_2ND = "TutorialToHeroCampPanel2Passed";

		public static string TUTORIAL_ID_UPGRADE_HERO_SKILL = "TutorialUseHeroSkillPointPassed";

		public static string TUTORIAL_ID_GO_TOURNAMENT = "TutorialToTournament";

		private static string DB_NAME = "/tutorialInfor.dat";

		private TutorialData data = new TutorialData();

		public TutorialUnit currentTutorial;

		private static ReadWriteDataTutorial instance;

		public static ReadWriteDataTutorial Instance => instance;

		public string[] AllTutorialKeys
		{
			get
			{
				return allTutorialKeys;
			}
			set
			{
				allTutorialKeys = value;
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
			data.ListTutorialData = new Dictionary<string, bool>();
			string[] array = AllTutorialKeys;
			foreach (string key in array)
			{
				if (!data.ListTutorialData.ContainsKey(key))
				{
					data.ListTutorialData.Add(key, PlayerPrefs.GetInt(key, 0) == 1);
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
			data = (TutorialData)binaryFormatter.Deserialize(fileStream);
			fileStream.Close();
		}

		public bool GetTutorialStatus(string tutorialID)
		{
			Load();
			bool flag = false;
			if (data.ListTutorialData.ContainsKey(tutorialID))
			{
				return data.ListTutorialData[tutorialID];
			}
			data.ListTutorialData.Add(tutorialID, value: false);
			SaveAll();
			return data.ListTutorialData[tutorialID];
		}

		public void SetTutorialStatus(string tutorialID, bool value)
		{
			Load();
			data.ListTutorialData[tutorialID] = value;
			SaveAll();
		}

		public void SkipAllTutorials()
		{
			string[] array = AllTutorialKeys;
			foreach (string tutorialID in array)
			{
				SetTutorialStatus(tutorialID, value: true);
			}
			UnityEngine.Debug.Log("Skip All Tutorial!");
		}

		public void SetCurrentTutorialPass()
		{
			if ((bool)currentTutorial)
			{
				currentTutorial.SetTutorialPassed();
			}
		}

		public void RestoreDataFromCloud(UserData_Tutorial restoredData)
		{
			data.ListTutorialData = new Dictionary<string, bool>();
			string[] array = AllTutorialKeys;
			foreach (string key in array)
			{
				if (!data.ListTutorialData.ContainsKey(key))
				{
					if (restoredData.ListTutorialData.ContainsKey(key))
					{
						data.ListTutorialData.Add(key, restoredData.ListTutorialData[key]);
					}
					else
					{
						data.ListTutorialData.Add(key, value: false);
					}
				}
			}
			UnityEngine.Debug.Log(data);
			SaveAll();
		}
	}
}

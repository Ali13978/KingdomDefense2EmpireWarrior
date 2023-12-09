using Middle;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Data
{
	public class ReadWriteDataTheme : MonoBehaviour
	{
		private static string DB_NAME = "/themeInfor.dat";

		private ThemeSerializeData data = new ThemeSerializeData();

		private static int themeIDMax = 2;

		private static int statueMaxLevel = 6;

		private List<ConditionParameter> conditionParameters = new List<ConditionParameter>();

		private static ReadWriteDataTheme instance;

		public static ReadWriteDataTheme Instance => instance;

		private void Awake()
		{
			instance = this;
			SaveDefaultData();
			Load();
			ReadParameterUnlockTheme();
		}

		private void SaveDefaultData()
		{
			if (!File.Exists(Application.persistentDataPath + DB_NAME))
			{
				FileStream fileStream = File.Create(Application.persistentDataPath + DB_NAME);
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				data.ListThemeIDUnlocked = new List<int>();
				int item = 0;
				data.ListThemeIDUnlocked.Add(item);
				data.lastThemeIDPlayed = 0;
				binaryFormatter.Serialize(fileStream, data);
				fileStream.Close();
			}
		}

		public void SaveThemeUnlockData(int themeIDUnlocked)
		{
			FileStream fileStream = File.Create(Application.persistentDataPath + DB_NAME);
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			if (!data.ListThemeIDUnlocked.Contains(themeIDUnlocked))
			{
				data.ListThemeIDUnlocked.Add(themeIDUnlocked);
			}
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

		public void SaveLastThemePlayed(int themeID)
		{
			FileStream fileStream = File.Create(Application.persistentDataPath + DB_NAME);
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			data.lastThemeIDPlayed = themeID;
			binaryFormatter.Serialize(fileStream, data);
			fileStream.Close();
		}

		private void Load()
		{
			FileStream fileStream = File.Open(Application.persistentDataPath + DB_NAME, FileMode.Open);
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			data = (ThemeSerializeData)binaryFormatter.Deserialize(fileStream);
			fileStream.Close();
		}

		public int GetThemeIDUnlocked()
		{
			int num = -1;
			if (PlayerPrefs.GetInt("TESTunlockAllTheme", 0) > 0)
			{
				return themeIDMax;
			}
			return data.ListThemeIDUnlocked[data.ListThemeIDUnlocked.Count - 1];
		}

		public bool IsNextThemeUnlock(int themeID)
		{
			bool flag = false;
			if (PlayerPrefs.GetInt("TESTunlockAllTheme", 0) > 0)
			{
				return true;
			}
			return data.ListThemeIDUnlocked.Contains(themeID + 1);
		}

		public bool IsReachMaxTheme(int themeID)
		{
			bool flag = false;
			return themeID == themeIDMax;
		}

		public Dictionary<int, string> GetListCondition(int themeID)
		{
			Dictionary<int, string> dictionary = new Dictionary<int, string>();
			switch (themeID)
			{
			case 1:
			{
				foreach (ConditionParameter conditionParameter in conditionParameters)
				{
					if (conditionParameter.isEnable_theme1)
					{
						dictionary.Add(conditionParameter.id, conditionParameter.condition_theme1);
					}
				}
				return dictionary;
			}
			case 2:
			{
				foreach (ConditionParameter conditionParameter2 in conditionParameters)
				{
					if (conditionParameter2.isEnable_theme2)
					{
						dictionary.Add(conditionParameter2.id, conditionParameter2.condition_theme2);
					}
				}
				return dictionary;
			}
			default:
				return dictionary;
			}
		}

		public string GetDescription(int conditionType, int themeID)
		{
			string result = string.Empty;
			switch (themeID)
			{
			case 1:
				result = conditionParameters[conditionType].description_theme1;
				break;
			case 2:
				result = conditionParameters[conditionType].description_theme2;
				break;
			}
			return result;
		}

		public int GetLastThemeIDPlayed()
		{
			return data.lastThemeIDPlayed;
		}

		public int GetTotalTheme()
		{
			return themeIDMax + 1;
		}

		public void RestoreDataFromCloud(UserData_Theme restoredData)
		{
			data.lastThemeIDPlayed = restoredData.lastThemeIDPlayed;
			data.ListThemeIDUnlocked = new List<int>();
			foreach (int item in restoredData.listThemeIDUnlocked)
			{
				data.ListThemeIDUnlocked.Add(item);
			}
			UnityEngine.Debug.Log(data);
			SaveAll();
		}

		private void ReadParameterUnlockTheme()
		{
			string text = "Parameters/Conditions/theme_unlock_parameter_" + Config.Instance.LanguageID;
			try
			{
				List<Dictionary<string, object>> list = CSVReader.Read(text);
				for (int i = 0; i < list.Count; i++)
				{
					ConditionParameter conditionParameter = new ConditionParameter();
					conditionParameter.id = (int)list[i]["id"];
					conditionParameter.conditionType = (string)list[i]["condition_type"];
					conditionParameter.isEnable_theme1 = ((int)list[i]["is_enable_theme_1"] == 1);
					conditionParameter.condition_theme1 = (string)list[i]["condition_theme_1"];
					conditionParameter.description_theme1 = (string)list[i]["description_theme1"];
					conditionParameter.isEnable_theme2 = ((int)list[i]["is_enable_theme_2"] == 1);
					conditionParameter.condition_theme2 = (string)list[i]["condition_theme_2"];
					conditionParameter.description_theme2 = (string)list[i]["description_theme2"];
					conditionParameters.Add(conditionParameter);
				}
			}
			catch (Exception)
			{
				ShowError(text);
				throw;
			}
		}

		private static void ShowError(string filePath)
		{
			UnityEngine.Debug.LogError("File " + filePath + ".csv không tồn tại hoặc dữ liệu trong file không đúng định dạng.");
		}
	}
}

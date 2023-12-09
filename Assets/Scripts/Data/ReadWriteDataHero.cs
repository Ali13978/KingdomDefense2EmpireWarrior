using Middle;
using Parameter;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Data
{
	public class ReadWriteDataHero : MonoBehaviour
	{
		private static string DB_NAME = "/heroInfor.dat";

		private HeroSerializeData data = new HeroSerializeData();

		public static int maxLevel = 9;

		public static int maxSkillLevel = 3;

		private static ReadWriteDataHero instance;

		public static ReadWriteDataHero Instance => instance;

		public event Action OnSkillPointChangeEvent;

		public event Action OnHeroLevelChangeEvent;

		public event Action OnBuyNewHero;

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
				data.ListHeroesData = new Dictionary<int, HeroData>();
				int num = 20;
				for (int i = 0; i < num; i++)
				{
					HeroData heroData = new HeroData();
					heroData.level = 0;
					data.ListHeroesData.Add(i, heroData);
				}
				data.ListHeroOwned = new List<int>();
				int item = 2;
				data.ListHeroOwned.Add(item);
				InitDefaultDataSkillPoint();
				InitDefaultDataPet();
				binaryFormatter.Serialize(fileStream, data);
				fileStream.Close();
			}
		}

		public void Save(int heroID, int level, int totalExp)
		{
			FileStream fileStream = File.Create(Application.persistentDataPath + DB_NAME);
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			data.ListHeroesData[heroID].level = level;
			data.ListHeroesData[heroID].totalExp = totalExp;
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
			data = (HeroSerializeData)binaryFormatter.Deserialize(fileStream);
			fileStream.Close();
		}

		public int GetCurrentHeroLevel(int heroID)
		{
			int num = -1;
			switch (ModeManager.Instance.gameMode)
			{
			case GameMode.CampaignMode:
				return data.ListHeroesData[heroID].level;
			case GameMode.DailyTrialMode:
				return maxLevel;
			case GameMode.TournamentMode:
				return data.ListHeroesData[heroID].level;
			default:
				return data.ListHeroesData[heroID].level;
			}
		}

		public int GetCurrentHeroTotalExp(int heroID)
		{
			return data.ListHeroesData[heroID].totalExp;
		}

		public void AddExp(int heroID, int amountEXP)
		{
			if (IsReachMaxLevel(heroID))
			{
				return;
			}
			data.ListHeroesData[heroID].totalExp += amountEXP;
			if (data.ListHeroesData[heroID].totalExp >= HeroParameter.Instance.GetEXPForCurrentLevel(heroID, 9))
			{
				UnityEngine.Debug.Log("Hero đã đạt max level");
				Save(heroID, 9, HeroParameter.Instance.GetEXPForCurrentLevel(heroID, 9));
			}
			if (amountEXP >= HeroParameter.Instance.GetEXPForCurrentLevel(heroID, 9))
			{
				UnityEngine.Debug.Log("Hero nhận được nhiều exp quá max level luôn rồi");
				Save(heroID, 9, HeroParameter.Instance.GetEXPForCurrentLevel(heroID, 9));
				return;
			}
			for (int i = 0; i < 10; i++)
			{
				if (data.ListHeroesData[heroID].totalExp < HeroParameter.Instance.GetEXPForCurrentLevel(heroID, i))
				{
					data.ListHeroesData[heroID].level = i;
					break;
				}
			}
			Save(heroID, data.ListHeroesData[heroID].level, data.ListHeroesData[heroID].totalExp);
		}

		public void LevelUpTo(int heroID, int heroLevel)
		{
			for (int i = 0; i < heroLevel; i++)
			{
				LevelUp(heroID);
			}
		}

		public void LevelUp(int heroID)
		{
			int currentHeroLevel = GetCurrentHeroLevel(heroID);
			if (!IsReachMaxLevel(heroID))
			{
				UnityEngine.Debug.Log("Current exp = " + data.ListHeroesData[heroID].totalExp);
				UnityEngine.Debug.Log("Exp level tiep theo = " + HeroParameter.Instance.GetEXPForCurrentLevel(heroID, currentHeroLevel));
				UnityEngine.Debug.Log("Exp can thiet de len level = " + GetExpToLevelUp(heroID));
				AddExp(heroID, GetExpToLevelUp(heroID));
			}
		}

		public bool IsReachMaxLevel(int heroID)
		{
			bool result = false;
			int currentHeroLevel = GetCurrentHeroLevel(heroID);
			if (currentHeroLevel >= maxLevel)
			{
				UnityEngine.Debug.Log("Hero đã max level!");
				result = true;
			}
			return result;
		}

		public int GetExpToLevelUp(int heroID)
		{
			int num = 0;
			int currentHeroLevel = GetCurrentHeroLevel(heroID);
			return HeroParameter.Instance.GetEXPForCurrentLevel(heroID, currentHeroLevel) - data.ListHeroesData[heroID].totalExp;
		}

		public int GetCurrentExp(int heroID)
		{
			int num = 0;
			int currentHeroLevel = GetCurrentHeroLevel(heroID);
			if (currentHeroLevel > 0)
			{
				return GetCurrentHeroTotalExp(heroID) - HeroParameter.Instance.GetEXPForCurrentLevel(heroID, currentHeroLevel - 1);
			}
			return GetCurrentHeroTotalExp(heroID);
		}

		public void OnLevelChange(bool isDispatchEventChange)
		{
			if (isDispatchEventChange && this.OnHeroLevelChangeEvent != null)
			{
				this.OnHeroLevelChangeEvent();
			}
		}

		public int GetHeroOwnedAmount()
		{
			return data.ListHeroOwned.Count;
		}

		public List<int> GetListHeroIDOwned()
		{
			return data.ListHeroOwned;
		}

		public bool IsHeroOwned(int heroID)
		{
			return data.ListHeroOwned.Contains(heroID);
		}

		public bool IsHeroOwned(List<int> listHeroID)
		{
			bool result = false;
			foreach (int item in listHeroID)
			{
				if (IsHeroOwned(item))
				{
					result = true;
				}
			}
			return result;
		}

		public void UnlockHero(int heroID)
		{
			data.ListHeroOwned.Add(heroID);
			SaveAll();
			UnityEngine.Debug.Log("Unlock Hero " + heroID);
			if (this.OnBuyNewHero != null)
			{
				this.OnBuyNewHero();
			}
		}

		public List<int> GetListHeroIDNotOwned()
		{
			List<int> list = new List<int>();
			List<int> listHeroID = HeroParameter.Instance.GetListHeroID();
			foreach (int item in listHeroID)
			{
				if (!IsHeroOwned(item))
				{
					list.Add(item);
				}
			}
			return list;
		}

		public void IncreaseSkillLevel(int heroID, int skillID)
		{
			if (data.ListHeroesData[heroID].skillPoints == null)
			{
				InitDefaultDataSkillPoint();
				SaveAll();
			}
			data.ListHeroesData[heroID].skillPoints[skillID]++;
			SaveAll();
		}

		public int GetSkillPoint(int heroID, int skillID)
		{
			int num = -1;
			if (data.ListHeroesData[heroID].skillPoints != null)
			{
				return data.ListHeroesData[heroID].skillPoints[skillID];
			}
			InitDefaultDataSkillPoint();
			SaveAll();
			return data.ListHeroesData[heroID].skillPoints[skillID];
		}

		private void InitDefaultDataSkillPoint()
		{
			int num = 20;
			for (int i = 0; i < num; i++)
			{
				data.ListHeroesData[i].skillPoints = new int[4]
				{
					1,
					0,
					0,
					0
				};
			}
		}

		public int GetCurrentSkillPoint(int heroID)
		{
			int num = -1;
			int currentHeroLevel = GetCurrentHeroLevel(heroID);
			return GetTotalSkillPoint(heroID, currentHeroLevel) - GetUsedSkillPoint(heroID);
		}

		private int GetTotalSkillPoint(int heroID, int heroLevel)
		{
			return HeroParameter.Instance.GetTotalSkillPoint(heroID, heroLevel);
		}

		public int GetUsedSkillPoint(int heroID)
		{
			int num = 0;
			if (data.ListHeroesData[heroID].skillPoints == null)
			{
				InitDefaultDataSkillPoint();
				SaveAll();
			}
			for (int i = 0; i < data.ListHeroesData[heroID].skillPoints.Length; i++)
			{
				num += data.ListHeroesData[heroID].skillPoints[i];
			}
			return num;
		}

		public bool IsMaxSkill(int heroID, int skillID)
		{
			bool flag = false;
			return GetSkillPoint(heroID, skillID) == maxSkillLevel;
		}

		public void ResetSkillPoint(int heroID)
		{
			if (data.ListHeroesData[heroID].skillPoints == null)
			{
				InitDefaultDataSkillPoint();
				SaveAll();
			}
			data.ListHeroesData[heroID].skillPoints[0] = 1;
			data.ListHeroesData[heroID].skillPoints[1] = 0;
			data.ListHeroesData[heroID].skillPoints[2] = 0;
			data.ListHeroesData[heroID].skillPoints[3] = 0;
			SaveAll();
		}

		public void OnSkillPointChange(bool isDispatchEventChange)
		{
			if (isDispatchEventChange && this.OnSkillPointChangeEvent != null)
			{
				this.OnSkillPointChangeEvent();
			}
		}

		public int GetHeroOwnPetAmount()
		{
			return GetListHeroIDOwnPet().Count;
		}

		public List<int> GetListHeroIDOwnPet()
		{
			List<int> list = new List<int>();
			List<int> listHeroIDOwned = GetListHeroIDOwned();
			foreach (int item in listHeroIDOwned)
			{
				if (IsPetUnlocked(item))
				{
					list.Add(item);
				}
			}
			return list;
		}

		private void InitDefaultDataPet()
		{
			int num = 20;
			for (int i = 0; i < num; i++)
			{
				data.ListHeroesData[i].havePet = false;
			}
		}

		public void UnlockPet(int heroID)
		{
			data.ListHeroesData[heroID].havePet = true;
			SaveAll();
		}

		public bool IsPetUnlocked(int heroID)
		{
			bool flag = false;
			switch (ModeManager.Instance.gameMode)
			{
			case GameMode.CampaignMode:
				return data.ListHeroesData[heroID].havePet;
			case GameMode.DailyTrialMode:
				return true;
			case GameMode.TournamentMode:
				return data.ListHeroesData[heroID].havePet;
			default:
				return data.ListHeroesData[heroID].havePet;
			}
		}

		public bool IsPetAvailable(int petID)
		{
			return true;
		}

		public void RestoreDataFromCloud(UserData_Hero restoredData)
		{
			foreach (UserData_Hero_Unique listHeroDatum in restoredData.listHeroData)
			{
				HeroData heroData = new HeroData();
				heroData.level = listHeroDatum.level;
				heroData.totalExp = listHeroDatum.exp;
				if (listHeroDatum.isOwned && !data.ListHeroOwned.Contains(listHeroDatum.id))
				{
					data.ListHeroOwned.Add(listHeroDatum.id);
				}
				heroData.havePet = listHeroDatum.ownedPet;
				heroData.skillPoints = new int[4];
				for (int i = 0; i < heroData.skillPoints.Length; i++)
				{
					heroData.skillPoints[i] = listHeroDatum.skillUpgraded[i];
				}
				data.ListHeroesData[listHeroDatum.id] = heroData;
			}
			UnityEngine.Debug.Log(data);
			SaveAll();
		}
	}
}

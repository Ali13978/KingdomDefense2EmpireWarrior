using Data;
using Middle;
using System.Collections.Generic;

namespace Parameter
{
	public class HeroParameter
	{
		public List<List<Hero>> listHero = new List<List<Hero>>();

		private static HeroParameter instance;

		public static HeroParameter Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new HeroParameter();
				}
				return instance;
			}
		}

		public void SetHeroParameter(Hero hero)
		{
			int count = listHero.Count;
			if (count <= hero.id)
			{
				List<Hero> list = new List<Hero>();
				list.Insert(hero.level, hero);
				listHero.Insert(hero.id, list);
			}
			else
			{
				List<Hero> list2 = listHero[hero.id];
				list2.Insert(hero.level, hero);
			}
		}

		public Hero GetHeroParameter(int id, int level)
		{
			if (CheckParameter(id, level))
			{
				return listHero[id][level];
			}
			return default(Hero);
		}

		public Hero GetPetParameter(PetConfigData petConfigData)
		{
			Hero result = default(Hero);
			result.id = petConfigData.Id + 1000;
			result.name = petConfigData.Petname;
			result.level = 1;
			result.respawn_time = petConfigData.Respawn_time;
			result.health = petConfigData.Health;
			result.health_regen = petConfigData.Health_regen;
			result.health_regen_cooldown = petConfigData.Health_regen_cooldown;
			result.armor_physics = petConfigData.Armor_physics;
			result.armor_magic = petConfigData.Armor_magic;
			result.critical_strike_change = 0;
			result.attack_physics_min = petConfigData.Atk_physics_min;
			result.attack_physics_max = petConfigData.Atk_physics_max;
			result.attack_magic_min = petConfigData.Atk_magic_min;
			result.attack_magic_max = petConfigData.Atk_magic_max;
			result.attack_cooldown = petConfigData.Atk_cooldown;
			result.attack_range_min = petConfigData.Atk_range_min;
			result.attack_range_max = petConfigData.Atk_range_max;
			result.attack_range_average = petConfigData.Atk_range_avg;
			result.speed = petConfigData.Speed;
			result.canAttackAir = petConfigData.Can_attack_air;
			return result;
		}

		private bool CheckParameter(int id, int level)
		{
			if (id >= GetNumberOfHero() || level > GetNumberOfLevel())
			{
				return false;
			}
			return true;
		}

		public int GetNumberOfHero()
		{
			return listHero.Count;
		}

		public int GetNumberOfLevel()
		{
			if (GetNumberOfHero() > 0)
			{
				return listHero[0].Count;
			}
			return 0;
		}

		public List<int> GetListHeroID()
		{
			List<int> list = new List<int>();
			for (int i = 0; i <= listHero.Count - 1; i++)
			{
				list.Add(i);
			}
			return list;
		}

		public List<int> GetListPetID()
		{
			List<int> list = new List<int>();
			List<int> listHeroID = GetListHeroID();
			foreach (int item in listHeroID)
			{
				list.Add(GetPetID(item));
			}
			return list;
		}

		public string GetHeroName(int heroId)
		{
			if (heroId >= 0 && heroId < listHero.Count)
			{
				Hero hero = listHero[heroId][0];
				return hero.name;
			}
			return "_";
		}

		public int GetHeroHealth(int heroId, int heroLevel)
		{
			if (heroId >= 0 && heroId < listHero.Count)
			{
				Hero hero = listHero[heroId][heroLevel];
				return hero.health;
			}
			return 0;
		}

		public int GetHeroHealthRegen(int heroId, int heroLevel)
		{
			if (heroId >= 0 && heroId < listHero.Count)
			{
				Hero hero = listHero[heroId][heroLevel];
				return hero.health_regen;
			}
			return 0;
		}

		public int GetHeroAttackCooldown(int heroId, int heroLevel)
		{
			if (heroId >= 0 && heroId < listHero.Count)
			{
				Hero hero = listHero[heroId][heroLevel];
				return hero.attack_cooldown;
			}
			return 0;
		}

		public bool IsPhysicsAttack(int heroId)
		{
			bool result = false;
			if (heroId >= 0 && heroId < listHero.Count)
			{
				Hero hero = listHero[heroId][0];
				result = (hero.attack_physics_min > 0);
			}
			return result;
		}

		public bool IsMagicAttack(int heroId)
		{
			bool result = false;
			if (heroId >= 0 && heroId < listHero.Count)
			{
				Hero hero = listHero[heroId][0];
				result = (hero.attack_magic_min > 0);
			}
			return result;
		}

		public bool CanAttackAir(int heroId)
		{
			bool result = false;
			if (heroId >= 0 && heroId < listHero.Count)
			{
				Hero hero = listHero[heroId][0];
				result = (hero.canAttackAir == 1);
			}
			return result;
		}

		public int GetHeroDamageMax(int heroId, int heroLevel)
		{
			int num = -1;
			if (heroId >= 0 && heroId < listHero.Count)
			{
				if (IsPhysicsAttack(heroId))
				{
					Hero hero = listHero[heroId][heroLevel];
					return hero.attack_physics_max;
				}
				Hero hero2 = listHero[heroId][heroLevel];
				return hero2.attack_magic_max;
			}
			return 0;
		}

		public string GetHeroDamageRange(int heroId, int heroLevel)
		{
			string empty = string.Empty;
			if (heroId >= 0 && heroId < listHero.Count)
			{
				if (IsPhysicsAttack(heroId))
				{
					Hero hero = listHero[heroId][heroLevel];
					object arg = hero.attack_physics_min;
					Hero hero2 = listHero[heroId][heroLevel];
					return arg + " - " + hero2.attack_physics_max;
				}
				Hero hero3 = listHero[heroId][heroLevel];
				object arg2 = hero3.attack_magic_min;
				Hero hero4 = listHero[heroId][heroLevel];
				return arg2 + " - " + hero4.attack_magic_max;
			}
			return "_";
		}

		public int GetHeroAttackRange(int heroId, int heroLevel)
		{
			if (heroId >= 0 && heroId < listHero.Count)
			{
				Hero hero = listHero[heroId][heroLevel];
				return hero.attack_range_max;
			}
			return 0;
		}

		public int GetHeroPhysicsArmor(int heroId, int heroLevel)
		{
			if (heroId >= 0 && heroId < listHero.Count)
			{
				Hero hero = listHero[heroId][heroLevel];
				return hero.armor_physics;
			}
			return 0;
		}

		public int GetHeroMagicArmor(int heroId, int heroLevel)
		{
			if (heroId >= 0 && heroId < listHero.Count)
			{
				Hero hero = listHero[heroId][heroLevel];
				return hero.armor_magic;
			}
			return 0;
		}

		public int GetHeroMovementSpeed(int heroId, int heroLevel)
		{
			if (heroId >= 0 && heroId < listHero.Count)
			{
				Hero hero = listHero[heroId][heroLevel];
				return hero.speed;
			}
			return 0;
		}

		public int GetSkillPoint(int heroId, int heroLevel, int skillId)
		{
			int result = -1;
			switch (ModeManager.Instance.gameMode)
			{
			case GameMode.CampaignMode:
				result = ((heroId < 0 || heroId >= listHero.Count) ? (-1) : ReadWriteDataHero.Instance.GetSkillPoint(heroId, skillId));
				break;
			case GameMode.DailyTrialMode:
				result = ReadWriteDataHero.maxSkillLevel;
				break;
			case GameMode.TournamentMode:
				result = ((heroId < 0 || heroId >= listHero.Count) ? (-1) : ReadWriteDataHero.Instance.GetSkillPoint(heroId, skillId));
				break;
			}
			return result;
		}

		public int GetTotalSkillPoint(int heroID, int heroLevel)
		{
			int num = 0;
			for (int i = 0; i <= heroLevel; i++)
			{
				int num2 = num;
				Hero hero = listHero[heroID][i];
				num = num2 + hero.skillPointBonus;
			}
			return num;
		}

		public int GetEXPForCurrentLevel(int heroId, int heroLevel)
		{
			if (heroId >= 0 && heroId < listHero.Count)
			{
				Hero hero = listHero[heroId][heroLevel];
				return hero.exp_per_level;
			}
			return 0;
		}

		public int GetEXPForNextLevel(int heroId, int heroLevel)
		{
			if (heroId >= 0 && heroId < listHero.Count)
			{
				if (heroLevel == 0)
				{
					Hero hero = listHero[heroId][heroLevel];
					return hero.exp_per_level;
				}
				Hero hero2 = listHero[heroId][heroLevel];
				int exp_per_level = hero2.exp_per_level;
				Hero hero3 = listHero[heroId][heroLevel - 1];
				return exp_per_level - hero3.exp_per_level;
			}
			return 0;
		}

		public int GetPetID(int heroID)
		{
			return heroID + 1000;
		}
	}
}

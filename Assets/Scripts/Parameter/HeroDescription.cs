using System.Collections.Generic;

namespace Parameter
{
	public class HeroDescription : Singleton<HeroDescription>
	{
		public List<List<HeroDes>> listHeroDes = new List<List<HeroDes>>();

		public void ClearData()
		{
			listHeroDes.Clear();
		}

		public void SetHeroParameter(HeroDes hero)
		{
			int count = listHeroDes.Count;
			if (count <= hero.id)
			{
				List<HeroDes> list = new List<HeroDes>();
				list.Insert(hero.skillID, hero);
				listHeroDes.Insert(hero.id, list);
			}
			else
			{
				List<HeroDes> list2 = listHeroDes[hero.id];
				list2.Insert(hero.skillID, hero);
			}
		}

		public HeroDes GetHeroParameter(int id, int level)
		{
			if (CheckParameter(id, level))
			{
				return listHeroDes[id][level];
			}
			return default(HeroDes);
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
			return listHeroDes.Count;
		}

		public int GetNumberOfLevel()
		{
			if (GetNumberOfHero() > 0)
			{
				return listHeroDes[0].Count;
			}
			return 0;
		}

		public string GetHeroName(int heroId)
		{
			if (heroId >= 0 && heroId < listHeroDes.Count)
			{
				HeroDes heroDes = listHeroDes[heroId][0];
				return heroDes.name;
			}
			return "_";
		}

		public string GetHeroShortDescription(int heroId)
		{
			if (heroId >= 0 && heroId < listHeroDes.Count)
			{
				HeroDes heroDes = listHeroDes[heroId][0];
				return heroDes.shortDescription;
			}
			return "_";
		}

		public string GetHeroFullDescription(int heroId)
		{
			if (heroId >= 0 && heroId < listHeroDes.Count)
			{
				HeroDes heroDes = listHeroDes[heroId][0];
				return heroDes.fullDescription;
			}
			return "_";
		}

		public string GetHeroSkillName(int heroId, int skillID)
		{
			if (heroId >= 0 && heroId < listHeroDes.Count)
			{
				HeroDes heroDes = listHeroDes[heroId][skillID];
				return heroDes.skillName;
			}
			return "_";
		}

		public string GetHeroSkillType(int heroId, int skillID)
		{
			if (heroId >= 0 && heroId < listHeroDes.Count)
			{
				HeroDes heroDes = listHeroDes[heroId][skillID];
				return heroDes.skillType;
			}
			return "_";
		}

		public string GetHeroSkillDescription(int heroId, int skillID)
		{
			if (heroId >= 0 && heroId < listHeroDes.Count)
			{
				HeroDes heroDes = listHeroDes[heroId][skillID];
				return heroDes.skillDescription;
			}
			return "_";
		}

		public string GetHeroSkillUnlock(int heroId, int skillID)
		{
			if (heroId >= 0 && heroId < listHeroDes.Count)
			{
				HeroDes heroDes = listHeroDes[heroId][skillID];
				return heroDes.skillUnlock;
			}
			return "_";
		}
	}
}

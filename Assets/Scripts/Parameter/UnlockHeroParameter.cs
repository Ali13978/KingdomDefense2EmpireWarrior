using Data;
using System.Collections.Generic;

namespace Parameter
{
	public class UnlockHeroParameter : Singleton<UnlockHeroParameter>
	{
		private List<HeroUnlockParam> listHeroUnlockParam = new List<HeroUnlockParam>();

		public void SetHeroUnlockParamParameter(HeroUnlockParam heroUnlockParam)
		{
			int count = listHeroUnlockParam.Count;
			if (count <= heroUnlockParam.id)
			{
				listHeroUnlockParam.Add(heroUnlockParam);
			}
		}

		public bool IsHeroUnlockByPlay(int heroID)
		{
			bool result = false;
			if (heroID < listHeroUnlockParam.Count && heroID >= 0)
			{
				HeroUnlockParam heroUnlockParam = listHeroUnlockParam[heroID];
				result = (heroUnlockParam.isUnlockByPlay == 1);
			}
			return result;
		}

		public int GetMapIDToUnlockHero(int heroID)
		{
			if (heroID < listHeroUnlockParam.Count && heroID >= 0)
			{
				HeroUnlockParam heroUnlockParam = listHeroUnlockParam[heroID];
				return heroUnlockParam.mapIDToUnlock;
			}
			return -1;
		}

		public bool IsHeroAvailableToUnlock(int heroID)
		{
			return ReadWriteDataMap.Instance.GetMapIDUnlocked() >= GetMapIDToUnlockHero(heroID);
		}

		public bool IsHeroUnlockByGem(int heroID)
		{
			bool result = false;
			if (heroID < listHeroUnlockParam.Count && heroID >= 0)
			{
				HeroUnlockParam heroUnlockParam = listHeroUnlockParam[heroID];
				result = (heroUnlockParam.isUnlockByGem == 1);
			}
			return result;
		}

		public bool IsHeroAvailableToBuy(int heroID)
		{
			return ReadWriteDataPlayerCurrency.Instance.GetCurrentGem() >= GetGemAmountToUnlockHero(heroID);
		}

		public int GetGemAmountToUnlockHero(int heroID)
		{
			if (heroID < listHeroUnlockParam.Count && heroID >= 0)
			{
				HeroUnlockParam heroUnlockParam = listHeroUnlockParam[heroID];
				return heroUnlockParam.gemAmountToUnlock;
			}
			return -1;
		}

		public HeroUnlockParam GetWeapon(int heroID)
		{
			return listHeroUnlockParam[heroID];
		}

		public List<int> GetListHeroNotOwnedByPlay()
		{
			List<int> list = new List<int>();
			foreach (HeroUnlockParam item in listHeroUnlockParam)
			{
				HeroUnlockParam current = item;
				if (current.isUnlockByPlay == 1 && !ReadWriteDataHero.Instance.IsHeroOwned(current.id))
				{
					list.Add(current.id);
				}
			}
			return list;
		}

		public List<int> GetListHeroNotOwnedByGem()
		{
			List<int> list = new List<int>();
			foreach (HeroUnlockParam item in listHeroUnlockParam)
			{
				HeroUnlockParam current = item;
				if (current.isUnlockByGem == 1 && !ReadWriteDataHero.Instance.IsHeroOwned(current.id))
				{
					list.Add(current.id);
				}
			}
			return list;
		}

		public bool IsHeroUnlockByRealMoney(int heroID)
		{
			bool result = false;
			if (heroID < listHeroUnlockParam.Count && heroID >= 0)
			{
				HeroUnlockParam heroUnlockParam = listHeroUnlockParam[heroID];
				result = (heroUnlockParam.isUnlockByRealMoney == 1);
			}
			return result;
		}
	}
}

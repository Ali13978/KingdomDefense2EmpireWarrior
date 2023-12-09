using Data;
using Parameter;
using System.Collections.Generic;

namespace Notify
{
	public class NotifyHeroCamp : NotifyUnit
	{
		protected override bool ShouldShowNotify()
		{
			return isAvailable();
		}

		private bool isAvailable()
		{
			bool result = false;
			if (isEnoughGemToUpgrade())
			{
				result = true;
			}
			if (isAvailableToBuy())
			{
				result = true;
			}
			if (isAvailableToUnlock())
			{
				result = true;
			}
			if (isAvailableToUpgradeHeroSkill())
			{
				result = true;
			}
			return result;
		}

		private bool isEnoughGemToUpgrade()
		{
			bool result = false;
			List<int> listHeroIDOwned = ReadWriteDataHero.Instance.GetListHeroIDOwned();
			foreach (int item in listHeroIDOwned)
			{
				if (HeroesLevelGemCalculator.IsEnoughGemToUpgrade(item))
				{
					result = true;
				}
			}
			return result;
		}

		private bool isAvailableToUnlock()
		{
			bool result = false;
			List<int> listHeroNotOwnedByPlay = Singleton<UnlockHeroParameter>.Instance.GetListHeroNotOwnedByPlay();
			foreach (int item in listHeroNotOwnedByPlay)
			{
				if (Singleton<UnlockHeroParameter>.Instance.IsHeroAvailableToUnlock(item))
				{
					result = true;
				}
			}
			return result;
		}

		private bool isAvailableToBuy()
		{
			bool result = false;
			List<int> listHeroNotOwnedByGem = Singleton<UnlockHeroParameter>.Instance.GetListHeroNotOwnedByGem();
			foreach (int item in listHeroNotOwnedByGem)
			{
				if (Singleton<UnlockHeroParameter>.Instance.IsHeroAvailableToBuy(item))
				{
					result = true;
				}
			}
			return result;
		}

		private bool isAvailableToUpgradeHeroSkill()
		{
			bool result = false;
			List<int> listHeroIDOwned = ReadWriteDataHero.Instance.GetListHeroIDOwned();
			foreach (int item in listHeroIDOwned)
			{
				if (ReadWriteDataHero.Instance.GetCurrentSkillPoint(item) >= 1)
				{
					result = true;
				}
			}
			return result;
		}
	}
}

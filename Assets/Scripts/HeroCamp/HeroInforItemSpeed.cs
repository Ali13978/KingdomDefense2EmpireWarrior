using Data;
using Parameter;
using UnityEngine;

namespace HeroCamp
{
	public class HeroInforItemSpeed : HeroInformationItem
	{
		private string valueCurrentLevel;

		private string valueNextLevel;

		public override void Init(int heroID, int heroLevel)
		{
			base.Init(heroID, heroLevel);
			valueCurrentLevel = UnitAbilitiesRanking.Instance.GetAttackSpeedDescriptionByValue(HeroParameter.Instance.GetHeroAttackCooldown(heroID, heroLevel));
			base.CurrentLevelValue.text = valueCurrentLevel;
			if (heroLevel + 1 > base.HeroMaxLevel)
			{
				heroLevel = base.HeroMaxLevel - 1;
			}
			valueNextLevel = UnitAbilitiesRanking.Instance.GetAttackSpeedDescriptionByValue(HeroParameter.Instance.GetHeroAttackCooldown(heroID, heroLevel + 1));
			base.NextLevelValue.text = valueNextLevel;
			if (valueNextLevel != valueCurrentLevel)
			{
				base.NextLevelValue.color = Color.green;
			}
			else
			{
				base.NextLevelValue.color = Color.white;
			}
		}
	}
}

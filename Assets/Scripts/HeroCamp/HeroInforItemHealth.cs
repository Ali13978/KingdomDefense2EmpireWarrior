using Parameter;
using UnityEngine;

namespace HeroCamp
{
	public class HeroInforItemHealth : HeroInformationItem
	{
		public override void Init(int heroID, int heroLevel)
		{
			base.Init(heroID, heroLevel);
			base.CurrentLevelValue.text = HeroParameter.Instance.GetHeroHealth(heroID, heroLevel).ToString();
			if (heroLevel + 1 > base.HeroMaxLevel)
			{
				heroLevel = base.HeroMaxLevel - 1;
			}
			base.NextLevelValue.text = HeroParameter.Instance.GetHeroHealth(heroID, heroLevel + 1).ToString();
			if (HeroParameter.Instance.GetHeroHealth(heroID, heroLevel + 1) > HeroParameter.Instance.GetHeroHealth(heroID, heroLevel))
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

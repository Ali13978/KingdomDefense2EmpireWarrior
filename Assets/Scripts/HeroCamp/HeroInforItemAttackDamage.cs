using Parameter;
using UnityEngine;
using UnityEngine.UI;

namespace HeroCamp
{
	public class HeroInforItemAttackDamage : HeroInformationItem
	{
		[SerializeField]
		private Image iconAttack;

		[SerializeField]
		private Sprite physicsAtkSprite;

		[SerializeField]
		private Sprite magicAtkSprite;

		public override void Init(int heroID, int heroLevel)
		{
			base.Init(heroID, heroLevel);
			base.CurrentLevelValue.text = HeroParameter.Instance.GetHeroDamageRange(heroID, heroLevel);
			if (heroLevel + 1 > base.HeroMaxLevel)
			{
				heroLevel = base.HeroMaxLevel - 1;
			}
			base.NextLevelValue.text = HeroParameter.Instance.GetHeroDamageRange(heroID, heroLevel + 1);
			if (HeroParameter.Instance.GetHeroDamageMax(heroID, heroLevel + 1) > HeroParameter.Instance.GetHeroDamageMax(heroID, heroLevel))
			{
				base.NextLevelValue.color = Color.green;
			}
			else
			{
				base.NextLevelValue.color = Color.white;
			}
			if (HeroParameter.Instance.IsPhysicsAttack(heroID))
			{
				iconAttack.sprite = physicsAtkSprite;
			}
			else
			{
				iconAttack.sprite = magicAtkSprite;
			}
		}
	}
}

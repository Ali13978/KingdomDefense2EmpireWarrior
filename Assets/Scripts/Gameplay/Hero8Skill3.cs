using Data;
using Parameter;

namespace Gameplay
{
	public class Hero8Skill3 : HeroSkillCommon
	{
		private int heroID = 8;

		private int skillID = 3;

		private int currentLevel;

		private int currentSkillLevel;

		private bool unLock;

		private int attackRangeBonusPercentage;

		private string buffKey = "BuffAttackRangeByPercentage";

		private HeroModel heroModel;

		private string description;

		public override void Init(HeroModel heroModel)
		{
			base.Init(heroModel);
			unLock = true;
			this.heroModel = heroModel;
			currentLevel = ReadWriteDataHero.Instance.GetCurrentHeroLevel(heroID);
			currentSkillLevel = HeroParameter.Instance.GetSkillPoint(heroID, currentLevel, skillID);
			HeroSkillParameter_8_3 heroSkillParameter_8_ = new HeroSkillParameter_8_3();
			heroSkillParameter_8_ = (HeroSkillParameter_8_3)HeroSkillParameter.Instance.GetHeroSkillsParameter(heroID, skillID);
			attackRangeBonusPercentage = heroSkillParameter_8_.getParam(currentSkillLevel - 1).attack_range_bonus_percentage;
			description = heroSkillParameter_8_.getParam(currentSkillLevel - 1).description;
			CustomInvoke(BuffAttackRange, 1f);
		}

		private void BuffAttackRange()
		{
			heroModel.BuffsHolder.AddBuff(buffKey, new Buff(isPositive: true, attackRangeBonusPercentage, 999999f), BuffStackLogic.ChooseMax, BuffStackLogic.ChooseMax);
		}
	}
}

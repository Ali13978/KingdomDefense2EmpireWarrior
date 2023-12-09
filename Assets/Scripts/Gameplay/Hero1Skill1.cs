using Data;
using Parameter;

namespace Gameplay
{
	public class Hero1Skill1 : HeroSkillCommon
	{
		private int heroID = 1;

		private int skillID = 1;

		private int currentLevel;

		private int currentSkillLevel;

		private bool unLock;

		private HeroModel heroModel;

		private int passiveCriticalStrikeValue;

		public override void Init(HeroModel heroModel)
		{
			base.Init(heroModel);
			unLock = true;
			this.heroModel = heroModel;
			currentLevel = ReadWriteDataHero.Instance.GetCurrentHeroLevel(heroID);
			currentSkillLevel = HeroParameter.Instance.GetSkillPoint(heroID, currentLevel, skillID);
			HeroSkillParameter_1_1 heroSkillParameter_1_ = new HeroSkillParameter_1_1();
			heroSkillParameter_1_ = (HeroSkillParameter_1_1)HeroSkillParameter.Instance.GetHeroSkillsParameter(heroID, skillID);
			passiveCriticalStrikeValue = heroSkillParameter_1_.getParam(currentSkillLevel - 1).bonus_crit;
		}

		public void Init(bool unlock, HeroModel heroModel, int _bonusCrit)
		{
			unLock = unlock;
			this.heroModel = heroModel;
			passiveCriticalStrikeValue = _bonusCrit;
			heroModel.OnAttackEvent += HeroModel_OnAttackEvent;
		}

		private void HeroModel_OnAttackEvent()
		{
			if (unLock)
			{
				AddPassiveCriticalStrike();
			}
		}

		private void AddPassiveCriticalStrike()
		{
			heroModel.HeroAttackController.CurrentCriticalStrikeChance = heroModel.HeroAttackController.OriginCriticalStrikeChance + passiveCriticalStrikeValue;
		}
	}
}

using Data;
using Parameter;

namespace Gameplay
{
	public class Hero5Skill1 : HeroSkillCommon
	{
		private int heroID = 5;

		private int skillID = 1;

		private int currentLevel;

		private int currentSkillLevel;

		private bool unLock;

		private HeroModel heroModel;

		private int bonusDamage;

		private float duration;

		private string description;

		private string buffkey = "IncreaseDamagePhysics";

		public override void Init(HeroModel heroModel)
		{
			base.Init(heroModel);
			unLock = true;
			this.heroModel = heroModel;
			currentLevel = ReadWriteDataHero.Instance.GetCurrentHeroLevel(heroID);
			currentSkillLevel = HeroParameter.Instance.GetSkillPoint(heroID, currentLevel, skillID);
			HeroSkillParameter_5_1 heroSkillParameter_5_ = new HeroSkillParameter_5_1();
			heroSkillParameter_5_ = (HeroSkillParameter_5_1)HeroSkillParameter.Instance.GetHeroSkillsParameter(heroID, skillID);
			bonusDamage = heroSkillParameter_5_.getParam(currentSkillLevel - 1).bonus_damage;
			duration = (float)heroSkillParameter_5_.getParam(currentSkillLevel - 1).duration / 1000f;
			description = heroSkillParameter_5_.getParam(currentSkillLevel - 1).description;
			CustomInvoke(BuffPhysicsDamage, 1f);
		}

		private void BuffPhysicsDamage()
		{
			heroModel.BuffsHolder.AddBuff(buffkey, new Buff(isPositive: true, bonusDamage, duration), BuffStackLogic.StackUp, BuffStackLogic.ChooseMax);
		}
	}
}

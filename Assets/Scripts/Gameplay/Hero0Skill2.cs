using Data;
using Parameter;

namespace Gameplay
{
	public class Hero0Skill2 : HeroSkillCommon
	{
		private int heroID;

		private int skillID = 2;

		private int currentLevel;

		private int currentSkillLevel;

		private bool unLock;

		private HeroModel heroModel;

		private float armor;

		public override void Init(HeroModel heroModel)
		{
			base.Init(heroModel);
			unLock = true;
			this.heroModel = heroModel;
			currentLevel = ReadWriteDataHero.Instance.GetCurrentHeroLevel(heroID);
			currentSkillLevel = HeroParameter.Instance.GetSkillPoint(heroID, currentLevel, skillID);
			HeroSkillParameter_0_2 heroSkillParameter_0_ = new HeroSkillParameter_0_2();
			heroSkillParameter_0_ = (HeroSkillParameter_0_2)HeroSkillParameter.Instance.GetHeroSkillsParameter(heroID, skillID);
			armor = (float)heroSkillParameter_0_.getParam(currentSkillLevel - 1).armor / 100f;
			AddPassiveArmor();
		}

		private void AddPassiveArmor()
		{
			if (unLock)
			{
				heroModel.HeroHealthController.OriginPhysicsArmor += armor;
				heroModel.HeroHealthController.OriginMagicArmor += armor;
			}
		}
	}
}

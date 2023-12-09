using Data;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class Hero1Skill2 : HeroSkillCommon
	{
		private int heroID = 1;

		private int skillID = 2;

		private int currentLevel;

		private int currentSkillLevel;

		private bool unLock;

		private string buffKey = "IncreaseAttackSpeed";

		private HeroModel heroModel;

		private int temporaryAttackSpeedBonus;

		private float duration;

		private float cooldownTime;

		private bool isAvailableToUse;

		private float cooldownTimeTracking;

		public override void Init(HeroModel heroModel)
		{
			base.Init(heroModel);
			unLock = true;
			this.heroModel = heroModel;
			currentLevel = ReadWriteDataHero.Instance.GetCurrentHeroLevel(heroID);
			currentSkillLevel = HeroParameter.Instance.GetSkillPoint(heroID, currentLevel, skillID);
			HeroSkillParameter_1_2 heroSkillParameter_1_ = new HeroSkillParameter_1_2();
			heroSkillParameter_1_ = (HeroSkillParameter_1_2)HeroSkillParameter.Instance.GetHeroSkillsParameter(heroID, skillID);
			temporaryAttackSpeedBonus = heroSkillParameter_1_.getParam(currentSkillLevel - 1).attack_speed_increase;
			duration = (float)heroSkillParameter_1_.getParam(currentSkillLevel - 1).duration / 1000f;
			cooldownTime = (float)heroSkillParameter_1_.getParam(currentSkillLevel - 1).cooldown_time / 1000f;
		}

		public override void Update()
		{
			base.Update();
			if (unLock && (!heroModel || heroModel.IsAlive))
			{
				if (cooldownTimeTracking == 0f)
				{
					isAvailableToUse = true;
				}
				cooldownTimeTracking = Mathf.MoveTowards(cooldownTimeTracking, 0f, Time.deltaTime);
			}
		}

		public void AddBuffAttackSpeed()
		{
			if (unLock && isAvailableToUse)
			{
				heroModel.BuffsHolder.AddBuff(buffKey, new Buff(isPositive: true, temporaryAttackSpeedBonus, duration), BuffStackLogic.ChooseMax, BuffStackLogic.ChooseMax);
				isAvailableToUse = false;
				cooldownTimeTracking = cooldownTime;
			}
		}
	}
}

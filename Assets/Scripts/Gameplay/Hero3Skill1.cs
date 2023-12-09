using Data;
using Parameter;
using System.Collections;
using UnityEngine;

namespace Gameplay
{
	public class Hero3Skill1 : HeroSkillCommon
	{
		private int heroID = 3;

		private int skillID = 1;

		private int currentLevel;

		private int currentSkillLevel;

		private bool unLock;

		private HeroModel heroModel;

		private int magicDamageBonus;

		private float attackSpeedIncrease;

		private float duration;

		private float timeTracking;

		private float cooldownTime;

		private string description;

		[SerializeField]
		private Hero3Skill1MagicOrb magicOrb;

		private string buffKey0 = "IncreaseAttackSpeed";

		private string buffKey1 = "IncreaseDamageMagic";

		public override void Update()
		{
			base.Update();
			if (unLock && (!heroModel || heroModel.IsAlive))
			{
				if (IsCooldownDone())
				{
					StartCoroutine(CastSkill());
				}
				timeTracking = Mathf.MoveTowards(timeTracking, 0f, Time.deltaTime);
			}
		}

		public override void OnHeroReturnPool()
		{
			base.OnHeroReturnPool();
			magicOrb.Hide();
		}

		public override void Init(HeroModel heroModel)
		{
			base.Init(heroModel);
			unLock = true;
			this.heroModel = heroModel;
			currentLevel = ReadWriteDataHero.Instance.GetCurrentHeroLevel(heroID);
			currentSkillLevel = HeroParameter.Instance.GetSkillPoint(heroID, currentLevel, skillID);
			HeroSkillParameter_3_1 heroSkillParameter_3_ = new HeroSkillParameter_3_1();
			heroSkillParameter_3_ = (HeroSkillParameter_3_1)HeroSkillParameter.Instance.GetHeroSkillsParameter(heroID, skillID);
			magicDamageBonus = heroSkillParameter_3_.getParam(currentSkillLevel - 1).magic_damage_bonus;
			attackSpeedIncrease = heroSkillParameter_3_.getParam(currentSkillLevel - 1).attack_speed_increase;
			duration = (float)heroSkillParameter_3_.getParam(currentSkillLevel - 1).duration / 1000f;
			cooldownTime = (float)heroSkillParameter_3_.getParam(currentSkillLevel - 1).cooldown_time / 1000f;
			description = heroSkillParameter_3_.getParam(currentSkillLevel - 1).description;
			timeTracking = cooldownTime;
		}

		private bool IsCooldownDone()
		{
			return timeTracking == 0f;
		}

		private IEnumerator CastSkill()
		{
			if (!IsEmptySpecialState())
			{
				yield return null;
			}
			heroModel.SetSpecialStateDuration(1f);
			heroModel.SetSpecialStateAnimationName(HeroAnimationController.animPassiveSkill_0);
			heroModel.GetFSMController().GetCurrentState().OnInput(StateInputType.SpecialState, HeroAnimationController.animPassiveSkill_0);
			AbilitiesBuff();
		}

		private void AbilitiesBuff()
		{
			magicOrb.Init(duration);
			heroModel.BuffsHolder.AddBuff(buffKey0, new Buff(isPositive: true, attackSpeedIncrease, duration), BuffStackLogic.ChooseMax, BuffStackLogic.ChooseMax);
			heroModel.BuffsHolder.AddBuff(buffKey1, new Buff(isPositive: true, magicDamageBonus, duration), BuffStackLogic.ChooseMax, BuffStackLogic.ChooseMax);
			timeTracking = cooldownTime;
		}
	}
}

using Data;
using Parameter;
using System.Collections;
using UnityEngine;

namespace Gameplay
{
	public class Hero4Skill1 : HeroSkillCommon
	{
		private int heroID = 4;

		private int skillID = 1;

		private int currentLevel;

		private int currentSkillLevel;

		private bool unLock;

		private HeroModel heroModel;

		private float physicsArmorBonus;

		private float magicArmorBonus;

		private float duration;

		private float timeTracking;

		private float cooldownTime;

		private string description;

		[SerializeField]
		private Hero4Skill1Inferno buffEffect;

		private string buffKey0 = "IncreaseArmorPhysics";

		private string buffKey1 = "IncreaseArmorMagic";

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
			buffEffect.Hide();
		}

		public override void Init(HeroModel heroModel)
		{
			base.Init(heroModel);
			unLock = true;
			this.heroModel = heroModel;
			currentLevel = ReadWriteDataHero.Instance.GetCurrentHeroLevel(heroID);
			currentSkillLevel = HeroParameter.Instance.GetSkillPoint(heroID, currentLevel, skillID);
			HeroSkillParameter_4_1 heroSkillParameter_4_ = new HeroSkillParameter_4_1();
			heroSkillParameter_4_ = (HeroSkillParameter_4_1)HeroSkillParameter.Instance.GetHeroSkillsParameter(heroID, skillID);
			physicsArmorBonus = (float)heroSkillParameter_4_.getParam(currentSkillLevel - 1).physics_armor_bonus / 100f;
			magicArmorBonus = (float)heroSkillParameter_4_.getParam(currentSkillLevel - 1).magic_armor_bonus / 100f;
			duration = (float)heroSkillParameter_4_.getParam(currentSkillLevel - 1).duration / 1000f;
			cooldownTime = (float)heroSkillParameter_4_.getParam(currentSkillLevel - 1).cooldown_time / 1000f;
			description = heroSkillParameter_4_.getParam(currentSkillLevel - 1).description;
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
			buffEffect.Init(duration);
			heroModel.BuffsHolder.AddBuff(buffKey0, new Buff(isPositive: true, physicsArmorBonus, duration), BuffStackLogic.ChooseMax, BuffStackLogic.ChooseMax);
			heroModel.BuffsHolder.AddBuff(buffKey1, new Buff(isPositive: true, magicArmorBonus, duration), BuffStackLogic.ChooseMax, BuffStackLogic.ChooseMax);
			timeTracking = cooldownTime;
		}
	}
}

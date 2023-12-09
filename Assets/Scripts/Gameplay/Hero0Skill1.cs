using Data;
using Parameter;
using System.Collections;
using UnityEngine;

namespace Gameplay
{
	public class Hero0Skill1 : HeroSkillCommon
	{
		private int heroID;

		private int skillID = 1;

		private int currentLevel;

		private int currentSkillLevel;

		private bool unLock;

		private HeroModel heroModel;

		private int damage;

		private float aoeRange;

		private float duration;

		private int change_percent;

		[SerializeField]
		private DamageToAOERange damageToAOERange;

		public override void Init(HeroModel heroModel)
		{
			base.Init(heroModel);
			unLock = true;
			this.heroModel = heroModel;
			currentLevel = ReadWriteDataHero.Instance.GetCurrentHeroLevel(heroID);
			currentSkillLevel = HeroParameter.Instance.GetSkillPoint(heroID, currentLevel, skillID);
			HeroSkillParameter_0_1 heroSkillParameter_0_ = new HeroSkillParameter_0_1();
			heroSkillParameter_0_ = (HeroSkillParameter_0_1)HeroSkillParameter.Instance.GetHeroSkillsParameter(heroID, skillID);
			damage = heroSkillParameter_0_.getParam(currentSkillLevel - 1).damage;
			duration = (float)heroSkillParameter_0_.getParam(currentSkillLevel - 1).duration / 1000f;
			change_percent = heroSkillParameter_0_.getParam(currentSkillLevel - 1).change_percent;
			aoeRange = (float)heroSkillParameter_0_.getParam(currentSkillLevel - 1).aoeRange / GameData.PIXEL_PER_UNIT;
			heroModel.OnAttackEvent += HeroModel_OnBeHitEvent;
			heroModel.OnSpecialStateEvent += HeroModel_OnSpecialStateEvent;
		}

		private void HeroModel_OnSpecialStateEvent()
		{
			DamageWithAOE();
		}

		private void HeroModel_OnBeHitEvent()
		{
			if (Random.Range(0, 100) < change_percent && unLock)
			{
				StartCoroutine(CastSkill());
			}
		}

		private IEnumerator CastSkill()
		{
			if (!IsEmptySpecialState())
			{
				yield return null;
			}
			heroModel.SetSpecialStateDuration(duration);
			heroModel.SetSpecialStateAnimationName(HeroAnimationController.animPassiveSkill_0);
			heroModel.GetFSMController().GetCurrentState().OnInput(StateInputType.SpecialState, HeroAnimationController.animPassiveSkill_0);
		}

		private void DamageWithAOE()
		{
			damageToAOERange.CastDamage(DamageType.Range, new CommonAttackDamage(damage, 0, aoeRange));
		}
	}
}

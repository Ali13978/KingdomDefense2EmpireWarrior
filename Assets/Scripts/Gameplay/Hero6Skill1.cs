using Data;
using Parameter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class Hero6Skill1 : HeroSkillCommon
	{
		private int heroID = 6;

		private int skillID = 1;

		private int currentLevel;

		private int currentSkillLevel;

		private bool unLock;

		private HeroModel heroModel;

		private int attackDamageBonusPercentage;

		private float duration;

		private float timeTracking;

		private float cooldownTime;

		private string description;

		private string increaseDamageBuffkey = "BuffAttackByPercentage";

		[SerializeField]
		private float animDuration;

		public override void Update()
		{
			base.Update();
			if (SingletonMonoBehaviour<GameData>.Instance.IsGameStart && unLock && (!heroModel || heroModel.IsAlive))
			{
				if (IsCooldownDone())
				{
					StartCoroutine(CastSkill());
				}
				timeTracking = Mathf.MoveTowards(timeTracking, 0f, Time.deltaTime);
			}
		}

		public override void Init(HeroModel heroModel)
		{
			base.Init(heroModel);
			unLock = true;
			this.heroModel = heroModel;
			currentLevel = ReadWriteDataHero.Instance.GetCurrentHeroLevel(heroID);
			currentSkillLevel = HeroParameter.Instance.GetSkillPoint(heroID, currentLevel, skillID);
			HeroSkillParameter_6_1 heroSkillParameter_6_ = new HeroSkillParameter_6_1();
			heroSkillParameter_6_ = (HeroSkillParameter_6_1)HeroSkillParameter.Instance.GetHeroSkillsParameter(heroID, skillID);
			attackDamageBonusPercentage = heroSkillParameter_6_.getParam(currentSkillLevel - 1).percent_attack_damage_bonus;
			duration = (float)heroSkillParameter_6_.getParam(currentSkillLevel - 1).duration / 1000f;
			cooldownTime = (float)heroSkillParameter_6_.getParam(currentSkillLevel - 1).cooldown_time / 1000f;
			description = heroSkillParameter_6_.getParam(currentSkillLevel - 1).description;
			InitFXs();
			timeTracking = cooldownTime;
		}

		private void InitFXs()
		{
			SingletonMonoBehaviour<SpawnFX>.Instance.InitFX(SpawnFX.INFERNO_GOLEM_AURA);
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
			heroModel.SetSpecialStateDuration(animDuration);
			heroModel.SetSpecialStateAnimationName(HeroAnimationController.animPassiveSkill_0);
			heroModel.GetFSMController().GetCurrentState().OnInput(StateInputType.SpecialState, HeroAnimationController.animPassiveSkill_0);
			AbilitiesBuff();
		}

		private void AbilitiesBuff()
		{
			List<CharacterModel> listActiveAlly = SingletonMonoBehaviour<GameData>.Instance.ListActiveAlly;
			foreach (CharacterModel item in listActiveAlly)
			{
				item.BuffsHolder.AddBuff(increaseDamageBuffkey, new Buff(isPositive: true, attackDamageBonusPercentage, duration), BuffStackLogic.ChooseMax, BuffStackLogic.ChooseMax);
				EffectController effect = SingletonMonoBehaviour<SpawnFX>.Instance.GetEffect(SpawnFX.INFERNO_GOLEM_AURA);
				effect.transform.position = item.transform.position;
				effect.Init(duration, item.transform);
			}
			timeTracking = cooldownTime;
		}
	}
}

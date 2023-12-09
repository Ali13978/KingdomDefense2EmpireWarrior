using Data;
using Parameter;
using System.Collections;
using UnityEngine;

namespace Gameplay
{
	public class Hero9Skill2 : HeroSkillCommon
	{
		private int heroID = 9;

		private int skillID = 2;

		private int currentLevel;

		private int currentSkillLevel;

		private bool unLock;

		private HeroModel heroModel;

		private int percentHealthActivate;

		private int healthAmountPercentage;

		private float duration;

		private float timeTracking;

		private float cooldownTime;

		private string description;

		[SerializeField]
		private float animDuration;

		[SerializeField]
		private float delayTimeCastSkill;

		public override void Update()
		{
			base.Update();
			if (SingletonMonoBehaviour<GameData>.Instance.IsGameStart && unLock && (!heroModel || heroModel.IsAlive))
			{
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
			HeroSkillParameter_9_2 heroSkillParameter_9_ = new HeroSkillParameter_9_2();
			heroSkillParameter_9_ = (HeroSkillParameter_9_2)HeroSkillParameter.Instance.GetHeroSkillsParameter(heroID, skillID);
			percentHealthActivate = heroSkillParameter_9_.getParam(currentSkillLevel - 1).health_percentage_active;
			healthAmountPercentage = heroSkillParameter_9_.getParam(currentSkillLevel - 1).health_amount;
			duration = (float)heroSkillParameter_9_.getParam(currentSkillLevel - 1).duration / 1000f;
			cooldownTime = (float)heroSkillParameter_9_.getParam(currentSkillLevel - 1).cooldown_time / 1000f;
			description = heroSkillParameter_9_.getParam(currentSkillLevel - 1).description;
			InitFXs();
			timeTracking = cooldownTime * 0.65f;
			heroModel.OnBeHitEvent += HeroModel_OnBeHitEvent;
		}

		private void HeroModel_OnBeHitEvent()
		{
			if (IsCooldownDone() && IsEmptySpecialState() && IsInThreatenState() && unLock && (bool)heroModel.currentTarget)
			{
				StartCoroutine(CastSkill());
			}
		}

		private bool IsInThreatenState()
		{
			bool result = false;
			if ((float)heroModel.HeroHealthController.CurrentHealth <= (float)percentHealthActivate / 100f * (float)heroModel.HeroHealthController.OriginHealth)
			{
				result = true;
			}
			return result;
		}

		private void InitFXs()
		{
			SingletonMonoBehaviour<SpawnFX>.Instance.InitFX(SpawnFX.EFFECT_HEAL_0);
		}

		private bool IsCooldownDone()
		{
			return timeTracking == 0f;
		}

		private IEnumerator CastSkill()
		{
			heroModel.SetSpecialStateDuration(duration);
			heroModel.SetSpecialStateAnimationName(HeroAnimationController.animPassiveSkill_1);
			heroModel.GetFSMController().GetCurrentState().OnInput(StateInputType.SpecialState, HeroAnimationController.animPassiveSkill_1);
			timeTracking = cooldownTime;
			yield return new WaitForSeconds(delayTimeCastSkill);
			heroModel.HeroHealthController.AddHealth((int)((float)(heroModel.HeroHealthController.OriginHealth * healthAmountPercentage) / 100f));
			EffectController fx = SingletonMonoBehaviour<SpawnFX>.Instance.GetEffect(SpawnFX.EFFECT_HEAL_0);
			fx.Init(duration - delayTimeCastSkill, heroModel.BuffsHolder.transform);
		}
	}
}

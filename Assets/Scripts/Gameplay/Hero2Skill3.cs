using Data;
using DG.Tweening;
using Parameter;
using System.Collections;
using UnityEngine;

namespace Gameplay
{
	public class Hero2Skill3 : HeroSkillCommon
	{
		private int heroID = 2;

		private int skillID = 3;

		private int currentLevel;

		private int currentSkillLevel;

		private bool unLock;

		private HeroModel heroModel;

		private float aoeRange;

		private int damage;

		private string buffkey = "Slow";

		private int slowValue;

		private float slowDuration;

		private float cooldownTime;

		private string description;

		[SerializeField]
		private float animationTime = 2.5f;

		private EffectAttack effectAttackSender;

		private float cooldownTimeTracking;

		private Tweener tween;

		[SerializeField]
		private DamageToAOERange damageToAOERange;

		[SerializeField]
		private EffectCaster effectCaster;

		public override void Update()
		{
			base.Update();
			if (unLock && (!heroModel || heroModel.IsAlive))
			{
				cooldownTimeTracking = Mathf.MoveTowards(cooldownTimeTracking, 0f, Time.deltaTime);
			}
		}

		public override void Init(HeroModel heroModel)
		{
			base.Init(heroModel);
			unLock = true;
			this.heroModel = heroModel;
			currentLevel = ReadWriteDataHero.Instance.GetCurrentHeroLevel(heroID);
			currentSkillLevel = HeroParameter.Instance.GetSkillPoint(heroID, currentLevel, skillID);
			HeroSkillParameter_2_3 heroSkillParameter_2_ = new HeroSkillParameter_2_3();
			heroSkillParameter_2_ = (HeroSkillParameter_2_3)HeroSkillParameter.Instance.GetHeroSkillsParameter(heroID, skillID);
			aoeRange = (float)heroSkillParameter_2_.getParam(currentSkillLevel - 1).aoeRange / GameData.PIXEL_PER_UNIT;
			damage = heroSkillParameter_2_.getParam(currentSkillLevel - 1).damage;
			slowValue = heroSkillParameter_2_.getParam(currentSkillLevel - 1).slow_value;
			slowDuration = (float)heroSkillParameter_2_.getParam(currentSkillLevel - 1).slow_duration / 1000f;
			cooldownTime = (float)heroSkillParameter_2_.getParam(currentSkillLevel - 1).cooldown_time / 1000f;
			description = heroSkillParameter_2_.getParam(currentSkillLevel - 1).description;
			cooldownTimeTracking = cooldownTime;
			effectAttackSender.buffKey = buffkey;
			effectAttackSender.debuffChance = 100;
			effectAttackSender.debuffEffectValue = slowValue;
			effectAttackSender.debuffEffectDuration = slowDuration;
			effectAttackSender.damageFXType = DamageFXType.Stun;
			heroModel.OnHitEnemyEvent += HeroModel_OnHitEnemyEvent;
			heroModel.OnSpecialStateEvent += HeroModel_OnSpecialStateEvent;
			SingletonMonoBehaviour<SpawnFX>.Instance.InitFX(SpawnFX.GROUND_STOMP);
			SingletonMonoBehaviour<SpawnFX>.Instance.InitFX(SpawnFX.EFFECT_STUN);
		}

		private void HeroModel_OnHitEnemyEvent()
		{
			if (IsCooldownDone() && unLock)
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
			heroModel.SetSpecialStateDuration(animationTime);
			heroModel.SetSpecialStateAnimationName(HeroAnimationController.animPassiveSkill_2);
			heroModel.GetFSMController().GetCurrentState().OnInput(StateInputType.SpecialState, HeroAnimationController.animPassiveSkill_2);
		}

		private void CastFX()
		{
			effectCaster.CastEffect(SpawnFX.GROUND_STOMP, 2f, base.transform.position);
		}

		private void HeroModel_OnSpecialStateEvent()
		{
			DamageWithAOE();
			CastFX();
			cooldownTimeTracking = cooldownTime;
		}

		private bool IsCooldownDone()
		{
			return cooldownTimeTracking == 0f;
		}

		private void DamageWithAOE()
		{
			damageToAOERange.CastDamage(DamageType.Range, new CommonAttackDamage(damage, 0, aoeRange), effectAttackSender);
		}
	}
}

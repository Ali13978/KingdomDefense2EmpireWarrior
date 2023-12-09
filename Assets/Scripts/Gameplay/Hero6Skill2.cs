using Data;
using Parameter;
using System.Collections;
using UnityEngine;

namespace Gameplay
{
	public class Hero6Skill2 : HeroSkillCommon
	{
		private int heroID = 6;

		private int skillID = 2;

		private int currentLevel;

		private int currentSkillLevel;

		private bool unLock;

		private HeroModel heroModel;

		private int physicsDamage;

		private int magicDamage;

		private float aoeRange;

		private float timeTracking;

		private float cooldownTime;

		private string description;

		[SerializeField]
		private float animDuration;

		[SerializeField]
		private float delayTimeCastSkill;

		[SerializeField]
		private float effectLifeTime;

		[SerializeField]
		private DamageToAOERange damageToAOERange;

		[SerializeField]
		private EffectCaster effectCaster;

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
			HeroSkillParameter_6_2 heroSkillParameter_6_ = new HeroSkillParameter_6_2();
			heroSkillParameter_6_ = (HeroSkillParameter_6_2)HeroSkillParameter.Instance.GetHeroSkillsParameter(heroID, skillID);
			physicsDamage = heroSkillParameter_6_.getParam(currentSkillLevel - 1).physics_damage;
			magicDamage = heroSkillParameter_6_.getParam(currentSkillLevel - 1).magic_damage;
			aoeRange = (float)heroSkillParameter_6_.getParam(currentSkillLevel - 1).aoe_range / GameData.PIXEL_PER_UNIT;
			cooldownTime = (float)heroSkillParameter_6_.getParam(currentSkillLevel - 1).cooldown_time / 1000f;
			description = heroSkillParameter_6_.getParam(currentSkillLevel - 1).description;
			InitFXs();
			timeTracking = cooldownTime;
			heroModel.OnHitEnemyEvent += HeroModel_OnHitEnemyEvent;
		}

		private void InitFXs()
		{
			SingletonMonoBehaviour<SpawnFX>.Instance.InitFX(SpawnFX.THOR_LANDING_THUNDER);
		}

		private bool IsCooldownDone()
		{
			return timeTracking == 0f;
		}

		private void HeroModel_OnHitEnemyEvent()
		{
			if (IsCooldownDone() && IsEmptySpecialState() && unLock)
			{
				StartCoroutine(CastSkill());
			}
		}

		private IEnumerator CastSkill()
		{
			heroModel.SetSpecialStateDuration(animDuration);
			heroModel.SetSpecialStateAnimationName(HeroAnimationController.animActiveSkill);
			heroModel.GetFSMController().GetCurrentState().OnInput(StateInputType.SpecialState, HeroAnimationController.animActiveSkill);
			timeTracking = cooldownTime;
			yield return new WaitForSeconds(delayTimeCastSkill);
			damageToAOERange.CastDamage(DamageType.Range, new CommonAttackDamage(physicsDamage, magicDamage, aoeRange));
			effectCaster.CastEffect(SpawnFX.THOR_LANDING_THUNDER, effectLifeTime, base.transform.position);
		}
	}
}

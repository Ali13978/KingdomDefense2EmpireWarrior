using Data;
using Parameter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class Hero8Skill2 : HeroSkillCommon
	{
		private int heroID = 8;

		private int skillID = 2;

		private int currentLevel;

		private int currentSkillLevel;

		private bool unLock;

		private string buffKey = "Stun";

		private int enemyAffected;

		private float skillRange;

		private float duration;

		private HeroModel heroModel;

		private float timeTracking;

		private float cooldownTime;

		private string description;

		[SerializeField]
		private float animDuration;

		[SerializeField]
		private float delayTimeCastSkill;

		[SerializeField]
		private DamageToEnemiesInRange damageToEnemiesInRange;

		private EffectAttack effectAttack;

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

		public void OnDrawGizmosSelected()
		{
			if (unLock)
			{
				Gizmos.color = Color.yellow;
				Gizmos.DrawWireSphere(heroModel.transform.position, skillRange);
			}
		}

		public override void Init(HeroModel heroModel)
		{
			base.Init(heroModel);
			unLock = true;
			this.heroModel = heroModel;
			currentLevel = ReadWriteDataHero.Instance.GetCurrentHeroLevel(heroID);
			currentSkillLevel = HeroParameter.Instance.GetSkillPoint(heroID, currentLevel, skillID);
			HeroSkillParameter_8_2 heroSkillParameter_8_ = new HeroSkillParameter_8_2();
			heroSkillParameter_8_ = (HeroSkillParameter_8_2)HeroSkillParameter.Instance.GetHeroSkillsParameter(heroID, skillID);
			enemyAffected = 1;
			skillRange = (float)heroSkillParameter_8_.getParam(currentSkillLevel - 1).skill_range / GameData.PIXEL_PER_UNIT;
			duration = (float)heroSkillParameter_8_.getParam(currentSkillLevel - 1).duration / 1000f;
			cooldownTime = (float)heroSkillParameter_8_.getParam(currentSkillLevel - 1).cooldown_time / 1000f;
			description = heroSkillParameter_8_.getParam(currentSkillLevel - 1).description;
			effectAttack = default(EffectAttack);
			effectAttack.buffKey = buffKey;
			effectAttack.debuffChance = 100;
			effectAttack.debuffEffectValue = 100;
			effectAttack.debuffEffectDuration = duration;
			effectAttack.damageFXType = DamageFXType.Stun;
			timeTracking = cooldownTime * 0.6f;
			InitFXs();
		}

		private void InitFXs()
		{
			SingletonMonoBehaviour<SpawnFX>.Instance.InitFX(SpawnFX.EFFECT_STUN);
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
			timeTracking = cooldownTime;
			heroModel.SetSpecialStateDuration(animDuration);
			heroModel.SetSpecialStateAnimationName(HeroAnimationController.animPassiveSkill_1);
			heroModel.GetFSMController().GetCurrentState().OnInput(StateInputType.SpecialState, HeroAnimationController.animPassiveSkill_1);
			yield return new WaitForSeconds(delayTimeCastSkill);
			List<EnemyModel> enemiesInAoeRange = GameTools.GetListEnemiesInRange(base.gameObject, new CommonAttackDamage(0, 0, skillRange));
			if (enemiesInAoeRange.Count > 0)
			{
				damageToEnemiesInRange.CastDamage(enemyAffected, DamageType.Range, new CommonAttackDamage(0, 0, skillRange), effectAttack);
			}
		}
	}
}

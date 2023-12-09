using Data;
using Parameter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class Hero6Skill3 : HeroSkillCommon
	{
		private int heroID = 6;

		private int skillID = 3;

		private int currentLevel;

		private int currentSkillLevel;

		private bool unLock;

		private HeroModel heroModel;

		private int physicsDamage;

		private int magicDamage;

		private float skillRange;

		private float timeTracking;

		private float cooldownTime;

		private string description;

		[SerializeField]
		private DamageToAOERange damageToAOERange;

		private EffectAttack effectAttackSender;

		[SerializeField]
		private float animDuration;

		[SerializeField]
		private float delayTimeCastSkill;

		[SerializeField]
		private float effectLifeTime;

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
			HeroSkillParameter_6_3 heroSkillParameter_6_ = new HeroSkillParameter_6_3();
			heroSkillParameter_6_ = (HeroSkillParameter_6_3)HeroSkillParameter.Instance.GetHeroSkillsParameter(heroID, skillID);
			physicsDamage = heroSkillParameter_6_.getParam(currentSkillLevel - 1).physics_damage;
			magicDamage = heroSkillParameter_6_.getParam(currentSkillLevel - 1).magic_damage;
			skillRange = (float)heroSkillParameter_6_.getParam(currentSkillLevel - 1).skill_range / GameData.PIXEL_PER_UNIT;
			cooldownTime = (float)heroSkillParameter_6_.getParam(currentSkillLevel - 1).cooldown_time / 1000f;
			description = heroSkillParameter_6_.getParam(currentSkillLevel - 1).description;
			InitFXs();
			timeTracking = cooldownTime;
		}

		public void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(base.transform.position, skillRange);
		}

		private void InitFXs()
		{
			SingletonMonoBehaviour<SpawnFX>.Instance.InitFX(SpawnFX.THOR_AIR_THUNDER);
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
			List<EnemyModel> listFlyingEnemies = GameTools.GetListFlyingEnemiesInRange(base.gameObject, new CommonAttackDamage(physicsDamage, magicDamage, isTargetAir: true, skillRange));
			timeTracking = cooldownTime;
			if (listFlyingEnemies.Count > 0)
			{
				heroModel.SetSpecialStateDuration(animDuration);
				heroModel.SetSpecialStateAnimationName(HeroAnimationController.animPassiveSkill_0);
				heroModel.GetFSMController().GetCurrentState().OnInput(StateInputType.SpecialState, HeroAnimationController.animPassiveSkill_0);
				yield return new WaitForSeconds(delayTimeCastSkill);
				foreach (EnemyModel item in listFlyingEnemies)
				{
					item.ProcessDamage(DamageType.Range, new CommonAttackDamage(physicsDamage, magicDamage, isTargetAir: true, skillRange));
					EffectController effect = SingletonMonoBehaviour<SpawnFX>.Instance.GetEffect(SpawnFX.THOR_AIR_THUNDER);
					effect.transform.position = item.transform.position;
					effect.Init(effectLifeTime, item.transform);
				}
			}
		}
	}
}

using Data;
using Parameter;
using System.Collections;
using UnityEngine;

namespace Gameplay
{
	public class Hero7Skill2 : HeroSkillCommon
	{
		private int heroID = 7;

		private int skillID = 2;

		private int currentLevel;

		private int currentSkillLevel;

		private bool unLock;

		private HeroModel heroModel;

		private int chanceToCastSkill;

		private int physicsDamage;

		private int magicDamage;

		private float skillRange;

		private float timeTracking;

		private float cooldownTime;

		private string description;

		[SerializeField]
		private float animDuration;

		[SerializeField]
		private float delayTimeCastSkill;

		[SerializeField]
		private GameObject targetPosition;

		[SerializeField]
		private DamageToAOERange damageToAOERange;

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
			HeroSkillParameter_7_2 heroSkillParameter_7_ = new HeroSkillParameter_7_2();
			heroSkillParameter_7_ = (HeroSkillParameter_7_2)HeroSkillParameter.Instance.GetHeroSkillsParameter(heroID, skillID);
			chanceToCastSkill = heroSkillParameter_7_.getParam(currentSkillLevel - 1).chance_to_cast;
			physicsDamage = heroSkillParameter_7_.getParam(currentSkillLevel - 1).physics_damage;
			magicDamage = heroSkillParameter_7_.getParam(currentSkillLevel - 1).magic_damage;
			skillRange = (float)heroSkillParameter_7_.getParam(currentSkillLevel - 1).skill_range / GameData.PIXEL_PER_UNIT;
			cooldownTime = (float)heroSkillParameter_7_.getParam(currentSkillLevel - 1).cooldown_time / 1000f;
			description = heroSkillParameter_7_.getParam(currentSkillLevel - 1).description;
			timeTracking = cooldownTime;
			heroModel.OnHitEnemyEvent += HeroModel_OnHitEnemyEvent;
			InitFXs();
		}

		private void InitFXs()
		{
			SingletonMonoBehaviour<SpawnFX>.Instance.InitFX(SpawnFX.GROUND_BREAK);
		}

		public void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(targetPosition.transform.position, skillRange);
		}

		private bool IsCooldownDone()
		{
			return timeTracking == 0f;
		}

		private bool IsAbleToCastSkill()
		{
			return chanceToCastSkill <= Random.Range(0, 100);
		}

		private void HeroModel_OnHitEnemyEvent()
		{
			if (IsCooldownDone() && IsEmptySpecialState() && IsAbleToCastSkill() && unLock)
			{
				StartCoroutine(CastSkill());
			}
		}

		private IEnumerator CastSkill()
		{
			heroModel.SetSpecialStateDuration(animDuration);
			heroModel.SetSpecialStateAnimationName(HeroAnimationController.animPassiveSkill_1);
			heroModel.GetFSMController().GetCurrentState().OnInput(StateInputType.SpecialState, HeroAnimationController.animPassiveSkill_1);
			timeTracking = cooldownTime;
			yield return new WaitForSeconds(delayTimeCastSkill);
			damageToAOERange.CastDamage(targetPosition, DamageType.Range, new CommonAttackDamage(physicsDamage, magicDamage, skillRange));
			EffectController fx = SingletonMonoBehaviour<SpawnFX>.Instance.GetEffect(SpawnFX.GROUND_BREAK);
			fx.Init(3f, targetPosition.transform.position);
		}
	}
}

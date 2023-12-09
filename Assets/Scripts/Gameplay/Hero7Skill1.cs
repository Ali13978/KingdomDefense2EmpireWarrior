using Data;
using DG.Tweening;
using Parameter;
using System.Collections;
using UnityEngine;

namespace Gameplay
{
	public class Hero7Skill1 : HeroSkillCommon
	{
		private int heroID = 7;

		private int skillID = 1;

		private int currentLevel;

		private int currentSkillLevel;

		private bool unLock;

		private HeroModel heroModel;

		private int percentHealthActivate;

		private int amountOfTrap;

		private float trapLifeTime;

		private int physicsDamage;

		private int magicDamage;

		private int slowPercent;

		private float slowDuration;

		private float skillRange;

		private float cooldownTime;

		private float timeTracking;

		private string description;

		[SerializeField]
		private float animDuration;

		[SerializeField]
		private float delayTimeCastSkill;

		[SerializeField]
		private HeroSkillAOECommon skillObject;

		private string buffKey = "Slow";

		private EffectAttack effectAttackSender;

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
			HeroSkillParameter_7_1 heroSkillParameter_7_ = new HeroSkillParameter_7_1();
			heroSkillParameter_7_ = (HeroSkillParameter_7_1)HeroSkillParameter.Instance.GetHeroSkillsParameter(heroID, skillID);
			percentHealthActivate = heroSkillParameter_7_.getParam(currentSkillLevel - 1).percent_health_activate;
			amountOfTrap = heroSkillParameter_7_.getParam(currentSkillLevel - 1).amount_of_trap;
			trapLifeTime = (float)heroSkillParameter_7_.getParam(currentSkillLevel - 1).trap_life_time / 1000f;
			physicsDamage = heroSkillParameter_7_.getParam(currentSkillLevel - 1).physics_damage;
			magicDamage = heroSkillParameter_7_.getParam(currentSkillLevel - 1).magic_damage;
			slowPercent = heroSkillParameter_7_.getParam(currentSkillLevel - 1).slow_percent;
			slowDuration = (float)heroSkillParameter_7_.getParam(currentSkillLevel - 1).slow_duration / 1000f;
			skillRange = (float)heroSkillParameter_7_.getParam(currentSkillLevel - 1).skill_range / GameData.PIXEL_PER_UNIT;
			cooldownTime = (float)heroSkillParameter_7_.getParam(currentSkillLevel - 1).cooldown_time / 1000f;
			description = heroSkillParameter_7_.getParam(currentSkillLevel - 1).description;
			timeTracking = cooldownTime;
			heroModel.OnBeHitEvent += HeroModel_OnBeHitEvent;
			effectAttackSender = default(EffectAttack);
			effectAttackSender.buffKey = buffKey;
			effectAttackSender.debuffChance = 100;
			effectAttackSender.debuffEffectValue = slowPercent;
			effectAttackSender.debuffEffectDuration = slowDuration;
			effectAttackSender.damageFXType = DamageFXType.Slow;
			InitFXs();
		}

		private void HeroModel_OnBeHitEvent()
		{
			if (IsCooldownDone() && IsEmptySpecialState() && IsInThreatenState() && unLock && (bool)heroModel.currentTarget)
			{
				StartCoroutine(CastSkill(heroModel.currentTarget));
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
			SingletonMonoBehaviour<SpawnBullet>.Instance.InitExtendBullet(skillObject.gameObject);
			SingletonMonoBehaviour<SpawnFX>.Instance.InitFX(SpawnFX.EFFECT_SLOW);
			SingletonMonoBehaviour<SpawnFX>.Instance.InitFX(SpawnFX.TIMER_BOMB_EXPLOSION);
		}

		private bool IsCooldownDone()
		{
			return timeTracking == 0f;
		}

		private IEnumerator CastSkill(EnemyModel target)
		{
			heroModel.SetSpecialStateDuration(animDuration);
			heroModel.SetSpecialStateAnimationName(HeroAnimationController.animPassiveSkill_0);
			heroModel.GetFSMController().GetCurrentState().OnInput(StateInputType.SpecialState, HeroAnimationController.animPassiveSkill_0);
			timeTracking = cooldownTime;
			Vector3 offsetVector2 = target.transform.position - heroModel.transform.position;
			offsetVector2 = Vector3.Normalize(offsetVector2);
			ShortcutExtensions.DOMove(endValue: heroModel.transform.position - offsetVector2, target: heroModel.transform, duration: animDuration).SetEase(Ease.Linear).OnComplete(MoveToAssignedPositionComplete);
			yield return new WaitForSeconds(delayTimeCastSkill);
			for (int i = 0; i < amountOfTrap; i++)
			{
				HeroSkillAOECommon heroSkillAOECommon = SingletonMonoBehaviour<SpawnBullet>.Instance.GetHeroSkillAOECommon("Hero7Skill1Trap");
				heroSkillAOECommon.transform.position = target.transform.position + (Vector3)Random.insideUnitCircle * skillRange;
				heroSkillAOECommon.Init_DamageOnTrap(new CommonAttackDamage(physicsDamage, magicDamage, skillRange), effectAttackSender, trapLifeTime, SpawnFX.TIMER_BOMB_EXPLOSION, 1f);
			}
		}

		private void MoveToAssignedPositionComplete()
		{
			heroModel.SetAssignedPosition(heroModel.transform.position);
			if ((bool)heroModel.currentTarget)
			{
				heroModel.currentTarget.EnemyFindTargetController.Target = null;
				heroModel.AddTarget(null);
			}
		}
	}
}

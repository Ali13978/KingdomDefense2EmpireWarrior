using Data;
using Parameter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class Hero8Skill1 : HeroSkillCommon
	{
		private int heroID = 8;

		private int skillID = 1;

		private int currentLevel;

		private int currentSkillLevel;

		private bool unLock;

		private HeroModel heroModel;

		private int physicsDamage;

		private int magicDamage;

		private int attackDecreasePercentage;

		private float duration;

		private float knockBackDistance;

		private float skillRange;

		private float timeTracking;

		private float cooldownTime;

		private string description;

		[SerializeField]
		private float animDuration;

		[SerializeField]
		private float delayTimeCastSkill;

		[SerializeField]
		private float knockBackDuration;

		private string buffKey = "DecreaseAttackByPercentage";

		private EffectAttack effectAttack;

		private EnemyModel currentEnemy;

		private List<EnemyModel> listNearbyEnemies = new List<EnemyModel>();

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
			HeroSkillParameter_8_1 heroSkillParameter_8_ = new HeroSkillParameter_8_1();
			heroSkillParameter_8_ = (HeroSkillParameter_8_1)HeroSkillParameter.Instance.GetHeroSkillsParameter(heroID, skillID);
			physicsDamage = heroSkillParameter_8_.getParam(currentSkillLevel - 1).physics_damage;
			magicDamage = heroSkillParameter_8_.getParam(currentSkillLevel - 1).magic_damage;
			attackDecreasePercentage = heroSkillParameter_8_.getParam(currentSkillLevel - 1).attack_damage_decrease_percentage;
			duration = (float)heroSkillParameter_8_.getParam(currentSkillLevel - 1).duration / 1000f;
			knockBackDistance = (float)heroSkillParameter_8_.getParam(currentSkillLevel - 1).knock_back_distance / GameData.PIXEL_PER_UNIT;
			skillRange = (float)heroSkillParameter_8_.getParam(currentSkillLevel - 1).skill_range / GameData.PIXEL_PER_UNIT;
			cooldownTime = (float)heroSkillParameter_8_.getParam(currentSkillLevel - 1).cooldown_time / 1000f;
			description = heroSkillParameter_8_.getParam(currentSkillLevel - 1).description;
			effectAttack = default(EffectAttack);
			effectAttack.buffKey = buffKey;
			effectAttack.debuffChance = 100;
			effectAttack.debuffEffectValue = attackDecreasePercentage;
			effectAttack.debuffEffectDuration = duration;
			effectAttack.damageFXType = DamageFXType.None;
			timeTracking = cooldownTime * 0.5f;
			InitFXs();
			heroModel.OnHitEnemyEvent += HeroModel_OnHitEnemyEvent;
		}

		private void InitFXs()
		{
			SingletonMonoBehaviour<SpawnFX>.Instance.InitFX(SpawnFX.EFFECT_SELL_TOWER_ON_ALLY);
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
			heroModel.SetSpecialStateAnimationName(HeroAnimationController.animPassiveSkill_0);
			heroModel.GetFSMController().GetCurrentState().OnInput(StateInputType.SpecialState, HeroAnimationController.animPassiveSkill_0);
			timeTracking = cooldownTime;
			currentEnemy = heroModel.GetCurrentTarget();
			if (currentEnemy != null)
			{
				listNearbyEnemies = GameTools.GetListEnemiesInRange(currentEnemy.gameObject, new CommonAttackDamage(physicsDamage, magicDamage, skillRange));
			}
			yield return new WaitForSeconds(delayTimeCastSkill);
			float speed = knockBackDistance / knockBackDuration;
			if (listNearbyEnemies.Count <= 0)
			{
				yield break;
			}
			foreach (EnemyModel listNearbyEnemy in listNearbyEnemies)
			{
				if (GameTools.IsValidEnemy(listNearbyEnemy))
				{
					Enemy originalParameter = listNearbyEnemy.OriginalParameter;
					if (!originalParameter.isBoss && !listNearbyEnemy.IsAir && !listNearbyEnemy.IsUnderground && !listNearbyEnemy.IsInTunnel)
					{
						listNearbyEnemy.ProcessDamage(DamageType.Range, new CommonAttackDamage(physicsDamage, magicDamage), effectAttack);
						if (GameTools.IsValidEnemy(listNearbyEnemy))
						{
							listNearbyEnemy.SetSpecialStateDuration(knockBackDuration);
							listNearbyEnemy.enemyFsmController.GetCurrentState().OnInput(StateInputType.SpecialState);
						}
					}
				}
			}
			float countdown = knockBackDuration;
			while (countdown > 0f)
			{
				countdown -= Time.deltaTime;
				for (int num = listNearbyEnemies.Count - 1; num >= 0; num--)
				{
					PushbackSequence(listNearbyEnemies[num], speed);
				}
				yield return null;
			}
		}

		private void PushbackSequence(EnemyModel target, float knockbackSpeed)
		{
			if (GameTools.IsValidEnemy(target))
			{
				Enemy originalParameter = target.OriginalParameter;
				if (!originalParameter.isBoss && !target.IsAir && !target.IsUnderground && !target.IsInTunnel)
				{
					LineManager.Current.RequestMove(target, target.monsterPathData, (0f - knockbackSpeed) * Time.deltaTime);
				}
			}
		}
	}
}

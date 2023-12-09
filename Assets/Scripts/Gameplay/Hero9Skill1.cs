using Data;
using Parameter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class Hero9Skill1 : HeroSkillCommon
	{
		private int heroID = 9;

		private int skillID = 1;

		private int currentLevel;

		private int currentSkillLevel;

		private bool unLock;

		private HeroModel heroModel;

		private int enemyAffected;

		private float knocbackDistance;

		private float knocbackDuration;

		private float skillRange;

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
			HeroSkillParameter_9_1 heroSkillParameter_9_ = new HeroSkillParameter_9_1();
			heroSkillParameter_9_ = (HeroSkillParameter_9_1)HeroSkillParameter.Instance.GetHeroSkillsParameter(heroID, skillID);
			enemyAffected = heroSkillParameter_9_.getParam(currentSkillLevel - 1).enemy_affected;
			knocbackDistance = (float)heroSkillParameter_9_.getParam(currentSkillLevel - 1).knock_back_distance / GameData.PIXEL_PER_UNIT;
			knocbackDuration = (float)heroSkillParameter_9_.getParam(currentSkillLevel - 1).knock_back_duration / 1000f;
			skillRange = (float)heroSkillParameter_9_.getParam(currentSkillLevel - 1).skill_range / GameData.PIXEL_PER_UNIT;
			cooldownTime = (float)heroSkillParameter_9_.getParam(currentSkillLevel - 1).cooldown_time / 1000f;
			description = heroSkillParameter_9_.getParam(currentSkillLevel - 1).description;
			InitFXs();
			timeTracking = cooldownTime * 0.4f;
			heroModel.OnHitEnemyEvent += HeroModel_OnHitEnemyEvent;
		}

		private void HeroModel_OnHitEnemyEvent()
		{
			if (IsCooldownDone() && IsEmptySpecialState() && unLock)
			{
				StartCoroutine(CastSkill());
			}
		}

		private void InitFXs()
		{
			SingletonMonoBehaviour<SpawnFX>.Instance.InitFX(SpawnFX.EFFECT_POISON0);
		}

		private bool IsCooldownDone()
		{
			return timeTracking == 0f;
		}

		private IEnumerator CastSkill()
		{
			heroModel.SetSpecialStateDuration(animDuration);
			heroModel.SetSpecialStateAnimationName(HeroAnimationController.animPassiveSkill_0);
			heroModel.GetFSMController().GetCurrentState().OnInput(StateInputType.SpecialState, HeroAnimationController.animPassiveSkill_0);
			timeTracking = cooldownTime;
			yield return new WaitForSeconds(delayTimeCastSkill);
			List<EnemyModel> listEnemyInRange = GameTools.GetListEnemiesInRange(base.gameObject, new CommonAttackDamage(0, 0, skillRange));
			if (listEnemyInRange.Count <= 0)
			{
				yield break;
			}
			if (enemyAffected < listEnemyInRange.Count)
			{
				for (int i = 0; i < enemyAffected; i++)
				{
					StartCoroutine(PushbackSequence(listEnemyInRange[i]));
				}
			}
			else
			{
				for (int j = 0; j < listEnemyInRange.Count; j++)
				{
					StartCoroutine(PushbackSequence(listEnemyInRange[j]));
				}
			}
		}

		private IEnumerator PushbackSequence(EnemyModel target)
		{
			yield return new WaitForSeconds(0.1f);
			float pushTimeTracking = knocbackDuration;
			if (GameTools.IsValidEnemy(target))
			{
				target.EnemyAnimationController.TurnBack();
				EffectController effect = SingletonMonoBehaviour<SpawnFX>.Instance.GetEffect(SpawnFX.EFFECT_POISON0);
				effect.Init(0.77f, target.transform);
			}
			while (pushTimeTracking > 0f)
			{
				pushTimeTracking -= Time.deltaTime;
				if (GameTools.IsValidEnemy(target))
				{
					Enemy originalParameter = target.OriginalParameter;
					if (!originalParameter.isBoss)
					{
						target.SetSpecialStateDuration(pushTimeTracking);
						target.enemyFsmController.GetCurrentState().OnInput(StateInputType.SpecialState, EnemyAnimationController.animRunRight);
						Enemy originalParameter2 = target.OriginalParameter;
						float num = (float)originalParameter2.speed / GameData.PIXEL_PER_UNIT * Time.deltaTime;
						LineManager.Current.RequestMove(target, target.monsterPathData, 0f - num);
					}
				}
				yield return null;
			}
			yield return new WaitForSeconds(0.1f);
		}
	}
}

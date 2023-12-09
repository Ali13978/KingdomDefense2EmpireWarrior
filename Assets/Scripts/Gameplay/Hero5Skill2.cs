using Data;
using Parameter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class Hero5Skill2 : HeroSkillCommon
	{
		private int heroID = 5;

		private int skillID = 2;

		private int currentLevel;

		private int currentSkillLevel;

		private bool unLock;

		private HeroModel heroModel;

		private float skillRange;

		private int enemyAffected;

		private int enemyMin;

		private float cooldownTime;

		private float duration;

		private float timeTracking;

		private string description;

		private bool isCastSkill;

		private List<EnemyModel> enemiesInRange = new List<EnemyModel>();

		private string buffKey = "Slow";

		[SerializeField]
		private float animDuration;

		[SerializeField]
		private float delayTime;

		public override void Update()
		{
			base.Update();
			if (unLock && (!heroModel || heroModel.IsAlive) && !heroModel.HeroMovementController.isMoving)
			{
				if (IsCooldownDone())
				{
					TryToCastSkill();
				}
				timeTracking = Mathf.MoveTowards(timeTracking, 0f, Time.deltaTime);
			}
		}

		public void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(base.transform.position, skillRange);
		}

		public override void Init(HeroModel heroModel)
		{
			base.Init(heroModel);
			unLock = true;
			this.heroModel = heroModel;
			currentLevel = ReadWriteDataHero.Instance.GetCurrentHeroLevel(heroID);
			currentSkillLevel = HeroParameter.Instance.GetSkillPoint(heroID, currentLevel, skillID);
			HeroSkillParameter_5_2 heroSkillParameter_5_ = new HeroSkillParameter_5_2();
			heroSkillParameter_5_ = (HeroSkillParameter_5_2)HeroSkillParameter.Instance.GetHeroSkillsParameter(heroID, skillID);
			skillRange = (float)heroSkillParameter_5_.getParam(currentSkillLevel - 1).skill_range / GameData.PIXEL_PER_UNIT;
			enemyAffected = heroSkillParameter_5_.getParam(currentSkillLevel - 1).enemy_affected;
			enemyMin = heroSkillParameter_5_.getParam(currentSkillLevel - 1).enemy_min;
			cooldownTime = (float)heroSkillParameter_5_.getParam(currentSkillLevel - 1).cooldown_time / 1000f;
			duration = (float)heroSkillParameter_5_.getParam(currentSkillLevel - 1).duration / 1000f;
			description = heroSkillParameter_5_.getParam(currentSkillLevel - 1).description;
			timeTracking = cooldownTime * 0.4f;
			InitFXs();
		}

		private void InitFXs()
		{
			SingletonMonoBehaviour<SpawnFX>.Instance.InitFX(SpawnFX.EFFECT_ROOT);
		}

		private bool IsCooldownDone()
		{
			return timeTracking == 0f;
		}

		private void TryToCastSkill()
		{
			GetEnemiesInAoeRange();
			if (enemiesInRange.Count >= enemyMin && !isCastSkill)
			{
				timeTracking = cooldownTime;
				StartCoroutine(CastSkill());
			}
		}

		private IEnumerator CastSkill()
		{
			if (!IsEmptySpecialState())
			{
				yield return null;
			}
			heroModel.SetSpecialStateDuration(animDuration);
			heroModel.SetSpecialStateAnimationName(HeroAnimationController.animPassiveSkill_1);
			heroModel.GetFSMController().GetCurrentState().OnInput(StateInputType.SpecialState, HeroAnimationController.animPassiveSkill_1);
			yield return new WaitForSeconds(delayTime);
			if (!isCastSkill)
			{
				CastRoot();
			}
		}

		private void CastRoot()
		{
			isCastSkill = true;
			int num = (enemiesInRange.Count <= enemyAffected) ? enemiesInRange.Count : enemyAffected;
			for (int i = 0; i < num; i++)
			{
				UnityEngine.Debug.Log("troi quai ! " + num);
				if ((bool)enemiesInRange[i] && enemiesInRange[i].Id != 25)
				{
					enemiesInRange[i].ProcessEffect(buffKey, 100, duration, DamageFXType.Root);
				}
			}
			timeTracking = cooldownTime;
			isCastSkill = false;
		}

		private void GetEnemiesInAoeRange()
		{
			enemiesInRange.Clear();
			List<EnemyModel> listActiveEnemy = SingletonMonoBehaviour<GameData>.Instance.ListActiveEnemy;
			for (int i = 0; i < listActiveEnemy.Count; i++)
			{
				EnemyModel enemyModel = listActiveEnemy[i];
				if (!enemyModel.IsAir && !enemyModel.IsUnderground && !enemyModel.IsInTunnel)
				{
					float num = SingletonMonoBehaviour<GameData>.Instance.SqrDistance(base.gameObject, enemyModel.gameObject);
					if (num <= skillRange * skillRange)
					{
						enemiesInRange.Add(enemyModel);
					}
				}
			}
		}
	}
}

using Parameter;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class Tower2Ultimate1Skill0 : TowerUltimateCommon
	{
		private int towerID = 2;

		private int ultimateBranch = 1;

		private int skillID;

		private int enemyAffected;

		private int chanceToCast;

		private float stunDuration;

		private float skillRange;

		private float cooldownTime;

		private float timeTracking;

		private CommonAttackDamage commonAttackDamage = new CommonAttackDamage();

		private EffectAttack effectAttack;

		private TowerModel towerModel;

		[SerializeField]
		private DamageToEnemiesInRange damageToEnemiesInRange;

		public override void InitTowerModel(TowerModel towerModel)
		{
			this.towerModel = towerModel;
		}

		public override void UnlockUltimate(int ultiLevel)
		{
			base.UnlockUltimate(ultiLevel);
			ReadParameter(ultiLevel);
			TryToCastSkill();
		}

		public override void OnReturnPool()
		{
			base.OnReturnPool();
		}

		private void Update()
		{
			if (unlock)
			{
				if (isCooldownDone())
				{
					TryToCastSkill();
				}
				timeTracking = Mathf.MoveTowards(timeTracking, 0f, Time.deltaTime);
			}
		}

		public void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.cyan;
			Gizmos.DrawWireSphere(base.transform.position, skillRange);
		}

		private void ReadParameter(int currentSkillLevel)
		{
			chanceToCast = TowerSkillParameter.Instance.GetParamBySkillLevel(towerID, ultimateBranch, skillID, currentSkillLevel, 0);
			enemyAffected = TowerSkillParameter.Instance.GetParamBySkillLevel(towerID, ultimateBranch, skillID, currentSkillLevel, 1);
			stunDuration = TowerSkillParameter.Instance.GetParamBySkillLevel(towerID, ultimateBranch, skillID, currentSkillLevel, 2);
			skillRange = (float)TowerSkillParameter.Instance.GetParamBySkillLevel(towerID, ultimateBranch, skillID, currentSkillLevel, 3) / GameData.PIXEL_PER_UNIT;
			cooldownTime = TowerSkillParameter.Instance.GetParamBySkillLevel(towerID, ultimateBranch, skillID, currentSkillLevel, 4);
			unlock = true;
			timeTracking = cooldownTime;
			commonAttackDamage.aoeRange = skillRange;
			effectAttack.buffKey = "Stun";
			effectAttack.debuffChance = 100;
			effectAttack.debuffEffectValue = 100;
			effectAttack.debuffEffectDuration = stunDuration;
			effectAttack.damageFXType = DamageFXType.Thunder;
			InitFXs();
		}

		private void InitFXs()
		{
			SingletonMonoBehaviour<SpawnFX>.Instance.InitFX(SpawnFX.EFFECT_THUNDER);
		}

		private void TryToCastSkill()
		{
			if (!unlock)
			{
				return;
			}
			if (chanceToCast > 0 && Random.Range(0, 100) < chanceToCast)
			{
				List<EnemyModel> listEnemiesInRange = GameTools.GetListEnemiesInRange(base.gameObject, commonAttackDamage);
				if (listEnemiesInRange.Count > 0)
				{
					damageToEnemiesInRange.CastDamage(enemyAffected, DamageType.Range, commonAttackDamage, effectAttack);
					towerModel.towerSoundController.PlayCastSkillSound(skillID);
				}
			}
			timeTracking = cooldownTime;
		}

		private bool isCooldownDone()
		{
			return timeTracking == 0f;
		}
	}
}

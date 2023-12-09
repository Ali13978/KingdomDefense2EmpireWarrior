using Parameter;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class Tower2Ultimate1Skill1 : TowerUltimateCommon
	{
		private int towerID = 2;

		private int ultimateBranch = 1;

		private int skillID = 1;

		private int enemyAffected;

		private int physicsDamage;

		private int magicDamage;

		private float skillRange;

		private float cooldownTime;

		private float timeTracking;

		private CommonAttackDamage commonAttackDamage;

		private AttackWithSpecialEffect attackWithSpecialEffect;

		private TowerModel towerModel;

		[SerializeField]
		private DamageToEnemiesInRange damageToEnemiesInRange;

		[SerializeField]
		private AttackAnimationController attackAnimationController;

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
			attackAnimationController.OnReturnPool();
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
			Gizmos.color = Color.magenta;
			Gizmos.DrawWireSphere(base.transform.position, skillRange);
		}

		private void ReadParameter(int currentSkillLevel)
		{
			physicsDamage = TowerSkillParameter.Instance.GetParamBySkillLevel(towerID, ultimateBranch, skillID, currentSkillLevel, 0);
			enemyAffected = TowerSkillParameter.Instance.GetParamBySkillLevel(towerID, ultimateBranch, skillID, currentSkillLevel, 1);
			cooldownTime = TowerSkillParameter.Instance.GetParamBySkillLevel(towerID, ultimateBranch, skillID, currentSkillLevel, 2);
			skillRange = (float)TowerSkillParameter.Instance.GetParamBySkillLevel(towerID, ultimateBranch, skillID, currentSkillLevel, 3) / GameData.PIXEL_PER_UNIT;
			unlock = true;
			timeTracking = cooldownTime;
			commonAttackDamage = new CommonAttackDamage();
			commonAttackDamage.physicsDamage = physicsDamage;
			commonAttackDamage.magicDamage = magicDamage;
			commonAttackDamage.aoeRange = skillRange;
			commonAttackDamage.targetType.isAir = true;
			attackWithSpecialEffect.attackFXType = AttackFXType.Electric;
			attackWithSpecialEffect.duration = 0.75f;
		}

		private void TryToCastSkill()
		{
			if (unlock)
			{
				List<EnemyModel> listEnemiesInRange = GameTools.GetListEnemiesInRange(base.gameObject, commonAttackDamage);
				if (listEnemiesInRange.Count > 0)
				{
					damageToEnemiesInRange.CastDamage(enemyAffected, DamageType.Range, commonAttackDamage, attackWithSpecialEffect);
					towerModel.towerSoundController.PlayCastSkillSound(skillID);
				}
				timeTracking = cooldownTime;
			}
		}

		private bool isCooldownDone()
		{
			return timeTracking == 0f;
		}
	}
}

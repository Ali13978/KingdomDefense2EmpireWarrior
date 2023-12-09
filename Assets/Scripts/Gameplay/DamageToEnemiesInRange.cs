using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class DamageToEnemiesInRange : MonoBehaviour
	{
		[SerializeField]
		private DamageToSingleEnemy damageToSingleEnemy;

		[SerializeField]
		private AttackAnimationController attackAnimationController;

		private List<EnemyModel> enemiesInAoeRange = new List<EnemyModel>();

		public void CastDamage(int numberOfEnemy, DamageType damageType, CommonAttackDamage commonAttackDamage, AttackWithSpecialEffect attackWithSpecialEffect)
		{
			enemiesInAoeRange = GameTools.GetListEnemiesInRange(base.gameObject, commonAttackDamage);
			if (enemiesInAoeRange.Count <= 0)
			{
				return;
			}
			for (int i = 0; i < numberOfEnemy; i++)
			{
				int index = UnityEngine.Random.Range(0, enemiesInAoeRange.Count);
				damageToSingleEnemy.CastDamage(damageType, enemiesInAoeRange[index], commonAttackDamage);
				if (attackWithSpecialEffect.attackFXType == AttackFXType.Electric)
				{
					RunAnimation(enemiesInAoeRange[index].gameObject, attackWithSpecialEffect.duration);
				}
			}
		}

		public void CastDamage(int numberOfEnemy, DamageType damageType, CommonAttackDamage commonAttackDamage, EffectAttack effectAttack)
		{
			enemiesInAoeRange = GameTools.GetListEnemiesInRange(base.gameObject, commonAttackDamage);
			if (enemiesInAoeRange.Count > 0)
			{
				for (int i = 0; i < numberOfEnemy; i++)
				{
					damageToSingleEnemy.CastDamage(damageType, enemiesInAoeRange[Random.Range(0, enemiesInAoeRange.Count)], commonAttackDamage, effectAttack);
				}
			}
		}

		private void RunAnimation(GameObject target, float lifeTime)
		{
			if (!(attackAnimationController == null))
			{
				attackAnimationController.Init(target, lifeTime);
			}
		}
	}
}

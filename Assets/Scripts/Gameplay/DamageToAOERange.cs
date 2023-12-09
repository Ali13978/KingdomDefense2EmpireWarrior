using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class DamageToAOERange : MonoBehaviour
	{
		[SerializeField]
		private DamageToSingleEnemy damageToSingleEnemy;

		private List<EnemyModel> enemiesInAoeRange = new List<EnemyModel>();

		public void CastDamage(GameObject targetPosition, DamageType damageType, CommonAttackDamage commonAttackDamage)
		{
			enemiesInAoeRange = GameTools.GetListEnemiesInRange(targetPosition, commonAttackDamage);
			for (int i = 0; i < enemiesInAoeRange.Count; i++)
			{
				damageToSingleEnemy.CastDamage(damageType, enemiesInAoeRange[i], commonAttackDamage);
			}
		}

		public void CastDamage(DamageType damageType, CommonAttackDamage commonAttackDamage)
		{
			enemiesInAoeRange = GameTools.GetListEnemiesInRange(base.gameObject, commonAttackDamage);
			for (int i = 0; i < enemiesInAoeRange.Count; i++)
			{
				damageToSingleEnemy.CastDamage(damageType, enemiesInAoeRange[i], commonAttackDamage);
			}
		}

		public void CastDamage(DamageType damageType, CommonAttackDamage commonAttackDamage, EffectAttack effectAttack)
		{
			enemiesInAoeRange = GameTools.GetListEnemiesInRange(base.gameObject, commonAttackDamage);
			for (int i = 0; i < enemiesInAoeRange.Count; i++)
			{
				damageToSingleEnemy.CastDamage(damageType, enemiesInAoeRange[i], commonAttackDamage, effectAttack);
			}
		}
	}
}

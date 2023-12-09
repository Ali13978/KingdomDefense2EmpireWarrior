using UnityEngine;

namespace Gameplay
{
	public class DamageToSingleEnemy : MonoBehaviour
	{
		public void CastDamage(DamageType damageType, EnemyModel enemy, CommonAttackDamage attackDamageParameter)
		{
			enemy.ProcessDamage(damageType, attackDamageParameter);
		}

		public void CastDamage(DamageType damageType, EnemyModel enemy, CommonAttackDamage attackDamageParameter, EffectAttack effectAttack)
		{
			enemy.ProcessDamage(damageType, attackDamageParameter, effectAttack);
		}
	}
}

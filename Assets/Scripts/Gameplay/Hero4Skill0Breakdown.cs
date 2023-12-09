using MyCustom;
using UnityEngine;

namespace Gameplay
{
	public class Hero4Skill0Breakdown : CustomMonoBehaviour
	{
		private string buffKey;

		private int slowPercent;

		private float duration;

		[SerializeField]
		private DamageToAOERange damageToAOERange;

		private CommonAttackDamage commonAttackDamageSender;

		private EffectAttack effectAttackSender;

		public void Init(int physicsDamage, float aoeRange, string buffKey, float duration)
		{
			commonAttackDamageSender = new CommonAttackDamage();
			commonAttackDamageSender.physicsDamage = physicsDamage;
			commonAttackDamageSender.magicDamage = 0;
			commonAttackDamageSender.criticalStrikeChance = 0;
			commonAttackDamageSender.isIgnoreArmor = false;
			commonAttackDamageSender.aoeRange = aoeRange;
			effectAttackSender.buffKey = buffKey;
			effectAttackSender.debuffChance = 100;
			effectAttackSender.debuffEffectValue = 100;
			effectAttackSender.debuffEffectDuration = duration;
			effectAttackSender.damageFXType = DamageFXType.Stun;
			DamageWithAOE();
			CustomInvoke(ReturnPool, duration);
		}

		public void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(base.transform.position, commonAttackDamageSender.aoeRange);
		}

		private void DamageWithAOE()
		{
			damageToAOERange.CastDamage(DamageType.Range, commonAttackDamageSender, effectAttackSender);
		}

		private void ReturnPool()
		{
			SingletonMonoBehaviour<SpawnBullet>.Instance.Push(base.gameObject);
		}
	}
}

using MyCustom;
using UnityEngine;

namespace Gameplay
{
	public class Hero3Skill3SunStrike : CustomMonoBehaviour
	{
		[SerializeField]
		private DamageToAOERange damageToAOERange;

		private CommonAttackDamage commonAttackDamageSender;

		public void Init(int _physicsDamage, int _magicDamage, float _aoeRange, float _lifeTime, float _distance)
		{
			commonAttackDamageSender = new CommonAttackDamage();
			commonAttackDamageSender.physicsDamage = _physicsDamage;
			commonAttackDamageSender.magicDamage = _magicDamage;
			commonAttackDamageSender.criticalStrikeChance = 0;
			commonAttackDamageSender.isIgnoreArmor = false;
			commonAttackDamageSender.aoeRange = _aoeRange;
			DamageWithAOE();
			CustomInvoke(OnMoveComplete, _lifeTime);
		}

		public void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(base.transform.position, commonAttackDamageSender.aoeRange);
		}

		private void DamageWithAOE()
		{
			damageToAOERange.CastDamage(DamageType.Range, commonAttackDamageSender);
		}

		private void OnMoveComplete()
		{
			SingletonMonoBehaviour<SpawnBullet>.Instance.Push(base.gameObject);
		}
	}
}

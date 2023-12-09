using DG.Tweening;
using UnityEngine;

namespace Gameplay
{
	public class Hero5Skill3LightningSpear : MonoBehaviour
	{
		private float duration;

		[SerializeField]
		private DamageToAOERange damageToAOERange;

		[SerializeField]
		private EffectCaster effectCaster;

		private CommonAttackDamage commonAttackDamageSender;

		private Vector3 target;

		private bool isReady;

		private void Update()
		{
			if (isReady)
			{
				base.transform.right = target - base.transform.position;
			}
		}

		public void Init(float skillRange, int damagePhysics, float duration, Vector2 targetPosition)
		{
			this.duration = duration;
			target = targetPosition;
			commonAttackDamageSender = new CommonAttackDamage();
			commonAttackDamageSender.physicsDamage = damagePhysics;
			commonAttackDamageSender.magicDamage = 0;
			commonAttackDamageSender.criticalStrikeChance = 0;
			commonAttackDamageSender.isIgnoreArmor = false;
			commonAttackDamageSender.aoeRange = skillRange;
			base.transform.DOMove(targetPosition, duration).OnComplete(OnMoveComplete);
			isReady = true;
		}

		public void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(base.transform.position, commonAttackDamageSender.aoeRange);
		}

		private void OnMoveComplete()
		{
			DamageWithAOE();
			effectCaster.CastEffect(SpawnFX.LIGHTNING_EXPLOSION_2, 0.75f, target);
			ReturnPool();
		}

		private void DamageWithAOE()
		{
			damageToAOERange.CastDamage(DamageType.Range, commonAttackDamageSender);
		}

		private void DamageToSingleEnemy(EnemyModel enemy)
		{
			enemy.ProcessDamage(DamageType.Range, commonAttackDamageSender);
		}

		private void ReturnPool()
		{
			isReady = false;
			SingletonMonoBehaviour<SpawnBullet>.Instance.Push(base.gameObject);
		}
	}
}

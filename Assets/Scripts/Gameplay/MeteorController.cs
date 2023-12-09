using DG.Tweening;
using UnityEngine;

namespace Gameplay
{
	public class MeteorController : MonoBehaviour
	{
		[SerializeField]
		private DamageToAOERange damageToAOERange;

		[SerializeField]
		private EffectCaster effectCaster;

		private CommonAttackDamage commonAttackDamageSender;

		public void Init(int _damage, float _aoeRange, float _lifeTime, float _distance)
		{
			Transform transform = base.transform;
			Vector3 position = base.transform.position;
			transform.DOMoveY(position.y - _distance, _lifeTime).SetEase(Ease.Linear).OnComplete(OnMoveComplete);
			EffectController effect = SingletonMonoBehaviour<SpawnFX>.Instance.GetEffect(SpawnFX.LIGHTNING_PROJECTILE_SHADOW);
			EffectController effectController = effect;
			Vector3 position2 = base.transform.position;
			float x = position2.x;
			Vector3 position3 = base.transform.position;
			effectController.Init(_lifeTime, new Vector2(x, position3.y - _distance));
			effect.DoFadeIn(_lifeTime, 1f);
			effect.SetBiggerOverTime(_lifeTime);
			commonAttackDamageSender = new CommonAttackDamage();
			commonAttackDamageSender.physicsDamage = _damage;
			commonAttackDamageSender.magicDamage = 0;
			commonAttackDamageSender.criticalStrikeChance = 0;
			commonAttackDamageSender.isIgnoreArmor = false;
			commonAttackDamageSender.aoeRange = _aoeRange;
		}

		public void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(base.transform.position, commonAttackDamageSender.aoeRange);
		}

		private void DamageWithAOE()
		{
			ExplosionNoTarget();
			damageToAOERange.CastDamage(DamageType.Range, commonAttackDamageSender);
		}

		private void ExplosionNoTarget()
		{
			effectCaster.CastEffect(SpawnFX.METEOR_EXPLOSION, 3f, base.gameObject.transform.position);
		}

		private void OnMoveComplete()
		{
			SingletonMonoBehaviour<CameraController>.Instance.ShakeNormal();
			DamageWithAOE();
			SingletonMonoBehaviour<SpawnBullet>.Instance.Push(base.gameObject);
		}
	}
}

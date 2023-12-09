using DG.Tweening;
using GeneralVariable;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class Hero3Skill0Meteor : MonoBehaviour
	{
		private List<EnemyModel> enemiesInAoeRange = new List<EnemyModel>();

		private int physicsDamage;

		private int magicDamage;

		private float aoeRange;

		private float lifeTime;

		private float moveSpeed;

		private Vector2 heroPosition;

		private Vector2 skillCastPosition;

		private CommonAttackDamage commonAttackDamageSender;

		[SerializeField]
		private RotationOverPivot meteorObject;

		[SerializeField]
		private GameObject shadow;

		public void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(base.transform.position, aoeRange);
		}

		public void Init(int physicsDamage, int magicDamage, float aoeRange, float lifeTime, float moveSpeed, Vector2 heroPosition, Vector2 skillCastPosition, float _distance)
		{
			this.physicsDamage = physicsDamage;
			this.magicDamage = magicDamage;
			this.aoeRange = aoeRange;
			this.lifeTime = lifeTime;
			this.moveSpeed = moveSpeed;
			this.heroPosition = heroPosition;
			this.skillCastPosition = skillCastPosition;
			shadow.SetActive(value: false);
			if (skillCastPosition.x - heroPosition.x > 0f)
			{
				meteorObject.RotateDirection = 1;
			}
			else
			{
				meteorObject.RotateDirection = -1;
			}
			Transform transform = base.transform;
			Vector3 position = base.transform.position;
			transform.DOMoveY(position.y - _distance, 0.5f).SetEase(Ease.Linear).OnComplete(OnFallingComplete);
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other.gameObject.tag == GeneralVariable.GeneralVariable.ENEMY_TAG)
			{
				EnemyModel component = other.gameObject.GetComponent<EnemyModel>();
				if (!component.IsAir && component.IsAlive && !component.IsUnderground)
				{
					DamageToSingleEnemy(component);
				}
			}
		}

		private void DamageToSingleEnemy(EnemyModel enemy)
		{
			commonAttackDamageSender = new CommonAttackDamage();
			commonAttackDamageSender.physicsDamage = physicsDamage;
			commonAttackDamageSender.magicDamage = magicDamage;
			commonAttackDamageSender.criticalStrikeChance = 0;
			commonAttackDamageSender.isIgnoreArmor = false;
			enemy.ProcessDamage(DamageType.Range, commonAttackDamageSender);
			UnityEngine.Debug.Log("Deal damage!");
		}

		private void OnFallingComplete()
		{
			shadow.SetActive(value: true);
			EffectController effect = SingletonMonoBehaviour<SpawnFX>.Instance.GetEffect(SpawnFX.METEOR_EXPLOSION2);
			effect.transform.position = skillCastPosition;
			effect.Init(2f);
			SingletonMonoBehaviour<CameraController>.Instance.ShakeNormal();
			Vector3 a = Vector3.Normalize(skillCastPosition - heroPosition);
			Vector3 a2 = a * moveSpeed * lifeTime;
			a2 += base.transform.position;
			base.transform.DOMove(a2, lifeTime).OnComplete(OnMoveComplete);
		}

		private void OnMoveComplete()
		{
			EffectController effect = SingletonMonoBehaviour<SpawnFX>.Instance.GetEffect(SpawnFX.METEOR_SELF_EXPLOSION);
			effect.transform.position = base.transform.position;
			effect.Init(2f);
			shadow.SetActive(value: false);
			SingletonMonoBehaviour<SpawnBullet>.Instance.Push(base.gameObject);
		}
	}
}

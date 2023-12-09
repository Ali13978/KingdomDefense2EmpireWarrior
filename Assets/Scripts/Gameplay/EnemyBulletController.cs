using System;
using UnityEngine;

namespace Gameplay
{
	public class EnemyBulletController : MonoBehaviour
	{
		private CharacterModel targetModel;

		private CommonAttackDamage commonAttackDamageSender;

		[Space]
		[Header("New trajectory parameter")]
		public float originSpeed = 3f;

		private float currentSpeed;

		[SerializeField]
		private float speedUpOverTime = 0.5f;

		[SerializeField]
		private float hitDistance = 0.2f;

		[SerializeField]
		private float ballisticOffset = 0.5f;

		[SerializeField]
		private int bulletDirection = 1;

		[SerializeField]
		private bool bulletRandomDirection;

		private Vector2 originPoint;

		private Vector2 aimPoint;

		private Vector2 myVirtualPosition;

		private Vector2 myPreviousPosition;

		private float counter;

		private bool activeTrajectoryShot;

		[Space]
		[Header("Effect")]
		[SerializeField]
		private GameObject groundEffectPrefab;

		private CastEffectOnDiedTarget castEffectOnDieTarget;

		private ExplosionInTarget explosionInTarget;

		private void Awake()
		{
			castEffectOnDieTarget = GetComponent<CastEffectOnDiedTarget>();
			explosionInTarget = GetComponent<ExplosionInTarget>();
			SingletonMonoBehaviour<SpawnFX>.Instance.InitExtendObject(groundEffectPrefab, 0);
		}

		private void FixedUpdate()
		{
			if (!activeTrajectoryShot)
			{
				return;
			}
			counter += Time.fixedDeltaTime;
			currentSpeed += Time.fixedDeltaTime * speedUpOverTime;
			if (targetModel != null)
			{
				aimPoint = targetModel.transform.position;
			}
			if ((double)aimPoint.x < 6.5 && (double)aimPoint.x > -6.5)
			{
				Vector2 vector = aimPoint - originPoint;
				Vector2 vector2 = aimPoint - myVirtualPosition;
				myVirtualPosition = Vector2.Lerp(originPoint, aimPoint, counter * currentSpeed / vector.magnitude);
				base.transform.position = AddBallisticOffset(vector.magnitude, (float)bulletDirection * vector2.magnitude);
				myPreviousPosition = base.transform.position;
				if (vector2.magnitude <= hitDistance)
				{
					if (targetModel != null && targetModel.IsAlive)
					{
						DamageToAlly();
					}
					else if ((bool)castEffectOnDieTarget)
					{
						castEffectOnDieTarget.CastEffect(base.gameObject.transform);
					}
					activeTrajectoryShot = false;
					ReturnPool();
				}
			}
			else
			{
				if ((bool)castEffectOnDieTarget)
				{
					castEffectOnDieTarget.CastEffect(base.gameObject.transform);
				}
				ReturnPool();
			}
		}

		public void Init(CharacterModel _characterModel, int physicsDamage)
		{
			targetModel = _characterModel;
			commonAttackDamageSender = new CommonAttackDamage();
			commonAttackDamageSender.physicsDamage = physicsDamage;
			commonAttackDamageSender.magicDamage = 0;
			commonAttackDamageSender.criticalStrikeChance = 0;
			CalculateParameterForTrajectoryShot();
		}

		public void DamageToAlly()
		{
			if ((bool)explosionInTarget)
			{
				explosionInTarget.CastExplosion(targetModel.transform);
			}
			if (commonAttackDamageSender.criticalStrikeChance > 0 && UnityEngine.Random.Range(0, 100) < commonAttackDamageSender.criticalStrikeChance)
			{
				commonAttackDamageSender.physicsDamage *= 2;
				commonAttackDamageSender.magicDamage *= 2;
			}
			targetModel.ProcessDamage(DamageType.Range, commonAttackDamageSender);
		}

		private void CalculateParameterForTrajectoryShot()
		{
			originPoint = (myVirtualPosition = (myPreviousPosition = base.transform.position));
			aimPoint = targetModel.transform.position;
			counter = 0f;
			currentSpeed = originSpeed;
			if (bulletRandomDirection)
			{
				bulletDirection = ((UnityEngine.Random.Range(0, 100) > 50) ? 1 : (-1));
			}
			activeTrajectoryShot = true;
		}

		private Vector2 AddBallisticOffset(float originDistance, float distanceToAim)
		{
			if (ballisticOffset > 0f)
			{
				float num = Mathf.Sin((float)Math.PI * ((originDistance - distanceToAim) / originDistance));
				num *= originDistance;
				return myVirtualPosition + ballisticOffset * num * Vector2.up;
			}
			return myVirtualPosition;
		}

		public void ReturnPool()
		{
			SingletonMonoBehaviour<SpawnBullet>.Instance.Push(base.gameObject);
		}
	}
}

using DG.Tweening;
using GeneralVariable;
using System;
using UnityEngine;

namespace Gameplay
{
	public class BulletMoveController : MonoBehaviour
	{
		public enum MoveType
		{
			ParabolShot,
			TrajectoryShot,
			DirectShot,
			LineShot,
			HomingMissileShot
		}

		[SerializeField]
		private MoveType moveType;

		private CastEffectOnDiedTarget castEffectOnDiedTarget;

		[Header("Parabol shot param")]
		[SerializeField]
		private float gravity;

		private Rigidbody2D rigid2D;

		private EnemyModel targetEnemy;

		private Vector2 targetPosition;

		private BulletModel bulletModel;

		private float firingAngle;

		private float velocityX;

		private float velocityY;

		private Vector3 startPosition;

		private float parabolShotDuration;

		private float parabolShotTimeTracking;

		private float totalVelo;

		private bool activeParabolShot;

		[Space]
		[Header("Trajectory shot param")]
		public float originSpeed = 3f;

		private float currentTimeSpeed;

		[SerializeField]
		private float speedUpOverTime = 0.5f;

		[SerializeField]
		private float hitDistance = 0.2f;

		private float sqHitDistance;

		private Vector3 midPoint;

		private float inverseEstimatedDur;

		[SerializeField]
		private float ballisticOffset = 0.5f;

		[SerializeField]
		private int bulletDirection = 1;

		[SerializeField]
		private bool bulletRandomDirection;

		private Vector3 originPoint;

		private Vector3 aimPoint;

		private Vector2 myVirtualPosition;

		private Vector2 myPreviousPosition;

		private float counter;

		private bool activeTrajectoryShot;

		[Space]
		[Header("Direct shot param")]
		[SerializeField]
		private float directShotSpeed;

		private float directShotMaxRange;

		private float directShotDuration;

		private float directShotTimeOut;

		private float directShotTimeTracking;

		private bool activateDirectShot;

		private Vector3 endPoint;

		[Space]
		[Header("Line shot param")]
		[SerializeField]
		private float lineShotSpeed;

		[SerializeField]
		private float lineShotMaxRange;

		private bool activateLineShot;

		private float lineShotDuration;

		private Vector3 rootPositionLineShot;

		private Vector3 targetPositionLineShot;

		private Vector3 endPositionLineShot;

		[Space]
		[Header("Homing missile param")]
		[SerializeField]
		private Transform missileSprite;

		[SerializeField]
		private float maxSpeed;

		private bool activateHomingMissile;

		public float kProportionalConst = 0.45f;

		private Vector2 subValue;

		private Vector2 desiredVelocity;

		private Vector2 error;

		private Vector2 currentVelocity;

		private Vector2 sForce;

		private Vector3 p1;

		private Vector3 p2;

		private Vector3 finalPos;

		private void Awake()
		{
			GetAllComponents();
			bulletModel.OnInitialized += BulletModel_OnInitialized;
		}

		private void GetAllComponents()
		{
			bulletModel = GetComponent<BulletModel>();
			rigid2D = GetComponent<Rigidbody2D>();
			castEffectOnDiedTarget = GetComponent<CastEffectOnDiedTarget>();
			sqHitDistance = hitDistance * hitDistance;
		}

		private void Update()
		{
			if (activeParabolShot)
			{
				parabolShotTimeTracking += Time.deltaTime;
				if (parabolShotTimeTracking > parabolShotDuration)
				{
					if ((bool)castEffectOnDiedTarget)
					{
						castEffectOnDiedTarget.CastEffect(base.transform);
					}
					OnParabolShotComplete();
				}
			}
			if (activateDirectShot)
			{
				directShotTimeTracking += Time.deltaTime;
				if (directShotTimeTracking > directShotTimeOut)
				{
					OnDirectShotComplete();
				}
			}
			if (activeTrajectoryShot)
			{
				currentTimeSpeed += Time.deltaTime * speedUpOverTime;
				counter += Time.deltaTime * currentTimeSpeed;
				if (targetEnemy != null)
				{
					aimPoint = targetEnemy.transform.position;
				}
				if ((double)aimPoint.x < 6.5 && (double)aimPoint.x > -6.5)
				{
					float num = counter * inverseEstimatedDur;
					p1 = originPoint + (midPoint - originPoint) * num;
					p2 = midPoint + (aimPoint - midPoint) * num;
					finalPos = p1 + (p2 - p1) * num;
					base.transform.position = finalPos;
					if (num >= 1f)
					{
						if (targetEnemy != null && targetEnemy.IsAlive)
						{
							bulletModel.AttackEnemy(bulletModel.target);
						}
						else if ((bool)castEffectOnDiedTarget)
						{
							castEffectOnDiedTarget.CastEffect(base.transform);
						}
						activeTrajectoryShot = false;
						bulletModel.ReturnPool();
					}
				}
				else
				{
					if ((bool)castEffectOnDiedTarget)
					{
						castEffectOnDiedTarget.CastEffect(base.gameObject.transform);
					}
					bulletModel.ReturnPool();
				}
			}
			if (activateHomingMissile)
			{
				subValue = targetPosition - (Vector2)base.transform.position;
				subValue.Normalize();
				desiredVelocity = subValue * maxSpeed;
				error = desiredVelocity - currentVelocity;
				sForce = error * kProportionalConst;
				currentVelocity += sForce * Time.deltaTime;
				base.transform.Translate(new Vector3(currentVelocity.x, currentVelocity.y, 0f));
				float z = Mathf.Atan2(currentVelocity.y, currentVelocity.x) * 57.29578f;
				missileSprite.transform.rotation = Quaternion.Slerp(missileSprite.transform.rotation, Quaternion.Euler(0f, 0f, z), 100f * Time.deltaTime);
				if (GameTools.SquareDistance(bulletModel.transform.position, targetPosition) < sqHitDistance)
				{
					bulletModel.OnMoveToPosition();
					activateHomingMissile = false;
				}
			}
		}

		private void BulletModel_OnInitialized()
		{
			switch (moveType)
			{
			case MoveType.ParabolShot:
				CalculateParameterForParabolShot();
				BulletGoParabolPath();
				break;
			case MoveType.TrajectoryShot:
				CalculateParameterForTrajectoryShot();
				break;
			case MoveType.DirectShot:
				CalculateParameterForDirectShot();
				BulletGoDirectPath();
				break;
			case MoveType.LineShot:
				CalculateParameterForLineShot();
				BulletGoLineShot();
				break;
			case MoveType.HomingMissileShot:
				CalculateParameterForHomingMissilehot();
				break;
			}
		}

		private void CalculateParameterForHomingMissilehot()
		{
			targetPosition = bulletModel.targetPosition;
			activateHomingMissile = true;
		}

		private void CalculateParameterForTrajectoryShot()
		{
			targetEnemy = bulletModel.target;
			originPoint = (myVirtualPosition = (myPreviousPosition = base.transform.position));
			aimPoint = targetEnemy.transform.position;
			counter = 0f;
			currentTimeSpeed = 1f;
			float min = 0.7f;
			bulletDirection = 1;
			if (bulletRandomDirection)
			{
				bulletDirection = ((UnityEngine.Random.Range(0, 100) > 50) ? 1 : (-1));
				min = 0.25f;
			}
			activeTrajectoryShot = true;
			float magnitude = (aimPoint - originPoint).magnitude;
			float num = magnitude / originSpeed;
			if (num > 0f)
			{
				inverseEstimatedDur = 1f / num;
			}
			else
			{
				num = 10f;
			}
			midPoint = originPoint + (aimPoint - originPoint) * 0.5f;
			midPoint.y = Mathf.Max(originPoint.y, aimPoint.y) + UnityEngine.Random.Range(min, 1f) * (float)bulletDirection;
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

		private void CalculateParameterForParabolShot()
		{
			targetEnemy = bulletModel.target;
			startPosition = bulletModel.transform.position;
			targetPosition = targetEnemy.preMovePos;
			float x = targetPosition.x;
			Vector3 position = base.transform.position;
			float num = x - position.x;
			float y = targetPosition.y;
			Vector3 position2 = base.transform.position;
			float num2 = y - position2.y;
			firingAngle = Mathf.Atan((num2 + (0f - gravity) / 2f) / num);
			totalVelo = num / Mathf.Cos(firingAngle);
			velocityX = totalVelo * Mathf.Cos(firingAngle);
			velocityY = totalVelo * Mathf.Sin(firingAngle);
			parabolShotDuration = Mathf.Abs((targetPosition.x - startPosition.x) / velocityX);
			parabolShotTimeTracking = 0f;
			activeParabolShot = true;
		}

		private void BulletGoParabolPath()
		{
			rigid2D.velocity = new Vector2(velocityX, velocityY);
		}

		private void OnTriggerEnter2D(Collider2D coll)
		{
			if (coll.tag == GeneralVariable.GeneralVariable.ENEMY_TAG)
			{
				EnemyModel component = coll.gameObject.GetComponent<EnemyModel>();
				if (GameTools.IsValidEnemy(component) && !component.IsUnderground)
				{
					bulletModel.AttackEnemy(component);
				}
			}
		}

		private void CalculateParameterForDirectShot()
		{
			targetEnemy = bulletModel.target;
			if ((bool)targetEnemy)
			{
				directShotMaxRange = bulletModel.commonAttackDamage.maxRange;
				directShotDuration = Vector3.Distance(base.gameObject.transform.position, targetEnemy.transform.position) / directShotSpeed;
				directShotTimeOut = directShotMaxRange / directShotSpeed;
				directShotTimeTracking = 0f;
				activateDirectShot = true;
				endPoint = getDirectShotEndPoint(base.gameObject, targetEnemy.gameObject, directShotMaxRange);
			}
		}

		private Vector3 getDirectShotEndPoint(GameObject source, GameObject target, float maxRange)
		{
			Vector3 zero = Vector3.zero;
			float num = 0f;
			Vector3 position = target.transform.position;
			float x = position.x;
			Vector3 position2 = source.transform.position;
			if (x >= position2.x)
			{
				Vector3 position3 = target.transform.position;
				float y = position3.y;
				Vector3 position4 = source.transform.position;
				if (y >= position4.y)
				{
					Vector3 from = target.transform.position - source.transform.position;
					num = Vector3.Angle(from, Vector3.right);
					UnityEngine.Debug.Log("angle1 = " + num);
					float newX = maxRange * Mathf.Cos(num * (float)Math.PI / 180f);
					float newY = maxRange * Mathf.Cos((90f - num) * (float)Math.PI / 180f);
					zero.Set(newX, newY, 0f);
				}
			}
			Vector3 position5 = target.transform.position;
			float x2 = position5.x;
			Vector3 position6 = source.transform.position;
			if (x2 > position6.x)
			{
				Vector3 position7 = target.transform.position;
				float y2 = position7.y;
				Vector3 position8 = source.transform.position;
				if (y2 < position8.y)
				{
					Vector3 from2 = target.transform.position - source.transform.position;
					num = Vector3.Angle(from2, Vector3.right);
					UnityEngine.Debug.Log("angle2 = " + num);
					float newX2 = maxRange * Mathf.Cos(num * (float)Math.PI / 180f);
					float newY2 = (0f - maxRange) * Mathf.Cos((90f - num) * (float)Math.PI / 180f);
					zero.Set(newX2, newY2, 0f);
				}
			}
			Vector3 position9 = target.transform.position;
			float x3 = position9.x;
			Vector3 position10 = source.transform.position;
			if (x3 <= position10.x)
			{
				Vector3 position11 = target.transform.position;
				float y3 = position11.y;
				Vector3 position12 = source.transform.position;
				if (y3 <= position12.y)
				{
					Vector3 from3 = target.transform.position - source.transform.position;
					num = Vector3.Angle(from3, Vector3.left);
					UnityEngine.Debug.Log("angle3 = " + num);
					float newX3 = (0f - maxRange) * Mathf.Cos(num * (float)Math.PI / 180f);
					float newY3 = (0f - maxRange) * Mathf.Cos((90f - num) * (float)Math.PI / 180f);
					zero.Set(newX3, newY3, 0f);
				}
			}
			Vector3 position13 = target.transform.position;
			float x4 = position13.x;
			Vector3 position14 = source.transform.position;
			if (x4 < position14.x)
			{
				Vector3 position15 = target.transform.position;
				float y4 = position15.y;
				Vector3 position16 = source.transform.position;
				if (y4 > position16.y)
				{
					Vector3 from4 = target.transform.position - source.transform.position;
					num = Vector3.Angle(from4, Vector3.left);
					UnityEngine.Debug.Log("angle4 = " + num);
					float newX4 = (0f - maxRange) * Mathf.Cos(num * (float)Math.PI / 180f);
					float newY4 = maxRange * Mathf.Cos((90f - num) * (float)Math.PI / 180f);
					zero.Set(newX4, newY4, 0f);
				}
			}
			return zero;
		}

		private void BulletGoDirectPath()
		{
			bulletModel.transform.DOMove(2f * targetEnemy.transform.position, directShotDuration).OnComplete(OnDirectShotComplete);
		}

		private void CalculateParameterForLineShot()
		{
			targetEnemy = bulletModel.target;
			if ((bool)targetEnemy)
			{
				rootPositionLineShot = bulletModel.towerModel.transform.position;
				targetPositionLineShot = targetEnemy.transform.position;
				endPositionLineShot = BulletCalculator.GetEndPositionLineShot(rootPositionLineShot, targetPositionLineShot, lineShotMaxRange);
				lineShotDuration = Vector3.Distance(rootPositionLineShot, endPositionLineShot) / lineShotSpeed;
			}
		}

		private void BulletGoLineShot()
		{
			bulletModel.transform.position = targetPositionLineShot;
			bulletModel.transform.DOMove(endPositionLineShot, lineShotDuration).OnComplete(OnLineShotComplete);
		}

		private void OnLineShotComplete()
		{
			activateDirectShot = false;
			bulletModel.ReturnPool();
		}

		private void OnDirectShotComplete()
		{
			activateDirectShot = false;
			bulletModel.ReturnPool();
		}

		private void OnParabolShotComplete()
		{
			activeParabolShot = false;
			bulletModel.OnMoveToPosition();
		}

		private void OnDestroy()
		{
			bulletModel.OnInitialized -= BulletModel_OnInitialized;
		}
	}
}

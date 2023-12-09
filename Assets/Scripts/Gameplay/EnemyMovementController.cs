using DG.Tweening;
using Parameter;
using SSR.Core.Architecture;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class EnemyMovementController : EnemyController
	{
		private List<string> slowBuffKeys = new List<string>
		{
			"Slow"
		};

		private List<string> stunBuffKeys = new List<string>
		{
			"Stun"
		};

		[Space]
		[SerializeField]
		private OrderedEventDispatcher onStartRun;

		[SerializeField]
		private OrderedEventDispatcher onFinishPath;

		private bool isRunning;

		[SerializeField]
		private bool haveRestOnTheWay;

		[SerializeField]
		private float timeStepToRest;

		[SerializeField]
		private float restTimeMin;

		[SerializeField]
		private float restTimeMax;

		[NonSerialized]
		public int currentLine;

		private Tween tween;

		private float speed;

		private float originSpeed;

		private float timeTrackingRest;

		private int currentWaypoint;

		private float boostedSpeed;

		public float currentTweenPosition;

		private float currentSlowPercentage;

		private float currentStunPercentage;

		public float MinRestDuration => restTimeMin / 1000f;

		public float MaxRestDuration => restTimeMax / 1000f;

		public bool HaveRestOnTheWay => haveRestOnTheWay;

		public float DelayToRest => timeStepToRest / 1000f;

		public float Speed
		{
			get
			{
				return speed;
			}
			set
			{
				speed = value;
			}
		}

		public float SpeedMultiplier => (Speed + boostedSpeed) / OriginSpeed;

		public float OriginSpeed
		{
			get
			{
				return originSpeed;
			}
			private set
			{
				originSpeed = value;
			}
		}

		public override void Initialize()
		{
			base.Initialize();
			base.EnemyModel.BuffsHolder.OnBuffValueChanged += BuffsHolder_OnBuffValueChanged;
			GameEventCenter.Instance.Subscribe(GameEventType.OnEnemyMoveToEndPoint, new EnemySubscriberData(GameTools.GetUniqueId(), OnMoveToEndPoint));
		}

		public override void OnAppear()
		{
			base.OnAppear();
			boostedSpeed = 0f;
			Enemy originalParameter = base.EnemyModel.OriginalParameter;
			Speed = (float)originalParameter.speed / GameData.PIXEL_PER_UNIT;
			OriginSpeed = Speed;
		}

		public override void OnReturnPool()
		{
			base.OnReturnPool();
			base.EnemyModel.transform.DOKill();
		}

		private void EnemyFindTargetController_OnTargetRemoved(CharacterModel obj)
		{
		}

		public float MoveToPosition(float fullPos, float randomPosition)
		{
			return fullPos + UnityEngine.Random.Range(0f, randomPosition);
		}

		public void MoveToPosition(float fullPos, float parentSpeed, bool obsolete)
		{
			base.EnemyModel.startPosRatio = fullPos * (parentSpeed / originSpeed);
		}

		private void OnMoveToEndPoint(EnemyModel enemyModel)
		{
			if (base.EnemyModel.GetInstanceID() == enemyModel.GetInstanceID())
			{
				SingletonMonoBehaviour<CameraController>.Instance.ShakeNormal();
				onFinishPath.Dispatch();
				GameLogicController gameLogicController = GameplayManager.Instance.gameLogicController;
				Enemy originalParameter = base.EnemyModel.OriginalParameter;
				gameLogicController.DecreaseHealth(originalParameter.lifeTaken);
				if (EnemyParameterManager.Instance.IsEnemyHasMoreThanOneLife(enemyModel.Id))
				{
					GameData instance = SingletonMonoBehaviour<GameData>.Instance;
					int totalEnemy = instance.TotalEnemy;
					Enemy originalParameter2 = base.EnemyModel.OriginalParameter;
					instance.TotalEnemy = totalEnemy - originalParameter2.lifeCount;
				}
				else
				{
					SingletonMonoBehaviour<GameData>.Instance.TotalEnemy--;
				}
				base.EnemyModel.ReturnPool(0f);
			}
		}

		private void BuffsHolder_OnBuffValueChanged(string buffKey, bool added)
		{
			if (slowBuffKeys.Contains(buffKey))
			{
				ApplySlowBuffs();
			}
			if (stunBuffKeys.Contains(buffKey))
			{
				ApplyStunBuffs();
			}
		}

		private void ApplySlowBuffs()
		{
			currentSlowPercentage = base.EnemyModel.BuffsHolder.GetBuffsValue(slowBuffKeys);
			if (!(currentStunPercentage > 0f))
			{
				Speed = OriginSpeed * (1f - currentSlowPercentage / 100f);
				Speed = Mathf.Clamp(Speed, 0f, 1f);
			}
		}

		private void ApplyStunBuffs()
		{
			currentStunPercentage = base.EnemyModel.BuffsHolder.GetBuffsValue(stunBuffKeys);
			Speed = OriginSpeed * (1f - currentStunPercentage / 100f);
			Speed = Mathf.Clamp(Speed, 0f, 1f);
		}
	}
}

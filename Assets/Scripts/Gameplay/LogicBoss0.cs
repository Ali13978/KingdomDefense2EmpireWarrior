using SSR.Core.Architecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class LogicBoss0 : EnemyController
	{
		[Header("Events")]
		[SerializeField]
		private OrderedEventDispatcher onStartAttack = new OrderedEventDispatcher();

		[SerializeField]
		private OrderedEventDispatcher onFinishAttack = new OrderedEventDispatcher();

		[SerializeField]
		private OrderedEventDispatcher onCancelAttack = new OrderedEventDispatcher();

		[Space]
		[Header("Skill paramerters")]
		[SerializeField]
		private float coolDownTime;

		[SerializeField]
		private int minionAmount;

		[SerializeField]
		private int minionId;

		[SerializeField]
		private List<int> minionGate;

		[Space]
		[Header("Common setting")]
		[SerializeField]
		private bool activateAtStart;

		[SerializeField]
		private float attackAnimationDuration = 1f;

		[SerializeField]
		private float minSpeed = 0.05f;

		[SerializeField]
		private float randomTimeDelayMin;

		[SerializeField]
		private float randomTimeDelayMax = 2f;

		[SerializeField]
		private float attackAnimationDurationRatio;

		[SerializeField]
		private float randomPosition;

		private float timeTracking;

		private bool attacking;

		private int lastGate;

		private EnemyMovementController enemyMovementController;

		private EnemyAnimationController enemyAnimationController;

		private WaitForSeconds waitForAnimation;

		public override void OnAppear()
		{
			base.OnAppear();
			timeTracking = ((!activateAtStart) ? (coolDownTime / 1000f) : 0f);
		}

		private void Start()
		{
			enemyMovementController = base.EnemyModel.EnemyMovementController;
			enemyAnimationController = base.EnemyModel.EnemyAnimationController;
			waitForAnimation = new WaitForSeconds(attackAnimationDuration);
			SingletonMonoBehaviour<SpawnEnemy>.Instance.InitAdditionEnemy(minionId);
		}

		public override void Update()
		{
			base.Update();
			if (!attacking)
			{
				if (ShouldAttack())
				{
					StartCoroutine(CreateMinions());
				}
				timeTracking = Mathf.MoveTowards(timeTracking, 0f, Time.deltaTime);
			}
		}

		private bool ShouldAttack()
		{
			return timeTracking == 0f && enemyMovementController.Speed >= minSpeed;
		}

		private IEnumerator CreateMinions()
		{
			attacking = true;
			onStartAttack.Dispatch();
			if (!IsCurrentSpeedGreaterThanMinSpeed())
			{
				yield return null;
			}
			if (!IsEnemyAlive())
			{
				yield return null;
			}
			if (GameTools.IsValidEnemy(base.EnemyModel))
			{
				base.EnemyModel.SetSpecialStateDuration(attackAnimationDuration);
				base.EnemyModel.SetSpecialStateAnimationName(EnemyAnimationController.animSpecialAttack);
				base.EnemyModel.GetFSMController().GetCurrentState().OnInput(StateInputType.SpecialState, EnemyAnimationController.animSpecialAttack);
				base.EnemyModel.EnemyAnimationController.ToSpecialAttackState();
				yield return waitForAnimation;
				int randomGate = minionGate[generateRandomNumber(0, minionGate.Count)];
				UnityEngine.Debug.Log("Spawn " + minionAmount + " minions");
				for (int i = 0; i < minionAmount; i++)
				{
					SingletonMonoBehaviour<SpawnEnemy>.Instance.SpawnAdditionEnemyAtGate(minionId, 0f, randomGate, randomPosition);
					yield return new WaitForSeconds(Random.Range(randomTimeDelayMin, randomTimeDelayMax));
				}
			}
			timeTracking = coolDownTime / 1000f;
			attacking = false;
			onFinishAttack.Dispatch();
		}

		public void OnDisable()
		{
			attacking = false;
		}

		public int generateRandomNumber(int min, int max)
		{
			int num = Random.Range(min, max);
			if (num == lastGate)
			{
				return generateRandomNumber(min, max);
			}
			lastGate = num;
			return num;
		}
	}
}

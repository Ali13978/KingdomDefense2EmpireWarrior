using SSR.Core.Architecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class LogicBoss1 : EnemyController
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
		private float minSpeed = 0.05f;

		private float timeTracking;

		private bool attacking;

		private int lastGate;

		[SerializeField]
		private float randomTimeDelayMin;

		[SerializeField]
		private float randomTimeDelayMax = 2f;

		[SerializeField]
		private float randomPosition;

		private EnemyMovementController enemyMovementController;

		private WaitForSeconds waitForAnimation;

		private WaitUntil waitForAvailableSpeed;

		private WaitUntil waitForLivingState;

		public override void OnAppear()
		{
			base.OnAppear();
			timeTracking = ((!activateAtStart) ? (coolDownTime / 1000f) : 0f);
		}

		private void Start()
		{
			enemyMovementController = base.EnemyModel.EnemyMovementController;
			SingletonMonoBehaviour<SpawnEnemy>.Instance.InitAdditionEnemy(minionId);
			waitForAvailableSpeed = new WaitUntil(IsCurrentSpeedGreaterThanMinSpeed);
			waitForLivingState = new WaitUntil(IsBossAlive);
		}

		public new void Update()
		{
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
			yield return waitForAvailableSpeed;
			yield return waitForLivingState;
			UnityEngine.Debug.Log("Spawn " + minionAmount + " minions");
			for (int i = 0; i < minionAmount; i++)
			{
				int randomGate = minionGate[generateRandomNumber(0, minionGate.Count)];
				SingletonMonoBehaviour<SpawnEnemy>.Instance.SpawnAdditionEnemyAtGate(minionId, 0f, randomGate, randomPosition);
				yield return new WaitForSeconds(Random.Range(randomTimeDelayMin, randomTimeDelayMax));
			}
			timeTracking = coolDownTime / 1000f;
			attacking = false;
			onFinishAttack.Dispatch();
		}

		private new bool IsCurrentSpeedGreaterThanMinSpeed()
		{
			return enemyMovementController.Speed > minSpeed;
		}

		private bool IsBossAlive()
		{
			return base.EnemyModel.IsAlive;
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

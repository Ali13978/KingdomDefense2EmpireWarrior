using Middle;
using UnityEngine;

namespace Gameplay
{
	public class LogicEnemySkillCreateOthers : EnemyController
	{
		[SerializeField]
		private int enemyID;

		public GameObject dieEffect;

		private EnemyMovementController enemyMovementController;

		private int currentGate = -1;

		private Vector2 poolPos = new Vector2(100f, 100f);

		public override void OnAppear()
		{
			base.OnAppear();
			currentGate = base.EnemyModel.Gate;
		}

		private void Awake()
		{
			enemyMovementController = base.EnemyModel.EnemyMovementController;
			SingletonMonoBehaviour<SpawnEnemy>.Instance.InitAdditionEnemy(enemyID);
		}

		public void CreateOtherEnemy(float delayTime)
		{
			if (dieEffect != null)
			{
				ObjectPool.Spawn(dieEffect, base.EnemyModel.transform.position);
			}
			CustomInvoke(DoCreate, delayTime);
		}

		private void DoCreate()
		{
			EnemyModel otherEnemy = SingletonMonoBehaviour<SpawnEnemy>.Instance.Get(enemyID);
			otherEnemy.SetDataStartRun(enemyID, (int)MiddleDelivery.Instance.BattleLevel, currentGate, enemyMovementController.currentLine, 0f, 0);
			otherEnemy.monsterPathData = new MonsterPathData(LineManager.Current.GetLineIndex(base.EnemyModel.Gate, base.EnemyModel.moveLine), base.EnemyModel.transform.position, delegate
			{
				GameEventCenter.Instance.Trigger(GameEventType.OnEnemyMoveToEndPoint, otherEnemy);
			});
			otherEnemy.GetFSMController();
		}

		public void OnDisable()
		{
		}
	}
}

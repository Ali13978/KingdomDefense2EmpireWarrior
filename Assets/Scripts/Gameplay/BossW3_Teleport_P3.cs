using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace Gameplay
{
	public class BossW3_Teleport_P3 : BossW3BaseState
	{
		private float minLineProp;

		private float maxLineProp;

		public BossW3_Teleport_P3(LogicEnemyBossW3 logicEnemy, float minLineProp, float maxLineProp)
			: base(logicEnemy)
		{
			if (GameTools.IsValidEnemy(enemyModel))
			{
				enemyModel.SetSpecialStateDuration(999999f);
				enemyModel.GetFSMController().GetCurrentState().SetTransition(EntityStateEnum.EnemySpecialState);
				this.minLineProp = minLineProp;
				this.maxLineProp = maxLineProp;
				logicEnemy.StartCoroutine(TeleportSequence());
			}
		}

		private IEnumerator TeleportSequence()
		{
			float transformDur = 1.15f;
			logicEnemy.animator.Play(LogicEnemyBossW3.animTurnToBird);
			yield return new WaitForSeconds(transformDur);
			MonsterPathAnchor teleRoadAnchor = logicEnemy.GetRandomPosOnRoad(minLineProp, maxLineProp);
			Vector3 telePos = teleRoadAnchor.pos;
			float dis = (enemyModel.transform.position - telePos).magnitude;
			float moveTime = dis / logicEnemy.teleSpeed;
			logicEnemy.animator.Play(LogicEnemyBossW3.animBirdRunRight);
			Transform transform = enemyModel.transform;
			float x = telePos.x;
			Vector3 position = enemyModel.transform.position;
			transform.localScale = new Vector3((x > position.x) ? 1 : (-1), 1f, 1f);
			enemyModel.transform.DOMove(telePos, moveTime);
			yield return new WaitForSeconds(moveTime + 0.1f);
			logicEnemy.animator.Play(LogicEnemyBossW3.animTurnToBoss);
			enemyModel.monsterPathData = new MonsterPathData(teleRoadAnchor, delegate
			{
				UnityEngine.Debug.LogError("22222222222 ve dich roi!!!");
				GameEventCenter.Instance.Trigger(GameEventType.OnEnemyMoveToEndPoint, enemyModel);
			});
			yield return new WaitForSeconds(transformDur);
			if (logicEnemy.summonRoutineCountdown > 0)
			{
				logicEnemy.curState = new BossW3_TurnAndSummonEnemies_P1(logicEnemy);
			}
			else
			{
				logicEnemy.curState = new BossW3_MoveToGoal_P4(logicEnemy);
			}
		}
	}
}

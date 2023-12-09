using UnityEngine;

namespace Gameplay
{
	public class BossW3_MoveCloseToGoal_P2 : BossW3BaseState
	{
		private float moveMaxDuration = 30f;

		private int nearGateSegmentId;

		private float nearGateLenProgress;

		private float countdown;

		public BossW3_MoveCloseToGoal_P2(LogicEnemyBossW3 logicEnemy)
			: base(logicEnemy)
		{
			if (GameTools.IsValidEnemy(enemyModel))
			{
				enemyModel.GetFSMController().GetCurrentState().SetTransition(EntityStateEnum.EnemyMove);
				CalculateNearGoalThreshold(enemyModel.monsterPathData.secondAnchor, 0.8f, 0.9f, out nearGateSegmentId, out nearGateLenProgress);
				countdown = moveMaxDuration;
			}
		}

		public override void OnUpdate(float dt)
		{
			base.OnUpdate(dt);
			countdown -= dt;
			if (countdown <= 0f || enemyModel.monsterPathData.secondAnchor.pathSegmentId > nearGateSegmentId || (enemyModel.monsterPathData.secondAnchor.pathSegmentId == nearGateSegmentId && enemyModel.monsterPathData.secondAnchor.lenProgress >= nearGateLenProgress))
			{
				logicEnemy.curState = new BossW3_Teleport_P3(logicEnemy, 0.35f, 0.65f);
			}
		}

		public void CalculateNearGoalThreshold(MonsterPathAnchor anchor, float minEndLineProp, float maxEndLineProp, out int nearsegmentId, out float nearlenProgress)
		{
			LineData line = LineManager.Current.GetLine(anchor.curLineIndex);
			float num = Random.Range(minEndLineProp, maxEndLineProp);
			float num2 = line.Length * num;
			int num3 = 0;
			for (int i = 0; i < line.segmentLengths.Length; i++)
			{
				if (line.segmentLengths[i] >= num2)
				{
					num3 = i;
					break;
				}
				num2 -= line.segmentLengths[i];
			}
			nearsegmentId = num3;
			nearlenProgress = num2;
		}
	}
}

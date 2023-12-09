namespace Gameplay
{
	public class BossW3_MoveToGoal_P4 : BossW3BaseState
	{
		public BossW3_MoveToGoal_P4(LogicEnemyBossW3 logicEnemy)
			: base(logicEnemy)
		{
			if (GameTools.IsValidEnemy(enemyModel))
			{
				enemyModel.GetFSMController().GetCurrentState().SetTransition(EntityStateEnum.EnemyMove);
			}
		}
	}
}

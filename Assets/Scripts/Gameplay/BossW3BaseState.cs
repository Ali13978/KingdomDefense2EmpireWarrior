namespace Gameplay
{
	public class BossW3BaseState
	{
		public LogicEnemyBossW3 logicEnemy;

		public EnemyModel enemyModel;

		public BossW3BaseState(LogicEnemyBossW3 logicEnemy)
		{
			this.logicEnemy = logicEnemy;
			enemyModel = logicEnemy.EnemyModel;
		}

		public virtual void OnUpdate(float dt)
		{
		}
	}
}

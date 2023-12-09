using Gameplay;

public class EnemyFsmController : EntityFSMController
{
	public EnemyFsmController(EnemyModel enemyModel)
	{
		AddState(EntityStateEnum.EnemyMove, new NewEnemyMoveState(enemyModel, this));
		AddState(EntityStateEnum.EnemyWaitForAtk, new NewEnemyWaitForAtkState(enemyModel, this));
		AddState(EntityStateEnum.EnemyAttack, new NewEnemyAttackState(enemyModel, this));
		AddState(EntityStateEnum.EnemyRangerAtk, new NewEnemyRangeAtkState(enemyModel, this));
		AddState(EntityStateEnum.EnemyRest, new NewEnemyRestState(enemyModel, this));
		AddState(EntityStateEnum.EnemySpecialState, new NewEnemySpecialState(enemyModel, this));
		AddState(EntityStateEnum.EnemyDie, new NewEnemyDieState(enemyModel, this));
		CreateTransition(EntityStateEnum.EnemyMove, EntityStateEnum.EnemyWaitForAtk);
		CreateTransition(EntityStateEnum.EnemyMove, EntityStateEnum.EnemyRangerAtk);
		CreateTransition(EntityStateEnum.EnemyMove, EntityStateEnum.EnemyRest);
		CreateTransition(EntityStateEnum.EnemyWaitForAtk, EntityStateEnum.EnemyMove);
		CreateTransition(EntityStateEnum.EnemyWaitForAtk, EntityStateEnum.EnemyAttack);
		CreateTransition(EntityStateEnum.EnemyAttack, EntityStateEnum.EnemyMove);
		CreateTransition(EntityStateEnum.EnemyRangerAtk, EntityStateEnum.EnemyAttack);
		CreateTransition(EntityStateEnum.EnemyRangerAtk, EntityStateEnum.EnemyMove);
		CreateTransition(EntityStateEnum.EnemyRest, EntityStateEnum.EnemyMove);
		CreateTransition(EntityStateEnum.EnemySpecialState, EntityStateEnum.EnemyMove);
		CreateTransitionFromAllState(EntityStateEnum.EnemySpecialState, EntityStateEnum.EnemyDie);
		CreateTransitionFromAllState(EntityStateEnum.EnemyDie);
		SetCurrentState(stateDictionary[EntityStateEnum.EnemyMove]);
	}
}

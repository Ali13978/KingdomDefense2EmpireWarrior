using Gameplay;

public class NewEnemyDieState : NewEnemyState
{
	private float delayDestroy;

	public NewEnemyDieState(EnemyModel enemyModel, INewFSMController fsmController)
		: base(enemyModel, fsmController)
	{
		delayDestroy = enemyModel.GetDieDuration();
	}

	public override void OnStartState()
	{
		base.OnStartState();
		enemyModel.EnemyAnimationController.ToDieState();
		enemyModel.ReturnPool(delayDestroy);
	}
}

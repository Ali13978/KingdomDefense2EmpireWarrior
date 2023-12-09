using Gameplay;

public class NewEnemySpecialState : NewEnemyState
{
	private float countdown;

	public NewEnemySpecialState(EnemyModel enemyModel, INewFSMController fsmController)
		: base(enemyModel, fsmController)
	{
	}

	public override void OnStartState()
	{
		base.OnStartState();
		countdown = enemyModel.GetSpecialStateDuration();
	}

	public override void Update(float dt)
	{
		base.Update(dt);
		countdown -= dt;
		if (countdown <= 0f)
		{
			SetTransition(EntityStateEnum.EnemyMove);
		}
	}
}

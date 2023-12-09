using Gameplay;

public class NewEnemyState : NewEntityState
{
	public EnemyModel enemyModel;

	public NewEnemyState(EnemyModel enemyModel, INewFSMController fSMController)
		: base(fSMController)
	{
		this.enemyModel = enemyModel;
	}

	public override void OnInput(StateInputType inputType, params object[] args)
	{
		base.OnInput(inputType, args);
		if (inputType == StateInputType.SpecialState)
		{
			SetTransition(EntityStateEnum.EnemySpecialState);
		}
	}
}

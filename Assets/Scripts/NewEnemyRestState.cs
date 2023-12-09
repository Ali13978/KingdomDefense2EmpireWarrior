using Gameplay;
using UnityEngine;

public class NewEnemyRestState : NewEnemyState
{
	private float minRestDur;

	private float maxRestDur;

	private float restCountdown;

	public NewEnemyRestState(EnemyModel enemyModel, INewFSMController fsmController)
		: base(enemyModel, fsmController)
	{
		minRestDur = enemyModel.EnemyMovementController.MinRestDuration;
		maxRestDur = enemyModel.EnemyMovementController.MaxRestDuration;
	}

	public override void OnStartState()
	{
		base.OnStartState();
		restCountdown = Random.Range(minRestDur, maxRestDur);
	}

	public override void Update(float dt)
	{
		base.Update(dt);
		restCountdown -= dt;
		if (restCountdown <= 0f)
		{
			SetTransition(EntityStateEnum.EnemyMove);
		}
	}
}

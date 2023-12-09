using Gameplay;

public class NewHeroWakeUpState : NewHeroIdleState
{
	private float countdown;

	public NewHeroWakeUpState(CharacterModel heroModel, INewFSMController fsmController)
		: base(heroModel, fsmController)
	{
	}

	public override void OnStartState()
	{
		EnemyModel enemyModel = FindTargetInRange();
		if (enemyModel != null)
		{
			OnInput(StateInputType.MonsterInAtkRange, enemyModel);
		}
		else
		{
			if (!heroModel.GetAnimationController().ContainAppearAnim())
			{
				SetTransition(EntityStateEnum.HeroIdle);
				return;
			}
			heroModel.GetAnimationController().ToAppearState();
		}
		countdown = 0.8f;
	}

	public override void Update(float dt)
	{
		base.Update(dt);
		countdown -= dt;
		if (countdown <= 0f)
		{
			SetTransition(EntityStateEnum.HeroIdle);
		}
	}
}

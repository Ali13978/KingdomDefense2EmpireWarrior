using Gameplay;

public class NewHeroDieState : NewHeroState
{
	private EntityStateEnum defaultReturnState;

	private float countdown;

	public NewHeroDieState(CharacterModel heroModel, INewFSMController fSMController, EntityStateEnum defaultReturnState)
		: base(heroModel, fSMController)
	{
		this.defaultReturnState = defaultReturnState;
	}

	public override void OnStartState()
	{
		base.OnStartState();
		countdown = heroModel.GetDieDuration();
		heroModel.GetAnimationController().ToDieState();
	}

	public override void Update(float dt)
	{
		base.Update(dt);
		countdown -= dt;
		if (countdown <= 0f)
		{
			SetTransition(defaultReturnState);
		}
	}

	public override void OnInput(StateInputType inputType, params object[] args)
	{
		base.OnInput(inputType, args);
		if (inputType == StateInputType.Resurge)
		{
			SetTransition(defaultReturnState);
		}
	}
}

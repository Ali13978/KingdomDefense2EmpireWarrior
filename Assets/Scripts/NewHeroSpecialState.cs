using Gameplay;

public class NewHeroSpecialState : NewHeroState
{
	private float countdown;

	private string animationName;

	public NewHeroSpecialState(CharacterModel heroModel, INewFSMController fSMController)
		: base(heroModel, fSMController)
	{
	}

	public override void OnStartState()
	{
		base.OnStartState();
		countdown = heroModel.GetSpecialStateDuration();
		animationName = heroModel.GetSpecialStateAnimationName();
		heroModel.GetAnimationController().ToSpecialState(animationName, countdown);
		heroModel.IsSpecialState = true;
	}

	public override void OnExitState()
	{
		base.OnExitState();
		heroModel.IsSpecialState = false;
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

	public override void OnInput(StateInputType inputType, params object[] args)
	{
		base.OnInput(inputType, args);
		if (inputType == StateInputType.SpecialState)
		{
		}
	}
}

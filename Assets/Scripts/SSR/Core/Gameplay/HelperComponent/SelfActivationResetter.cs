namespace SSR.Core.Gameplay.HelperComponent
{
	public class SelfActivationResetter : GameObjectLocalResetter
	{
		public override void ResetToLastSavedState()
		{
			ResetActivations();
		}

		public override void SaveCurrentState()
		{
			SaveActivations();
		}
	}
}

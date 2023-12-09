namespace SSR.Core.Gameplay.HelperComponent
{
	public class LocalScaleResetter : GameObjectLocalResetter
	{
		public override void ResetToLastSavedState()
		{
			ResetScales();
		}

		public override void SaveCurrentState()
		{
			SaveScales();
		}
	}
}

namespace SSR.Core.Gameplay.HelperComponent
{
	public class LocalPositionResetter : GameObjectLocalResetter
	{
		public override void ResetToLastSavedState()
		{
			ResetPositions();
		}

		public override void SaveCurrentState()
		{
			SavePositions();
		}
	}
}

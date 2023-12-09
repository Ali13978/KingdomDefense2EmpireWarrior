namespace SSR.Core.Gameplay.HelperComponent
{
	public interface IJetPackRideYSpeedModel
	{
		float CurrentSpeed
		{
			get;
		}

		void OnInitialize();

		void OnStartFall();

		void OnStartRise();

		void UpdateRise();

		void UpdateFall();

		void OnHitGround();

		void OnHitCeil();
	}
}

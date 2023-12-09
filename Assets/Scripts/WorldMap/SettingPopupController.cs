using Gameplay;

namespace WorldMap
{
	public class SettingPopupController : GameplayPopupController
	{
		public void Init()
		{
			OpenWithScaleAnimation();
		}

		public override void OpenWithScaleAnimation()
		{
			base.OpenWithScaleAnimation();
		}

		public override void CloseWithScaleAnimation()
		{
			base.CloseWithScaleAnimation();
		}
	}
}

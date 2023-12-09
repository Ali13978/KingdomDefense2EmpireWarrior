namespace LifetimePopup
{
	public class FreeResourcesButtonController : ButtonController
	{
		public override void OnClick()
		{
			base.OnClick();
			OpenFacebookServicesPopup();
		}

		private void OpenFacebookServicesPopup()
		{
			SingletonMonoBehaviour<LifetimeCanvas>.Instance.FreeResourcesPopupController.Init();
		}
	}
}

namespace MainMenu
{
	public class PolicyButtonController : ButtonController
	{
		public override void OnClick()
		{
			base.OnClick();
			SingletonMonoBehaviour<UIRootController>.Instance.policyPopupController.Init();
		}
	}
}

namespace MainMenu
{
	public class CreditButtonController : ButtonController
	{
		public override void OnClick()
		{
			base.OnClick();
			SingletonMonoBehaviour<UIRootController>.Instance.creditPopupController.Init();
		}
	}
}

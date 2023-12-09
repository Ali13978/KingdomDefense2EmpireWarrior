namespace Gameplay
{
	public class UltimateInforButtonController : ButtonController
	{
		public override void OnClick()
		{
			base.OnClick();
			SingletonMonoBehaviour<UIRootController>.Instance.UpgradeTowerPopupController.ultimateInforGroup.TogglePopup();
		}
	}
}

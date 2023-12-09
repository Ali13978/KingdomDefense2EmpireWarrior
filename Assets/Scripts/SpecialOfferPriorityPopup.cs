public class SpecialOfferPriorityPopup : GameplayPriorityPopupController
{
	public SaleBundleItem saleBundleItem;

	public override void InitPriority(PopupPriorityEnum priority)
	{
		base.InitPriority(priority);
		saleBundleItem.Init();
		saleBundleItem.RefreshStatus();
	}
}

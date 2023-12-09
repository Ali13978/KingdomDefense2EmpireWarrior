public class DailyBoosterOfferPriorityPopup : SpecialOfferPriorityPopup
{
	private int subscribeId = -1;

	public override void InitPriority(PopupPriorityEnum priority)
	{
		base.InitPriority(priority);
		subscribeId = GameTools.GetUniqueId();
		GameEventCenter.Instance.Subscribe(GameEventType.OnCompletePurchase, new SimpleSubscriberData(GameTools.GetUniqueId(), OnSomethingWasPurchased));
	}

	private void OnSomethingWasPurchased()
	{
		if (GameTools.IsSubscriptionActive(SubscriptionTypeEnum.dailyBooster))
		{
			CloseWithScaleAnimation();
		}
	}

	public override void Close()
	{
		base.Close();
		if (subscribeId > 0)
		{
			GameEventCenter.Instance.Unsubscribe(subscribeId, GameEventType.OnCompletePurchase);
		}
		GameEventCenter.Instance.Unsubscribe((saleBundleItem as DailyBoosterStoreItem).subscribeId, GameEventType.OnCompletePurchase);
	}
}

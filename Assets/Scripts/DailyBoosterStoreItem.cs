using Services.PlatformSpecific;
using UnityEngine;
using UnityEngine.UI;

public class DailyBoosterStoreItem : SaleBundleItem
{
	public Button purchaseButton;

	public int subscribeId = -1;

	public override void Init()
	{
		base.Init();
		bool flag = GameTools.IsSubscriptionActive(SubscriptionTypeEnum.dailyBooster);
		int subcribedur = bundleParam.Subcribedur;
		SubscriptionTypeEnum subId = SubscriptionTypeEnum.dailyBooster;
		purchaseButton.interactable = !flag;
		string empty = string.Empty;
		if (!flag)
		{
			empty = string.Format(GameTools.GetLocalization("DAYS"), subcribedur);
		}
		else
		{
			int days = (GameTools.GetEndSubscriptionTime(subId) - GameTools.GetNow()).Days;
			empty = string.Format(GameTools.GetLocalization("DAYS_LEFT"), days);
			currentPrice.text = "SUBSCRIBED";
		}
		title.text = $"{GameTools.GetLocalization(subId.ToString())} - {empty}";
		if (subscribeId < 0)
		{
			subscribeId = GameTools.GetUniqueId();
		}
		GameEventCenter.Instance.Unsubscribe(subscribeId, GameEventType.OnCompletePurchase);
		if (!flag)
		{
			GameEventCenter.Instance.Subscribe(GameEventType.OnCompletePurchase, new SimpleSubscriberData(GameTools.GetUniqueId(), ((SaleBundleItem)this).Init));
		}
	}

	public override void SetListItem()
	{
		int[] itemids = bundleParam.Itemids;
		int[] itemquatities = bundleParam.Itemquatities;
		int subcribedur = bundleParam.Subcribedur;
		RewardItem[] array = new RewardItem[itemids.Length];
		for (int i = 0; i < itemids.Length; i++)
		{
			array[i] = new RewardItem(RewardType.Item, itemids[i], itemquatities[i] / subcribedur, isDisplayQuantity: true);
		}
		itemGroup.InitListItems(array);
	}

	public override void ProcessPurchase()
	{
		PlatformSpecificServicesProvider.Services.InApPurchase.PurchaseSubscription(productID, SubscriptionTypeEnum.dailyBooster);
		UnityEngine.Debug.Log("purchase daily booster");
	}
}

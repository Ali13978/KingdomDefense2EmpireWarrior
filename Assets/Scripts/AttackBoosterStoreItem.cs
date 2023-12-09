using LifetimePopup;
using Services.PlatformSpecific;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AttackBoosterStoreItem : SaleBundleItem
{
	public SubscriptionTypeEnum subTypeEnum;

	public TextMeshProUGUI boosterDesc;

	public Button purchaseButton;

	private int subscribeId;

	public override void Init()
	{
		base.Init();
		int subcribedur = bundleParam.Subcribedur;
		int num = bundleParam.Itemquatities[0];
		bool flag = GameTools.IsSubscriptionActive(subTypeEnum);
		string empty = string.Empty;
		if (!flag)
		{
			empty = string.Format(GameTools.GetLocalization("DAYS"), subcribedur);
		}
		else
		{
			int days = (GameTools.GetEndSubscriptionTime(subTypeEnum) - GameTools.GetNow()).Days;
			empty = ((days <= 0) ? $"{Mathf.RoundToInt((float)(GameTools.GetEndSubscriptionTime(subTypeEnum) - GameTools.GetNow()).TotalHours)}H left" : string.Format(GameTools.GetLocalization("DAYS_LEFT"), days));
			currentPrice.text = "SUBSCRIBED";
		}
		title.text = $"{GameTools.GetLocalization(subTypeEnum.ToString())} - {empty}";
		boosterDesc.text = string.Format(GameTools.GetLocalization("ATTACK_BOOSTER_DESC"), subcribedur, num);
		purchaseButton.interactable = !flag;
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

	public override void ProcessPurchase()
	{
		if (GameTools.IsSubscriptionActive(SubscriptionTypeEnum.doubleAttack) || GameTools.IsSubscriptionActive(SubscriptionTypeEnum.fiftyPercentAtkBoost))
		{
			SingletonMonoBehaviour<LifetimeCanvas>.Instance.NotifyPopupController.Init(GameTools.GetLocalization("TWO_BOOSTER_WARNING"), "OK", null);
		}
		else
		{
			PlatformSpecificServicesProvider.Services.InApPurchase.PurchaseSubscription(productID, subTypeEnum);
		}
	}
}

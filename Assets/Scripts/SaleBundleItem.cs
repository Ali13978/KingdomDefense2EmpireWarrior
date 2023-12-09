using Data;
using LifetimePopup;
using MyCustom;
using RewardPopup;
using Services.PlatformSpecific;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SaleBundleItem : ButtonController
{
	public UnityEvent onShow;

	public UnityEvent onHide;

	[Space]
	public string productID;

	public string titleID;

	[Space]
	public Text title;

	public Text currentPrice;

	[SerializeField]
	private Text saleOffRate;

	[HideInInspector]
	public SaleBundleConfigData bundleParam;

	[Space]
	[Header("Show up item")]
	public GeneralItemGroupController itemGroup;

	[SerializeField]
	private Image heroAvatar;

	[SerializeField]
	private Image heroName;

	[SerializeField]
	private Text heroLevel;

	[Space]
	[Header("Time Limited Sale")]
	[SerializeField]
	private Text timeCountdown;

	private TimeSpan currentTimeCount;

	private void Update()
	{
		if (base.gameObject.activeSelf && bundleParam != null && bundleParam.Bundletype.Equals(StoreBundleType.TimeLimited.ToString()) && (bool)timeCountdown && currentTimeCount.TotalSeconds > 0.0)
		{
			UpdateTimeCountdown();
		}
	}

	private void UpdateTimeCountdown()
	{
		currentTimeCount = currentTimeCount.Subtract(TimeSpan.FromSeconds(Time.deltaTime));
		if (currentTimeCount.TotalSeconds < 0.0)
		{
			UnityEngine.Debug.Log("out of time!");
			Hide();
			ReadWriteDataSaleBundle.Instance.SetSpecialPackExpired(productID);
			ReadWriteDataSaleBundle.Instance.SetLastTimePlay();
			SingletonMonoBehaviour<LifetimeCanvas>.Instance.StorePopupController.SaleBundleGroupController.RefreshItemStatus();
		}
		else
		{
			string text = $"{currentTimeCount.Hours + currentTimeCount.Days * 24:D2}:{currentTimeCount.Minutes:D2}:{currentTimeCount.Seconds:D2}";
			timeCountdown.text = text;
		}
	}

	public virtual void Init()
	{
		bundleParam = StoreBundleData.GetDataSaleBundle(productID);
		SetTitle();
		SetListHero();
		SetListItem();
		SetListPrice();
		if (bundleParam.Bundletype.Equals(StoreBundleType.TimeLimited.ToString()))
		{
			currentTimeCount = ReadWriteDataSaleBundle.Instance.getCountdownTime(productID);
			Show();
		}
		else
		{
			Show();
		}
	}

	public void RefreshStatus()
	{
		bundleParam = StoreBundleData.GetDataSaleBundle(productID);
		int[] heroid = bundleParam.Heroid;
		bool flag = false;
		if (heroid.Length > 0)
		{
			for (int i = 0; i < heroid.Length; i++)
			{
				if (ReadWriteDataHero.Instance.IsHeroOwned(heroid[i]))
				{
					flag = true;
					Hide();
				}
			}
		}
		if (flag)
		{
			Hide();
		}
	}

	private void SetTitle()
	{
		if ((bool)title)
		{
			title.text = GameTools.GetLocalization(titleID);
		}
	}

	private void SetListHero()
	{
		int[] heroid = bundleParam.Heroid;
		if (heroid.Length == 1)
		{
			int num = heroid[0];
			if ((bool)heroAvatar)
			{
				heroAvatar.sprite = Resources.Load<Sprite>($"HeroesAvatar/avatar_hero_{num}");
			}
			if ((bool)heroName)
			{
				heroName.sprite = Resources.Load<Sprite>($"HeroesName/name_hero_{num}");
			}
			if ((bool)heroLevel)
			{
				heroLevel.text = (bundleParam.Herolevel + 1).ToString();
			}
		}
	}

	public virtual void SetListItem()
	{
		int[] itemids = bundleParam.Itemids;
		int[] itemquatities = bundleParam.Itemquatities;
		int gembonus = bundleParam.Gembonus;
		if (itemids.Length > 0 && gembonus > 0)
		{
			RewardItem[] array = new RewardItem[itemids.Length + 1];
			RewardItem rewardItem = new RewardItem();
			rewardItem.rewardType = RewardType.Gem;
			rewardItem.value = gembonus;
			rewardItem.isDisplayQuantity = true;
			array[0] = rewardItem;
			for (int i = 0; i < itemids.Length; i++)
			{
				RewardItem rewardItem2 = new RewardItem();
				rewardItem2.rewardType = RewardType.Item;
				rewardItem2.itemID = itemids[i];
				rewardItem2.value = itemquatities[i];
				rewardItem2.isDisplayQuantity = true;
				array[i + 1] = rewardItem2;
			}
			if ((bool)itemGroup)
			{
				itemGroup.InitListItems(array);
			}
		}
		else if ((bool)itemGroup)
		{
			itemGroup.HideAllItems();
		}
	}

	private void SetListPrice()
	{
		int saleoffpercent = bundleParam.Saleoffpercent;
		decimal num = 0m;
		num = ((!StaticMethod.IsInternetConnectionAvailable()) ? ((decimal)bundleParam.Defaultprice) : PlatformSpecificServicesProvider.Services.InApPurchase.GetLocalizedProductPrice(productID));
		string iSOCurrencyCode = PlatformSpecificServicesProvider.Services.InApPurchase.GetISOCurrencyCode(productID);
		int noDecimalFracment = BitConverter.GetBytes(decimal.GetBits(num)[3])[2];
		currentPrice.text = PlatformSpecificServicesProvider.Services.InApPurchase.GetFormatedProductPrice(iSOCurrencyCode, num, noDecimalFracment);
		if (saleOffRate != null)
		{
			saleOffRate.text = "-" + saleoffpercent.ToString() + "%";
		}
	}

	public override void OnClick()
	{
		base.OnClick();
		ProcessPurchase();
	}

	public virtual void ProcessPurchase()
	{
		PlatformSpecificServicesProvider.Services.InApPurchase.PurchaseSaleBundle(productID);
	}

	private void Show()
	{
		if (!base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(value: true);
		}
		onShow.Invoke();
	}

	public void Hide()
	{
		if (base.gameObject.activeSelf)
		{
			base.gameObject.SetActive(value: false);
		}
		onHide.Invoke();
	}
}

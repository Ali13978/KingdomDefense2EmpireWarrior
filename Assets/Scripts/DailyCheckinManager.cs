using Data;
using System;
using Tutorial;
using UnityEngine;

public class DailyCheckinManager : MonoBehaviour
{
	private enum SpecialOfferCode
	{
		Starter,
		Trio,
		LandSky,
		TwoGods
	}

	public static DailyCheckinManager Instance;

	private bool firstTimeOfSession = true;

	public void Awake()
	{
		if (Instance != null)
		{
			UnityEngine.Object.Destroy(this);
			return;
		}
		Instance = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	public void WorldMapCheckIn(WorldMapTutorial worldMapTutorial)
	{
		if (!worldMapTutorial.IsShowingTutorial())
		{
			CheckDailyBooster();
			int mapIDUnlocked = ReadWriteDataMap.Instance.GetMapIDUnlocked();
			if (mapIDUnlocked > 2)
			{
				CheckDailyReward();
				ShowSpecialOfferPopup();
			}
		}
	}

	public void CheckDailyBooster()
	{
		DateTime dateTime = GameTools.GetLastTimeCheckInSubscription(SubscriptionTypeEnum.dailyBooster);
		DateTime endSubscriptionTime = GameTools.GetEndSubscriptionTime(SubscriptionTypeEnum.dailyBooster);
		if (!GameTools.IsSubscriptionActive(SubscriptionTypeEnum.dailyBooster) && ((GameTools.GetNow() - endSubscriptionTime).Days > 14 || (GameTools.GetNow() - dateTime).Days > 14 || dateTime.DayOfYear == endSubscriptionTime.DayOfYear || (dateTime - endSubscriptionTime).Minutes >= 0))
		{
			return;
		}
		UnityEngine.Debug.LogFormat("__Check daily booster: lastCheckIn day {0}, today {1}", dateTime.DayOfYear, GameTools.GetNow().DayOfYear);
		if (dateTime.DayOfYear != GameTools.GetNow().DayOfYear)
		{
			int dayOfYear = GameTools.GetNow().DayOfYear;
			if ((endSubscriptionTime - GameTools.GetNow()).TotalMinutes < 0.0)
			{
				dayOfYear = endSubscriptionTime.DayOfYear;
			}
			int num = 0;
			while (num < 15 && dateTime.DayOfYear != dayOfYear)
			{
				num++;
				UnityEngine.Debug.LogFormat("___Counting daily booster day, lastcheck before {0} and after {1}, target {2}", dateTime.DayOfYear, dateTime.AddDays(1.0).DayOfYear, dayOfYear);
				dateTime = dateTime.AddDays(1.0);
			}
			GameTools.SetLastTimeCheckInSubscription(SubscriptionTypeEnum.dailyBooster, GameTools.GetNow());
			SaleBundleConfigData dataSaleBundle = StoreBundleData.GetDataSaleBundle("kd.sale.bundle.dailybooster");
			int[] itemids = dataSaleBundle.Itemids;
			int[] itemquatities = dataSaleBundle.Itemquatities;
			int subcribedur = dataSaleBundle.Subcribedur;
			int days = (GameTools.GetEndSubscriptionTime(SubscriptionTypeEnum.dailyBooster) - GameTools.GetNow()).Days;
			int num2 = dataSaleBundle.Subcribedur - days;
			string customTitle = string.Format("{0}\n<size=60%>Day {1} ({2})", GameTools.GetLocalization("dailyBooster"), num2, string.Format(GameTools.GetLocalization("DAYS_LEFT"), days));
			RewardItem[] array = new RewardItem[itemids.Length];
			for (int i = 0; i < itemids.Length; i++)
			{
				int num3 = itemquatities[i] / subcribedur * num;
				array[i] = new RewardItem(RewardType.Item, itemids[i], num3, isDisplayQuantity: true);
				ReadWriteDataPowerUpItem.Instance.ChangeItemQuantity(itemids[i], num3);
			}
			RewardPriorityPopupController rewardPriorityPopupController = PriorityPopupManager.Instance.CreatePopup(PriorityPopupManager.Instance.rewardPopupPrefab, PopupPriorityEnum.Normal) as RewardPriorityPopupController;
			rewardPriorityPopupController.SetRewardData(array, customTitle);
		}
	}

	public void CheckDailyReward()
	{
		int currentDay = ReadWriteDataDailyReward.Instance.GetCurrentDay();
		if (!ReadWriteDataDailyReward.Instance.IsReceivedReward(currentDay))
		{
			PriorityPopupManager.Instance.CreatePopup(PriorityPopupManager.Instance.dailyRewardPopupPrefab, PopupPriorityEnum.Normal);
		}
	}

	public void ShowSpecialOfferPopup()
	{
		bool flag = false;
		if (GameTools.GetNow().DayOfYear != PlayerPrefs.GetInt("LastDayShowSpecialOffer"))
		{
			flag = true;
		}
		if (firstTimeOfSession)
		{
			flag = true;
		}
		if (!flag)
		{
			return;
		}
		PlayerPrefs.SetInt("LastDayShowSpecialOffer", GameTools.GetNow().DayOfYear);
		switch (ReadWriteDataSaleBundle.Instance.GetCurrentAvailableSpeciakPackIndex())
		{
		case 0:
			PriorityPopupManager.Instance.CreatePopup(PriorityPopupManager.Instance.offerStarterPopup, PopupPriorityEnum.Normal);
			break;
		case 1:
			PriorityPopupManager.Instance.CreatePopup(PriorityPopupManager.Instance.offerTrioPopup, PopupPriorityEnum.Normal);
			break;
		case 2:
			PriorityPopupManager.Instance.CreatePopup(PriorityPopupManager.Instance.offerLandskyPopup, PopupPriorityEnum.Normal);
			break;
		case 3:
			PriorityPopupManager.Instance.CreatePopup(PriorityPopupManager.Instance.offerTwoGodPopup, PopupPriorityEnum.Normal);
			break;
		default:
			if (!GameTools.IsSubscriptionActive(SubscriptionTypeEnum.dailyBooster) && firstTimeOfSession)
			{
				PriorityPopupManager.Instance.CreatePopup(PriorityPopupManager.Instance.dailyBoosterPopup, PopupPriorityEnum.Normal);
			}
			break;
		}
		firstTimeOfSession = false;
	}
}

using Data;
using MyCustom;
using Services.PlatformSpecific;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestConfigMono : SingletonMonoBehaviour<TestConfigMono>
{
	[Space]
	[Header("CURRENCY")]
	[SerializeField]
	private InputField inputFieldGem;

	[Space]
	[Header("CURRENCY")]
	[SerializeField]
	private InputField inputFieldStar;

	private int gemNumber;

	private int starsNumber;

	[Space]
	[Header("UNLOCK MAP")]
	[SerializeField]
	private Toggle toggleUnlockAllMap;

	private int unlockAllMap;

	[Space]
	[Header("UNLOCK THEME")]
	[SerializeField]
	private Toggle toggleUnlockAllTheme;

	private int unlockAllTheme;

	[Space]
	[Header("USE CUSTOM CSV")]
	[SerializeField]
	private Toggle toggleUseCustomCSV;

	private int useCustomCSV;

	[Space]
	[Header("DEFAULT TOURNAMENT MAP")]
	public InputField inputTourMap;

	private int tourDefaultMapId;

	public Toggle toggleTestSeasonReward;

	public Toggle toggleTestFriendReward;

	public Toggle toggleTestFriendRewardNoFakeUser;

	[Space]
	[Header("PUSH NOTIFY")]
	public InputField inputTitle;

	public InputField inputContent;

	public InputField inputDelaySecond;

	[Space]
	[Header("Unlock Hero")]
	public InputField inputHeroID;

	private const string keyTest = "TEST";

	private const string KeyGemNumber = "TESTgemNumber";

	private const string KeyStarsNumber = "TESTstarsNumber";

	private const string KeyUnlockAllMap = "TESTunlockAllMap";

	private const string KeyUnlockAllTheme = "TESTunlockAllTheme";

	private const string keyUseCustomCSV = "TESTcustomCSV";

	private const string keyDefaultTourMap = "TESTtourDefaultMap";

	public void Init()
	{
		LoadPlayerprefs();
		ApplyValue();
	}

	public void ApplyValue()
	{
		Singleton<TestConfig>.Instance.gemNumber = gemNumber;
		Singleton<TestConfig>.Instance.starsNumber = starsNumber;
		Singleton<TestConfig>.Instance.unlockAllMaps = (unlockAllMap == 1);
		Singleton<TestConfig>.Instance.unlockAllThemes = (unlockAllTheme == 1);
		Singleton<TestConfig>.Instance.useCustomCSV = (useCustomCSV == 1);
		Singleton<TestConfig>.Instance.tourDefaultMapId = tourDefaultMapId;
	}

	private void SaveToPlayerprefs()
	{
		PlayerPrefs.SetInt("TESTgemNumber", gemNumber);
		PlayerPrefs.SetInt("TESTstarsNumber", starsNumber);
		PlayerPrefs.SetInt("TESTunlockAllMap", unlockAllMap);
		PlayerPrefs.SetInt("TESTunlockAllTheme", unlockAllTheme);
		PlayerPrefs.SetInt("TESTcustomCSV", useCustomCSV);
		PlayerPrefs.SetInt("TESTtourDefaultMap", tourDefaultMapId);
	}

	private void LoadValueFromUI()
	{
		gemNumber = int.Parse(inputFieldGem.text);
		starsNumber = int.Parse(inputFieldStar.text);
		unlockAllMap = (toggleUnlockAllMap.isOn ? 1 : 0);
		unlockAllTheme = (toggleUnlockAllTheme.isOn ? 1 : 0);
		useCustomCSV = (toggleUseCustomCSV.isOn ? 1 : 0);
		tourDefaultMapId = int.Parse(inputTourMap.text);
		GameTools.isTestingSeasonReward = toggleTestSeasonReward.isOn;
		GameTools.isTestingFriendReward = toggleTestFriendReward.isOn;
		GameTools.isTestingFriendRewardNoFakeUser = toggleTestFriendRewardNoFakeUser.isOn;
	}

	private void SynUiFromValue()
	{
		inputFieldGem.text = gemNumber.ToString();
		inputFieldStar.text = starsNumber.ToString();
		toggleUnlockAllMap.isOn = (unlockAllMap == 1);
		toggleUnlockAllTheme.isOn = (unlockAllTheme == 1);
		toggleUseCustomCSV.isOn = (useCustomCSV == 1);
		inputTourMap.text = tourDefaultMapId.ToString();
	}

	private void LoadPlayerprefs()
	{
		gemNumber = PlayerPrefs.GetInt("TESTgemNumber", 0);
		starsNumber = PlayerPrefs.GetInt("TESTstarsNumber", 0);
		unlockAllMap = PlayerPrefs.GetInt("TESTunlockAllMap", 0);
		unlockAllTheme = PlayerPrefs.GetInt("TESTunlockAllTheme", 0);
		useCustomCSV = PlayerPrefs.GetInt("TESTcustomCSV", 0);
		tourDefaultMapId = PlayerPrefs.GetInt("TESTtourDefaultMap", 0);
	}

	public void ClearFreeResoucesData()
	{
		ReadWriteDataFreeResources.Instance.ResetFreeResourcesDailyData();
	}

	public void SkipAllTutorial()
	{
		ReadWriteDataTutorial.Instance.SkipAllTutorials();
	}

	public void OnGotoTomorrowBtnClicked()
	{
		for (int i = 0; i < 3; i++)
		{
			SubscriptionTypeEnum subId = (SubscriptionTypeEnum)i;
			if (GameTools.IsSubscriptionActive(subId))
			{
				GameTools.SetEndSubscriptionTime(subId, GameTools.GetEndSubscriptionTime(subId).AddDays(-1.0));
				GameTools.SetLastTimeCheckInSubscription(subId, GameTools.GetLastTimeCheckInSubscription(subId).AddDays(-1.0));
			}
		}
		byte[] listRunningEvent = GameTools.GetListRunningEvent();
		for (int j = 0; j < listRunningEvent.Length; j++)
		{
			GameTools.WriteTimeStamp(GameTools.EVENT_ENDTIME_PREFIX + listRunningEvent[j], GameTools.ReadTimeStamp(GameTools.EVENT_ENDTIME_PREFIX + listRunningEvent[j]).AddDays(-1.0));
		}
		GameTools.SetDayOfYearUpdateEvent(GameTools.GetNow().AddDays(-1.0).DayOfYear);
		EventQuestSystem.Instance.Init();
		NextDayDailyReward();
		UnityEngine.Debug.Log("you are on tomorrow");
	}

	public void OnUnsubcribeAllBtnClicked()
	{
		for (int i = 0; i < 3; i++)
		{
			SubscriptionTypeEnum subId = (SubscriptionTypeEnum)i;
			GameTools.SetEndSubscriptionTime(subId, GameTools.GetNow().AddDays(-1.0));
			GameTools.SetLastTimeCheckInSubscription(subId, GameTools.GetNow().AddDays(-1.0));
		}
	}

	public void OnCompleteAllEventBtnClicked()
	{
		List<RunningEventData> listEvent = new List<RunningEventData>();
		List<EventConfigData> list = new List<EventConfigData>();
		byte[] listRunningEvent = GameTools.GetListRunningEvent();
		List<int> listUnclaimedReward = GameTools.GetListUnclaimedReward();
		for (int i = 0; i < listUnclaimedReward.Count; i++)
		{
			list.Add(CommonData.Instance.eventIdToEventData[listUnclaimedReward[i]]);
		}
		for (int j = 0; j < listRunningEvent.Length; j++)
		{
			list.Add(CommonData.Instance.eventIdToEventData[listRunningEvent[j]]);
		}
		GameTools.SaveListRunningEvent(listEvent);
		GameTools.SaveListUnclaimedReward(list);
		GameTools.SetDayOfYearUpdateEvent(GameTools.GetNow().AddDays(-1.0).DayOfYear);
		EventQuestSystem.Instance.Init();
		UnityEngine.Debug.Log(">>>> complete all events");
	}

	public void OnAlmostCompleteEventBtnclicked()
	{
		List<RunningEventData> list = new List<RunningEventData>();
		byte[] listRunningEvent = GameTools.GetListRunningEvent();
		for (int i = 0; i < listRunningEvent.Length; i++)
		{
			RunningEventData runningEventData = new RunningEventData();
			runningEventData.configData = CommonData.Instance.eventIdToEventData[listRunningEvent[i]];
			GameTools.GetRunningEventProgress(runningEventData);
			runningEventData.curProgress.Value = runningEventData.configData.Targetquantity - 1;
			list.Add(runningEventData);
		}
		GameTools.SaveListRunningEvent(list);
		GameTools.SetDayOfYearUpdateEvent(GameTools.GetNow().AddDays(-1.0).DayOfYear);
		EventQuestSystem.Instance.Init();
		UnityEngine.Debug.Log(">>>>>> almost complete all events");
	}

	public void NextDayDailyReward()
	{
		ReadWriteDataDailyReward.Instance.TryIncreaseDay();
	}

	public void PushNotify()
	{
		PlatformSpecificServicesProvider.Services.GameNotification.PushNotify(inputContent.text, int.Parse(inputDelaySecond.text));
		StaticMethod.ClearInputField(inputContent);
		StaticMethod.ClearInputField(inputDelaySecond);
	}

	public void UnlockHero()
	{
		if (!string.IsNullOrEmpty(inputHeroID.text))
		{
			int num = int.Parse(inputHeroID.text);
			if (!ReadWriteDataHero.Instance.IsHeroOwned(num))
			{
				ReadWriteDataHero.Instance.UnlockHero(num);
				StaticMethod.ClearInputField(inputHeroID);
				UnityEngine.Debug.Log("Test unlock hero ID = " + num);
			}
			else
			{
				UnityEngine.Debug.Log("Hero da duoc unlock!");
			}
		}
		else
		{
			UnityEngine.Debug.Log("Input field is empty!");
		}
	}

	public void Open()
	{
		base.gameObject.SetActive(value: true);
		LoadPlayerprefs();
		SynUiFromValue();
	}

	public void Close()
	{
		LoadValueFromUI();
		ApplyValue();
		SaveToPlayerprefs();
		base.gameObject.SetActive(value: false);
	}
}

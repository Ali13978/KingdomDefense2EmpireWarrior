using ApplicationEntry;
using System;
using System.Collections.Generic;
using UnityEngine;

public class EventQuestSystem : MonoBehaviour
{
	[HideInInspector]
	public List<RunningEventData> runningEventList = new List<RunningEventData>();

	[HideInInspector]
	public List<EventConfigData> unclaimedRewardList = new List<EventConfigData>();

	private int requiredEasyEvent = 2;

	private int requiredHardEvent = 1;

	public static EventQuestSystem Instance
	{
		get;
		set;
	}

	private void Start()
	{
		if ((bool)Instance)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		Instance = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		Init();
	}

	private void OnApplicationFocus(bool focus)
	{
		int dayOfYearUpdateEvent = GameTools.GetDayOfYearUpdateEvent();
		if (GameTools.GetNow().DayOfYear != dayOfYearUpdateEvent)
		{
			Init();
		}
	}

	public void Init()
	{
		runningEventList.Clear();
		unclaimedRewardList.Clear();
		List<int> listUnclaimedReward = GameTools.GetListUnclaimedReward();
		UnityEngine.Debug.Log(">>>>>>> num of unclaim event: " + listUnclaimedReward.Count);
		for (int i = 0; i < listUnclaimedReward.Count; i++)
		{
			unclaimedRewardList.Add(CommonData.Instance.eventIdToEventData[listUnclaimedReward[i]]);
		}
		bool flag = false;
		int j = 0;
		int k = 0;
		byte[] listRunningEvent = GameTools.GetListRunningEvent();
		UnityEngine.Debug.Log(">>>>>>> num of running event: " + listRunningEvent.Length);
		for (int l = 0; l < listRunningEvent.Length; l++)
		{
			RunningEventData runningEventData = new RunningEventData();
			runningEventData.configData = CommonData.Instance.eventIdToEventData[listRunningEvent[l]];
			GameTools.GetRunningEventProgress(runningEventData);
			if ((GameTools.GetNow() - runningEventData.endTime).TotalSeconds > 0.0)
			{
				flag = true;
				continue;
			}
			runningEventList.Add(runningEventData);
			if (runningEventData.configData.EVENTQUESTTYPE == EventQuestType.Easy)
			{
				j++;
			}
			if (runningEventData.configData.EVENTQUESTTYPE == EventQuestType.Hard)
			{
				k++;
			}
		}
		int dayOfYearUpdateEvent = GameTools.GetDayOfYearUpdateEvent();
		if (GameTools.GetNow().DayOfYear != dayOfYearUpdateEvent)
		{
			GameTools.SetDayOfYearUpdateEvent(GameTools.GetNow().DayOfYear);
			List<RunningEventData> holidayEventIdsFromRemoteConfig = GetHolidayEventIdsFromRemoteConfig();
			for (int m = 0; m < holidayEventIdsFromRemoteConfig.Count; m++)
			{
				if (!IsEventRunning(holidayEventIdsFromRemoteConfig[m].configData.Eventid) && (holidayEventIdsFromRemoteConfig[m].endTime - GameTools.GetNow()).TotalSeconds > 0.0)
				{
					runningEventList.Add(holidayEventIdsFromRemoteConfig[m]);
					flag = true;
				}
			}
			for (; j < requiredEasyEvent; j++)
			{
				runningEventList.Add(GetNewRandomEvent(EventQuestType.Easy));
				flag = true;
			}
			for (; k < requiredHardEvent; k++)
			{
				runningEventList.Add(GetNewRandomEvent(EventQuestType.Hard));
				flag = true;
			}
		}
		GameEventCenter.Instance.UnsubscribeEventQuestEvent();
		for (int num = runningEventList.Count - 1; num >= 0; num--)
		{
			runningEventList[num].SubscribeTrigger();
		}
		if (flag)
		{
			SaveEvent();
		}
	}

	public void OnEventComplete(RunningEventData eventData)
	{
		unclaimedRewardList.Add(eventData.configData);
		for (int num = runningEventList.Count - 1; num >= 0; num--)
		{
			if (runningEventList[num] == eventData)
			{
				runningEventList.RemoveAt(num);
				break;
			}
		}
		SaveEvent();
	}

	public void SaveEvent()
	{
		GameTools.SaveListRunningEvent(runningEventList);
		GameTools.SaveListUnclaimedReward(unclaimedRewardList);
	}

	public void ClaimEventReward(int unclaimedIndex)
	{
		if (unclaimedIndex >= unclaimedRewardList.Count)
		{
			UnityEngine.Debug.LogError("try to claim event out of list, error id " + unclaimedIndex);
			return;
		}
		unclaimedRewardList.RemoveAt(unclaimedIndex);
		GameTools.SaveListUnclaimedReward(unclaimedRewardList);
	}

	public bool IsEventRunning(int eventId)
	{
		for (int num = runningEventList.Count - 1; num >= 0; num--)
		{
			if (runningEventList[num].configData.Eventid == eventId)
			{
				return true;
			}
		}
		return false;
	}

	private DateTime GetMoment0(DateTime day)
	{
		day = day.AddHours(-day.Hour);
		day = day.AddMinutes(-day.Minute);
		day = day.AddSeconds(-day.Second);
		return day;
	}

	private RunningEventData GetNewRandomEvent(EventQuestType eventType)
	{
		int count = CommonData.Instance.eventTypeToEventData[eventType].Count;
		int num = UnityEngine.Random.Range(0, count);
		while (IsEventRunning(CommonData.Instance.eventTypeToEventData[eventType][num].Eventid))
		{
			num++;
			if (num >= count)
			{
				num = 0;
			}
		}
		EventConfigData eventConfigData = CommonData.Instance.eventTypeToEventData[eventType][num];
		return new RunningEventData(eventConfigData, GetMoment0(GameTools.GetNow()).AddDays(eventConfigData.Durationinday));
	}

	public List<RunningEventData> GetHolidayEventIdsFromRemoteConfig()
	{
		List<RunningEventData> list = new List<RunningEventData>();
		int holidayEventId = ApplicationEntry.ApplicationEntry.Instance.ReadWriteRemoteSettingData.GetHolidayEventId();
		UnityEngine.Debug.LogFormat(">>>>>>>>>> holiday event Id: {0}, start time {1}", holidayEventId, ApplicationEntry.ApplicationEntry.Instance.ReadWriteRemoteSettingData.GetHolidayStartDay());
		if (holidayEventId > 0 && !IsEventRunning(holidayEventId))
		{
			EventConfigData eventConfigData = CommonData.Instance.eventIdToEventData[holidayEventId];
			DateTime day = GameTools.FromUnixTimeToDateTime(ApplicationEntry.ApplicationEntry.Instance.ReadWriteRemoteSettingData.GetHolidayStartDay());
			DateTime dateTime = GetMoment0(day).AddDays(eventConfigData.Durationinday);
			if ((dateTime - GameTools.GetNow()).TotalMinutes > 0.0)
			{
				list.Add(new RunningEventData(eventConfigData, dateTime));
			}
		}
		return list;
	}
}

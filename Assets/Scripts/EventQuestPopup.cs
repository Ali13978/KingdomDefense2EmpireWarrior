using Gameplay;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventQuestPopup : GameplayPopupController
{
	[Space]
	public EventQuestEntry sampleEntry;

	public Text noEventText;

	public GameObject eventScrollObj;

	public RectTransform scrollContent;

	public RectTransform scrollHandle;

	public float heightOfEventEntry = 90f;

	private List<EventQuestEntry> eventList = new List<EventQuestEntry>();

	private bool isInited;

	public void Init()
	{
		if (!isInited)
		{
			isInited = true;
			eventList.Add(sampleEntry);
			RectTransform rectTransform = scrollHandle;
			Vector2 offsetMin = scrollHandle.offsetMin;
			rectTransform.offsetMin = new Vector2(offsetMin.x, 0f);
			RectTransform rectTransform2 = scrollHandle;
			Vector2 offsetMax = scrollHandle.offsetMax;
			rectTransform2.offsetMax = new Vector2(offsetMax.x, 0f);
		}
		int count = EventQuestSystem.Instance.unclaimedRewardList.Count;
		int count2 = EventQuestSystem.Instance.runningEventList.Count;
		int num = count2 + count;
		for (int i = eventList.Count; i < num; i++)
		{
			EventQuestEntry eventQuestEntry = Object.Instantiate(sampleEntry, sampleEntry.transform.parent);
			eventQuestEntry.transform.localPosition = sampleEntry.transform.localPosition + new Vector3(0f, (float)(-i) * heightOfEventEntry, 0f);
			eventList.Add(eventQuestEntry);
		}
		for (int j = 0; j < eventList.Count; j++)
		{
			eventList[j].gameObject.SetActive(j < num);
		}
		noEventText.gameObject.SetActive(num == 0);
		eventScrollObj.SetActive(num > 0);
		if (num > 0)
		{
			for (int k = 0; k < count; k++)
			{
				eventList[k].Init(EventQuestSystem.Instance.unclaimedRewardList[k], this);
			}
			for (int l = 0; l < count2; l++)
			{
				eventList[count + l].Init(EventQuestSystem.Instance.runningEventList[l], this);
			}
			RectTransform rectTransform3 = scrollContent;
			Vector2 sizeDelta = scrollContent.sizeDelta;
			rectTransform3.sizeDelta = new Vector2(sizeDelta.x, (float)num * heightOfEventEntry + 100f);
		}
		OpenWithScaleAnimation();
	}

	public void UpdateEventList(EventQuestEntry claimedEntry)
	{
		int count = EventQuestSystem.Instance.unclaimedRewardList.Count;
		int num = 0;
		while (true)
		{
			if (num < count)
			{
				if (eventList[num] == claimedEntry)
				{
					break;
				}
				num++;
				continue;
			}
			return;
		}
		EventQuestSystem.Instance.ClaimEventReward(num);
		Init();
	}
}

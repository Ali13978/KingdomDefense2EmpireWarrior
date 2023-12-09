using Data;
using LifetimePopup;
using Services.PlatformSpecific;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventQuestEntry : MonoBehaviour
{
	public Image iconEvent;

	public TextMeshProUGUI textEventTitle;

	public Text textProgress;

	public GameObject progressBar;

	public Text timeRemainText;

	public GameObject inactiveBtn;

	public GameObject claimBtn;

	public TextMeshProUGUI rewardText;

	private float countdownUpdateTime;

	private bool isEventCompleted;

	private EventConfigData curEventData;

	private RunningEventData runningEventData;

	private EventQuestPopup eventPopup;

	public void Init(EventConfigData completedEventData, EventQuestPopup eventPopup)
	{
		isEventCompleted = true;
		curEventData = completedEventData;
		this.eventPopup = eventPopup;
		Init();
	}

	public void Init(RunningEventData runningEventData, EventQuestPopup eventPopup)
	{
		isEventCompleted = false;
		curEventData = runningEventData.configData;
		this.runningEventData = runningEventData;
		this.eventPopup = eventPopup;
		Init();
	}

	private void Init()
	{
		iconEvent.sprite = Resources.Load<Sprite>("EventQuest/icon/EventIcon" + (int)curEventData.EVENTICONTYPE);
		textEventTitle.text = string.Format(GameTools.GetLocalization(curEventData.Eventtitlekey), curEventData.Targetquantity, curEventData.Textextradata);
		UnityEngine.Debug.LogFormat(">>>>>>>> event {0}, complete: {1}", textEventTitle.text, isEventCompleted);
		if (isEventCompleted)
		{
			textProgress.text = string.Format("{0}/{0}", curEventData.Targetquantity);
			progressBar.transform.localScale = Vector3.one;
			timeRemainText.text = string.Empty;
			inactiveBtn.SetActive(value: false);
			claimBtn.SetActive(value: true);
		}
		else
		{
			DateTime d = runningEventData.endTime.AddDays(-runningEventData.configData.Durationinday);
			if ((d - GameTools.GetNow()).TotalMinutes > 0.0)
			{
				textProgress.text = "??/??";
				progressBar.transform.localScale = new Vector3(0f, 1f, 1f);
				countdownUpdateTime = 1E+09f;
				timeRemainText.text = "Upcoming event";
			}
			else
			{
				textProgress.text = $"{runningEventData.curProgress.Value}/{curEventData.Targetquantity}";
				progressBar.transform.localScale = new Vector3((float)runningEventData.curProgress.Value * 1f / (float)curEventData.Targetquantity, 1f, 1f);
				countdownUpdateTime = 0f;
			}
			inactiveBtn.SetActive(value: true);
			claimBtn.SetActive(value: false);
		}
		string arg = string.Empty;
		if (curEventData.REWARDTYPE == RewardType.Gem)
		{
			arg = GameTools.ConvertIconToText(TdSpriteId.Gem);
		}
		else if (curEventData.REWARDTYPE == RewardType.Item)
		{
			arg = GameTools.ConvertIconToText((TdSpriteId)curEventData.Rewardid);
		}
		rewardText.text = string.Format("{0}\n<size=180%>{1} {2}", (!isEventCompleted) ? "REWARD" : "CLAIM", curEventData.Rewardquantity, arg);
	}

	private void Update()
	{
		if (isEventCompleted)
		{
			return;
		}
		if (runningEventData == null)
		{
			isEventCompleted = true;
			UnityEngine.Debug.LogFormat(">>>>>>>>>>>> evetn {0} have null runningData", textEventTitle.text);
			return;
		}
		countdownUpdateTime -= Time.deltaTime;
		if (countdownUpdateTime <= 0f)
		{
			countdownUpdateTime = 1.1f;
			TimeSpan timeSpan = runningEventData.endTime - GameTools.GetNow();
			if (timeSpan.TotalSeconds < 0.0)
			{
				countdownUpdateTime = 1E+08f;
				timeRemainText.text = string.Empty;
			}
			else
			{
				timeRemainText.text = string.Format("{0} {1}d{2}h{3}m{4}s", GameTools.GetLocalization("TIME_LEFT"), timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
			}
		}
	}

	public void OnClaimRewardBtnClicked()
	{
		List<RewardItem> list = new List<RewardItem>();
		if (curEventData.REWARDTYPE == RewardType.Gem)
		{
			list.Add(new RewardItem(RewardType.Gem, curEventData.Rewardquantity, isDisplayQuantity: true));
			ReadWriteDataPlayerCurrency.Instance.ChangeGem(curEventData.Rewardquantity, isDispatchEventChange: true);
		}
		else if (curEventData.REWARDTYPE == RewardType.Item)
		{
			list.Add(new RewardItem(RewardType.Item, curEventData.Rewardid, curEventData.Rewardquantity, isDisplayQuantity: true));
			ReadWriteDataPowerUpItem.Instance.ChangeItemQuantity(curEventData.Rewardquantity, curEventData.Rewardid);
		}
		SingletonMonoBehaviour<LifetimeCanvas>.Instance.RewardPopupController.Init(list.ToArray());
		PlatformSpecificServicesProvider.Services.Analytics.SendEvent_CompleteEvent(curEventData.Eventid);
		eventPopup.UpdateEventList(this);
	}
}

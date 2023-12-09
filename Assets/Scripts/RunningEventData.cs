using System;

public class RunningEventData
{
	public EventConfigData configData;

	public CustomObscuredInt curProgress = new CustomObscuredInt();

	public DateTime endTime;

	private DateTime lastSaveTime;

	private int subscribeId;

	public RunningEventData()
	{
		lastSaveTime = GameTools.GetNow().AddDays(-1.0);
	}

	public RunningEventData(EventConfigData configData, DateTime endTime)
	{
		this.configData = configData;
		this.endTime = endTime;
		lastSaveTime = GameTools.GetNow().AddDays(-1.0);
	}

	public void OnTriggerEvent(EventTriggerData triggerData)
	{
		if (IsValidTrigger(triggerData))
		{
			curProgress.Value += triggerData.addedQuantity;
			if (curProgress.Value >= configData.Targetquantity)
			{
				GameEventCenter.Instance.Unsubscribe(subscribeId);
				EventQuestSystem.Instance.OnEventComplete(this);
			}
			else if (triggerData.forceSaveProgress || (GameTools.GetNow() - lastSaveTime).TotalSeconds > 4.0)
			{
				GameTools.SaveRunningEventProgress(this);
				lastSaveTime = GameTools.GetNow();
			}
		}
	}

	public bool IsValidTrigger(EventTriggerData triggerData)
	{
		if (triggerData.triggerType != configData.EVENTTRIGGERTYPE)
		{
			return false;
		}
		if ((GameTools.GetNow() - endTime.AddDays(-configData.Durationinday)).TotalSeconds < 0.0)
		{
			return false;
		}
		if ((endTime - GameTools.GetNow()).TotalSeconds < 0.0)
		{
			return false;
		}
		switch (configData.COMPAREVALUEMODE)
		{
		case CompareValueMode.None:
			return true;
		case CompareValueMode.OneToOne:
			return triggerData.triggerValue == configData.Triggervalue[0];
		case CompareValueMode.OneAmong:
			for (int num = configData.Triggervalue.Length - 1; num >= 0; num--)
			{
				if (configData.Triggervalue[num] == triggerData.triggerValue)
				{
					return true;
				}
			}
			break;
		}
		return false;
	}

	public void SubscribeTrigger()
	{
		subscribeId = GameTools.GetUniqueId();
		EventTriggerSubscriberData data = new EventTriggerSubscriberData(subscribeId, OnTriggerEvent);
		switch (configData.EVENTTRIGGERTYPE)
		{
		case EventTriggerType.KillMonster:
			GameEventCenter.Instance.Subscribe(GameEventType.EventKillMonster, data);
			break;
		case EventTriggerType.WinCampaign:
		case EventTriggerType.LoseCampaign:
			GameEventCenter.Instance.Subscribe(GameEventType.EventCampaign, data);
			break;
		case EventTriggerType.UseItem:
			GameEventCenter.Instance.Subscribe(GameEventType.EventUseItem, data);
			break;
		case EventTriggerType.UseHeroWinCampaign:
			GameEventCenter.Instance.Subscribe(GameEventType.EventUseHero, data);
			break;
		case EventTriggerType.PlayTournament:
			GameEventCenter.Instance.Subscribe(GameEventType.EventPlayTournament, data);
			break;
		case EventTriggerType.InviteFriend:
			GameEventCenter.Instance.Subscribe(GameEventType.EventInviteFriend, data);
			break;
		case EventTriggerType.EarnGold:
			GameEventCenter.Instance.Subscribe(GameEventType.EventEarnGold, data);
			break;
		case EventTriggerType.UseGem:
			GameEventCenter.Instance.Subscribe(GameEventType.EventUseGem, data);
			break;
		}
	}

	public bool IsTargetReached()
	{
		return curProgress.Value >= configData.Targetquantity;
	}
}

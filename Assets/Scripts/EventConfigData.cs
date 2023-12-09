using System;
using UnityEngine;

[Serializable]
public class EventConfigData
{
	[SerializeField]
	private int eventid;

	[SerializeField]
	private EventIconType eventicontype;

	[SerializeField]
	private int durationinday;

	[SerializeField]
	private EventQuestType eventquesttype;

	[SerializeField]
	private EventTriggerType eventtriggertype;

	[SerializeField]
	private int[] triggervalue = new int[0];

	[SerializeField]
	private CompareValueMode comparevaluemode;

	[SerializeField]
	private int targetquantity;

	[SerializeField]
	private string eventtitlekey;

	[SerializeField]
	private RewardType rewardtype;

	[SerializeField]
	private int rewardid;

	[SerializeField]
	private int rewardquantity;

	[SerializeField]
	private string textextradata;

	public int Eventid
	{
		get
		{
			return eventid;
		}
		set
		{
			eventid = value;
		}
	}

	public EventIconType EVENTICONTYPE
	{
		get
		{
			return eventicontype;
		}
		set
		{
			eventicontype = value;
		}
	}

	public int Durationinday
	{
		get
		{
			return durationinday;
		}
		set
		{
			durationinday = value;
		}
	}

	public EventQuestType EVENTQUESTTYPE
	{
		get
		{
			return eventquesttype;
		}
		set
		{
			eventquesttype = value;
		}
	}

	public EventTriggerType EVENTTRIGGERTYPE
	{
		get
		{
			return eventtriggertype;
		}
		set
		{
			eventtriggertype = value;
		}
	}

	public int[] Triggervalue
	{
		get
		{
			return triggervalue;
		}
		set
		{
			triggervalue = value;
		}
	}

	public CompareValueMode COMPAREVALUEMODE
	{
		get
		{
			return comparevaluemode;
		}
		set
		{
			comparevaluemode = value;
		}
	}

	public int Targetquantity
	{
		get
		{
			return targetquantity;
		}
		set
		{
			targetquantity = value;
		}
	}

	public string Eventtitlekey
	{
		get
		{
			return eventtitlekey;
		}
		set
		{
			eventtitlekey = value;
		}
	}

	public RewardType REWARDTYPE
	{
		get
		{
			return rewardtype;
		}
		set
		{
			rewardtype = value;
		}
	}

	public int Rewardid
	{
		get
		{
			return rewardid;
		}
		set
		{
			rewardid = value;
		}
	}

	public int Rewardquantity
	{
		get
		{
			return rewardquantity;
		}
		set
		{
			rewardquantity = value;
		}
	}

	public string Textextradata
	{
		get
		{
			return textextradata;
		}
		set
		{
			textextradata = value;
		}
	}
}

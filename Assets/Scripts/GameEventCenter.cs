using Gameplay;
using System;
using System.Collections.Generic;
using UnityEngine;

public class GameEventCenter
{
	public delegate void SimpleSubscribeMethod();

	public delegate void EnemySubscribeMethod(EnemyModel enemy);

	public delegate void SelectCharacterMethod(int characterId);

	public delegate void AllySubscribeMethod(AllyModel ally);

	public delegate void ClickButtonMethod(ClickedObjectData clickedObjData);

	public delegate void DamageInfoMethod(CommonAttackDamage damageInfo);

	public delegate void EventTriggerMethod(EventTriggerData data);

	private Dictionary<int, Type> eventTypeToSubcriberType = new Dictionary<int, Type>
	{
		{
			0,
			typeof(SimpleSubscriberData)
		},
		{
			1,
			typeof(EnemySubscriberData)
		},
		{
			2,
			typeof(SelectCharacterSubscriberData)
		},
		{
			3,
			typeof(AllySubscriberData)
		},
		{
			4,
			typeof(ClickButtonSubscriberData)
		},
		{
			5,
			typeof(DamageInfoSubscriberData)
		},
		{
			6,
			typeof(EventTriggerSubscriberData)
		}
	};

	private List<GameEventType> gameEventList = new List<GameEventType>();

	private Dictionary<GameEventType, List<ISubscriberData>> subscriptions = new Dictionary<GameEventType, List<ISubscriberData>>();

	private static GameEventCenter _instance;

	private List<GameEventType> ingameEvents = new List<GameEventType>
	{
		GameEventType.OnTournamentMapRuleReceived,
		GameEventType.OnTournamentPriceConstantsReceived,
		GameEventType.OnEnemyMoveToEndPoint,
		GameEventType.OnSelectAlly,
		GameEventType.OnSelectEnemy,
		GameEventType.OnSelectHero,
		GameEventType.OnSelectPet,
		GameEventType.OnClickButton,
		GameEventType.OnAfterCalculateMagicDamage,
		GameEventType.OnAfterCalculatePhysicsDamage,
		GameEventType.OnBeforeCalculatePhysicsDamage,
		GameEventType.OnCompletePurchase
	};

	private List<GameEventType> eventTriggerEvents = new List<GameEventType>
	{
		GameEventType.EventKillMonster,
		GameEventType.EventCampaign,
		GameEventType.EventUseItem,
		GameEventType.EventUseHero,
		GameEventType.EventPlayTournament,
		GameEventType.EventInviteFriend,
		GameEventType.EventEarnGold,
		GameEventType.EventUseGem
	};

	public static GameEventCenter Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new GameEventCenter();
				_instance.Initialization();
			}
			return _instance;
		}
	}

	public void Initialization()
	{
		string[] names = Enum.GetNames(typeof(GameEventType));
		for (int i = 0; i < names.Length; i++)
		{
			GameEventType gameEventType = (GameEventType)Enum.Parse(typeof(GameEventType), names[i]);
			gameEventList.Add(gameEventType);
			subscriptions.Add(gameEventType, new List<ISubscriberData>());
		}
	}

	public void Subscribe(GameEventType gameEvent, ISubscriberData data)
	{
		if (data.GetType() == eventTypeToSubcriberType[(int)gameEvent / 1000])
		{
			subscriptions[gameEvent].Add(data);
		}
	}

	public void UnsubscribeAll()
	{
		UnityEngine.Debug.Log("__Unsubscribe all events");
		for (int i = 0; i < gameEventList.Count; i++)
		{
			subscriptions[gameEventList[i]].Clear();
		}
	}

	public void UnsubscribeIngameEvent()
	{
		UnityEngine.Debug.Log("___ unsubscribe all ingame events");
		for (int i = 0; i < ingameEvents.Count; i++)
		{
			subscriptions[ingameEvents[i]].Clear();
		}
	}

	public void UnsubscribeEventQuestEvent()
	{
		for (int i = 0; i < eventTriggerEvents.Count; i++)
		{
			subscriptions[eventTriggerEvents[i]].Clear();
		}
	}

	public void Unsubscribe(int subscriberId, GameEventType gameEvent)
	{
		List<ISubscriberData> list = subscriptions[gameEvent];
		for (int num = list.Count - 1; num >= 0; num--)
		{
			if (list[num].subscriberId == subscriberId)
			{
				list.RemoveAt(num);
			}
		}
	}

	public void Unsubscribe(int subscriberId)
	{
		for (int num = gameEventList.Count - 1; num >= 0; num--)
		{
			Unsubscribe(subscriberId, gameEventList[num]);
		}
	}

	public void Unsubscribe(GameEventType gameEvent)
	{
		subscriptions[gameEvent].Clear();
	}

	public void Trigger(GameEventType gameEvent, object gameEventData)
	{
		List<ISubscriberData> list = subscriptions[gameEvent];
		for (int num = list.Count - 1; num >= 0; num--)
		{
			list[num].OnEventTrigger(gameEventData);
		}
	}
}

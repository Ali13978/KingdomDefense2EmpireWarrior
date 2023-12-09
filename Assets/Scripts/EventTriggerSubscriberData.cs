public class EventTriggerSubscriberData : ISubscriberData
{
	private GameEventCenter.EventTriggerMethod method;

	public EventTriggerSubscriberData(int subscriberId, GameEventCenter.EventTriggerMethod method)
	{
		base.subscriberId = subscriberId;
		this.method = method;
	}

	public override void OnEventTrigger(object gameEventData)
	{
		method((EventTriggerData)gameEventData);
	}
}

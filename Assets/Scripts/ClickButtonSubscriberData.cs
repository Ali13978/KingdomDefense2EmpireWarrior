public class ClickButtonSubscriberData : ISubscriberData
{
	private GameEventCenter.ClickButtonMethod method;

	public ClickButtonSubscriberData(int subscriberId, GameEventCenter.ClickButtonMethod method)
	{
		base.subscriberId = subscriberId;
		this.method = method;
	}

	public override void OnEventTrigger(object gameEventData)
	{
		method((ClickedObjectData)gameEventData);
	}
}

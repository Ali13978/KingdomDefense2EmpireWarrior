public class SimpleSubscriberData : ISubscriberData
{
	private GameEventCenter.SimpleSubscribeMethod method;

	public SimpleSubscriberData(int subscriberId, GameEventCenter.SimpleSubscribeMethod method)
	{
		base.subscriberId = subscriberId;
		this.method = method;
	}

	public override void OnEventTrigger(object gameEventData)
	{
		method();
	}
}

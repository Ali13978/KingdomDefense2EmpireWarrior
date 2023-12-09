public abstract class ISubscriberData
{
	public int subscriberId;

	public abstract void OnEventTrigger(object gameEventData);
}

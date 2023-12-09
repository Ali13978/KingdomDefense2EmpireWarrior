public class SelectCharacterSubscriberData : ISubscriberData
{
	private GameEventCenter.SelectCharacterMethod method;

	public SelectCharacterSubscriberData(int subscriberId, GameEventCenter.SelectCharacterMethod method)
	{
		base.subscriberId = subscriberId;
		this.method = method;
	}

	public override void OnEventTrigger(object gameEventData)
	{
		method((int)gameEventData);
	}
}

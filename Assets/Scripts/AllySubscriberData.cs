using Gameplay;

public class AllySubscriberData : ISubscriberData
{
	private GameEventCenter.AllySubscribeMethod method;

	public AllySubscriberData(int subscriberId, GameEventCenter.AllySubscribeMethod method)
	{
		base.subscriberId = subscriberId;
		this.method = method;
	}

	public override void OnEventTrigger(object gameEventData)
	{
		method((AllyModel)gameEventData);
	}
}

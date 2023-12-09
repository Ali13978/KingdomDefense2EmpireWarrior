using Gameplay;

public class EnemySubscriberData : ISubscriberData
{
	private GameEventCenter.EnemySubscribeMethod method;

	public EnemySubscriberData(int subscriberId, GameEventCenter.EnemySubscribeMethod method)
	{
		base.subscriberId = subscriberId;
		this.method = method;
	}

	public override void OnEventTrigger(object gameEventData)
	{
		method((EnemyModel)gameEventData);
	}
}

public class DamageInfoSubscriberData : ISubscriberData
{
	private GameEventCenter.DamageInfoMethod method;

	public DamageInfoSubscriberData(int subscriberId, GameEventCenter.DamageInfoMethod method)
	{
		base.subscriberId = subscriberId;
		this.method = method;
	}

	public override void OnEventTrigger(object gameEventData)
	{
		method((CommonAttackDamage)gameEventData);
	}
}

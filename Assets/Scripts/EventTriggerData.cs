public class EventTriggerData
{
	public EventTriggerType triggerType;

	public int triggerValue;

	public int addedQuantity;

	public bool forceSaveProgress;

	public EventTriggerData(EventTriggerType triggerType, int addedQuantity, bool forceSaveProgress = false)
	{
		this.triggerType = triggerType;
		this.addedQuantity = addedQuantity;
		this.forceSaveProgress = forceSaveProgress;
	}

	public EventTriggerData(EventTriggerType triggerType, int triggerValue, int addedQuantity, bool forceSaveProgress = false)
		: this(triggerType, addedQuantity, forceSaveProgress)
	{
		this.triggerValue = triggerValue;
	}
}

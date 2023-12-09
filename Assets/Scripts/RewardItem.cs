public class RewardItem
{
	public RewardType rewardType;

	public int itemID;

	public int value;

	public bool isDisplayQuantity;

	public RewardItem()
	{
	}

	public RewardItem(RewardType rewardType, int quantity, bool isDisplayQuantity)
	{
		this.rewardType = rewardType;
		value = quantity;
		this.isDisplayQuantity = isDisplayQuantity;
	}

	public RewardItem(RewardType rewardType, int itemId, int quantity, bool isDisplayQuantity)
		: this(rewardType, quantity, isDisplayQuantity)
	{
		itemID = itemId;
	}
}

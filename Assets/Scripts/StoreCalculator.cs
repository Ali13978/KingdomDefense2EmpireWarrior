using Data;
using Parameter;

public static class StoreCalculator
{
	public static bool IsEnoughMoneyToBuy(int itemID)
	{
		bool flag = false;
		int currentGem = ReadWriteDataPlayerCurrency.Instance.GetCurrentGem();
		int price = Singleton<PowerUpItemParameter>.Instance.GetPrice(itemID);
		return currentGem >= price;
	}
}

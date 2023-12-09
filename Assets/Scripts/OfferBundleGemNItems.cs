using System;

[Serializable]
public class OfferBundleGemNItems
{
	public int saleRate;

	public OfferType offerType;

	public int timeCountDownHours;

	public string offerBundleID;

	public int gemAmount;

	public int[] itemsAmount;
}

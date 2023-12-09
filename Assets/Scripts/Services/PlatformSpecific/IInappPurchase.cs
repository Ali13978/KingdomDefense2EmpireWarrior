namespace Services.PlatformSpecific
{
	public interface IInappPurchase
	{
		void PurchaseGem(string productID);

		void PurchaseOfferBundle(string productID);

		void PurchaseHero(string productID);

		void PurchaseSaleBundle(string productID);

		void PurchaseHeroMaxLevel(string productID, string heroItemID);

		void PurchaseHeroPet(string productID, string heroItemID);

		void PurchaseSubscription(string subscritionId, SubscriptionTypeEnum subType);

		string GetLocalizedProductTitle(string productID);

		string GetLocalizedProductDescription(string productID);

		decimal GetLocalizedProductPrice(string productID);

		string GetLocalizedProductPriceString(string productID);

		string GetISOCurrencyCode(string productID);

		string GetFormatedProductPrice(string ISOCurrencyCode, decimal amount, int noDecimalFracment);
	}
}

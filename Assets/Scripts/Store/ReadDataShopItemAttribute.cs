using UnityEngine;

namespace Store
{
	public class ReadDataShopItemAttribute : MonoBehaviour
	{
		[SerializeField]
		private ShopItemAttribute shopItemAttribute;

		public int GetGemPackValue(string productID)
		{
			int result = -1;
			foreach (GemPack item in shopItemAttribute.listGemPack)
			{
				if (item.gemPackID.Equals(productID))
				{
					result = item.gemPackValue;
				}
			}
			return result;
		}

		public string GetGemPackTitle(string productID)
		{
			return "Test Title";
		}

		public string GetGemPackDescription(string productID)
		{
			return "Test Description";
		}

		public string GetGemPackPriceString(string productID)
		{
			return "$0.01";
		}

		public string GetHeroItemID(int heroID)
		{
			string result = string.Empty;
			foreach (HeroItem item in shopItemAttribute.listHeroItem)
			{
				if (item.heroID == heroID)
				{
					result = item.heroItemID;
				}
			}
			return result;
		}

		public int GetHeroID(string productID)
		{
			int result = -1;
			foreach (HeroItem item in shopItemAttribute.listHeroItem)
			{
				if (item.heroItemID.Equals(productID))
				{
					result = item.heroID;
				}
			}
			return result;
		}
	}
}

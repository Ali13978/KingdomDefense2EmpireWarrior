using UnityEngine;

namespace OfferPopup
{
	public class ReadDataOfferBundle : MonoBehaviour
	{
		[SerializeField]
		private OfferBundle offerBundle;

		public OfferBundleSingleHero GetOfferBundleSingleHero(string bundleID)
		{
			OfferBundleSingleHero result = new OfferBundleSingleHero();
			foreach (OfferBundleSingleHero item in offerBundle.bundlesSingleHero)
			{
				if (item.offerBundleID.Equals(bundleID))
				{
					result = item;
				}
			}
			return result;
		}

		public OfferBundleGemNItems GetOfferBundleItems(string bundleID)
		{
			OfferBundleGemNItems result = new OfferBundleGemNItems();
			foreach (OfferBundleGemNItems bundlesItem in offerBundle.bundlesItems)
			{
				if (bundlesItem.offerBundleID.Equals(bundleID))
				{
					result = bundlesItem;
				}
			}
			return result;
		}

		public OfferComboHeroes GetOfferBundleComboHeroes(string bundleID)
		{
			OfferComboHeroes result = new OfferComboHeroes();
			foreach (OfferComboHeroes bundlesComboHero in offerBundle.bundlesComboHeroes)
			{
				if (bundlesComboHero.offerBundleID.Equals(bundleID))
				{
					result = bundlesComboHero;
				}
			}
			return result;
		}
	}
}

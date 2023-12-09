using System.Collections.Generic;
using UnityEngine;

public class OfferBundle : ScriptableObject
{
	[Space]
	public List<OfferBundleSingleHero> bundlesSingleHero = new List<OfferBundleSingleHero>();

	[Space]
	public List<OfferBundleGemNItems> bundlesItems = new List<OfferBundleGemNItems>();

	[Space]
	public List<OfferComboHeroes> bundlesComboHeroes = new List<OfferComboHeroes>();
}

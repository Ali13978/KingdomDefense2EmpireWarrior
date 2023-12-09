using System.Collections.Generic;
using UnityEngine;

public class ShopItemAttribute : ScriptableObject
{
	[Header("GemPack Attribute")]
	public List<GemPack> listGemPack = new List<GemPack>();

	[Header("Hero Attribute")]
	public List<HeroItem> listHeroItem = new List<HeroItem>();
}

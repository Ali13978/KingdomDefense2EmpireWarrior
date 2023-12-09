using UnityEngine;

namespace GiftcodeSystem
{
	public static class GiftCodeStaticVariable
	{
		public const string PREFIX_HERO = "HERO";

		public const string PREFIX_GEMS = "GEMS";

		public const string PREFIX_HERONGEM = "HERONGEM";

		public static string GetGiftCodeType(string json)
		{
			return JsonUtility.FromJson<GiftCode>(json).type;
		}

		public static GiftCodeGems GetGiftCodeGems(string json)
		{
			return JsonUtility.FromJson<GiftCodeGems>(json);
		}

		public static GiftCodeHero GetGiftCodeHero(string json)
		{
			return JsonUtility.FromJson<GiftCodeHero>(json);
		}

		public static GiftCodeHeroNGem GetGiftCodeHeroNGem(string json)
		{
			return JsonUtility.FromJson<GiftCodeHeroNGem>(json);
		}
	}
}

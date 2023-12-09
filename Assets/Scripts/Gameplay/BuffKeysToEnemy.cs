namespace Gameplay
{
	public static class BuffKeysToEnemy
	{
		private static string[] allKeys = new string[6]
		{
			"None",
			"Slow",
			"Burning",
			"Stun",
			"Bleed",
			"ReduceMagicArmor"
		};

		public const string NONE = "None";

		public const string SLOW = "Slow";

		public const string BURNING = "Burning";

		public const string STUN = "Stun";

		public const string BLEED = "Bleed";

		public const string REDUCE_MAGIC_ARMOR = "ReduceMagicArmor";

		public const string DEF_DOWN = "DefDown";

		public const string DECREASE_ATTACK_BY_PERCENTAGE = "DecreaseAttackByPercentage";

		public static string[] AllKeys
		{
			get
			{
				return allKeys;
			}
			set
			{
				allKeys = value;
			}
		}
	}
}

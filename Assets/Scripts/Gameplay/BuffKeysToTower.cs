namespace Gameplay
{
	public static class BuffKeysToTower
	{
		private static string[] allKeys = new string[7]
		{
			"Silent",
			"DamageDecrementCommon",
			"DamageIncrementCommon",
			"AttackRangeIncrementCommon",
			"HellfireCooldownTimeDecrementCommon",
			"InstantKillRateIncrementCommon",
			"IncreaseAttackSpeedByPercentage"
		};

		public const string Silent = "Silent";

		public const string DamageDecrementCommon = "DamageDecrementCommon";

		public const string DamageIncrementCommon = "DamageIncrementCommon";

		public const string AttackRangeIncrementCommon = "AttackRangeIncrementCommon";

		public const string HellfireCooldownTimeDecrementCommon = "HellfireCooldownTimeDecrementCommon";

		public const string InstantKillRateIncrementCommon = "InstantKillRateIncrementCommon";

		public const string INCREASE_ATTACK_SPEED_BY_PERCENTAGE = "IncreaseAttackSpeedByPercentage";

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

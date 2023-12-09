using MyCustom;
using Parameter;

namespace Data
{
	public class UnitAbilitiesRanking : CustomMonoBehaviour
	{
		public AbilitiesRank abilitiesRank;

		private static UnitAbilitiesRanking instance;

		public static UnitAbilitiesRanking Instance => instance;

		private void Awake()
		{
			instance = this;
		}

		public string GetArmorDescriptionByValue(int armor)
		{
			string result = string.Empty;
			if (IsValueBelowMin(armor, abilitiesRank.ArmorValue[0]))
			{
				result = Singleton<NotificationDescription>.Instance.GetNotiContent(abilitiesRank.ArmorDescription[0]);
			}
			else if (IsValueInRange(armor, abilitiesRank.ArmorValue[0], abilitiesRank.ArmorValue[1]))
			{
				result = Singleton<NotificationDescription>.Instance.GetNotiContent(abilitiesRank.ArmorDescription[1]);
			}
			else if (IsValueInRange(armor, abilitiesRank.ArmorValue[1], abilitiesRank.ArmorValue[2]))
			{
				result = Singleton<NotificationDescription>.Instance.GetNotiContent(abilitiesRank.ArmorDescription[2]);
			}
			else if (IsValueAboveMax(armor, abilitiesRank.ArmorValue[2]))
			{
				result = Singleton<NotificationDescription>.Instance.GetNotiContent(abilitiesRank.ArmorDescription[3]);
			}
			if (armor == 0)
			{
				result = Singleton<NotificationDescription>.Instance.GetNotiContent(abilitiesRank.ArmorDescription[4]);
			}
			return result;
		}

		public string GetAttackSpeedDescriptionByValue(int reloadTime)
		{
			string result = string.Empty;
			if (IsValueAboveMax(reloadTime, abilitiesRank.AttackSpeedValue[0]))
			{
				result = Singleton<NotificationDescription>.Instance.GetNotiContent(abilitiesRank.AttackSpeedDescription[0]);
			}
			else if (IsValueInRange(reloadTime, abilitiesRank.AttackSpeedValue[1], abilitiesRank.AttackSpeedValue[0]))
			{
				result = Singleton<NotificationDescription>.Instance.GetNotiContent(abilitiesRank.AttackSpeedDescription[1]);
			}
			else if (IsValueInRange(reloadTime, abilitiesRank.AttackSpeedValue[2], abilitiesRank.AttackSpeedValue[1]))
			{
				result = Singleton<NotificationDescription>.Instance.GetNotiContent(abilitiesRank.AttackSpeedDescription[2]);
			}
			else if (IsValueBelowMin(reloadTime, abilitiesRank.AttackSpeedValue[2]))
			{
				result = Singleton<NotificationDescription>.Instance.GetNotiContent(abilitiesRank.AttackSpeedDescription[3]);
			}
			return result;
		}

		public string GetAttackRangeDescriptionByValue(int attackRange)
		{
			string result = string.Empty;
			if (IsValueBelowMin(attackRange, abilitiesRank.AttackRangeValue[0]))
			{
				result = Singleton<NotificationDescription>.Instance.GetNotiContent(abilitiesRank.AttackRangeDescription[0]);
			}
			else if (IsValueInRange(attackRange, abilitiesRank.AttackRangeValue[0], abilitiesRank.AttackRangeValue[1]))
			{
				result = Singleton<NotificationDescription>.Instance.GetNotiContent(abilitiesRank.AttackRangeDescription[1]);
			}
			else if (IsValueInRange(attackRange, abilitiesRank.AttackRangeValue[1], abilitiesRank.AttackRangeValue[2]))
			{
				result = Singleton<NotificationDescription>.Instance.GetNotiContent(abilitiesRank.AttackRangeDescription[2]);
			}
			else if (IsValueAboveMax(attackRange, abilitiesRank.AttackRangeValue[2]))
			{
				result = Singleton<NotificationDescription>.Instance.GetNotiContent(abilitiesRank.AttackRangeDescription[3]);
			}
			return result;
		}

		public string GetMoveSpeedDescriptionByValue(int moveSpeed)
		{
			string result = string.Empty;
			if (IsValueBelowMin(moveSpeed, abilitiesRank.MSpeedValue[0]))
			{
				result = Singleton<NotificationDescription>.Instance.GetNotiContent(abilitiesRank.MSpeedDescription[0]);
			}
			else if (IsValueInRange(moveSpeed, abilitiesRank.MSpeedValue[0], abilitiesRank.MSpeedValue[1]))
			{
				result = Singleton<NotificationDescription>.Instance.GetNotiContent(abilitiesRank.MSpeedDescription[1]);
			}
			else if (IsValueInRange(moveSpeed, abilitiesRank.MSpeedValue[1], abilitiesRank.MSpeedValue[2]))
			{
				result = Singleton<NotificationDescription>.Instance.GetNotiContent(abilitiesRank.MSpeedDescription[2]);
			}
			else if (IsValueAboveMax(moveSpeed, abilitiesRank.MSpeedValue[2]))
			{
				result = Singleton<NotificationDescription>.Instance.GetNotiContent(abilitiesRank.MSpeedDescription[3]);
			}
			return result;
		}

		private bool IsValueInRange(int value, int minValue, int maxValue)
		{
			return value >= minValue && value <= maxValue;
		}

		private bool IsValueAboveMax(int value, int maxValue)
		{
			return value > maxValue;
		}

		private bool IsValueBelowMin(int value, int minValue)
		{
			return value < minValue;
		}
	}
}

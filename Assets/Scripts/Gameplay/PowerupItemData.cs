using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class PowerupItemData : MonoBehaviour
	{
		private int totalPowerupItemUsed;

		private int powerupItemLimitUse;

		public int TotalPowerupItemUsed
		{
			get
			{
				return totalPowerupItemUsed;
			}
			private set
			{
				totalPowerupItemUsed = value;
			}
		}

		public void InitValue()
		{
			totalPowerupItemUsed = 0;
		}

		public void IncreaseUseAmount()
		{
			totalPowerupItemUsed++;
		}

		public bool IsReachedLimitUse()
		{
			bool result = false;
			int powerupItemLimit = MapRuleParameter.Instance.GetPowerupItemLimit();
			if (totalPowerupItemUsed >= powerupItemLimit)
			{
				result = true;
			}
			return result;
		}
	}
}

using MyCustom;
using System.Collections.Generic;

namespace Upgrade
{
	public class UpgradeGroupController : CustomMonoBehaviour
	{
		public List<TierUpgradeController> listTierUpgrade = new List<TierUpgradeController>();

		public int currentUpgradeLevel;

		public void RefreshListTier()
		{
			for (int i = 0; i < listTierUpgrade.Count; i++)
			{
				if (i <= currentUpgradeLevel)
				{
					listTierUpgrade[i].ViewUpgraded();
				}
				else if (i == currentUpgradeLevel + 1)
				{
					listTierUpgrade[i].ViewCanUpgrade();
				}
				else
				{
					listTierUpgrade[i].ViewCannotUpgrade();
				}
			}
		}
	}
}

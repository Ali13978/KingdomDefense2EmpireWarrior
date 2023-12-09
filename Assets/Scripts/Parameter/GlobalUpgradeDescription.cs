using System.Collections.Generic;

namespace Parameter
{
	public class GlobalUpgradeDescription : Singleton<GlobalUpgradeDescription>
	{
		private List<GlobalUpgradeDes> listUpgradeDescription = new List<GlobalUpgradeDes>();

		public void ClearData()
		{
			listUpgradeDescription.Clear();
		}

		public void SetGlobalUpgradeParameters(GlobalUpgradeDes gud)
		{
			int count = listUpgradeDescription.Count;
			if (count <= gud.id)
			{
				listUpgradeDescription.Add(gud);
			}
		}

		public string GetTitle(int guID)
		{
			if (guID < listUpgradeDescription.Count && guID >= 0)
			{
				GlobalUpgradeDes globalUpgradeDes = listUpgradeDescription[guID];
				return globalUpgradeDes.title;
			}
			return "--";
		}

		public string GetDescription(int guID)
		{
			if (guID < listUpgradeDescription.Count && guID >= 0)
			{
				GlobalUpgradeDes globalUpgradeDes = listUpgradeDescription[guID];
				return globalUpgradeDes.description;
			}
			return "--";
		}
	}
}

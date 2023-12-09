using System.Collections.Generic;

namespace Parameter
{
	public class PowerupItemDescription : Singleton<PowerupItemDescription>
	{
		private List<PowerupItemDes> listPowerupItemDescription = new List<PowerupItemDes>();

		private bool CheckId(int tipID)
		{
			return tipID >= 0 && tipID < listPowerupItemDescription.Count;
		}

		public void ClearData()
		{
			listPowerupItemDescription.Clear();
		}

		public void SetPowerupItemParameter(PowerupItemDes tip)
		{
			int count = listPowerupItemDescription.Count;
			if (count <= tip.id)
			{
				listPowerupItemDescription.Add(tip);
			}
		}

		public string GetName(int tipID)
		{
			if (tipID < listPowerupItemDescription.Count && tipID >= 0)
			{
				PowerupItemDes powerupItemDes = listPowerupItemDescription[tipID];
				return powerupItemDes.name;
			}
			return "--";
		}

		public string GetDescription(int tipID)
		{
			if (tipID < listPowerupItemDescription.Count && tipID >= 0)
			{
				PowerupItemDes powerupItemDes = listPowerupItemDescription[tipID];
				return powerupItemDes.description;
			}
			return "--";
		}
	}
}

using System.Collections.Generic;

namespace Gameplay
{
	public class TowerUltimateController : TowerController
	{
		public List<TowerUltimateCommon> listTowerUltimate = new List<TowerUltimateCommon>();

		public List<int> currentLevelUpgrade = new List<int>();

		public override void OnAppear()
		{
			base.OnAppear();
			ClearDataUpgrade();
			AddDefaultData();
		}

		public override void OnReturnPool()
		{
			base.OnReturnPool();
			ClearDataUpgrade();
		}

		private void ClearDataUpgrade()
		{
			foreach (TowerUltimateCommon item in listTowerUltimate)
			{
				item.OnReturnPool();
			}
			currentLevelUpgrade.Clear();
		}

		private void AddDefaultData()
		{
			foreach (TowerUltimateCommon item in listTowerUltimate)
			{
				item.InitTowerModel(base.TowerModel);
			}
			currentLevelUpgrade.Add(-1);
			currentLevelUpgrade.Add(-1);
		}
	}
}

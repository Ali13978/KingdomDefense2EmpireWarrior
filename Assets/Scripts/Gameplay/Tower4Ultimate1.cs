namespace Gameplay
{
	public class Tower4Ultimate1 : TowerUltimateCommon
	{
		private int towerID = 4;

		private int ultimateID = 1;

		private TowerModel towerModel;

		public override void InitTowerModel(TowerModel towerModel)
		{
			this.towerModel = towerModel;
		}

		public override void UnlockUltimate(int ultiLevel)
		{
			base.UnlockUltimate(ultiLevel);
			unlock = true;
			ReadParameter(ultiLevel);
		}

		public override void OnReturnPool()
		{
			base.OnReturnPool();
		}

		private void ReadParameter(int currentUltiLevel)
		{
		}
	}
}

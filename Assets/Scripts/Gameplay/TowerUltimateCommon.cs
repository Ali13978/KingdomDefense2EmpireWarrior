using MyCustom;

namespace Gameplay
{
	public class TowerUltimateCommon : CustomMonoBehaviour
	{
		public bool unlock;

		public virtual void UnlockUltimate(int ultiLevel)
		{
			SingletonMonoBehaviour<TowerControlSoundController>.Instance.PlayUpgradeUltimate();
		}

		public virtual void InitTowerModel(TowerModel towerModel)
		{
			unlock = false;
		}

		public virtual void OnReturnPool()
		{
			unlock = false;
		}
	}
}

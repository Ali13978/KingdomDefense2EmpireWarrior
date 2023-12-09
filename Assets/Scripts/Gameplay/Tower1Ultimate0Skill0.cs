using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class Tower1Ultimate0Skill0 : TowerUltimateCommon
	{
		private int towerID = 1;

		private int ultimateBranch;

		private int skillID;

		private float attackRangeFar;

		private TowerModel towerModel;

		private TowerSpawnAllyController towerSpawnAllyController;

		[SerializeField]
		private GameObject bulletPrefab;

		public override void InitTowerModel(TowerModel towerModel)
		{
			this.towerModel = towerModel;
			towerSpawnAllyController = towerModel.towerSpawnAllyController;
		}

		public override void UnlockUltimate(int ultiLevel)
		{
			base.UnlockUltimate(ultiLevel);
			unlock = true;
			SingletonMonoBehaviour<SpawnBullet>.Instance.InitExtendBullet(bulletPrefab);
			ReadParameter(ultiLevel);
			TryToUnlockRangeAttackAbility();
		}

		public override void OnReturnPool()
		{
			base.OnReturnPool();
		}

		private void ReadParameter(int currentSkillLevel)
		{
			attackRangeFar = (float)TowerSkillParameter.Instance.GetParamBySkillLevel(towerID, ultimateBranch, skillID, currentSkillLevel, 0) / GameData.PIXEL_PER_UNIT;
		}

		public void TryToUnlockRangeAttackAbility()
		{
			if (unlock)
			{
				towerSpawnAllyController.UnlockRangeAttackAbility(attackRangeFar);
			}
		}
	}
}

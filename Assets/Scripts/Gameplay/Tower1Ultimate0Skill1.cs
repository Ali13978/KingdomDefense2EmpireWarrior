using Parameter;

namespace Gameplay
{
	public class Tower1Ultimate0Skill1 : TowerUltimateCommon
	{
		private int towerID = 1;

		private int ultimateBranch;

		private int skillID = 1;

		private float bonusArmorPhysics;

		private float bonusArmorMagic;

		private TowerModel towerModel;

		private TowerSpawnAllyController towerSpawnAllyController;

		public override void InitTowerModel(TowerModel towerModel)
		{
			this.towerModel = towerModel;
			towerSpawnAllyController = towerModel.towerSpawnAllyController;
		}

		public override void UnlockUltimate(int ultiLevel)
		{
			base.UnlockUltimate(ultiLevel);
			unlock = true;
			ReadParameter(ultiLevel);
			TryToAddPassiveArmor();
		}

		public override void OnReturnPool()
		{
			base.OnReturnPool();
		}

		private void ReadParameter(int currentSkillLevel)
		{
			bonusArmorPhysics = TowerSkillParameter.Instance.GetParamBySkillLevel(towerID, ultimateBranch, skillID, currentSkillLevel, 0);
			bonusArmorMagic = TowerSkillParameter.Instance.GetParamBySkillLevel(towerID, ultimateBranch, skillID, currentSkillLevel, 1);
		}

		public void TryToAddPassiveArmor()
		{
			if (unlock)
			{
				towerSpawnAllyController.AddPassiveArmor(bonusArmorPhysics, bonusArmorMagic);
			}
		}
	}
}

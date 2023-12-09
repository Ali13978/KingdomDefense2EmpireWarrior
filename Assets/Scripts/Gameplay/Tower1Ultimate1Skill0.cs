using Parameter;

namespace Gameplay
{
	public class Tower1Ultimate1Skill0 : TowerUltimateCommon
	{
		private int towerID = 1;

		private int ultimateBranch = 1;

		private int skillID;

		private TowerModel towerModel;

		private TowerSpawnAllyController towerSpawnAllyController;

		private int dodgeChance;

		private int ignoreArmorChance;

		public override void InitTowerModel(TowerModel towerModel)
		{
			this.towerModel = towerModel;
			towerSpawnAllyController = towerModel.towerSpawnAllyController;
		}

		public override void UnlockUltimate(int ultiLevel)
		{
			base.UnlockUltimate(ultiLevel);
			ReadParameter(ultiLevel);
			TryToUnlockAbility();
		}

		public override void OnReturnPool()
		{
			base.OnReturnPool();
		}

		private void ReadParameter(int currentSkillLevel)
		{
			dodgeChance = TowerSkillParameter.Instance.GetParamBySkillLevel(towerID, ultimateBranch, skillID, currentSkillLevel, 0);
			ignoreArmorChance = TowerSkillParameter.Instance.GetParamBySkillLevel(towerID, ultimateBranch, skillID, currentSkillLevel, 1);
			unlock = true;
		}

		public void TryToUnlockAbility()
		{
			if (unlock)
			{
				towerSpawnAllyController.UnlockDodgeAbility(dodgeChance);
				towerSpawnAllyController.UnlockIgnoreArmorAbility(ignoreArmorChance);
				towerModel.towerSoundController.PlayCastSkillSound(skillID);
			}
		}
	}
}

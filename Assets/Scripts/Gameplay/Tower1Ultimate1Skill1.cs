using Parameter;

namespace Gameplay
{
	public class Tower1Ultimate1Skill1 : TowerUltimateCommon
	{
		private int towerID = 1;

		private int ultimateBranch = 1;

		private int skillID = 1;

		private TowerModel towerModel;

		private TowerSpawnAllyController towerSpawnAllyController;

		private float duration;

		private float cooldownTime;

		private string description;

		public override void InitTowerModel(TowerModel towerModel)
		{
			this.towerModel = towerModel;
			towerSpawnAllyController = towerModel.towerSpawnAllyController;
		}

		public override void UnlockUltimate(int ultiLevel)
		{
			base.UnlockUltimate(ultiLevel);
			ReadParameter(ultiLevel);
			TryToUnlockSkillSlash();
		}

		public override void OnReturnPool()
		{
			base.OnReturnPool();
		}

		private void ReadParameter(int currentSkillLevel)
		{
			duration = TowerSkillParameter.Instance.GetParamBySkillLevel(towerID, ultimateBranch, skillID, currentSkillLevel, 1);
			cooldownTime = TowerSkillParameter.Instance.GetParamBySkillLevel(towerID, ultimateBranch, skillID, currentSkillLevel, 2);
			unlock = true;
		}

		public void TryToUnlockSkillSlash()
		{
			if (unlock)
			{
				towerSpawnAllyController.UnlockSkillSlash(duration, cooldownTime, description, isActiveAtStart: true);
				towerModel.towerSoundController.PlayCastSkillSound(skillID);
			}
		}
	}
}

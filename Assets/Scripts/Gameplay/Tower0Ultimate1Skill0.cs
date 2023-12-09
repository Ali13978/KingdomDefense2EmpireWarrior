using Parameter;

namespace Gameplay
{
	public class Tower0Ultimate1Skill0 : TowerUltimateCommon
	{
		private int towerID;

		private int ultimateBranch = 1;

		private int skillID;

		private int chanceToInstantKill;

		private float duration;

		private TowerModel towerModel;

		private string buffKey = "InstantKillRateIncrementCommon";

		public override void InitTowerModel(TowerModel towerModel)
		{
			this.towerModel = towerModel;
		}

		public override void UnlockUltimate(int ultiLevel)
		{
			base.UnlockUltimate(ultiLevel);
			ReadParameter(ultiLevel);
			AddBuff();
		}

		public override void OnReturnPool()
		{
			base.OnReturnPool();
		}

		private void ReadParameter(int currentSkillLevel)
		{
			chanceToInstantKill = TowerSkillParameter.Instance.GetParamBySkillLevel(towerID, ultimateBranch, skillID, currentSkillLevel, 0);
			duration = TowerSkillParameter.Instance.GetParamBySkillLevel(towerID, ultimateBranch, skillID, currentSkillLevel, 1);
			unlock = true;
		}

		private void AddBuff()
		{
			bool isPositive = true;
			towerModel.BuffsHolder.AddBuff(buffKey, new Buff(isPositive, chanceToInstantKill, duration), BuffStackLogic.ChooseMax, BuffStackLogic.ChooseMax);
			towerModel.towerSoundController.PlayCastSkillSound(skillID);
		}
	}
}

using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class Tower0Ultimate1Skill1 : TowerUltimateCommon
	{
		private int towerID;

		private int ultimateBranch = 1;

		private int skillID = 1;

		private string buffKey = "Bleed";

		private int chanceToBleed;

		private int bonusDamagePercentage;

		private float duration;

		private TowerModel towerModel;

		public override void InitTowerModel(TowerModel towerModel)
		{
			this.towerModel = towerModel;
		}

		public override void UnlockUltimate(int ultiLevel)
		{
			base.UnlockUltimate(ultiLevel);
			ReadParameter(ultiLevel);
		}

		public override void OnReturnPool()
		{
			base.OnReturnPool();
		}

		private void ReadParameter(int currentSkillLevel)
		{
			chanceToBleed = TowerSkillParameter.Instance.GetParamBySkillLevel(towerID, ultimateBranch, skillID, currentSkillLevel, 0);
			bonusDamagePercentage = TowerSkillParameter.Instance.GetParamBySkillLevel(towerID, ultimateBranch, skillID, currentSkillLevel, 1);
			duration = TowerSkillParameter.Instance.GetParamBySkillLevel(towerID, ultimateBranch, skillID, currentSkillLevel, 2);
			unlock = true;
			InitFXs();
		}

		private void InitFXs()
		{
			SingletonMonoBehaviour<SpawnFX>.Instance.InitFX(SpawnFX.EFFECT_BLEED);
		}

		public void TryToCastBleed()
		{
			if (unlock && Random.Range(0, 100) < chanceToBleed && towerModel.towerFindEnemyController.Targets.Count > 0)
			{
				CastBleed(towerModel.towerFindEnemyController.Targets[0]);
			}
		}

		private void CastBleed(EnemyModel enemyModel)
		{
			enemyModel.ProcessEffect(buffKey, bonusDamagePercentage, duration, DamageFXType.Bleed);
			towerModel.towerSoundController.PlayCastSkillSound(skillID);
		}
	}
}

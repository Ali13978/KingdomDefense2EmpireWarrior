using Parameter;

namespace Gameplay
{
	public class Tower2Ultimate0Skill1 : TowerUltimateCommon
	{
		private int towerID = 2;

		private int ultimateBranch;

		private int skillID = 1;

		private int chanceToCastSkill;

		private int damageBurn;

		private float duration;

		private float skillRange;

		private TowerModel towerModel;

		public bool firstTimeUpgrade;

		public int DamageBurn
		{
			get
			{
				return damageBurn;
			}
			set
			{
				damageBurn = value;
			}
		}

		public int ChanceToCastSkill
		{
			get
			{
				return chanceToCastSkill;
			}
			set
			{
				chanceToCastSkill = value;
			}
		}

		public float Duration
		{
			get
			{
				return duration;
			}
			set
			{
				duration = value;
			}
		}

		public override void InitTowerModel(TowerModel towerModel)
		{
			this.towerModel = towerModel;
		}

		public override void UnlockUltimate(int ultiLevel)
		{
			base.UnlockUltimate(ultiLevel);
			unlock = true;
			firstTimeUpgrade = true;
			ReadParameter(ultiLevel);
			SingletonMonoBehaviour<SpawnFX>.Instance.InitFX(SpawnFX.METEOR_EXPLOSION);
			SingletonMonoBehaviour<SpawnFX>.Instance.InitFX(SpawnFX.EFFECT_BURNING);
		}

		public override void OnReturnPool()
		{
			base.OnReturnPool();
			firstTimeUpgrade = false;
		}

		private void ReadParameter(int currentSkillLevel)
		{
			ChanceToCastSkill = TowerSkillParameter.Instance.GetParamBySkillLevel(towerID, ultimateBranch, skillID, currentSkillLevel, 0);
			DamageBurn = TowerSkillParameter.Instance.GetParamBySkillLevel(towerID, ultimateBranch, skillID, currentSkillLevel, 1);
			Duration = TowerSkillParameter.Instance.GetParamBySkillLevel(towerID, ultimateBranch, skillID, currentSkillLevel, 2);
			skillRange = (float)TowerSkillParameter.Instance.GetParamBySkillLevel(towerID, ultimateBranch, skillID, currentSkillLevel, 3) / GameData.PIXEL_PER_UNIT;
		}
	}
}

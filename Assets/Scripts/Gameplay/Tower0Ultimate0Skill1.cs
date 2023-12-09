using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class Tower0Ultimate0Skill1 : TowerUltimateCommon
	{
		private int towerID;

		private int ultimateBranch;

		private int skillID = 1;

		private int chanceToCastSkill;

		private int damage;

		private int slowPercent;

		private float slowTime;

		private string buffKey = "Slow";

		[Space]
		[SerializeField]
		private GameObject arrowPrefab;

		[SerializeField]
		private string bulletName;

		private TowerModel towerModel;

		private CommonAttackDamage commonAttackDamage = new CommonAttackDamage();

		private EffectAttack effectAttack;

		public override void InitTowerModel(TowerModel towerModel)
		{
			this.towerModel = towerModel;
		}

		public override void UnlockUltimate(int ultiLevel)
		{
			base.UnlockUltimate(ultiLevel);
			unlock = true;
			ReadParameter(ultiLevel);
			SingletonMonoBehaviour<SpawnBullet>.Instance.InitExtendBullet(arrowPrefab.gameObject);
			SingletonMonoBehaviour<SpawnFX>.Instance.InitFX(SpawnFX.EFFECT_ITEM_FREEZE);
			CastSkill();
		}

		public override void OnReturnPool()
		{
			base.OnReturnPool();
		}

		private void ReadParameter(int currentSkillLevel)
		{
			chanceToCastSkill = TowerSkillParameter.Instance.GetParamBySkillLevel(towerID, ultimateBranch, skillID, currentSkillLevel, 0);
			damage = TowerSkillParameter.Instance.GetParamBySkillLevel(towerID, ultimateBranch, skillID, currentSkillLevel, 1);
			slowPercent = TowerSkillParameter.Instance.GetParamBySkillLevel(towerID, ultimateBranch, skillID, currentSkillLevel, 2);
			slowTime = TowerSkillParameter.Instance.GetParamBySkillLevel(towerID, ultimateBranch, skillID, currentSkillLevel, 3);
			commonAttackDamage.physicsDamage = damage;
			effectAttack.buffKey = buffKey;
			effectAttack.debuffChance = 100;
			effectAttack.debuffEffectValue = slowPercent;
			effectAttack.debuffEffectDuration = slowTime;
			effectAttack.damageFXType = DamageFXType.Freezing;
		}

		public void TryToCastFreezingArrow()
		{
			if (unlock && chanceToCastSkill > 0 && Random.Range(0, 100) < chanceToCastSkill)
			{
				CastSkill();
			}
		}

		private void CastSkill()
		{
			if (towerModel.towerFindEnemyController.Targets.Count > 0)
			{
				UnityEngine.Debug.Log("Cast skill freezing arrow!");
				Tower originalParameter = towerModel.OriginalParameter;
				BulletModel bulletByName = SingletonMonoBehaviour<SpawnBullet>.Instance.GetBulletByName(bulletName);
				Vector3 position = towerModel.gunBarrel.position;
				float num = originalParameter.attackRangeMax;
				bulletByName.transform.position = position;
				bulletByName.gameObject.SetActive(value: true);
				EnemyModel target = towerModel.towerFindEnemyController.Targets[0];
				bulletByName.InitFromTower(towerModel, new CommonAttackDamage(damage, 0), effectAttack, target);
			}
		}
	}
}

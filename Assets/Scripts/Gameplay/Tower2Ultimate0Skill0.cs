using Parameter;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class Tower2Ultimate0Skill0 : TowerUltimateCommon
	{
		private int towerID = 2;

		private int ultimateBranch;

		private int skillID;

		private int damage;

		private float skillRange;

		private float cooldownTime;

		private string description;

		private float timeTracking;

		private TowerModel towerModel;

		[SerializeField]
		private Transform gunPos;

		[SerializeField]
		private GameObject cannonBallPrefab;

		[SerializeField]
		private string bulletName;

		public override void InitTowerModel(TowerModel towerModel)
		{
			this.towerModel = towerModel;
		}

		public override void UnlockUltimate(int ultiLevel)
		{
			base.UnlockUltimate(ultiLevel);
			unlock = true;
			ReadParameter(ultiLevel);
			SingletonMonoBehaviour<SpawnBullet>.Instance.InitExtendBullet(cannonBallPrefab);
			CastSkill();
		}

		public override void OnReturnPool()
		{
			base.OnReturnPool();
		}

		private void Update()
		{
			if (unlock)
			{
				if (isCooldownDone())
				{
					TryToCastSkill();
				}
				timeTracking = Mathf.MoveTowards(timeTracking, 0f, Time.deltaTime);
			}
		}

		private void ReadParameter(int currentSkillLevel)
		{
			damage = TowerSkillParameter.Instance.GetParamBySkillLevel(towerID, ultimateBranch, skillID, currentSkillLevel, 0);
			skillRange = (float)TowerSkillParameter.Instance.GetParamBySkillLevel(towerID, ultimateBranch, skillID, currentSkillLevel, 1) / GameData.PIXEL_PER_UNIT;
			cooldownTime = TowerSkillParameter.Instance.GetParamBySkillLevel(towerID, ultimateBranch, skillID, currentSkillLevel, 2);
			timeTracking = cooldownTime;
		}

		public void TryToCastSkill()
		{
			if (unlock)
			{
				CastSkill();
			}
		}

		private void CastSkill()
		{
			List<EnemyModel> listActiveEnemy = SingletonMonoBehaviour<GameData>.Instance.ListActiveEnemy;
			if (listActiveEnemy.Count > 0)
			{
				EnemyModel enemyWithHighestHealth = SingletonMonoBehaviour<GameData>.Instance.getEnemyWithHighestHealth(isFlyEnemy: false, isUndergroundEnemy: false);
				if (!(enemyWithHighestHealth == null))
				{
					UnityEngine.Debug.Log("Cast skill Cannonball of Death!");
					BulletModel bulletModel = null;
					bulletModel = SingletonMonoBehaviour<SpawnBullet>.Instance.GetBulletByName(bulletName);
					bulletModel.transform.position = gunPos.position;
					bulletModel.gameObject.SetActive(value: true);
					bulletModel.InitFromTower(towerModel, new CommonAttackDamage(damage, 0, skillRange), enemyWithHighestHealth);
					timeTracking = cooldownTime;
				}
			}
		}

		private bool isCooldownDone()
		{
			return timeTracking == 0f;
		}
	}
}

using Parameter;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class Tower3Ultimate0Skill0 : TowerUltimateCommon
	{
		private int towerID = 3;

		private int ultimateBranch;

		private int skillID;

		private int numberOfDragon;

		private int maxDragon = 3;

		private int damagePerDragon;

		private TowerModel towerModel;

		private EnemyModel target;

		private List<MiniDragonController> miniDragonControllers = new List<MiniDragonController>();

		[SerializeField]
		private MiniDragonController miniDragonPrefab;

		[SerializeField]
		private Transform[] miniDragonPositions;

		[Space]
		[SerializeField]
		private GameObject miniDragonBullet;

		public override void InitTowerModel(TowerModel towerModel)
		{
			this.towerModel = towerModel;
			ClearData();
		}

		public override void UnlockUltimate(int ultiLevel)
		{
			base.UnlockUltimate(ultiLevel);
			unlock = true;
			SingletonMonoBehaviour<SpawnBullet>.Instance.InitExtendBullet(miniDragonBullet.gameObject);
			SingletonMonoBehaviour<SpawnTower>.Instance.InitMiniDragon(miniDragonPrefab.gameObject);
			ReadParameter(ultiLevel);
			TryToCreateMiniDragon();
		}

		public override void OnReturnPool()
		{
			base.OnReturnPool();
			ClearData();
		}

		private void ClearData()
		{
			foreach (MiniDragonController miniDragonController in miniDragonControllers)
			{
				miniDragonController.ReturnPool();
			}
			miniDragonControllers.Clear();
			List<MiniDragonController> list = new List<MiniDragonController>(GetComponentsInChildren<MiniDragonController>(includeInactive: true));
			foreach (MiniDragonController item in list)
			{
				item.ReturnPool();
			}
		}

		private void ReadParameter(int currentSkillLevel)
		{
			numberOfDragon = TowerSkillParameter.Instance.GetParamBySkillLevel(towerID, ultimateBranch, skillID, currentSkillLevel, 0);
			damagePerDragon = TowerSkillParameter.Instance.GetParamBySkillLevel(towerID, ultimateBranch, skillID, currentSkillLevel, 1);
		}

		public void TryToCreateMiniDragon()
		{
			if (unlock)
			{
				MiniDragonController miniDragon = SingletonMonoBehaviour<SpawnTower>.Instance.GetMiniDragon();
				miniDragonControllers.Add(miniDragon.GetComponent<MiniDragonController>());
				Vector3 position = miniDragonPositions[miniDragonControllers.Count - 1].position;
				miniDragon.Init(towerModel, damagePerDragon);
				miniDragon.transform.position = position;
				miniDragon.transform.parent = base.transform;
				miniDragon.gameObject.SetActive(value: true);
			}
		}

		public void StartAttack()
		{
			List<EnemyModel> targets = towerModel.towerFindEnemyController.Targets;
			if (targets.Count > 0)
			{
				target = targets[Random.Range(0, targets.Count)];
				foreach (MiniDragonController miniDragonController in miniDragonControllers)
				{
					miniDragonController.StartAttack(target);
				}
			}
		}
	}
}

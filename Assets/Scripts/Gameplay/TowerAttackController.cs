using Parameter;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay
{
	[DisallowMultipleComponent]
	public class TowerAttackController : TowerController
	{
		[SerializeField]
		private float maxAngleCanAttack = 360f;

		[SerializeField]
		private Transform gun;

		[SerializeField]
		private List<TowerAttackSingleTargetController> singleTargetAttackControllers = new List<TowerAttackSingleTargetController>();

		private Tower originalParameter;

		private GameObject gunBarrelRight;

		private GameObject gunBarrelLeft;

		private float originReloadTime;

		private float currentReloadTime;

		private float reloadTimeTracking;

		public UnityEvent StartAttack;

		private TowerFindEnemyController towerFindEnemyController;

		[SerializeField]
		private bool singleTargetBehavior;

		[SerializeField]
		private bool multiTargetBehavior;

		private List<string> increaseAttackSpeedBuffKeys = new List<string>
		{
			"IncreaseAttackSpeedByPercentage"
		};

		public override void Update()
		{
			base.Update();
			if (!SingletonMonoBehaviour<GameData>.Instance.IsGameOver)
			{
				if (ShouldAttack() && IsReloadDone())
				{
					Attack();
					reloadTimeTracking = currentReloadTime;
				}
				reloadTimeTracking = Mathf.MoveTowards(reloadTimeTracking, 0f, Time.deltaTime);
			}
		}

		public void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.red;
			Vector3 position = base.transform.position;
			Tower tower = base.TowerModel.OriginalParameter;
			Gizmos.DrawWireSphere(position, (float)tower.attackRangeMax / GameData.PIXEL_PER_UNIT);
		}

		private bool ShouldAttack()
		{
			if (towerFindEnemyController.Targets.Count == 0)
			{
				return false;
			}
			return true;
		}

		private bool IsReloadDone()
		{
			return reloadTimeTracking == 0f;
		}

		public override void Initialize()
		{
			base.Initialize();
			originalParameter = base.TowerModel.OriginalParameter;
			towerFindEnemyController = base.TowerModel.towerFindEnemyController;
			towerFindEnemyController.OnTargetRemoved += TowerFindEnemyController_OnTargetRemoved;
		}

		private void TowerFindEnemyController_OnTargetRemoved(EnemyModel target)
		{
			RemoveTarget(target);
		}

		public override void OnAppear()
		{
			base.OnAppear();
			Tower tower = base.TowerModel.OriginalParameter;
			originReloadTime = (float)tower.reload / 1000f;
			currentReloadTime = originReloadTime;
			reloadTimeTracking = originReloadTime;
			base.TowerModel.BuffsHolder.OnBuffValueChanged += BuffsHolder_OnBuffValueChanged;
		}

		private void BuffsHolder_OnBuffValueChanged(string buffKey, bool added)
		{
			if (increaseAttackSpeedBuffKeys.Contains(buffKey))
			{
				ApplyIncreaseAttackSpeed();
			}
		}

		private void ApplyIncreaseAttackSpeed()
		{
			float buffsValue = base.TowerModel.BuffsHolder.GetBuffsValue(increaseAttackSpeedBuffKeys);
			if (buffsValue > 0f)
			{
				currentReloadTime = originReloadTime / (buffsValue / 100f);
			}
			else
			{
				currentReloadTime = originReloadTime;
			}
			currentReloadTime = Mathf.Clamp(currentReloadTime, 0.1f, 999f);
		}

		public void Attack()
		{
			StartAttack.Invoke();
			int num = 0;
			if (singleTargetBehavior)
			{
				num = Mathf.Min(towerFindEnemyController.Targets.Count, singleTargetAttackControllers.Count);
				for (int i = 0; i < num; i++)
				{
					foreach (TowerAttackSingleTargetController singleTargetAttackController in singleTargetAttackControllers)
					{
						singleTargetAttackController.StartAttack(towerFindEnemyController.Targets[i]);
					}
				}
			}
			if (!multiTargetBehavior)
			{
				return;
			}
			if (towerFindEnemyController.Targets.Count == 1)
			{
				num = singleTargetAttackControllers.Count;
				for (int j = 0; j < num; j++)
				{
					singleTargetAttackControllers[j].StartAttack(towerFindEnemyController.Targets[0]);
				}
			}
			else
			{
				num = Mathf.Min(towerFindEnemyController.Targets.Count, singleTargetAttackControllers.Count);
				for (int k = 0; k < num; k++)
				{
					singleTargetAttackControllers[k].StartAttack(towerFindEnemyController.Targets[k]);
				}
			}
		}

		public void StopAttack()
		{
			foreach (TowerAttackSingleTargetController singleTargetAttackController in singleTargetAttackControllers)
			{
				singleTargetAttackController.StopAttack();
			}
		}

		public void RemoveTarget(EnemyModel enemyController)
		{
			foreach (TowerAttackSingleTargetController singleTargetAttackController in singleTargetAttackControllers)
			{
				if (singleTargetAttackController.Target == enemyController)
				{
					singleTargetAttackController.StopAttack();
				}
			}
		}

		public override void OnReturnPool()
		{
			base.OnReturnPool();
			StopAttack();
		}
	}
}

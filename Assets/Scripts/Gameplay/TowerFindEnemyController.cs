using Parameter;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	[DisallowMultipleComponent]
	public class TowerFindEnemyController : TowerController
	{
		[SerializeField]
		private int desiredTargetsNumber;

		private List<string> silentBuffKeys = new List<string>
		{
			"Silent"
		};

		private List<string> increaseAttackRangeBuffKeys = new List<string>
		{
			"AttackRangeIncrementCommon"
		};

		[SerializeField]
		private List<EnemyModel> targets = new List<EnemyModel>();

		private float attackRangeMax;

		private float buffedAttackRangeMax;

		private Tower originalParameter;

		private List<EnemyModel> allNewTargets = new List<EnemyModel>();

		public List<EnemyModel> Targets
		{
			get
			{
				return targets;
			}
			private set
			{
				targets = value;
			}
		}

		public int DesiredTargetsNumber
		{
			get
			{
				return desiredTargetsNumber;
			}
			private set
			{
				desiredTargetsNumber = value;
			}
		}

		public float AttackRangeMax
		{
			get
			{
				return attackRangeMax;
			}
			private set
			{
				attackRangeMax = value;
			}
		}

		public float BuffedAttackRangeMax => buffedAttackRangeMax;

		public event Action<EnemyModel> OnTargetRemoved;

		public override void Initialize()
		{
			base.Initialize();
			SetParameter();
			base.TowerModel.BuffsHolder.OnBuffValueChanged += BuffsHolder_OnBuffValueChanged;
		}

		public override void OnAppear()
		{
			base.OnAppear();
			base.TowerModel.IsSilent = false;
		}

		private void SetParameter()
		{
			originalParameter = base.TowerModel.OriginalParameter;
			Tower tower = base.TowerModel.OriginalParameter;
			AttackRangeMax = (float)tower.attackRangeMax / GameData.PIXEL_PER_UNIT;
			buffedAttackRangeMax = AttackRangeMax;
		}

		public override void OnReturnPool()
		{
			base.OnReturnPool();
			base.TowerModel.IsSilent = false;
			targets.Clear();
		}

		public override void OnBuildFinished()
		{
			base.OnBuildFinished();
		}

		private void BuffsHolder_OnBuffValueChanged(string buffKey, bool added)
		{
			base.TowerModel.IsSilent = (base.TowerModel.BuffsHolder.GetBuffsValue(silentBuffKeys) > 0f);
			if (base.TowerModel.IsSilent)
			{
				ApplyBuffSilent();
			}
			if (increaseAttackRangeBuffKeys.Contains(buffKey))
			{
				ApplyBuffIncreaseAttackRange();
			}
		}

		private void ApplyBuffSilent()
		{
			foreach (EnemyModel target in Targets)
			{
				if (this.OnTargetRemoved != null)
				{
					this.OnTargetRemoved(target);
				}
			}
			Targets.Clear();
		}

		private void ApplyBuffIncreaseAttackRange()
		{
			float num = 1f + base.TowerModel.BuffsHolder.GetBuffsValue(increaseAttackRangeBuffKeys) / 100f;
			if (num != 0f)
			{
				buffedAttackRangeMax = attackRangeMax * num;
			}
			else
			{
				buffedAttackRangeMax = attackRangeMax;
			}
		}

		public override void Update()
		{
			base.Update();
			if (!base.TowerModel.IsSilent && Targets.Count < DesiredTargetsNumber)
			{
				FindNewTarget();
			}
			UpdateRemoveTarget();
		}

		private void FindNewTarget()
		{
			FindAllNewTargetsInRange();
			if (allNewTargets.Count != 0)
			{
				int num = 0;
				while (Targets.Count < DesiredTargetsNumber && num < allNewTargets.Count)
				{
					Targets.Add(allNewTargets[num]);
					num++;
				}
			}
		}

		private void UpdateRemoveTarget()
		{
			if (Targets.Count == 0)
			{
				return;
			}
			for (int num = Targets.Count - 1; num >= 0; num--)
			{
				EnemyModel enemyModel = Targets[num];
				if (SingletonMonoBehaviour<GameData>.Instance.SqrDistance(base.TowerModel.gameObject, enemyModel.gameObject) > buffedAttackRangeMax * buffedAttackRangeMax || !enemyModel.gameObject.activeSelf || !enemyModel.IsAlive || enemyModel.IsInTunnel)
				{
					RemoveTarget(enemyModel);
				}
			}
		}

		private void RemoveTarget(EnemyModel target)
		{
			if (this.OnTargetRemoved != null)
			{
				this.OnTargetRemoved(target);
			}
			Targets.Remove(target);
		}

		private void FindAllNewTargetsInRange()
		{
			allNewTargets.Clear();
			List<EnemyModel> listActiveEnemy = SingletonMonoBehaviour<GameData>.Instance.ListActiveEnemy;
			bool isRoundAttack = originalParameter.isRoundAttack;
			bool isAirAttack = originalParameter.isAirAttack;
			float sqrMaxRange = buffedAttackRangeMax * buffedAttackRangeMax;
			if (isAirAttack && isRoundAttack)
			{
				for (int i = 0; i < listActiveEnemy.Count; i++)
				{
					EnemyModel enemyModel = listActiveEnemy[i];
					if (!Targets.Contains(enemyModel) && SingletonMonoBehaviour<GameData>.Instance.IsInRange(base.TowerModel.gameObject, enemyModel.gameObject, sqrMaxRange, 0f) && !enemyModel.IsUnderground && !enemyModel.IsInTunnel)
					{
						allNewTargets.Add(enemyModel);
					}
				}
			}
			else
			{
				if (isAirAttack || !isRoundAttack)
				{
					return;
				}
				for (int j = 0; j < listActiveEnemy.Count; j++)
				{
					EnemyModel enemyModel2 = listActiveEnemy[j];
					if (!Targets.Contains(enemyModel2) && !enemyModel2.IsAir && SingletonMonoBehaviour<GameData>.Instance.IsInRange(base.TowerModel.gameObject, enemyModel2.gameObject, sqrMaxRange, 0f) && !enemyModel2.IsUnderground)
					{
						allNewTargets.Add(enemyModel2);
					}
				}
			}
		}
	}
}

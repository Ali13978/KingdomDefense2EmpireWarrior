using Parameter;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public abstract class TowerAttackSingleTargetController : TowerController
	{
		[SerializeField]
		protected List<BulletParameter> bulletParametersInOneTurn = new List<BulletParameter>();

		private EnemyModel target;

		private List<string> damageIncrementPercentageBuffKeys = new List<string>
		{
			"DamageIncrementCommon"
		};

		private List<string> damageDecrementPercentageBuffKeys = new List<string>
		{
			"DamageDecrementCommon"
		};

		private List<string> intstantKillRateIncrementBuffKeys = new List<string>
		{
			"InstantKillRateIncrementCommon"
		};

		private float damageIncrementPercentage;

		private float damageDecrementPercentage;

		private float buffedDamagePhysics_min;

		private float buffedDamagePhysics_max;

		private float buffedDamageMagic_min;

		private float buffedDamageMagic_max;

		private float buffedCriticalStrikeChance;

		private int buffedInstantKillChance;

		public int BuffedDamagePhysics => (int)UnityEngine.Random.Range(buffedDamagePhysics_min, buffedDamagePhysics_max);

		public int BuffedDamageMagic => (int)UnityEngine.Random.Range(buffedDamageMagic_min, buffedDamageMagic_max);

		public float BuffedCriticalStrikeChance => (int)buffedCriticalStrikeChance;

		public int BuffedInstantKillChance => buffedInstantKillChance;

		public EnemyModel Target
		{
			get
			{
				return target;
			}
			private set
			{
				target = value;
			}
		}

		public abstract event Action<BulletModel, EnemyModel> OnFireBullet;

		public int GetNumberBulletInOneTurn()
		{
			return bulletParametersInOneTurn.Count;
		}

		public override void Initialize()
		{
			base.Initialize();
			base.TowerModel.BuffsHolder.OnBuffValueChanged += BuffsHolder_OnBuffValueChanged;
		}

		public override void OnAppear()
		{
			base.OnAppear();
		}

		public override void OnBuildFinished()
		{
			base.OnBuildFinished();
			UpdateBuffDamage();
		}

		private void BuffsHolder_OnBuffValueChanged(string buffKey, bool added)
		{
			UpdateBuffDamage();
			if (damageIncrementPercentageBuffKeys.Contains(buffKey))
			{
				UpdateDamageIncrement();
			}
			if (damageDecrementPercentageBuffKeys.Contains(buffKey))
			{
				UpdateDamageDecrement();
			}
			if (intstantKillRateIncrementBuffKeys.Contains(buffKey))
			{
				UpdateInstantKillRate();
			}
		}

		private void UpdateDamageIncrement()
		{
			damageIncrementPercentage = base.TowerModel.BuffsHolder.GetBuffsValue(damageIncrementPercentageBuffKeys);
		}

		private void UpdateDamageDecrement()
		{
			damageDecrementPercentage = base.TowerModel.BuffsHolder.GetBuffsValue(damageDecrementPercentageBuffKeys);
		}

		private void UpdateInstantKillRate()
		{
			float buffsValue = base.TowerModel.BuffsHolder.GetBuffsValue(intstantKillRateIncrementBuffKeys);
			Tower originalParameter = base.TowerModel.OriginalParameter;
			buffedInstantKillChance = originalParameter.instantKillChance + (int)buffsValue;
		}

		private void UpdateBuffDamage()
		{
			float num = 1f + (damageIncrementPercentage - damageDecrementPercentage) / 100f;
			if (num < 0f)
			{
				num = 0f;
			}
			buffedDamagePhysics_min = (float)base.TowerModel.finalDamagePhysics_min * num;
			buffedDamagePhysics_max = (float)base.TowerModel.finalDamagePhysics_max * num;
			buffedDamageMagic_min = (float)base.TowerModel.finalDamageMagic_min * num;
			buffedDamageMagic_max = (float)base.TowerModel.finalDamageMagic_max * num;
			buffedCriticalStrikeChance = (float)base.TowerModel.finalCriticalStrikeChange * num;
			Tower originalParameter = base.TowerModel.OriginalParameter;
			buffedInstantKillChance = originalParameter.instantKillChance;
		}

		public void StartAttack(EnemyModel target)
		{
			this.target = target;
			OnStartAttack();
		}

		protected abstract void OnStartAttack();

		public abstract void StopAttack();
	}
}

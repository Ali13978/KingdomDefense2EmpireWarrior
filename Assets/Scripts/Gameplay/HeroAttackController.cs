using Parameter;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class HeroAttackController : HeroController
	{
		private List<string> increaseAttackSpeedBuffKeys = new List<string>
		{
			"IncreaseAttackSpeed"
		};

		private List<string> increasePhysicsDamageBuffKeys = new List<string>
		{
			"IncreaseDamagePhysics"
		};

		private List<string> increaseMagicDamageBuffKeys = new List<string>
		{
			"IncreaseDamageMagic"
		};

		private List<string> increaseAttackPercentBuffKey = new List<string>
		{
			"BuffAttackByPercentage"
		};

		private List<string> increaseAttackRangePercentBuffKey = new List<string>
		{
			"BuffAttackRangeByPercentage"
		};

		[Header("Specific")]
		[SerializeField]
		private List<BulletParameter> bulletParametersInOneTurn = new List<BulletParameter>();

		[Space]
		[SerializeField]
		private int attackFrame;

		private Hero heroParameter;

		public float CurrentAttackRangeMax;

		private float cooldownTimeTracking;

		private float cooldownTime;

		private int originCriticalStrikeChance;

		private int currentCriticalStrikeChance;

		private int atkPhysicsMin;

		private int atkPhysicsMax;

		private int atkMagicMin;

		private int atkMagicMax;

		[NonSerialized]
		public EnemyModel currentTarget;

		public bool isMeleeAttackForRangeHero;

		public float AttackRangeMax
		{
			get
			{
				Hero originalParameter = base.HeroModel.OriginalParameter;
				return (float)originalParameter.attack_range_max / GameData.PIXEL_PER_UNIT;
			}
		}

		public float AttackRangeAverage
		{
			get
			{
				Hero originalParameter = base.HeroModel.OriginalParameter;
				return (float)originalParameter.attack_range_average / GameData.PIXEL_PER_UNIT;
			}
		}

		public float AttackRangeMin
		{
			get
			{
				Hero originalParameter = base.HeroModel.OriginalParameter;
				return (float)originalParameter.attack_range_min / GameData.PIXEL_PER_UNIT;
			}
		}

		public float CooldownTime
		{
			get
			{
				return cooldownTime;
			}
			set
			{
				cooldownTime = value;
			}
		}

		public int OriginCriticalStrikeChance
		{
			get
			{
				return originCriticalStrikeChance;
			}
			set
			{
				originCriticalStrikeChance = value;
			}
		}

		public int CurrentCriticalStrikeChance
		{
			get
			{
				return currentCriticalStrikeChance;
			}
			set
			{
				currentCriticalStrikeChance = value;
			}
		}

		public override void OnAppear()
		{
			base.OnAppear();
			GetAllComponents();
			SetParameter();
			if (base.HeroModel.RangeHero)
			{
				SpawnBullet instance = SingletonMonoBehaviour<SpawnBullet>.Instance;
				Hero originalParameter = base.HeroModel.OriginalParameter;
				instance.InitBulletsFromHeroes(originalParameter.id, 0);
			}
			base.HeroModel.BuffsHolder.OnBuffValueChanged += BuffsHolder_OnBuffValueChanged;
		}

		private void SetParameter()
		{
			Hero originalParameter = base.HeroModel.OriginalParameter;
			CooldownTime = (float)originalParameter.attack_cooldown / 1000f;
			cooldownTimeTracking = 0f;
			OriginCriticalStrikeChance = heroParameter.critical_strike_change;
			CurrentCriticalStrikeChance = OriginCriticalStrikeChance;
			atkPhysicsMin = heroParameter.attack_physics_min;
			atkPhysicsMax = heroParameter.attack_physics_max;
			atkMagicMin = heroParameter.attack_magic_min;
			atkMagicMax = heroParameter.attack_magic_max;
			CurrentAttackRangeMax = AttackRangeMax;
		}

		private void GetAllComponents()
		{
			heroParameter = base.HeroModel.OriginalParameter;
		}

		public int GetAtkPhysicsMin()
		{
			return atkPhysicsMin;
		}

		public int GetAtkPhysicsMax()
		{
			return atkPhysicsMax;
		}

		public int GetAtkMagicMin()
		{
			return atkMagicMin;
		}

		public int GetAtkMagicMax()
		{
			return atkMagicMax;
		}

		private void Start()
		{
			base.HeroModel.OnHitEnemyEvent += HeroModel_OnHitEnemyEvent;
		}

		private void HeroModel_OnHitEnemyEvent()
		{
			if ((bool)base.HeroModel.currentTarget)
			{
				DamageToEnemy(base.HeroModel.currentTarget);
			}
		}

		private void OnDestroy()
		{
			base.HeroModel.OnHitEnemyEvent -= HeroModel_OnHitEnemyEvent;
		}

		private void BuffsHolder_OnBuffValueChanged(string buffKey, bool added)
		{
			if (increaseAttackSpeedBuffKeys.Contains(buffKey))
			{
				ApplyIncreaseAttackSpeed();
			}
			if (increasePhysicsDamageBuffKeys.Contains(buffKey))
			{
				ApplyIncreasePhysicsDamage();
			}
			if (increaseMagicDamageBuffKeys.Contains(buffKey))
			{
				ApplyIncreaseMagicDamage();
			}
			if (increaseAttackPercentBuffKey.Contains(buffKey))
			{
				ApplyBuffAttackByPercent();
			}
			if (increaseAttackRangePercentBuffKey.Contains(buffKey))
			{
				ApplyBuffAttackRangeByPercent();
			}
		}

		private void ApplyIncreaseAttackSpeed()
		{
			float buffsValue = base.HeroModel.BuffsHolder.GetBuffsValue(increaseAttackSpeedBuffKeys);
			Hero originalParameter = base.HeroModel.OriginalParameter;
			float num = (float)originalParameter.attack_cooldown / 1000f;
			Hero originalParameter2 = base.HeroModel.OriginalParameter;
			cooldownTime = num - (float)originalParameter2.attack_cooldown * buffsValue / 100f / 1000f;
			cooldownTime = Mathf.Clamp(cooldownTime, 0.1f, 999f);
		}

		private void ApplyIncreasePhysicsDamage()
		{
			if (HeroParameter.Instance.IsPhysicsAttack(base.HeroModel.HeroID))
			{
				float buffsValue = base.HeroModel.BuffsHolder.GetBuffsValue(increasePhysicsDamageBuffKeys);
				atkPhysicsMin = heroParameter.attack_physics_min + (int)buffsValue;
				atkPhysicsMax = heroParameter.attack_physics_max + (int)buffsValue;
			}
		}

		private void ApplyIncreaseMagicDamage()
		{
			if (HeroParameter.Instance.IsMagicAttack(base.HeroModel.HeroID))
			{
				float buffsValue = base.HeroModel.BuffsHolder.GetBuffsValue(increaseMagicDamageBuffKeys);
				atkMagicMin = heroParameter.attack_magic_min + (int)buffsValue;
				atkMagicMax = heroParameter.attack_magic_max + (int)buffsValue;
			}
		}

		private void ApplyBuffAttackByPercent()
		{
			float buffsValue = base.HeroModel.BuffsHolder.GetBuffsValue(increaseAttackPercentBuffKey);
			float num = buffsValue / 100f;
			if (HeroParameter.Instance.IsPhysicsAttack(base.HeroModel.HeroID))
			{
				atkPhysicsMin = heroParameter.attack_physics_min + Mathf.RoundToInt((float)heroParameter.attack_physics_min * num);
				atkPhysicsMax = heroParameter.attack_physics_max + Mathf.RoundToInt((float)heroParameter.attack_physics_max * num);
			}
			if (HeroParameter.Instance.IsMagicAttack(base.HeroModel.HeroID))
			{
				atkMagicMin = heroParameter.attack_magic_min + Mathf.RoundToInt((float)heroParameter.attack_magic_min * num);
				atkMagicMax = heroParameter.attack_magic_max + Mathf.RoundToInt((float)heroParameter.attack_magic_max * num);
			}
		}

		private void ApplyBuffAttackRangeByPercent()
		{
			float buffsValue = base.HeroModel.BuffsHolder.GetBuffsValue(increaseAttackRangePercentBuffKey);
			float num = buffsValue / 100f;
			CurrentAttackRangeMax = AttackRangeMax + AttackRangeMax * num;
		}

		public void PrepareToMeleeAttack()
		{
		}

		public void DamageToEnemy(EnemyModel enemy)
		{
			CommonAttackDamage commonAttackDamage = new CommonAttackDamage();
			commonAttackDamage.physicsDamage = UnityEngine.Random.Range(atkPhysicsMin / attackFrame, atkPhysicsMax / attackFrame);
			commonAttackDamage.magicDamage = UnityEngine.Random.Range(atkMagicMin / attackFrame, atkMagicMax / attackFrame);
			commonAttackDamage.criticalStrikeChance = CurrentCriticalStrikeChance;
			commonAttackDamage.damageSource = CharacterType.Hero;
			commonAttackDamage.sourceId = base.HeroModel.HeroID;
			enemy.ProcessDamage(DamageType.Melee, commonAttackDamage);
		}

		public void PrepareToRangeAttack()
		{
			int count = bulletParametersInOneTurn.Count;
			for (int i = 0; i < count; i++)
			{
				StartCoroutine(CreateBullet(bulletParametersInOneTurn[i]));
			}
		}

		private IEnumerator CreateBullet(BulletParameter bulletParameter)
		{
			yield return new WaitForSeconds(bulletParameter.delayTime);
			if ((bool)base.HeroModel.currentTarget)
			{
				Hero originalParameter = base.HeroModel.OriginalParameter;
				BulletModel forHero = SingletonMonoBehaviour<SpawnBullet>.Instance.GetForHero(originalParameter.id, 0);
				forHero.transform.eulerAngles = bulletParameter.gunBarrel.eulerAngles;
				CommonAttackDamage commonAttackDamage = new CommonAttackDamage();
				commonAttackDamage.physicsDamage = UnityEngine.Random.Range(atkPhysicsMin, atkPhysicsMax) / bulletParametersInOneTurn.Count;
				commonAttackDamage.magicDamage = UnityEngine.Random.Range(atkMagicMin, atkMagicMax) / bulletParametersInOneTurn.Count;
				commonAttackDamage.criticalStrikeChance = CurrentCriticalStrikeChance;
				commonAttackDamage.targetType.isAir = base.HeroModel.CanAttackAirEnemy();
				forHero.transform.position = bulletParameter.gunBarrel.position;
				forHero.gameObject.SetActive(value: true);
				forHero.InitFromHero(base.HeroModel, commonAttackDamage, base.HeroModel.currentTarget);
			}
		}
	}
}

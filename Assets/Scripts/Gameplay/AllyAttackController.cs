using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Gameplay
{
	public class AllyAttackController : AllyController
	{
		public UnityEvent OnAttack;

		private List<string> increaseAttackSpeedBuffKeys = new List<string>
		{
			"IncreaseAttackSpeed"
		};

		private List<string> increasePhysicsDamageBuffKeys = new List<string>
		{
			"IncreaseDamagePhysics"
		};

		private CommonAttackDamage commonAttackDamageSender;

		private float cooldownTimeTracking;

		private float cooldownTime;

		[Space]
		[Header("Hero attack type")]
		public bool meleeAttack;

		public bool rangeAttack;

		public bool rangeAttackForMeleeAlly;

		[Space]
		[Header("Specific for range attack")]
		[SerializeField]
		private Transform gunPos;

		[SerializeField]
		private string bulletName;

		[Space]
		[SerializeField]
		private int attackFrame;

		private int atkPhysicsMin;

		private int atkPhysicsMax;

		public float Cooldowntime
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

		public override void OnAppear()
		{
			base.OnAppear();
			SetParameter();
			base.AllyModel.BuffsHolder.OnBuffValueChanged += BuffsHolder_OnBuffValueChanged;
		}

		public override void OnReturnPool()
		{
			base.OnReturnPool();
			rangeAttack = false;
			rangeAttackForMeleeAlly = false;
		}

		private void SetParameter()
		{
			cooldownTime = base.AllyModel.AttackCooldown + Random.Range(0f, 0.5f);
			cooldownTimeTracking = 0f;
			rangeAttack = false;
			rangeAttackForMeleeAlly = false;
			atkPhysicsMin = base.AllyModel.PhysicsDamage_min;
			atkPhysicsMax = base.AllyModel.PhysicsDamage_max;
		}

		private void GetAllComponents()
		{
		}

		private void Awake()
		{
			GetAllComponents();
		}

		private void Start()
		{
			base.AllyModel.OnHitEnemyEvent += AllyModel_OnHitEnemyEvent;
		}

		private void AllyModel_OnHitEnemyEvent()
		{
			if ((bool)base.AllyModel.currentTarget)
			{
				DamageToEnemy(base.AllyModel.currentTarget);
				OnAttack.Invoke();
			}
		}

		private void OnDestroy()
		{
			base.AllyModel.OnHitEnemyEvent -= AllyModel_OnHitEnemyEvent;
		}

		public void PrepareToMeleeAttack()
		{
		}

		public void PrepareToRangeAttack()
		{
			StartCoroutine(CreateBullet());
		}

		private IEnumerator CreateBullet()
		{
			yield return new WaitForSeconds(0.2f);
			if ((bool)base.AllyModel.currentTarget)
			{
				BulletModel bulletByName = SingletonMonoBehaviour<SpawnBullet>.Instance.GetBulletByName(bulletName);
				bulletByName.transform.eulerAngles = gunPos.eulerAngles;
				commonAttackDamageSender = new CommonAttackDamage();
				commonAttackDamageSender.physicsDamage = Random.Range(base.AllyModel.PhysicsDamage_min, base.AllyModel.PhysicsDamage_max);
				commonAttackDamageSender.magicDamage = 0;
				commonAttackDamageSender.criticalStrikeChance = 0;
				commonAttackDamageSender.ignoreArmorChance = 0;
				commonAttackDamageSender.isIgnoreArmor = (Random.Range(0, 100) < commonAttackDamageSender.ignoreArmorChance);
				bulletByName.transform.position = gunPos.position;
				bulletByName.gameObject.SetActive(value: true);
				if ((bool)base.AllyModel.TowerSpawnAllyController)
				{
					bulletByName.InitFromTower(base.AllyModel.TowerSpawnAllyController.TowerModel, commonAttackDamageSender, base.AllyModel.currentTarget);
				}
				else
				{
					bulletByName.InitCommon(commonAttackDamageSender, base.AllyModel.currentTarget);
				}
			}
		}

		private void DamageToEnemy(EnemyModel enemy)
		{
			commonAttackDamageSender = new CommonAttackDamage();
			commonAttackDamageSender.physicsDamage = Random.Range(atkPhysicsMin / attackFrame, atkPhysicsMax / attackFrame);
			commonAttackDamageSender.magicDamage = 0;
			commonAttackDamageSender.criticalStrikeChance = base.AllyModel.GetCriticalStrikeChance();
			commonAttackDamageSender.ignoreArmorChance = base.AllyModel.GetIgnoreArmorChance();
			commonAttackDamageSender.isIgnoreArmor = (Random.Range(0, 100) < commonAttackDamageSender.ignoreArmorChance);
			commonAttackDamageSender.sourceId = base.AllyModel.Id;
			commonAttackDamageSender.damageSource = CharacterType.Ally;
			enemy.ProcessDamage(DamageType.Melee, commonAttackDamageSender);
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
		}

		private void ApplyIncreaseAttackSpeed()
		{
			float buffsValue = base.AllyModel.BuffsHolder.GetBuffsValue(increaseAttackSpeedBuffKeys);
			cooldownTime = base.AllyModel.AttackCooldown - base.AllyModel.AttackCooldown * buffsValue / 100f / 1000f;
			cooldownTime = Mathf.Clamp(cooldownTime, 0.1f, 999f);
		}

		private void ApplyIncreasePhysicsDamage()
		{
			float buffsValue = base.AllyModel.BuffsHolder.GetBuffsValue(increasePhysicsDamageBuffKeys);
			atkPhysicsMin = base.AllyModel.PhysicsDamage_min + (int)buffsValue;
			atkPhysicsMax = base.AllyModel.PhysicsDamage_max + (int)buffsValue;
		}
	}
}

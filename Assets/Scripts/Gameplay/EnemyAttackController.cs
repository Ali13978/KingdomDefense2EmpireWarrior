using Parameter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class EnemyAttackController : EnemyController
	{
		private List<string> reduceAttackDamageByPercentageBuffKeys = new List<string>
		{
			"DecreaseAttackByPercentage"
		};

		private Enemy originalParameter;

		private float cooldownTime;

		private CharacterModel target;

		private CommonAttackDamage commonAttackDamageSender;

		private float damageRatio;

		private float attackRangeAverage;

		private float attackRangeMax;

		[Space]
		[Header("Attack type")]
		public bool meleeAttack = true;

		public bool rangeAttack;

		[SerializeField]
		private int attackFrame = 1;

		[Space]
		[Header("Range Attack Attribute")]
		[SerializeField]
		private float delayTimeToCreateBullet;

		[SerializeField]
		private Transform gunBarrel;

		[SerializeField]
		private GameObject bulletPrefab;

		[SerializeField]
		private string bulletName;

		[Space]
		[Header("Effect")]
		[SerializeField]
		private bool haveEffectOnAttack;

		[SerializeField]
		private GameObject attackEffectPrefab;

		private CastEffectOnDiedTarget castEffectOnDieTarget;

		public float GetCooldownTime()
		{
			return cooldownTime;
		}

		public float GetRangerAtkRange()
		{
			return attackRangeMax;
		}

		public override void OnAppear()
		{
			base.OnAppear();
		}

		public override void Initialize()
		{
			base.Initialize();
			base.EnemyModel.BuffsHolder.OnBuffValueChanged += BuffsHolder_OnBuffValueChanged;
		}

		private void EnemyModel_OnStartRun(int obj)
		{
			SetParameter();
		}

		private void SetParameter()
		{
			originalParameter = base.EnemyModel.OriginalParameter;
			cooldownTime = (float)originalParameter.attack_cooldown / 1000f + Random.Range(0f, 0.5f);
			damageRatio = 1f;
			attackRangeAverage = (float)originalParameter.attack_range_average / GameData.PIXEL_PER_UNIT;
			attackRangeMax = (float)originalParameter.attack_range_max / GameData.PIXEL_PER_UNIT;
		}

		private void GetAllComponents()
		{
			castEffectOnDieTarget = GetComponent<CastEffectOnDiedTarget>();
		}

		private void Awake()
		{
			GetAllComponents();
			base.EnemyModel.enemyAttackController = this;
			base.EnemyModel.OnHitAllyEvent += EnemyModel_OnHitAllyEvent;
			base.EnemyModel.OnStartRun += EnemyModel_OnStartRun;
			if (rangeAttack)
			{
				SingletonMonoBehaviour<SpawnBullet>.Instance.InitExtendBullet(bulletPrefab);
			}
			if (haveEffectOnAttack)
			{
				SingletonMonoBehaviour<SpawnFX>.Instance.InitExtendObject(attackEffectPrefab, 0);
			}
		}

		private void OnDestroy()
		{
			base.EnemyModel.OnHitAllyEvent -= EnemyModel_OnHitAllyEvent;
			base.EnemyModel.OnStartRun -= EnemyModel_OnStartRun;
		}

		private void EnemyModel_OnHitAllyEvent()
		{
			if ((bool)base.EnemyModel.EnemyFindTargetController.Target)
			{
				DamageToAlly(base.EnemyModel.EnemyFindTargetController.Target);
			}
		}

		public void PrepareToMeleeAttack()
		{
			base.EnemyModel.EnemyAnimationController.ToMeleeAttackState();
		}

		private void DamageToAlly(CharacterModel characterModel)
		{
			if ((bool)castEffectOnDieTarget)
			{
				castEffectOnDieTarget.CastEffect(characterModel.transform);
			}
			commonAttackDamageSender = new CommonAttackDamage();
			commonAttackDamageSender.physicsDamage = Random.Range((int)(damageRatio * (float)originalParameter.attack_physics_min / (float)attackFrame), (int)(damageRatio * (float)originalParameter.attack_physics_max / (float)attackFrame));
			commonAttackDamageSender.magicDamage = Random.Range((int)(damageRatio * (float)originalParameter.attack_magic_min / (float)attackFrame), (int)(damageRatio * (float)originalParameter.attack_magic_max / (float)attackFrame));
			commonAttackDamageSender.criticalStrikeChance = 0;
			if (commonAttackDamageSender.criticalStrikeChance > 0 && Random.Range(0, 100) < commonAttackDamageSender.criticalStrikeChance)
			{
				commonAttackDamageSender.physicsDamage *= 2;
				commonAttackDamageSender.magicDamage *= 2;
			}
			characterModel.ProcessDamage(DamageType.Melee, commonAttackDamageSender);
		}

		public void PrepareToRangeAttack()
		{
			base.EnemyModel.EnemyAnimationController.ToRangeAttackState();
			StartCoroutine(CreateBullet());
		}

		private IEnumerator CreateBullet()
		{
			yield return new WaitForSeconds(delayTimeToCreateBullet);
			if ((bool)base.EnemyModel.EnemyFindTargetController.Target)
			{
				EnemyBulletController enemyBulletByName = SingletonMonoBehaviour<SpawnBullet>.Instance.GetEnemyBulletByName(bulletName);
				enemyBulletByName.transform.eulerAngles = gunBarrel.eulerAngles;
				float num = damageRatio;
				Enemy enemy = base.EnemyModel.OriginalParameter;
				int min = (int)(num * (float)enemy.attack_physics_min);
				float num2 = damageRatio;
				Enemy enemy2 = base.EnemyModel.OriginalParameter;
				int physicsDamage = Random.Range(min, (int)(num2 * (float)enemy2.attack_physics_max));
				enemyBulletByName.transform.position = gunBarrel.position;
				enemyBulletByName.gameObject.SetActive(value: true);
				enemyBulletByName.Init(base.EnemyModel.EnemyFindTargetController.Target, physicsDamage);
			}
		}

		private void BuffsHolder_OnBuffValueChanged(string buffKey, bool added)
		{
			if (reduceAttackDamageByPercentageBuffKeys.Contains(buffKey))
			{
				ApplyBuffDecreaseAttackDamage();
			}
		}

		private void ApplyBuffDecreaseAttackDamage()
		{
			float num = base.EnemyModel.BuffsHolder.GetBuffsValue(reduceAttackDamageByPercentageBuffKeys) / 100f;
			damageRatio = 1f - num;
		}
	}
}

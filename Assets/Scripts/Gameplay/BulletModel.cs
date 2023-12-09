using MyCustom;
using Parameter;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class BulletModel : CustomMonoBehaviour
	{
		[SerializeField]
		private DamageToAOERange damageToAOERange;

		public float damageAOERange;

		[SerializeField]
		private DamageToSingleEnemy damageToSingleEnemy;

		[Space]
		[Header("View")]
		[SerializeField]
		private bool isChangeAppearanceWhenCrit;

		private SpriteRenderer spriteRenderer;

		[SerializeField]
		private Sprite critBulletImage;

		[SerializeField]
		private Sprite normalBulletImage;

		[SerializeField]
		private GameObject critBulletFX;

		[Space]
		[Header("FX")]
		[SerializeField]
		private bool hasExplosionAOE;

		[SerializeField]
		private bool hasExplosionInTarget;

		[SerializeField]
		private float explosionDuration;

		public DamageFXType damageFxType;

		[SerializeField]
		private string explosionFXName;

		[SerializeField]
		[HideInInspector]
		public int id = -1;

		[SerializeField]
		[HideInInspector]
		public int level = -1;

		private bool isInitFromTower;

		private bool isInitFromHero;

		[HideInInspector]
		public CommonAttackDamage commonAttackDamage;

		private EffectAttack effectAttack;

		[NonSerialized]
		public int ignoreArmorChance;

		[NonSerialized]
		public EnemyModel target;

		[NonSerialized]
		public Vector2 targetPosition;

		private List<EnemyModel> enemiesInAoeRange = new List<EnemyModel>();

		[NonSerialized]
		public TowerModel towerModel;

		private HeroModel heroModel;

		private Vector3 cachedPosition;

		public Vector3 CachedPosition => cachedPosition;

		public event Action<List<EnemyModel>> onDamageAoe;

		public event Action OnInitialized;

		public void Awake()
		{
			GetAllComponents();
		}

		private void Update()
		{
			UpdateCachedPosition();
		}

		private void GetAllComponents()
		{
			spriteRenderer = GetComponent<SpriteRenderer>();
		}

		public void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.red;
			Gizmos.DrawWireSphere(base.transform.position, commonAttackDamage.aoeRange);
		}

		public void InitFromTower(TowerModel _towerModel, CommonAttackDamage commonAttackDamage, EffectAttack effectAttack, EnemyModel target)
		{
			this.effectAttack = effectAttack;
			InitFromTower(_towerModel, commonAttackDamage, target);
		}

		public void InitFromTower(TowerModel _towerModel, CommonAttackDamage commonAttackDamage, EnemyModel target)
		{
			isInitFromTower = true;
			towerModel = _towerModel;
			this.target = target;
			this.commonAttackDamage = commonAttackDamage;
			this.commonAttackDamage.damageSource = CharacterType.Tower;
			this.commonAttackDamage.sourceId = GameTools.GetTowerSourceId(_towerModel.Level, _towerModel.Id);
			this.commonAttackDamage.isCrit = false;
			if (commonAttackDamage.criticalStrikeChance > 0 && UnityEngine.Random.Range(0, 100) < commonAttackDamage.criticalStrikeChance)
			{
				this.commonAttackDamage.isCrit = true;
				this.commonAttackDamage.physicsDamage *= 2;
				this.commonAttackDamage.magicDamage *= 2;
			}
			this.commonAttackDamage.isIgnoreArmor = false;
			if (commonAttackDamage.ignoreArmorChance > 0 && UnityEngine.Random.Range(0, 100) < commonAttackDamage.ignoreArmorChance)
			{
				this.commonAttackDamage.isIgnoreArmor = true;
			}
			this.commonAttackDamage.isInstantKill = false;
			if (commonAttackDamage.instantKillChance > 0 && UnityEngine.Random.Range(0, 100) < commonAttackDamage.instantKillChance)
			{
				this.commonAttackDamage.isInstantKill = true;
			}
			this.commonAttackDamage.aoeRange = commonAttackDamage.aoeRange;
			InitEvent();
		}

		public void InitCommon(CommonAttackDamage commonAttackDamage, EffectAttack effectAttack, EnemyModel target)
		{
			this.effectAttack = effectAttack;
			InitCommon(commonAttackDamage, target);
		}

		public void InitCommon(CommonAttackDamage commonAttackDamage, EnemyModel target)
		{
			this.target = target;
			this.commonAttackDamage = commonAttackDamage;
			this.commonAttackDamage.isCrit = false;
			if (commonAttackDamage.criticalStrikeChance > 0 && UnityEngine.Random.Range(0, 100) < commonAttackDamage.criticalStrikeChance)
			{
				this.commonAttackDamage.isCrit = true;
				this.commonAttackDamage.physicsDamage *= 2;
				this.commonAttackDamage.magicDamage *= 2;
			}
			this.commonAttackDamage.isIgnoreArmor = false;
			if (commonAttackDamage.ignoreArmorChance > 0 && UnityEngine.Random.Range(0, 100) < commonAttackDamage.ignoreArmorChance)
			{
				this.commonAttackDamage.isIgnoreArmor = true;
			}
			this.commonAttackDamage.isInstantKill = false;
			if (commonAttackDamage.instantKillChance > 0 && UnityEngine.Random.Range(0, 100) < commonAttackDamage.instantKillChance)
			{
				this.commonAttackDamage.isInstantKill = true;
			}
			this.commonAttackDamage.aoeRange = commonAttackDamage.aoeRange;
			InitEvent();
		}

		public void InitFromHero(HeroModel _heroModel, CommonAttackDamage commonAttackDamage, EnemyModel target, EffectAttack effectAttack)
		{
			this.effectAttack = effectAttack;
			InitFromHero(_heroModel, commonAttackDamage, target);
		}

		public void InitFromHero(HeroModel _heroModel, CommonAttackDamage commonAttackDamage, EnemyModel target)
		{
			isInitFromHero = true;
			this.commonAttackDamage = commonAttackDamage;
			this.commonAttackDamage.damageSource = CharacterType.Hero;
			this.commonAttackDamage.sourceId = _heroModel.HeroID;
			if (damageAOERange > 0f)
			{
				this.commonAttackDamage.aoeRange = damageAOERange;
			}
			this.target = target;
			this.commonAttackDamage.isCrit = false;
			if (commonAttackDamage.criticalStrikeChance > 0 && UnityEngine.Random.Range(0, 100) < commonAttackDamage.criticalStrikeChance)
			{
				this.commonAttackDamage.isCrit = true;
				this.commonAttackDamage.physicsDamage *= 2;
				this.commonAttackDamage.magicDamage *= 2;
			}
			this.commonAttackDamage.isIgnoreArmor = false;
			if (commonAttackDamage.ignoreArmorChance > 0 && UnityEngine.Random.Range(0, 100) < commonAttackDamage.ignoreArmorChance)
			{
				this.commonAttackDamage.isIgnoreArmor = true;
			}
			if (isChangeAppearanceWhenCrit)
			{
				setBulletView();
			}
			InitEvent();
		}

		public void InitFromHero(HeroModel _heroModel, CommonAttackDamage commonAttackDamage, Vector2 targetPosition)
		{
			isInitFromHero = true;
			this.commonAttackDamage = commonAttackDamage;
			this.commonAttackDamage.damageSource = CharacterType.Hero;
			this.commonAttackDamage.sourceId = _heroModel.HeroID;
			this.targetPosition = targetPosition;
			this.commonAttackDamage.isCrit = false;
			if (commonAttackDamage.criticalStrikeChance > 0 && UnityEngine.Random.Range(0, 100) < commonAttackDamage.criticalStrikeChance)
			{
				this.commonAttackDamage.isCrit = true;
				this.commonAttackDamage.physicsDamage *= 2;
				this.commonAttackDamage.magicDamage *= 2;
			}
			this.commonAttackDamage.isIgnoreArmor = false;
			if (commonAttackDamage.ignoreArmorChance > 0 && UnityEngine.Random.Range(0, 100) < commonAttackDamage.ignoreArmorChance)
			{
				this.commonAttackDamage.isIgnoreArmor = true;
			}
			if (isChangeAppearanceWhenCrit)
			{
				setBulletView();
			}
			InitEvent();
		}

		private void setBulletView()
		{
			if (commonAttackDamage.isCrit)
			{
				if ((bool)spriteRenderer)
				{
					spriteRenderer.sprite = critBulletImage;
				}
				critBulletFX.SetActive(value: true);
			}
			else
			{
				if ((bool)spriteRenderer)
				{
					spriteRenderer.sprite = normalBulletImage;
				}
				critBulletFX.SetActive(value: false);
			}
		}

		private void InitEvent()
		{
			if (this.OnInitialized != null)
			{
				this.OnInitialized();
			}
		}

		private void UpdateCachedPosition()
		{
			cachedPosition = base.transform.position;
		}

		public void AttackEnemy(EnemyModel enemy)
		{
			if (enemy != null)
			{
				if (commonAttackDamage.aoeRange > 0f)
				{
					DamageWithAOE();
				}
				else
				{
					DamageWithoutAOE(enemy);
				}
			}
		}

		private void DamageWithoutAOE(EnemyModel enemy)
		{
			Enemy originalParameter = enemy.OriginalParameter;
			if (originalParameter.armor_physics <= 0)
			{
				Enemy originalParameter2 = enemy.OriginalParameter;
				if (originalParameter2.armor_magic <= 0)
				{
					if (isInitFromTower)
					{
						towerModel.towerSoundController.PlayHitEnemyWithoutArmor();
					}
					goto IL_0063;
				}
			}
			if (isInitFromTower)
			{
				towerModel.towerSoundController.PlayHitEnemyWithArmor();
			}
			goto IL_0063;
			IL_0063:
			DamageToSingleEnemy(enemy);
		}

		private void DamageToSingleEnemy(EnemyModel enemy)
		{
			damageToSingleEnemy.CastDamage(DamageType.Range, enemy, commonAttackDamage, effectAttack);
			if (hasExplosionInTarget)
			{
				ExplosionInTarget();
			}
		}

		private void DamageWithAOE()
		{
			if (isInitFromTower)
			{
				towerModel.towerSoundController.PlayExplosion();
			}
			if (hasExplosionAOE)
			{
				ExplosionNoTarget();
			}
			if (this.onDamageAoe != null)
			{
				this.onDamageAoe(enemiesInAoeRange);
			}
			damageToAOERange.CastDamage(DamageType.Range, commonAttackDamage);
		}

		private EffectController SpawnExplosion()
		{
			EffectController effectController = null;
			return SingletonMonoBehaviour<SpawnFX>.Instance.GetExplosion(explosionFXName);
		}

		private void ExplosionNoTarget()
		{
			Vector3 position = base.transform.position;
			EffectController effectController = SpawnExplosion();
			effectController.Init(explosionDuration);
			effectController.transform.position = position;
		}

		private void ExplosionInTarget()
		{
			Vector3 zero = Vector3.zero;
			if (target != null && target.gameObject.activeSelf)
			{
				zero = target.transform.position;
				EffectController effectController = SpawnExplosion();
				effectController.Init(explosionDuration, target.transform);
			}
		}

		public void ReturnPool()
		{
			SingletonMonoBehaviour<SpawnBullet>.Instance.Push(this);
		}

		public void Show()
		{
			if ((bool)spriteRenderer)
			{
				spriteRenderer.enabled = true;
			}
		}

		public void Hide()
		{
			if ((bool)spriteRenderer)
			{
				spriteRenderer.enabled = false;
			}
		}

		public void OnMoveToPosition()
		{
			DamageWithAOE();
			ReturnPool();
		}
	}
}

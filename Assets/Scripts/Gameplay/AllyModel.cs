using Parameter;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class AllyModel : CharacterModel
	{
		[Header("Required components")]
		[SerializeField]
		private AllyMovementController allyMovementController;

		[SerializeField]
		private AllyAttackController allyAttackController;

		[SerializeField]
		private AllyHealthController allyHealthController;

		[SerializeField]
		private AllyAnimationController allyAnimationController;

		[SerializeField]
		private UnitSoundController unitSoundController;

		public bool freeAlly;

		public bool controlledAlly;

		public EntityFSMController allyFsmController;

		private string specialStateAnimationName;

		private float specialStateDuration;

		[SerializeField]
		[HideInInspector]
		private List<AllyController> controllers;

		private Vector3 PoolPos = new Vector3(1000f, 1000f, 0f);

		private Collider2D collider2D;

		private SpriteRenderer spriteRenderer;

		[Space]
		[Header("Target")]
		public EnemyModel currentTarget;

		private Tower towerParameter;

		private Hero heroParameter;

		private int id;

		private int level;

		private int health;

		private int physicsArmor;

		private int magicArmor;

		private int physicsDamage_min;

		private int physicsDamage_max;

		private int criticalStrikeChange;

		private float attackCooldown;

		private float attackRangeAverage;

		private float attackRangeMin;

		private float attackRangeMax;

		private float currentAttackRangeMax;

		private float moveSpeed;

		private int dodgeChance;

		private int currentDodgeChance;

		private int ignoreArmorChance;

		private int currentIgnoreArmorChance;

		private TowerSpawnAllyController towerSpawnAllyController;

		public AllyMovementController AllyMovementController
		{
			get
			{
				return allyMovementController;
			}
			private set
			{
				allyMovementController = value;
			}
		}

		public AllyAttackController AllyAttackController
		{
			get
			{
				return allyAttackController;
			}
			private set
			{
				allyAttackController = value;
			}
		}

		public AllyHealthController AllyHealthController
		{
			get
			{
				return allyHealthController;
			}
			private set
			{
				allyHealthController = value;
			}
		}

		public AllyAnimationController AllyAnimationController
		{
			get
			{
				return allyAnimationController;
			}
			set
			{
				allyAnimationController = value;
			}
		}

		public UnitSoundController UnitSoundController
		{
			get
			{
				return unitSoundController;
			}
			set
			{
				unitSoundController = value;
			}
		}

		public int Id
		{
			get
			{
				return id;
			}
			private set
			{
				id = value;
			}
		}

		public int Level
		{
			get
			{
				return level;
			}
			private set
			{
				level = value;
			}
		}

		public int Health
		{
			get
			{
				return health;
			}
			private set
			{
				health = value;
			}
		}

		public int PhysicsArmor
		{
			get
			{
				return physicsArmor;
			}
			private set
			{
				physicsArmor = value;
			}
		}

		public int MagicArmor
		{
			get
			{
				return magicArmor;
			}
			set
			{
				magicArmor = value;
			}
		}

		public int PhysicsDamage_min
		{
			get
			{
				return physicsDamage_min;
			}
			private set
			{
				physicsDamage_min = value;
			}
		}

		public int PhysicsDamage_max
		{
			get
			{
				return physicsDamage_max;
			}
			private set
			{
				physicsDamage_max = value;
			}
		}

		public int CriticalStrikeChange
		{
			get
			{
				return criticalStrikeChange;
			}
			private set
			{
				criticalStrikeChange = value;
			}
		}

		public float AttackCooldown
		{
			get
			{
				return attackCooldown;
			}
			private set
			{
				attackCooldown = value;
			}
		}

		public float AttackRangeAverage
		{
			get
			{
				return attackRangeAverage;
			}
			private set
			{
				attackRangeAverage = value;
			}
		}

		public float AttackRangeMin
		{
			get
			{
				return attackRangeMin;
			}
			private set
			{
				attackRangeMin = value;
			}
		}

		public float AttackRangeMax
		{
			get
			{
				return attackRangeMax;
			}
			set
			{
				attackRangeMax = value;
			}
		}

		public float CurrentAttackRangeMax
		{
			get
			{
				return currentAttackRangeMax;
			}
			set
			{
				currentAttackRangeMax = value;
			}
		}

		public float MoveSpeed
		{
			get
			{
				return moveSpeed;
			}
			private set
			{
				moveSpeed = value;
			}
		}

		public int DodgeChance
		{
			get
			{
				return dodgeChance;
			}
			set
			{
				dodgeChance = value;
			}
		}

		public int CurrentDodgeChance
		{
			get
			{
				return currentDodgeChance;
			}
			set
			{
				currentDodgeChance = value;
			}
		}

		public int IgnoreArmorChance
		{
			get
			{
				return ignoreArmorChance;
			}
			set
			{
				ignoreArmorChance = value;
			}
		}

		public int CurrentIgnoreArmorChance
		{
			get
			{
				return currentIgnoreArmorChance;
			}
			set
			{
				currentIgnoreArmorChance = value;
			}
		}

		public TowerSpawnAllyController TowerSpawnAllyController
		{
			get
			{
				return towerSpawnAllyController;
			}
			set
			{
				towerSpawnAllyController = value;
			}
		}

		public event Action OnHitEnemyEvent;

		private void Awake()
		{
			GetAllComponents();
			GetControllers();
			InitializeControllers();
		}

		private void GetAllComponents()
		{
			collider2D = GetComponent<Collider2D>();
			spriteRenderer = GetComponent<SpriteRenderer>();
		}

		private void TurnOnCollider()
		{
			collider2D.enabled = true;
		}

		private void TurnOffCollider()
		{
			collider2D.enabled = false;
		}

		private void GetControllers()
		{
			if (controllers == null || controllers.Count == 0)
			{
				controllers = new List<AllyController>(GetComponentsInChildren<AllyController>(includeInactive: true));
			}
		}

		private void InitializeControllers()
		{
			for (int i = 0; i < controllers.Count; i++)
			{
				AllyController allyController = controllers[i];
				allyController.AllyModel = this;
				allyController.Initialize();
			}
		}

		public void InitFromTower(Tower _towerParameter, TowerSpawnAllyController _towerSpawnAllyController)
		{
			towerParameter = _towerParameter;
			TowerSpawnAllyController = _towerSpawnAllyController;
			Id = 4000 + GameTools.GetTowerSourceId(towerParameter.level, towerParameter.id);
			level = towerParameter.level;
			Health = towerParameter.unit_health;
			PhysicsArmor = towerParameter.unit_armor_physics;
			MagicArmor = towerParameter.unit_armor_magic;
			PhysicsDamage_min = towerParameter.damage_Physics_min;
			PhysicsDamage_max = towerParameter.damage_Physics_max;
			CriticalStrikeChange = towerParameter.criticalStrikeChance;
			AttackCooldown = (float)towerParameter.unit_attackCooldown / 1000f;
			AttackRangeMin = (float)towerParameter.unit_attack_range_min / GameData.PIXEL_PER_UNIT;
			AttackRangeAverage = (float)towerParameter.unit_attack_range_average / GameData.PIXEL_PER_UNIT;
			AttackRangeMax = (float)towerParameter.unit_attack_range_max / GameData.PIXEL_PER_UNIT;
			CurrentAttackRangeMax = AttackRangeMax;
			moveSpeed = towerParameter.unit_moveSpeed;
			DodgeChance = towerParameter.unit_dodge_chance;
			IgnoreArmorChance = towerParameter.ignoreArmorChance;
			SingletonMonoBehaviour<GameData>.Instance.ListActiveAlly.Add(this);
			TurnOnCollider();
			IsAlive = true;
			for (int i = 0; i < controllers.Count; i++)
			{
				controllers[i].OnAppear();
			}
		}

		public void InitFromHero(Hero _heroParameter, float parameterScale, float lifeTime)
		{
			heroParameter = _heroParameter;
			Id = 2000 + heroParameter.id;
			level = heroParameter.level;
			Health = (int)((float)heroParameter.health * parameterScale);
			PhysicsArmor = (int)((float)heroParameter.armor_physics * parameterScale);
			MagicArmor = (int)((float)heroParameter.armor_magic * parameterScale);
			PhysicsDamage_min = (int)((float)heroParameter.attack_physics_min * parameterScale);
			PhysicsDamage_max = (int)((float)heroParameter.attack_physics_max * parameterScale);
			CriticalStrikeChange = (int)((float)heroParameter.critical_strike_change * parameterScale);
			AttackCooldown = (float)heroParameter.attack_cooldown / 1000f;
			AttackRangeAverage = (float)heroParameter.attack_range_average / GameData.PIXEL_PER_UNIT;
			AttackRangeMin = (float)heroParameter.attack_range_min / GameData.PIXEL_PER_UNIT;
			AttackRangeMax = (float)heroParameter.attack_range_max / GameData.PIXEL_PER_UNIT;
			CurrentAttackRangeMax = AttackRangeMax;
			moveSpeed = heroParameter.speed;
			SingletonMonoBehaviour<GameData>.Instance.ListActiveAlly.Add(this);
			TurnOnCollider();
			IsAlive = true;
			SetAssignedPosition(base.transform.position);
			for (int i = 0; i < controllers.Count; i++)
			{
				controllers[i].OnAppear();
			}
			Color color = spriteRenderer.color;
			spriteRenderer.color = new Color(color.r, color.g, color.b, 1f);
			CustomInvoke(EndOfLifeTime, lifeTime);
		}

		public void OnHitEnemy()
		{
			if (this.OnHitEnemyEvent != null)
			{
				this.OnHitEnemyEvent();
			}
		}

		public override void ProcessDamage(DamageType damageType, CommonAttackDamage commonAttackDamage)
		{
			AllyHealthController.ChangeHealth(damageType, commonAttackDamage);
		}

		public override void RestoreHealth()
		{
			base.RestoreHealth();
			AllyHealthController.RestoreHealth();
		}

		public override void IncreaseHealth(int hpAmount)
		{
			base.IncreaseHealth(hpAmount);
			AllyHealthController.AddHealth(hpAmount);
		}

		public override void Dead()
		{
			unitSoundController.PlayDie();
			TurnOffCollider();
			if (TowerSpawnAllyController != null)
			{
				TowerSpawnAllyController.RemoveAllyFromListControl(this);
			}
			IsAlive = false;
			IsInvisible = false;
		}

		private void EndOfLifeTime()
		{
			Color color = spriteRenderer.color;
			if (IsAlive)
			{
				IsAlive = false;
				allyAnimationController.ToDisappearState();
				ReturnPool(1f);
			}
		}

		private void OnFadeOutComplete()
		{
			TurnOffCollider();
			ReturnPool(0f);
		}

		public override void ReturnPool(float delayTime)
		{
			IsAlive = false;
			IsInvisible = false;
			allyFsmController = null;
			SingletonMonoBehaviour<GameData>.Instance.ListActiveAlly.Remove(this);
			TowerSpawnAllyController = null;
			for (int i = 0; i < controllers.Count; i++)
			{
				controllers[i].OnReturnPool();
			}
			SingletonMonoBehaviour<SpawnAlly>.Instance.Push(this, delayTime);
		}

		public override void AddTarget(EnemyModel enemy)
		{
			currentTarget = enemy;
		}

		public override EnemyModel GetCurrentTarget()
		{
			return currentTarget;
		}

		public override bool CanAttackAirEnemy()
		{
			return false;
		}

		public override float GetRangerRange()
		{
			return CurrentAttackRangeMax;
		}

		public override float GetMeleeRange()
		{
			return AttackRangeAverage;
		}

		public override float GetAttackRangeMin()
		{
			return AttackRangeMin;
		}

		public override int GetCriticalStrikeChance()
		{
			return CriticalStrikeChange;
		}

		public override int GetDodgeChance()
		{
			return CurrentDodgeChance;
		}

		public override int GetIgnoreArmorChance()
		{
			return CurrentIgnoreArmorChance;
		}

		public override float GetSpeed()
		{
			return AllyMovementController.MoveSpeed;
		}

		public override IAnimationController GetAnimationController()
		{
			return allyAnimationController;
		}

		public override void DoMeleeAttack()
		{
			allyAttackController.PrepareToMeleeAttack();
		}

		public override void DoRangeAttack()
		{
			allyAttackController.PrepareToRangeAttack();
		}

		public override float GetAtkCooldownDuration()
		{
			return allyAttackController.Cooldowntime;
		}

		public override Vector3 GetAssignedPosition()
		{
			return allyMovementController.assignedPosition;
		}

		public override void SetAssignedPosition(Vector3 assignedPos)
		{
			allyMovementController.assignedPosition = assignedPos;
		}

		public override float GetDieDuration()
		{
			return 2f;
		}

		public override float GetSpecialStateDuration()
		{
			return specialStateDuration;
		}

		public override void SetSpecialStateDuration(float duration)
		{
			specialStateDuration = duration;
		}

		public override string GetSpecialStateAnimationName()
		{
			return specialStateAnimationName;
		}

		public override void SetSpecialStateAnimationName(string animationName)
		{
			specialStateAnimationName = animationName;
		}

		public override EntityFSMController GetFSMController()
		{
			if (allyFsmController == null)
			{
				allyFsmController = new NewAllyFsmController(this);
			}
			return allyFsmController;
		}

		public override bool IsRanger()
		{
			return allyAttackController.rangeAttack;
		}

		public override int GetCurHp()
		{
			return allyHealthController.CurrentHealth;
		}

		public override int GetMaxHp()
		{
			return allyHealthController.OriginHealth;
		}
	}
}

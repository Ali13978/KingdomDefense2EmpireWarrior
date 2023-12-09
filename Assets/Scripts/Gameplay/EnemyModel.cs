using GeneralVariable;
using Middle;
using MyCustom;
using Parameter;
using SSR.Core.Architecture;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class EnemyModel : CustomMonoBehaviour
	{
		public OrderedEventDispatcher OnAppearEvent;

		public EntityStateEnum curState;

		public Vector3 preMovePos;

		private Enemy enemy;

		[Header("Basic Parameter")]
		[SerializeField]
		private bool canAttack;

		[HideInInspector]
		public int moveLine;

		[HideInInspector]
		public MonsterPathData monsterPathData;

		public float startPosRatio;

		[Header("Required components")]
		[SerializeField]
		private EnemyHealthController healthController;

		[SerializeField]
		private EnemyMovementController movementController;

		[SerializeField]
		private EnemyEffectController effectController;

		[SerializeField]
		private EnemyFindTargetController enemyFindTargetController;

		[SerializeField]
		private EnemyAnimationController enemyAnimationController;

		[SerializeField]
		private EnemySoundController enemySoundController;

		[SerializeField]
		private BuffsHolder buffsHolder;

		public EnemyAttackController enemyAttackController;

		private EnemyFsmController _enemyFsmController;

		private Vector3 cachedPosition;

		private int level;

		private int id;

		private int gate;

		[SerializeField]
		[HideInInspector]
		private List<EnemyController> controllers;

		private Collider2D collider2D;

		private string specialStateAnimationName;

		private float specialStateDuration;

		private int subscriberId;

		private GameObject selectIndicator;

		private float timeTrackingUpdateCachePos = 0.3f;

		private bool updatePhase = true;

		private bool isPositiveForEnemy;

		public bool IsAir
		{
			get;
			set;
		}

		public bool IsAlive
		{
			get;
			set;
		}

		public bool CanAttack
		{
			get
			{
				return canAttack;
			}
			set
			{
				canAttack = value;
			}
		}

		public bool IsUnderground
		{
			get;
			set;
		}

		public bool IsInTunnel
		{
			get;
			set;
		}

		public bool IsAttacking
		{
			get;
			set;
		}

		public bool IsSpecialAttacking
		{
			get;
			set;
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

		public int Gate
		{
			get
			{
				return gate;
			}
			private set
			{
				gate = value;
			}
		}

		public Enemy OriginalParameter
		{
			get
			{
				return enemy;
			}
			private set
			{
				enemy = value;
			}
		}

		public EnemyMovementController EnemyMovementController
		{
			get
			{
				return movementController;
			}
			private set
			{
				movementController = value;
			}
		}

		public EnemyHealthController EnemyHealthController
		{
			get
			{
				return healthController;
			}
			private set
			{
				healthController = value;
			}
		}

		public EnemyEffectController EnemyEffectController
		{
			get
			{
				return effectController;
			}
			private set
			{
				effectController = value;
			}
		}

		public EnemyFindTargetController EnemyFindTargetController
		{
			get
			{
				return enemyFindTargetController;
			}
			private set
			{
				enemyFindTargetController = value;
			}
		}

		public EnemyAnimationController EnemyAnimationController
		{
			get
			{
				return enemyAnimationController;
			}
			set
			{
				enemyAnimationController = value;
			}
		}

		public EnemySoundController EnemySoundController
		{
			get
			{
				return enemySoundController;
			}
			set
			{
				enemySoundController = value;
			}
		}

		public BuffsHolder BuffsHolder
		{
			get
			{
				return buffsHolder;
			}
			private set
			{
				buffsHolder = value;
			}
		}

		public EnemyFsmController enemyFsmController
		{
			get
			{
				if (IsAlive && _enemyFsmController == null)
				{
					_enemyFsmController = new EnemyFsmController(this);
				}
				return _enemyFsmController;
			}
		}

		public Vector3 CachedPosition => cachedPosition;

		public event Action OnHitAllyEvent;

		public event Action OnEnemyDied;

		public event Action<int> OnStartRun;

		public float GetDieDuration()
		{
			return 1f;
		}

		public void Awake()
		{
			GetAllComponents();
			GetControllers();
			InitializeControllers();
			Transform transform = base.gameObject.transform;
			Vector3 position = base.transform.position;
			float x = position.x;
			Vector3 position2 = base.transform.position;
			transform.position = new Vector3(x, position2.y, 0f);
		}

		private void Update()
		{
			if (updatePhase)
			{
				Vector3 a = base.transform.position - CachedPosition;
				Vector3 position = base.transform.position;
				Enemy originalParameter = OriginalParameter;
				preMovePos = position + 5f * ((float)originalParameter.speed * GameData.REVERSE_PIXEL_PER_UNIT) * a;
				if (timeTrackingUpdateCachePos == 0f)
				{
					UpdateCachedPosition();
				}
				timeTrackingUpdateCachePos = Mathf.MoveTowards(timeTrackingUpdateCachePos, 0f, Time.deltaTime * 2f);
				if (IsAlive && !SingletonMonoBehaviour<GameData>.Instance.IsGameOver && enemyFsmController != null)
				{
					enemyFsmController.OnUpdate(Time.deltaTime * 2f);
					if (IsAlive)
					{
						curState = enemyFsmController.GetCurrentState().entityStateEnum;
					}
				}
			}
			updatePhase = !updatePhase;
		}

		private void GetAllComponents()
		{
			collider2D = GetComponent<Collider2D>();
		}

		public EntityFSMController GetFSMController()
		{
			return enemyFsmController;
		}

		private void UpdateCachedPosition()
		{
			cachedPosition = base.transform.position;
			timeTrackingUpdateCachePos = 0.3f;
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
				controllers = new List<EnemyController>(GetComponentsInChildren<EnemyController>(includeInactive: true));
			}
		}

		private void InitializeControllers()
		{
			for (int i = 0; i < controllers.Count; i++)
			{
				EnemyController enemyController = controllers[i];
				enemyController.EnemyModel = this;
				enemyController.Initialize();
			}
		}

		public bool IsValidTarget(CharacterModel hero)
		{
			if (hero == null)
			{
				return false;
			}
			if (!hero.IsAlive)
			{
				return false;
			}
			return true;
		}

		private void SetParameter(int id, int level)
		{
			switch (ModeManager.Instance.gameMode)
			{
			case GameMode.CampaignMode:
				OriginalParameter = EnemyParameterManager.Instance.GetEnemyParameter(id, level);
				break;
			case GameMode.DailyTrialMode:
				OriginalParameter = EnemyParameterManager.Instance.GetEnemyParameter(id, level);
				break;
			case GameMode.TournamentMode:
			{
				int loopAmount = GameplayManager.Instance.endlessModeManager.LoopAmount;
				OriginalParameter = EnemyParameterManager.Instance.GetEnemyParameterForEndlessMode(id, level, loopAmount);
				break;
			}
			}
			IsAlive = true;
			Enemy originalParameter = OriginalParameter;
			IsAir = originalParameter.isAir;
			Enemy originalParameter2 = OriginalParameter;
			IsUnderground = originalParameter2.isUnderGround;
			IsInTunnel = false;
		}

		public void SetDataStartRun(int id, int level, int gate, int line, float startPosRatio = 0f, int customLifeCount = -1)
		{
			Level = level;
			Id = id;
			Gate = gate;
			moveLine = line;
			this.startPosRatio = startPosRatio;
			SetParameter(id, level);
			if (customLifeCount >= 0)
			{
				SingletonMonoBehaviour<GameData>.Instance.TotalEnemy += customLifeCount;
			}
			else
			{
				GameData instance = SingletonMonoBehaviour<GameData>.Instance;
				int totalEnemy = instance.TotalEnemy;
				Enemy originalParameter = OriginalParameter;
				instance.TotalEnemy = totalEnemy + originalParameter.lifeCount;
			}
			TurnOnCollider();
			buffsHolder.ResetBuffs();
			for (int i = 0; i < controllers.Count; i++)
			{
				controllers[i].OnAppear();
			}
			OnAppearEvent.Dispatch();
			if (this.OnStartRun != null)
			{
				this.OnStartRun(line);
			}
			UpdateCachedPosition();
			Enemy originalParameter2 = OriginalParameter;
			if (!originalParameter2.isBoss)
			{
				return;
			}
			UnityEngine.Debug.Log("Animation when boss appear!");
			SingletonMonoBehaviour<CameraController>.Instance.PinchZoomFov.MoveAndZoomToPosition(base.transform.position, 3f);
			SingletonMonoBehaviour<UIRootController>.Instance.uICinematicEffectController.OpenCinematic();
			GameObject gameObject = GameObject.FindGameObjectWithTag(GeneralVariable.GeneralVariable.ANIM_BOSS_IN_MAP);
			if (gameObject != null)
			{
				Enemy originalParameter3 = OriginalParameter;
				if (originalParameter3.id == 7)
				{
					UnityEngine.Object.Destroy(gameObject);
				}
				Enemy originalParameter4 = OriginalParameter;
				if (originalParameter4.id == 16)
				{
					gameObject.GetComponentInChildren<Animator>().SetTrigger("Effect");
					UnityEngine.Object.Destroy(gameObject, 2f);
				}
			}
		}

		public void ProcessEffect(string buffKey, int effectValue, float effectDuration, DamageFXType damageFXtype)
		{
			if (effectValue != 0)
			{
				BuffsHolder.AddBuff(buffKey, new Buff(isPositiveForEnemy, effectValue, effectDuration), BuffStackLogic.ChooseMax, BuffStackLogic.ChooseMax);
				EnemyEffectController.PlayDamageFX(damageFXtype, effectDuration);
			}
		}

		public void OnHitAlly()
		{
			if (this.OnHitAllyEvent != null)
			{
				this.OnHitAllyEvent();
			}
		}

		public void OnSelected()
		{
			subscriberId = GameTools.GetUniqueId();
			GameEventCenter.Instance.Subscribe(GameEventType.OnSelectEnemy, new SelectCharacterSubscriberData(subscriberId, OnSelectEnemyTrigged));
			selectIndicator = TrashMan.spawn(GeneralVariable.GeneralVariable.SELECT_ENEMY_INDICATOR_NAME);
			selectIndicator.GetComponent<FollowerManager>().Init(base.gameObject, Vector3.zero);
		}

		public void OnSelectEnemyTrigged(int enemyId)
		{
			Enemy originalParameter = OriginalParameter;
			if (enemyId != originalParameter.id)
			{
				UnsubscribeSelectEnemyEvent();
			}
		}

		private void UnsubscribeSelectEnemyEvent()
		{
			if (selectIndicator != null)
			{
				TrashMan.despawn(selectIndicator);
				selectIndicator = null;
			}
			if (subscriberId >= 0)
			{
				GameEventCenter.Instance.Unsubscribe(subscriberId, GameEventType.OnSelectEnemy);
				subscriberId = -1;
			}
		}

		public void ProcessDamage(DamageType damageType, CommonAttackDamage commonAttackDamage, EffectAttack effectAttack)
		{
			ProcessDamage(damageType, commonAttackDamage);
			if (UnityEngine.Random.Range(0, 100) < effectAttack.debuffChance)
			{
				ProcessEffect(effectAttack.buffKey, effectAttack.debuffEffectValue, effectAttack.debuffEffectDuration, effectAttack.damageFXType);
			}
		}

		public void ProcessDamage(DamageType damageType, CommonAttackDamage commonAttackDamage)
		{
			if (commonAttackDamage.sourceId == GameTools.blessedHeroId && ModeManager.Instance.gameMode == GameMode.TournamentMode && commonAttackDamage.damageSource == CharacterType.Hero)
			{
				commonAttackDamage.magicDamage *= 2;
				commonAttackDamage.physicsDamage *= 2;
			}
			else if (GameTools.cachedHavingBooster)
			{
				commonAttackDamage.magicDamage = Mathf.RoundToInt((float)commonAttackDamage.magicDamage * GameTools.cachedBoosterMultiplier * 0.82f);
				commonAttackDamage.physicsDamage = Mathf.RoundToInt((float)commonAttackDamage.physicsDamage * GameTools.cachedBoosterMultiplier * 0.82f);
			}
			switch (damageType)
			{
			case DamageType.Melee:
			{
				int num = UnityEngine.Random.Range(0, 100);
				Enemy originalParameter = OriginalParameter;
				if (num < originalParameter.change_avoid_melee)
				{
					EnemyEffectController.PlayFXMiss(0.5f);
					return;
				}
				EnemyHealthController.ChangeHealth(commonAttackDamage);
				break;
			}
			case DamageType.Range:
				EnemyHealthController.ChangeHealth(commonAttackDamage);
				break;
			case DamageType.Magic:
				EnemyHealthController.ChangeHealth(commonAttackDamage);
				break;
			}
			if (commonAttackDamage.isCrit)
			{
				EnemyEffectController.PlayFXCritical();
			}
		}

		public void Dead()
		{
			EnemySoundController.PlayDead();
			UnsubscribeSelectEnemyEvent();
			switch (ModeManager.Instance.gameMode)
			{
			case GameMode.CampaignMode:
			{
				GameData instance2 = SingletonMonoBehaviour<GameData>.Instance;
				Enemy originalParameter2 = OriginalParameter;
				instance2.IncreaseMoney(originalParameter2.value);
				break;
			}
			case GameMode.TournamentMode:
			{
				GameData instance = SingletonMonoBehaviour<GameData>.Instance;
				Enemy originalParameter = OriginalParameter;
				instance.IncreaseMoney(originalParameter.value);
				break;
			}
			}
			GameData instance3 = SingletonMonoBehaviour<GameData>.Instance;
			int totalExp = instance3.TotalExp;
			Enemy originalParameter3 = OriginalParameter;
			instance3.TotalExp = totalExp + originalParameter3.lifeTaken;
			GameplayManager.Instance.OnEnemyDie();
			IsAlive = false;
			IsAttacking = false;
			IsSpecialAttacking = false;
			TurnOffCollider();
			if (this.OnEnemyDied != null)
			{
				this.OnEnemyDied();
			}
			SingletonMonoBehaviour<GameData>.Instance.TotalEnemy--;
		}

		public void ReturnPool(float delayTime)
		{
			IsAlive = false;
			_enemyFsmController = null;
			monsterPathData = null;
			IsAttacking = false;
			IsSpecialAttacking = false;
			UnsubscribeSelectEnemyEvent();
			for (int i = 0; i < controllers.Count; i++)
			{
				controllers[i].OnReturnPool();
			}
			SingletonMonoBehaviour<GameData>.Instance.RemoveEnemyFromListActiveEnemy(this);
			SingletonMonoBehaviour<SpawnEnemy>.Instance.Push(this, delayTime);
		}

		public float GetSpecialStateDuration()
		{
			return specialStateDuration;
		}

		public void SetSpecialStateDuration(float duration)
		{
			specialStateDuration = duration;
		}

		public void SetSpecialStateAnimationName(string animationName)
		{
			specialStateAnimationName = animationName;
		}

		public string GetSpecialStateAnimationName()
		{
			return specialStateAnimationName;
		}
	}
}

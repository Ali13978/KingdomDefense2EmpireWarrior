using Data;
using Parameter;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class HeroModel : CharacterModel
	{
		[Header("General")]
		[SerializeField]
		private int heroID;

		[SerializeField]
		private GameObject selectedImage;

		private GameObject _selectIndicator;

		private float selectedImageScale;

		private Vector3 selectedImageVector = Vector3.zero;

		private Vector3 PoolPos = new Vector3(1000f, 1000f, 0f);

		private Collider2D collider2D;

		[Space]
		[Header("Attack type")]
		public EnemyModel currentTarget;

		private string specialStateAnimationName;

		private float specialStateDuration;

		private EntityFSMController heroFsmController;

		[Header("Required components")]
		[SerializeField]
		private HeroMovementController heroMovementController;

		[SerializeField]
		private HeroAttackController heroAttackController;

		[SerializeField]
		private HeroHealthController heroHealthController;

		[SerializeField]
		private HeroAnimationController heroAnimationController;

		[SerializeField]
		private HeroSkillController heroSkillController;

		[SerializeField]
		private UnitSoundController unitSoundController;

		[SerializeField]
		[HideInInspector]
		private List<HeroController> controllers;

		private Hero originalParameter;

		private PetConfigData petConfigData;

		private HeroModel petOwner;

		[HideInInspector]
		public HeroModel _pet;

		private GameObject selectIndicator
		{
			get
			{
				if (_selectIndicator == null)
				{
					_selectIndicator = (UnityEngine.Object.Instantiate(Resources.Load("FXs/SelectIndicator"), base.transform) as GameObject);
					_selectIndicator.transform.localPosition = Vector3.zero;
				}
				return _selectIndicator;
			}
		}

		public HeroMovementController HeroMovementController
		{
			get
			{
				return heroMovementController;
			}
			private set
			{
				heroMovementController = value;
			}
		}

		public HeroAttackController HeroAttackController
		{
			get
			{
				return heroAttackController;
			}
			private set
			{
				heroAttackController = value;
			}
		}

		public HeroHealthController HeroHealthController
		{
			get
			{
				return heroHealthController;
			}
			private set
			{
				heroHealthController = value;
			}
		}

		public HeroAnimationController HeroAnimationController
		{
			get
			{
				return heroAnimationController;
			}
			private set
			{
				heroAnimationController = value;
			}
		}

		public HeroSkillController HeroSkillController
		{
			get
			{
				return heroSkillController;
			}
			private set
			{
				heroSkillController = value;
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

		public int HeroID => heroID;

		public Hero OriginalParameter
		{
			get
			{
				return originalParameter;
			}
			private set
			{
				originalParameter = value;
			}
		}

		public bool MeleeHero
		{
			get
			{
				Hero hero = OriginalParameter;
				return hero.attack_range_max < 100;
			}
		}

		public bool RangeHero
		{
			get
			{
				Hero hero = OriginalParameter;
				return hero.attack_range_max > 100;
			}
		}

		public bool IsPet => HeroID >= 1000;

		public PetConfigData PetConfigData
		{
			get
			{
				return petConfigData;
			}
			set
			{
				petConfigData = value;
			}
		}

		public HeroModel PetOwner
		{
			get
			{
				return petOwner;
			}
			set
			{
				petOwner = value;
				value._pet = this;
			}
		}

		public event Action OnBeHitEvent;

		public event Action OnSpecialStateEvent;

		public event Action OnHitEnemyEvent;

		public event Action OnAttackEvent;

		public HeroModel GetPet()
		{
			return _pet;
		}

		private void Awake()
		{
			GetAllComponents();
			GetControllers();
			InitializeControllers();
		}

		private void Start()
		{
			OnAppear();
			if (IsPet)
			{
				heroFsmController = new PetFSMController(this);
			}
			else
			{
				heroFsmController = new HeroFsmController(this);
			}
			HeroesManager.Instance.onChooseHero += Instance_onChooseHero;
			HeroesManager.Instance.onUnChooseHero += Instance_onUnChooseHero;
		}

		private void OnDestroy()
		{
			HeroesManager.Instance.onChooseHero -= Instance_onChooseHero;
			HeroesManager.Instance.onUnChooseHero -= Instance_onUnChooseHero;
		}

		private void Instance_onChooseHero(int currentHeroID)
		{
			if (HeroID == currentHeroID)
			{
				ChooseHero();
				UnitSoundController.PlaySelect();
			}
			else
			{
				UnChooseHero();
			}
		}

		private void Instance_onUnChooseHero(int currentHeroID)
		{
			if (HeroID == currentHeroID)
			{
				UnChooseHero();
			}
		}

		private void OnAppear()
		{
			if (!IsPet)
			{
				GameplayManager.Instance.heroesManager.AddToList(HeroID, this);
			}
			ReloadData();
			if (!IsPet || GameTools.IsPetHavingAtkState(this))
			{
				SingletonMonoBehaviour<GameData>.Instance.ListActiveAlly.Add(this);
			}
			base.BuffsHolder.ResetBuffs();
			if (HavePet())
			{
				SummonPet();
			}
		}

		private bool HavePet()
		{
			return !IsPet && GameTools.IsUltimateHero(HeroID);
		}

		private void SummonPet()
		{
			int num = heroID + 1000;
			TrashMan.InitPool("Pet/pet_" + num);
			GameObject gameObject = TrashMan.spawn($"pet_{num}(Clone)");
			HeroModel component = gameObject.GetComponent<HeroModel>();
			component.PetOwner = this;
			component.transform.position = base.transform.position + new Vector3(0f, -0.3f, 0f);
			component.gameObject.SetActive(value: true);
		}

		private void ReloadData()
		{
			SetParameter();
			IsAlive = true;
			TurnOnCollider();
			UnChooseHero();
			foreach (HeroController controller in controllers)
			{
				controller.OnAppear();
			}
			if (!IsPet)
			{
				SingletonMonoBehaviour<GameplayUIHeroManager>.Instance.InitListHeroesSelected(HeroID);
			}
		}

		private void GetAllComponents()
		{
			collider2D = GetComponent<Collider2D>();
		}

		private void GetControllers()
		{
			if (controllers == null || controllers.Count == 0)
			{
				controllers = new List<HeroController>(GetComponentsInChildren<HeroController>(includeInactive: true));
			}
		}

		private void InitializeControllers()
		{
			for (int i = 0; i < controllers.Count; i++)
			{
				HeroController heroController = controllers[i];
				heroController.HeroModel = this;
				heroController.Initialize();
			}
		}

		private void SetParameter()
		{
			if (IsPet)
			{
				PetConfigData = CommonData.Instance.petConfig.dataArray[HeroID % 1000];
				OriginalParameter = HeroParameter.Instance.GetPetParameter(PetConfigData);
			}
			else
			{
				int currentHeroLevel = ReadWriteDataHero.Instance.GetCurrentHeroLevel(HeroID);
				OriginalParameter = HeroParameter.Instance.GetHeroParameter(HeroID, currentHeroLevel);
			}
			if (!IsPet)
			{
				HeroSkillController.InitHeroSkills();
			}
			else
			{
				HeroSkillController.InitPetSkills();
			}
		}

		private void ChooseHero()
		{
			selectedImageScale = heroAttackController.CurrentAttackRangeMax * GameData.PIXEL_PER_UNIT * (GameData.PIXEL_PER_UNIT / 100f) * 0.01f;
			selectedImageVector.Set(selectedImageScale, selectedImageScale, selectedImageScale);
			selectedImage.SetActive(value: true);
			selectedImage.transform.localScale = selectedImageVector;
			selectIndicator.SetActive(value: true);
		}

		private void UnChooseHero()
		{
			selectedImage.SetActive(value: false);
			selectIndicator.SetActive(value: false);
		}

		private void TurnOnCollider()
		{
			if (!IsPet)
			{
				collider2D.enabled = true;
			}
		}

		private void TurnOffCollider()
		{
			if (!IsPet)
			{
				collider2D.enabled = false;
			}
		}

		public override void ProcessDamage(DamageType damageType, CommonAttackDamage commonAttackDamage)
		{
			ChangeHealth(commonAttackDamage.physicsDamage, commonAttackDamage.magicDamage, commonAttackDamage.criticalStrikeChance);
		}

		public override void RestoreHealth()
		{
			base.RestoreHealth();
			if (!IsAlive)
			{
				Resurge();
			}
			else
			{
				HeroHealthController.RestoreHealth();
			}
		}

		public override void IncreaseHealth(int hpAmount)
		{
			base.IncreaseHealth(hpAmount);
			HeroHealthController.AddHealth(hpAmount);
		}

		public override void ChangeHealth(int damagePhysics, int damageMagic, int criticalStrikeChance = 0)
		{
			int num = 0;
			int magicAttackDamage = 0;
			if (damagePhysics > 0)
			{
				num = damagePhysics;
			}
			if (damageMagic > 0)
			{
				magicAttackDamage = damageMagic;
			}
			if (criticalStrikeChance > 0 && UnityEngine.Random.Range(0, 100) < criticalStrikeChance)
			{
				num *= 2;
			}
			HeroHealthController.ChangeHealth(num, magicAttackDamage);
		}

		public override void Dead()
		{
			TurnOffCollider();
			IsAlive = false;
			GetFSMController().GetCurrentState().OnInput(StateInputType.Die);
			IsSpecialState = false;
			foreach (HeroController controller in controllers)
			{
				controller.OnDead();
			}
			SingletonMonoBehaviour<GameplayUIHeroManager>.Instance.DisableHeroesUI(HeroID);
			CustomInvoke(Resurge, (float)originalParameter.respawn_time / 1000f);
			UnitSoundController.PlayDie();
		}

		private void Resurge()
		{
			CancelInvoke("Resurge");
			GetFSMController().GetCurrentState().OnInput(StateInputType.Resurge);
			ReloadData();
			EffectController effect = SingletonMonoBehaviour<SpawnFX>.Instance.GetEffect(SpawnFX.EFFECT_HEAL_0);
			effect.transform.position = base.transform.position;
			effect.Init(1f, base.transform);
			UnitSoundController.PlayRespawn();
			HeroHealthController.UpdateHealthView();
		}

		public override void ReturnPool(float delayTime)
		{
			IsAlive = false;
			IsSpecialState = false;
			SingletonMonoBehaviour<GameData>.Instance.ListActiveAlly.Remove(this);
			for (int i = 0; i < controllers.Count; i++)
			{
				controllers[i].OnReturnPool();
			}
			SingletonMonoBehaviour<SpawnAlly>.Instance.Push(this);
		}

		public void AddExp(int amountEXP)
		{
			ReadWriteDataHero.Instance.AddExp(HeroID, amountEXP);
			UnityEngine.Debug.Log("Hero " + HeroID + " add exp " + amountEXP);
		}

		public void OnHitEnemy()
		{
			if (this.OnHitEnemyEvent != null)
			{
				this.OnHitEnemyEvent();
			}
		}

		public void OnSpecialState()
		{
			if (this.OnSpecialStateEvent != null)
			{
				this.OnSpecialStateEvent();
			}
		}

		public void OnBeHit()
		{
			if (this.OnBeHitEvent != null)
			{
				this.OnBeHitEvent();
			}
		}

		public void OnAttack()
		{
			if (this.OnAttackEvent != null)
			{
				this.OnAttackEvent();
			}
		}

		public override bool IsRanger()
		{
			return RangeHero;
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
			if (IsPet)
			{
				return petConfigData.Can_attack_air > 0;
			}
			return HeroParameter.Instance.CanAttackAir(HeroID);
		}

		public override float GetRangerRange()
		{
			return heroAttackController.CurrentAttackRangeMax;
		}

		public override float GetMeleeRange()
		{
			return heroAttackController.AttackRangeAverage;
		}

		public override float GetAttackRangeMin()
		{
			return heroAttackController.AttackRangeMin;
		}

		public override float GetSpeed()
		{
			return HeroMovementController.GetSpeed();
		}

		public override IAnimationController GetAnimationController()
		{
			return HeroAnimationController;
		}

		public override void DoRangeAttack()
		{
			OnAttack();
			HeroAttackController.PrepareToRangeAttack();
		}

		public override void DoMeleeAttack()
		{
			OnAttack();
			HeroAttackController.PrepareToMeleeAttack();
		}

		public override float GetAtkCooldownDuration()
		{
			return HeroAttackController.CooldownTime;
		}

		public override Vector3 GetAssignedPosition()
		{
			return HeroMovementController.assignedPosition;
		}

		public override void SetAssignedPosition(Vector3 assignedPos)
		{
			HeroMovementController.assignedPosition = assignedPos;
		}

		public override float GetDieDuration()
		{
			return (float)originalParameter.respawn_time / 1000f;
		}

		public override float GetSpecialStateDuration()
		{
			return specialStateDuration;
		}

		public override void SetSpecialStateDuration(float duration)
		{
			specialStateDuration = duration;
		}

		public override void SetSpecialStateAnimationName(string animationName)
		{
			specialStateAnimationName = animationName;
		}

		public override string GetSpecialStateAnimationName()
		{
			return specialStateAnimationName;
		}

		public override EntityFSMController GetFSMController()
		{
			return heroFsmController;
		}

		public override int GetCurHp()
		{
			return HeroHealthController.CurrentHealth;
		}

		public override int GetMaxHp()
		{
			return HeroHealthController.OriginHealth;
		}
	}
}

using Middle;
using MyCustom;
using Parameter;
using SSR.Core.Architecture;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class TowerModel : CustomMonoBehaviour
	{
		public delegate void TowerCommonEventHandler(TowerModel towerModel);

		public delegate void TowerUpgradeHandler(TowerModel oldTower, TowerModel newTower);

		public OrderedEventDispatcher OnAppearEvent;

		[SerializeField]
		private int id;

		[SerializeField]
		private int level;

		[NonSerialized]
		public bool IsSilent;

		[NonSerialized]
		[HideInInspector]
		public int damagePhysics;

		[NonSerialized]
		[HideInInspector]
		public int damageMagic;

		[NonSerialized]
		[HideInInspector]
		public int bonusDmgPercent;

		[NonSerialized]
		[HideInInspector]
		public int bonusCriticalStrikeChange;

		private Tower tower;

		public Transform gun;

		public Transform gunBarrel;

		[SerializeField]
		[HideInInspector]
		private List<TowerController> controllers = new List<TowerController>();

		[HideInInspector]
		public TowerAttackController towerAttackController;

		[HideInInspector]
		public TowerSoundController towerSoundController;

		[HideInInspector]
		public TowerFindEnemyController towerFindEnemyController;

		[HideInInspector]
		public TowerSpawnAllyController towerSpawnAllyController;

		[Header("Required components")]
		[SerializeField]
		private BuffsHolder buffsHolder;

		public TowerUltimateController towerUltimateController;

		private Vector3 cachedPosition;

		private int regionID;

		public CanUpgradeStatus CanUpgradeStatus
		{
			get;
			set;
		}

		internal int finalDamagePhysics_min
		{
			get
			{
				Tower originalParameter = OriginalParameter;
				return (int)((float)originalParameter.damage_Physics_min * (1f + (float)bonusDmgPercent / 100f));
			}
		}

		internal int finalDamagePhysics_max
		{
			get
			{
				Tower originalParameter = OriginalParameter;
				return (int)((float)originalParameter.damage_Physics_max * (1f + (float)bonusDmgPercent / 100f));
			}
		}

		internal int finalDamageMagic_min
		{
			get
			{
				Tower originalParameter = OriginalParameter;
				return (int)((float)originalParameter.damage_Magic_min * (1f + (float)bonusDmgPercent / 100f));
			}
		}

		internal int finalDamageMagic_max
		{
			get
			{
				Tower originalParameter = OriginalParameter;
				return (int)((float)originalParameter.damage_Magic_max * (1f + (float)bonusDmgPercent / 100f));
			}
		}

		internal int finalCriticalStrikeChange
		{
			get
			{
				Tower originalParameter = OriginalParameter;
				return (int)((float)originalParameter.criticalStrikeChance * (1f + (float)bonusCriticalStrikeChange / 100f));
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

		public Tower OriginalParameter
		{
			get
			{
				return tower;
			}
			private set
			{
				tower = value;
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

		public Vector3 CachedPosition => cachedPosition;

		public int RegionID
		{
			get
			{
				return regionID;
			}
			private set
			{
				regionID = value;
			}
		}

		public event TowerCommonEventHandler OnReturnToPool;

		public event TowerUpgradeHandler OnUpgrade;

		public event TowerCommonEventHandler OnBuildFinished;

		public event TowerCommonEventHandler OnSell;

		private void Awake()
		{
			LoadParameters();
			PrototypeInitialize();
		}

		private void Update()
		{
			UpdateCachedPosition();
		}

		private void Initialize()
		{
			foreach (TowerController controller in controllers)
			{
				controller.TowerModel = this;
				controller.Initialize();
			}
		}

		private void LoadParameters()
		{
			OriginalParameter = TowerParameter.Instance.GetTowerParameter(Id, Level);
		}

		public Tower GetTowerParameter()
		{
			return OriginalParameter;
		}

		private void GetControllers()
		{
			controllers = new List<TowerController>(GetComponentsInChildren<TowerController>(includeInactive: true));
		}

		private void GetAllComponents()
		{
			towerAttackController = GetComponentInChildren<TowerAttackController>();
			towerSoundController = GetComponent<TowerSoundController>();
			towerFindEnemyController = GetComponentInChildren<TowerFindEnemyController>();
			towerSpawnAllyController = GetComponentInChildren<TowerSpawnAllyController>();
		}

		private void UpdateCachedPosition()
		{
			cachedPosition = base.transform.position;
		}

		private void PrototypeInitialize()
		{
			GetControllers();
			GetAllComponents();
			Initialize();
		}

		public void Appear()
		{
			foreach (TowerController controller in controllers)
			{
				controller.OnAppear();
			}
			OnAppearEvent.Dispatch();
		}

		public void Upgrade(int ultiNo)
		{
			Tower originalParameter = OriginalParameter;
			if (originalParameter.level + 1 < TowerParameter.Instance.GetNumberOfLevel())
			{
				Tower originalParameter2 = OriginalParameter;
				int num = originalParameter2.level + 1 + ultiNo;
				SpawnTower instance = SingletonMonoBehaviour<SpawnTower>.Instance;
				Tower originalParameter3 = OriginalParameter;
				TowerModel towerModel = instance.Get(originalParameter3.id, num);
				if (this.OnUpgrade != null)
				{
					this.OnUpgrade(this, towerModel);
				}
				towerModel.transform.position = base.transform.position;
				towerModel.gun.eulerAngles = gun.eulerAngles;
				TowerModel towerModel2 = towerModel;
				Tower originalParameter4 = OriginalParameter;
				towerModel2.StartBuild(originalParameter4.id, num, RegionID);
				towerModel.Appear();
				GameData instance2 = SingletonMonoBehaviour<GameData>.Instance;
				Tower originalParameter5 = towerModel.OriginalParameter;
				instance2.DecreaseMoney(originalParameter5.price);
				BuffsHolder.CopyBuff(towerModel.BuffsHolder);
				ReturnPool(isSold: false);
				UnityEngine.Debug.Log("Upgrade Tower Done!");
			}
			else
			{
				UnityEngine.Debug.LogError("Can't upgrade: Max level!");
			}
		}

		public void Sell()
		{
			GameData instance = SingletonMonoBehaviour<GameData>.Instance;
			Tower originalParameter = OriginalParameter;
			instance.IncreaseMoney(originalParameter.value);
			SingletonMonoBehaviour<TowerControlSoundController>.Instance.PlaySell();
			SingletonMonoBehaviour<BuildRegionManager>.Instance.listRegions[RegionID].DisplayBuildable();
			if (this.OnSell != null)
			{
				this.OnSell(this);
			}
			ReturnPool(isSold: true);
		}

		private void OnDisable()
		{
			if (GetComponent<TowerItem>() != null)
			{
				GetComponent<TowerModel>().enabled = false;
				GetComponent<TowerItem>().enabled = true;
			}
		}

		private void UpdateUpgrade(int level)
		{
			int num = -1;
			num = ((level > 2) ? TowerParameter.Instance.GetPrice(id, level) : TowerParameter.Instance.GetPrice(id, level + 1));
			if (num < 0)
			{
				CanUpgradeStatus = CanUpgradeStatus.MaxUpgrade;
			}
			else if (num <= SingletonMonoBehaviour<GameData>.Instance.Money)
			{
				CanUpgradeStatus = CanUpgradeStatus.CanUpgrade;
			}
			else
			{
				CanUpgradeStatus = CanUpgradeStatus.CannotUpgrade;
			}
		}

		private void RunEffectBuild()
		{
			EffectController effect = SingletonMonoBehaviour<SpawnFX>.Instance.GetEffect(SpawnFX.EFFECT_BUILD_TOWER);
			effect.transform.position = SingletonMonoBehaviour<BuildRegionManager>.Instance.listRegions[regionID].transform.position;
			effect.Init(2f);
		}

		public void StartBuild(int id, int level, int regionID)
		{
			RegionID = regionID;
			RunEffectBuild();
			if ((bool)towerSoundController && level > 0)
			{
				SingletonMonoBehaviour<TowerControlSoundController>.Instance.PlayUpgradeNormal(id);
			}
			SingletonMonoBehaviour<GameData>.Instance.AddTowerToActiveList(this);
			BuffsHolder.ResetBuffs();
			OnBuildDone();
		}

		private void OnBuildDone()
		{
			for (int i = 0; i < controllers.Count; i++)
			{
				controllers[i].OnBuildFinished();
			}
			if (this.OnBuildFinished != null)
			{
				this.OnBuildFinished(this);
			}
		}

		public void ChooseTower()
		{
			if (!IsSilent)
			{
				SingletonMonoBehaviour<UIRootController>.Instance.UpgradeTowerPopupController.Init(this, base.transform);
				Config.Instance.currentTowerRegionIDSelected = regionID;
				SingletonMonoBehaviour<GameData>.Instance.RecordingPosition = false;
				SingletonMonoBehaviour<GameData>.Instance.CurrentTowerSelected = this;
			}
		}

		public void UnChooseTower()
		{
			SingletonMonoBehaviour<UIRootController>.Instance.UpgradeTowerPopupController.Close();
			SingletonMonoBehaviour<GameData>.Instance.RecordingPosition = false;
			SingletonMonoBehaviour<GameData>.Instance.CurrentTowerSelected = null;
			GameplayManager.Instance.CurrentTowerRange.GetComponent<TowerRangeController>().HideRange();
		}

		public CanUpgradeStatus GetUpgradeEnable(int level)
		{
			UpdateUpgrade(level);
			return CanUpgradeStatus;
		}

		public void ReturnPool(bool isSold)
		{
			if (isSold)
			{
			}
			SingletonMonoBehaviour<GameData>.Instance.RemoveTowerFromActiveList(this, isSold);
			SpawnTower instance = SingletonMonoBehaviour<SpawnTower>.Instance;
			Tower originalParameter = OriginalParameter;
			int num = originalParameter.id;
			Tower originalParameter2 = OriginalParameter;
			instance.Push(this, num, originalParameter2.level);
			foreach (TowerController controller in controllers)
			{
				controller.OnReturnPool();
			}
			if (this.OnReturnToPool != null)
			{
				this.OnReturnToPool(this);
			}
		}
	}
}

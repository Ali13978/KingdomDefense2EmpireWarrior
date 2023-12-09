using Parameter;
using SSR.Core.Architecture;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class EnemyHealthController : EnemyController
	{
		[SerializeField]
		private OrderedEventDispatcher outOfHealthEvent;

		[Space]
		[SerializeField]
		private OrderedEventDispatcher onIncreaseHealthEvent;

		[Space]
		[SerializeField]
		private OrderedEventDispatcher onDecreaseHealthEvent;

		private List<string> overtimeDamageBuffKeys = new List<string>
		{
			"Burning"
		};

		private List<string> bleededBuffKeys = new List<string>
		{
			"Bleed"
		};

		private List<string> reduceMagicArmorBussKeys = new List<string>
		{
			"ReduceMagicArmor"
		};

		private List<string> defDownBuffKeys = new List<string>
		{
			"DefDown"
		};

		private float timeTrackingOverTimeDamage;

		private int originHealth;

		private int currentHealth;

		private bool isFirstHealthChange;

		private float currentPhysicsArmor;

		private float currentMagicArmor;

		private float originPhysicsArmor;

		private float originMagicArmor;

		private float ignoreDamgeChance;

		private float inputDamageIncrementPercentage;

		private BuffsHolder buffsHolder;

		[Space]
		[Header("Health Bar View")]
		[SerializeField]
		private bool haveHealthBar = true;

		[SerializeField]
		private Transform healthBarPosition;

		private UnitHealthView enemyHealthView;

		public int OriginHealth
		{
			get
			{
				return originHealth;
			}
			private set
			{
				originHealth = value;
			}
		}

		public int CurrentHealth
		{
			get
			{
				return currentHealth;
			}
			private set
			{
				if (currentHealth > 0 && value <= 0)
				{
					GameEventCenter.Instance.Trigger(GameEventType.EventKillMonster, new EventTriggerData(EventTriggerType.KillMonster, base.EnemyModel.Id, 1));
				}
				currentHealth = value;
				if (currentHealth <= 0 && base.EnemyModel.IsAlive)
				{
					outOfHealthEvent.Dispatch();
				}
			}
		}

		public float OriginPhysicsArmor
		{
			get
			{
				return originPhysicsArmor;
			}
			private set
			{
				originPhysicsArmor = value;
			}
		}

		public float CurrentPhysicsArmor
		{
			get
			{
				return currentPhysicsArmor;
			}
			private set
			{
				currentPhysicsArmor = value;
				currentPhysicsArmor = Mathf.Clamp(currentPhysicsArmor, 0f, 0.95f);
			}
		}

		public float OriginMagicArmor
		{
			get
			{
				return originMagicArmor;
			}
			private set
			{
				originMagicArmor = value;
			}
		}

		public float CurrentMagicArmor
		{
			get
			{
				return currentMagicArmor;
			}
			private set
			{
				currentMagicArmor = value;
				currentMagicArmor = Mathf.Clamp(currentMagicArmor, 0f, 0.95f);
			}
		}

		public override void OnAppear()
		{
			base.OnAppear();
			Enemy originalParameter = base.EnemyModel.OriginalParameter;
			OriginHealth = originalParameter.health;
			CurrentHealth = OriginHealth;
			ignoreDamgeChance = 0f;
			Enemy originalParameter2 = base.EnemyModel.OriginalParameter;
			OriginPhysicsArmor = (float)originalParameter2.armor_physics / 100f;
			CurrentPhysicsArmor = OriginPhysicsArmor;
			Enemy originalParameter3 = base.EnemyModel.OriginalParameter;
			OriginMagicArmor = (float)originalParameter3.armor_magic / 100f;
			CurrentMagicArmor = OriginMagicArmor;
		}

		public override void Initialize()
		{
			base.Initialize();
			base.EnemyModel.BuffsHolder.OnBuffValueChanged += BuffsHolder_OnBuffValueChanged;
		}

		public override void OnReturnPool()
		{
			base.OnReturnPool();
			if ((bool)enemyHealthView)
			{
				enemyHealthView.OnReturnPool();
			}
		}

		public void Awake()
		{
			buffsHolder = base.EnemyModel.BuffsHolder;
			base.EnemyModel.OnStartRun += EnemyModel_OnStartRun;
		}

		private void EnemyModel_OnStartRun(int obj)
		{
			if (haveHealthBar)
			{
				SetupHealthBar();
			}
		}

		public void SetupHealthBar()
		{
			enemyHealthView = SingletonMonoBehaviour<SpawnUnitHealthBar>.Instance.Get();
			Enemy originalParameter = base.EnemyModel.OriginalParameter;
			if (originalParameter.isBoss)
			{
				enemyHealthView.SetupHealth(CharacterType.Boss, base.gameObject, healthBarPosition);
			}
			else
			{
				enemyHealthView.SetupHealth(CharacterType.Enemy, base.gameObject, healthBarPosition);
			}
		}

		public void HideHealthBar()
		{
			if (haveHealthBar)
			{
				enemyHealthView.Hide();
			}
		}

		private void OnDestroy()
		{
			base.EnemyModel.OnStartRun -= EnemyModel_OnStartRun;
		}

		public override void Update()
		{
			base.Update();
			if (base.EnemyModel.IsAlive)
			{
				if (timeTrackingOverTimeDamage == 0f)
				{
					UpdateOvertimeDamage();
				}
				timeTrackingOverTimeDamage = Mathf.MoveTowards(timeTrackingOverTimeDamage, 0f, Time.deltaTime);
			}
		}

		public void AddHealth(int amount)
		{
			CurrentHealth += amount;
			if (CurrentHealth >= OriginHealth)
			{
				CurrentHealth = OriginHealth;
			}
			onIncreaseHealthEvent.Dispatch();
			UpdateHealthView();
		}

		public void ChangeHealth(CommonAttackDamage damageInfo)
		{
			if (CurrentHealth <= 0)
			{
				return;
			}
			if (inputDamageIncrementPercentage > 0f)
			{
				damageInfo.physicsDamage = (int)((float)damageInfo.physicsDamage * (1f + inputDamageIncrementPercentage / 100f));
				damageInfo.magicDamage = (int)((float)damageInfo.magicDamage * (1f + inputDamageIncrementPercentage / 100f));
			}
			damageInfo.targetInstanceId = base.EnemyModel.gameObject.GetInstanceID();
			damageInfo.targetEnemyModel = base.EnemyModel;
			if (damageInfo.physicsDamage > 0)
			{
				GameEventCenter.Instance.Trigger(GameEventType.OnBeforeCalculatePhysicsDamage, damageInfo);
				if (CurrentPhysicsArmor > 0f)
				{
					if (!damageInfo.isIgnoreArmor)
					{
						damageInfo.physicsDamage -= (int)((float)damageInfo.physicsDamage * CurrentPhysicsArmor);
					}
					else if (!damageInfo.isNotPlayIgnoreArmorEffect)
					{
						ObjectPool.Spawn(CommonData.Instance.IgnoreDefPrefab, base.EnemyModel.transform.position + new Vector3(0f, 0.2f, 0f));
					}
					damageInfo.physicsDamage = Mathf.Clamp(damageInfo.physicsDamage, 1, 999999);
				}
				if (damageInfo.isInstantKill)
				{
					Enemy originalParameter = base.EnemyModel.OriginalParameter;
					if (!originalParameter.isBoss)
					{
						damageInfo.physicsDamage = CurrentHealth;
					}
				}
				if (damageInfo.physicsDamage > 0)
				{
					GameEventCenter.Instance.Trigger(GameEventType.OnAfterCalculatePhysicsDamage, damageInfo);
					CurrentHealth -= damageInfo.physicsDamage;
					onDecreaseHealthEvent.Dispatch();
					UpdateHealthView();
				}
			}
			if (damageInfo.magicDamage > 0)
			{
				if (CurrentMagicArmor > 0f)
				{
					damageInfo.magicDamage -= (int)((float)damageInfo.magicDamage * CurrentMagicArmor);
					damageInfo.magicDamage = Mathf.Clamp(damageInfo.magicDamage, 1, 999999);
				}
				if (damageInfo.magicDamage > 0)
				{
					GameEventCenter.Instance.Trigger(GameEventType.OnAfterCalculateMagicDamage, damageInfo);
					CurrentHealth -= damageInfo.magicDamage;
					onDecreaseHealthEvent.Dispatch();
					UpdateHealthView();
				}
			}
		}

		private void UpdateHealthView()
		{
			if (haveHealthBar)
			{
				enemyHealthView.UpdateHealth(CurrentHealth, OriginHealth);
			}
		}

		private bool ShouldIgnoreDamage()
		{
			return (float)Random.Range(0, 100) < ignoreDamgeChance;
		}

		private void UpdateOvertimeDamage()
		{
			if (!base.EnemyModel.IsAlive)
			{
				timeTrackingOverTimeDamage = 1f;
				return;
			}
			int num = 0;
			for (int i = 0; i < overtimeDamageBuffKeys.Count; i++)
			{
				if (buffsHolder.TryGetBuffValue(overtimeDamageBuffKeys[i], out float buffValue))
				{
					num = (int)buffValue;
				}
			}
			if (num > 0)
			{
				ChangeHealth(new CommonAttackDamage(num, 0));
			}
			timeTrackingOverTimeDamage = 1f;
		}

		private void BuffsHolder_OnBuffValueChanged(string buffKey, bool added)
		{
			if (bleededBuffKeys.Contains(buffKey))
			{
				ApplyBuffBleeded();
			}
			if (reduceMagicArmorBussKeys.Contains(buffKey))
			{
				ApplyBuffReduceMagicArmor();
			}
			if (defDownBuffKeys.Contains(buffKey))
			{
				ApplyBuffDefdown();
			}
		}

		private void ApplyBuffBleeded()
		{
			inputDamageIncrementPercentage = base.EnemyModel.BuffsHolder.GetBuffsValue(bleededBuffKeys);
		}

		private void ApplyBuffReduceMagicArmor()
		{
			float num = base.EnemyModel.BuffsHolder.GetBuffsValue(reduceMagicArmorBussKeys) / 100f;
			CurrentMagicArmor = OriginMagicArmor - num;
			CurrentMagicArmor = Mathf.Clamp(CurrentMagicArmor, 0f, 1f);
		}

		private void ApplyBuffDefdown()
		{
			float num = base.EnemyModel.BuffsHolder.GetBuffsValue(defDownBuffKeys) / 100f;
			CurrentMagicArmor = OriginMagicArmor - num;
			CurrentMagicArmor = Mathf.Clamp(CurrentMagicArmor, 0f, 1f);
			CurrentPhysicsArmor = OriginPhysicsArmor - num;
			CurrentPhysicsArmor = Mathf.Clamp(CurrentPhysicsArmor, 0f, 1f);
		}
	}
}

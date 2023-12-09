using Parameter;
using SSR.Core.Architecture;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class HeroHealthController : HeroController
	{
		[SerializeField]
		private OrderedEventDispatcher onAppearEvent;

		[SerializeField]
		private OrderedEventDispatcher outOfHealthEvent;

		[SerializeField]
		private OrderedEventDispatcher onHealthChangeEvent;

		private List<string> increaseArmorPhysicsBuffKeys = new List<string>
		{
			"IncreaseArmorPhysics"
		};

		private List<string> increaseArmorMagicBuffKeys = new List<string>
		{
			"IncreaseArmorMagic"
		};

		private List<string> increaseArmorByPercentageBuffKeys = new List<string>
		{
			"BuffDeffenseByPercentage"
		};

		private List<string> buffHpKeys = new List<string>
		{
			"BuffHpByPercentage"
		};

		private int originHealth;

		private int currentHealth;

		private float currentPhysicsArmor;

		private float currentMagicArmor;

		private float originPhysicsArmor;

		private float originMagicArmor;

		private float ignoreDamgeChance;

		private BuffsHolder buffsHolder;

		private float timeTracking;

		private float timeTrackingRegenHealth;

		private int healthRegenValue;

		[Space]
		[Header("Health Bar View")]
		private UnitHealthView heroHealthView;

		[SerializeField]
		private Transform healthBarPosition;

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
				currentHealth = value;
				if (currentHealth <= 0)
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
			set
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
			set
			{
				currentPhysicsArmor = value;
				currentPhysicsArmor = Mathf.Clamp(currentPhysicsArmor, 0f, 0.99f);
			}
		}

		public float OriginMagicArmor
		{
			get
			{
				return originMagicArmor;
			}
			set
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
			set
			{
				currentMagicArmor = value;
				currentMagicArmor = Mathf.Clamp(currentMagicArmor, 0f, 0.99f);
			}
		}

		public override void OnAppear()
		{
			base.OnAppear();
			Hero originalParameter = base.HeroModel.OriginalParameter;
			OriginHealth = originalParameter.health;
			CurrentHealth = OriginHealth;
			Hero originalParameter2 = base.HeroModel.OriginalParameter;
			OriginPhysicsArmor = (float)originalParameter2.armor_physics / 100f;
			Hero originalParameter3 = base.HeroModel.OriginalParameter;
			OriginMagicArmor = (float)originalParameter3.armor_magic / 100f;
			onAppearEvent.Dispatch();
			CurrentPhysicsArmor = OriginPhysicsArmor;
			CurrentMagicArmor = OriginMagicArmor;
			Hero originalParameter4 = base.HeroModel.OriginalParameter;
			healthRegenValue = originalParameter4.health_regen;
			Hero originalParameter5 = base.HeroModel.OriginalParameter;
			timeTracking = (float)originalParameter5.health_regen_cooldown / 1000f;
			timeTrackingRegenHealth = timeTracking;
			ignoreDamgeChance = 0f;
			SetupHealthBar();
			base.HeroModel.BuffsHolder.OnBuffValueChanged += BuffsHolder_OnBuffValueChanged;
		}

		public override void Initialize()
		{
			base.Initialize();
		}

		public override void OnDead()
		{
			base.OnDead();
			heroHealthView.OnReturnPool();
		}

		public override void OnReturnPool()
		{
			base.OnReturnPool();
			heroHealthView.OnReturnPool();
		}

		public new void Update()
		{
			if (base.HeroModel.IsAlive)
			{
				if (timeTrackingRegenHealth == 0f)
				{
					AddHealth(healthRegenValue);
					timeTrackingRegenHealth = timeTracking;
				}
				if (CurrentHealth < OriginHealth)
				{
					timeTrackingRegenHealth = Mathf.MoveTowards(timeTrackingRegenHealth, 0f, Time.deltaTime);
				}
			}
		}

		public void SetupHealthBar()
		{
			heroHealthView = SingletonMonoBehaviour<SpawnUnitHealthBar>.Instance.Get();
			heroHealthView.SetupHealth(CharacterType.Ally, base.gameObject, healthBarPosition);
		}

		public void RestoreHealth()
		{
			currentHealth = OriginHealth;
			UpdateHealthView();
		}

		public void AddHealth(int amount)
		{
			CurrentHealth += amount;
			if (CurrentHealth >= OriginHealth)
			{
				CurrentHealth = OriginHealth;
			}
			UpdateHealthView();
		}

		public void ChangeHealth(int physicsAttackDamage, int magicAttackDamage)
		{
			if (CurrentHealth <= 0)
			{
				return;
			}
			if (physicsAttackDamage > 0)
			{
				int num = physicsAttackDamage;
				if (CurrentPhysicsArmor > 0f)
				{
					num -= (int)((float)physicsAttackDamage * CurrentPhysicsArmor);
					num = Mathf.Clamp(num, 1, 999999);
				}
				if (num > 0)
				{
					CurrentHealth -= num;
					onHealthChangeEvent.Dispatch();
					UpdateHealthView();
				}
			}
			if (magicAttackDamage > 0)
			{
				int num2 = magicAttackDamage;
				if (CurrentMagicArmor > 0f)
				{
					num2 -= (int)((float)magicAttackDamage * CurrentMagicArmor);
					num2 = Mathf.Clamp(num2, 1, 999999);
				}
				if (num2 > 0)
				{
					CurrentHealth -= num2;
					onHealthChangeEvent.Dispatch();
					UpdateHealthView();
				}
			}
			base.HeroModel.OnBeHit();
		}

		public void UpdateHealthView()
		{
			heroHealthView.UpdateHealth(CurrentHealth, OriginHealth);
			GameplayUIHeroManager instance = SingletonMonoBehaviour<GameplayUIHeroManager>.Instance;
			Hero originalParameter = base.HeroModel.OriginalParameter;
			instance.UpdateHeroHealthBarStatus(originalParameter.id, CurrentHealth, OriginHealth);
		}

		private void BuffsHolder_OnBuffValueChanged(string buffKey, bool added)
		{
			if (increaseArmorPhysicsBuffKeys.Contains(buffKey))
			{
				ApplyBuffArmor_Physics();
			}
			if (increaseArmorMagicBuffKeys.Contains(buffKey))
			{
				ApplyBuffArmor_Magic();
			}
			if (increaseArmorByPercentageBuffKeys.Contains(buffKey))
			{
				ApplyBuffArmor_Percentage();
			}
			if (buffHpKeys.Contains(buffKey))
			{
				ApplyBuffHp();
			}
		}

		private void ApplyBuffArmor_Physics()
		{
			float buffsValue = base.HeroModel.BuffsHolder.GetBuffsValue(increaseArmorPhysicsBuffKeys);
			CurrentPhysicsArmor = OriginPhysicsArmor + buffsValue;
		}

		private void ApplyBuffArmor_Magic()
		{
			float buffsValue = base.HeroModel.BuffsHolder.GetBuffsValue(increaseArmorPhysicsBuffKeys);
			CurrentMagicArmor = OriginMagicArmor + buffsValue;
		}

		private void ApplyBuffArmor_Percentage()
		{
			float buffsValue = base.HeroModel.BuffsHolder.GetBuffsValue(increaseArmorByPercentageBuffKeys);
			float num = buffsValue / 100f;
			OriginPhysicsArmor += OriginPhysicsArmor * num;
			CurrentPhysicsArmor = OriginPhysicsArmor;
			OriginMagicArmor += OriginMagicArmor * num;
			CurrentMagicArmor = OriginMagicArmor;
		}

		private void ApplyBuffHp()
		{
			float buffsValue = base.HeroModel.BuffsHolder.GetBuffsValue(buffHpKeys);
			float num = buffsValue / 100f;
			originHealth += Mathf.RoundToInt((float)originHealth * num);
			currentHealth = originHealth;
		}
	}
}

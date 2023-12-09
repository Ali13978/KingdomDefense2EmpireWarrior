using SSR.Core.Architecture;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class AllyHealthController : AllyController
	{
		[SerializeField]
		private OrderedEventDispatcher outOfHealthEvent;

		[Space]
		[SerializeField]
		private OrderedEventDispatcher onHealthChangeEvent;

		private List<string> increaseDodgeRateBuffKeys = new List<string>
		{
			"IncreaseDodgeRate"
		};

		private int originHealth;

		private int currentHealth;

		private bool isFirstHealthChange;

		private float currentPhysicsArmor;

		private float currentMagicArmor;

		private float originPhysicsArmor;

		private float originMagicArmor;

		private int dodgeChance;

		private BuffsHolder buffsHolder;

		[Space]
		[Header("Health Bar View")]
		private UnitHealthView allyHealthView;

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
			set
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
			set
			{
				currentMagicArmor = value;
				currentMagicArmor = Mathf.Clamp(currentMagicArmor, 0f, 0.95f);
			}
		}

		public override void OnAppear()
		{
			base.OnAppear();
			OriginHealth = base.AllyModel.Health;
			CurrentHealth = OriginHealth;
			OriginPhysicsArmor = (float)base.AllyModel.PhysicsArmor / 100f;
			CurrentPhysicsArmor = OriginPhysicsArmor;
			OriginMagicArmor = (float)base.AllyModel.MagicArmor / 100f;
			CurrentMagicArmor = OriginMagicArmor;
			dodgeChance = base.AllyModel.GetDodgeChance();
			SetupHealthBar();
		}

		public override void Initialize()
		{
			base.Initialize();
		}

		public override void OnReturnPool()
		{
			base.OnReturnPool();
			allyHealthView.OnReturnPool();
		}

		public void SetupHealthBar()
		{
			allyHealthView = SingletonMonoBehaviour<SpawnUnitHealthBar>.Instance.Get();
			allyHealthView.SetupHealth(CharacterType.Ally, base.gameObject, healthBarPosition);
		}

		public override void Update()
		{
			base.Update();
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

		public void ChangeHealth(DamageType damageType, CommonAttackDamage damageInfor)
		{
			if (CurrentHealth <= 0 || (damageType == DamageType.Melee && ShouldIgnoreMeleeDamage()))
			{
				return;
			}
			if (damageInfor.physicsDamage > 0)
			{
				int num = damageInfor.physicsDamage;
				if (CurrentPhysicsArmor > 0f)
				{
					num -= (int)((float)damageInfor.physicsDamage * CurrentPhysicsArmor);
					num = Mathf.Clamp(num, 1, 999999);
				}
				if (num > 0)
				{
					CurrentHealth -= num;
					onHealthChangeEvent.Dispatch();
					UpdateHealthView();
				}
			}
			if (damageInfor.magicDamage > 0)
			{
				int num2 = damageInfor.magicDamage;
				if (CurrentMagicArmor > 0f)
				{
					num2 -= (int)((float)damageInfor.magicDamage * CurrentMagicArmor);
					num2 = Mathf.Clamp(num2, 1, 999999);
				}
				if (num2 > 0)
				{
					CurrentHealth -= num2;
					onHealthChangeEvent.Dispatch();
					UpdateHealthView();
				}
			}
		}

		private void UpdateHealthView()
		{
			allyHealthView.UpdateHealth(CurrentHealth, OriginHealth);
		}

		private bool ShouldIgnoreMeleeDamage()
		{
			return dodgeChance > 0 && Random.Range(0, 100) < dodgeChance;
		}

		private void UpdateOvertimeDamage()
		{
		}

		private void BuffsHolder_OnBuffValueChanged(string buffKey, bool added)
		{
			if (increaseDodgeRateBuffKeys.Contains(buffKey))
			{
				ApplyBuffDodge();
			}
		}

		private void ApplyBuffDodge()
		{
			dodgeChance = base.AllyModel.GetDodgeChance() + (int)base.AllyModel.BuffsHolder.GetBuffsValue(increaseDodgeRateBuffKeys);
		}
	}
}

using GeneralVariable;
using MyCustom;
using UnityEngine;

namespace Gameplay
{
	public class HeroSkillAOECommon : CustomMonoBehaviour
	{
		private string buffKey;

		private int slowPercent;

		private float duration;

		[SerializeField]
		private DamageToAOERange damageToAOERange;

		private CommonAttackDamage commonAttackDamageSender;

		private EffectAttack effectAttackSender;

		private bool hasEffect;

		private string effectName;

		private float effectDuration;

		[SerializeField]
		private TimerBombCountdown timerBombCountdown;

		public void Init_DamageOnTrap(CommonAttackDamage _commonAttackDamageSender, EffectAttack _effectAttackSender, float duration, string effectName, float effectDuration)
		{
			commonAttackDamageSender = new CommonAttackDamage();
			commonAttackDamageSender.physicsDamage = _commonAttackDamageSender.physicsDamage;
			commonAttackDamageSender.magicDamage = _commonAttackDamageSender.magicDamage;
			commonAttackDamageSender.aoeRange = _commonAttackDamageSender.aoeRange;
			effectAttackSender.buffKey = _effectAttackSender.buffKey;
			effectAttackSender.debuffChance = _effectAttackSender.debuffChance;
			effectAttackSender.debuffEffectValue = _effectAttackSender.debuffEffectValue;
			effectAttackSender.debuffEffectDuration = _effectAttackSender.debuffEffectDuration;
			effectAttackSender.damageFXType = _effectAttackSender.damageFXType;
			hasEffect = true;
			this.effectName = effectName;
			this.effectDuration = effectDuration;
			CustomInvoke(ReturnPool, duration);
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other.CompareTag(GeneralVariable.GeneralVariable.ENEMY_TAG))
			{
				EnemyModel component = other.GetComponent<EnemyModel>();
				if (!component.IsAir && !component.IsUnderground && !component.IsInTunnel)
				{
					DamageWithAOE();
					CastEffect();
					ReturnPool();
				}
			}
		}

		public void Init_DamgeAfterTime(CommonAttackDamage _commonAttackDamageSender, float countdownTime, string effectName, float effectDuration)
		{
			commonAttackDamageSender = new CommonAttackDamage();
			commonAttackDamageSender.physicsDamage = _commonAttackDamageSender.physicsDamage;
			commonAttackDamageSender.magicDamage = _commonAttackDamageSender.magicDamage;
			commonAttackDamageSender.aoeRange = _commonAttackDamageSender.aoeRange;
			hasEffect = false;
			this.effectName = effectName;
			this.effectDuration = effectDuration;
			if ((bool)timerBombCountdown)
			{
				timerBombCountdown.Init(countdownTime);
			}
			CustomInvoke(DamageWithAOE, countdownTime);
			CustomInvoke(CastEffect, countdownTime);
			CustomInvoke(ReturnPool, countdownTime);
		}

		private void CastEffect()
		{
			if (!string.IsNullOrEmpty(effectName))
			{
				EffectController effect = SingletonMonoBehaviour<SpawnFX>.Instance.GetEffect(effectName);
				effect.transform.position = base.transform.position;
				effect.Init(effectDuration);
			}
		}

		public void Init_DamageImmediately(CommonAttackDamage _commonAttackDamageSender, EffectAttack _effectAttackSender, float effectLifeTime)
		{
			commonAttackDamageSender = new CommonAttackDamage();
			commonAttackDamageSender.physicsDamage = _commonAttackDamageSender.physicsDamage;
			commonAttackDamageSender.magicDamage = _commonAttackDamageSender.magicDamage;
			commonAttackDamageSender.aoeRange = _commonAttackDamageSender.aoeRange;
			effectAttackSender.buffKey = _effectAttackSender.buffKey;
			effectAttackSender.debuffChance = _effectAttackSender.debuffChance;
			effectAttackSender.debuffEffectValue = _effectAttackSender.debuffEffectValue;
			effectAttackSender.debuffEffectDuration = _effectAttackSender.debuffEffectDuration;
			effectAttackSender.damageFXType = _effectAttackSender.damageFXType;
			hasEffect = true;
			DamageWithAOE();
			CustomInvoke(ReturnPool, effectLifeTime);
		}

		private void DamageWithAOE()
		{
			if (hasEffect)
			{
				damageToAOERange.CastDamage(DamageType.Range, commonAttackDamageSender, effectAttackSender);
			}
			else
			{
				damageToAOERange.CastDamage(DamageType.Range, commonAttackDamageSender);
			}
			hasEffect = false;
		}

		private void ReturnPool()
		{
			SingletonMonoBehaviour<SpawnBullet>.Instance.Push(base.gameObject);
		}
	}
}

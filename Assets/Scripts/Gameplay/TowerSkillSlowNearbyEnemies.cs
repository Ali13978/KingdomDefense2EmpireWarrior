using Parameter;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class TowerSkillSlowNearbyEnemies : TowerController
	{
		[SerializeField]
		private int towerID;

		[SerializeField]
		private int towerLevel;

		public GameObject slowEffect;

		private bool skillReady;

		private float skillRange;

		private int chanceToCast;

		private int slowPercent;

		private float slowTime;

		private float cooldownTime;

		private float timeTracking;

		private EffectAttack effectAttack;

		private EffectAttack effectAttackSuper;

		private EffectAttack effectAttackOnBoss;

		private TowerDefaultSkill param;

		private bool showingSlowEffect;

		public override void OnAppear()
		{
			base.OnAppear();
			SetParameter();
		}

		public override void OnReturnPool()
		{
			base.OnReturnPool();
			skillReady = false;
		}

		private void SetParameter()
		{
			param = TowerDefaultSkillParameter.Instance.GetTowerParameter(towerID, towerLevel);
			chanceToCast = param.skillParam0;
			skillRange = (float)param.skillParam1 / GameData.PIXEL_PER_UNIT;
			slowPercent = param.skillParam2;
			slowTime = (float)param.skillParam3 / 1000f;
			cooldownTime = (float)param.skillParam4 / 1000f;
			effectAttack.buffKey = "Slow";
			effectAttack.debuffChance = chanceToCast;
			effectAttack.debuffEffectValue = slowPercent;
			effectAttack.debuffEffectDuration = slowTime;
			effectAttack.damageFXType = DamageFXType.Electric;
			effectAttackSuper.buffKey = "Slow";
			effectAttackSuper.debuffChance = chanceToCast;
			effectAttackSuper.debuffEffectValue = slowPercent + (int)((float)(100 - slowPercent) * 0.65f);
			effectAttackSuper.debuffEffectDuration = slowTime;
			effectAttackSuper.damageFXType = DamageFXType.Electric;
			effectAttackOnBoss.buffKey = "Slow";
			effectAttackOnBoss.debuffChance = chanceToCast;
			effectAttackOnBoss.debuffEffectValue = (int)((float)slowPercent * 0.25f);
			effectAttackOnBoss.debuffEffectDuration = slowTime;
			effectAttackOnBoss.damageFXType = DamageFXType.Electric;
			skillReady = true;
			InitFXs();
		}

		private void InitFXs()
		{
			SingletonMonoBehaviour<SpawnFX>.Instance.InitFX(SpawnFX.EFFECT_ELECTRIC);
		}

		public override void Update()
		{
			base.Update();
			if (SingletonMonoBehaviour<GameData>.Instance.IsGameStart && skillReady)
			{
				if (IsCooldownDone())
				{
					CastSlow();
				}
				timeTracking = Mathf.MoveTowards(timeTracking, 0f, Time.deltaTime);
			}
		}

		public void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(base.transform.position, skillRange);
		}

		private bool IsCooldownDone()
		{
			return timeTracking == 0f;
		}

		private void CastSlow()
		{
			CommonAttackDamage commonAttackDamage = new CommonAttackDamage(0, 0, skillRange);
			List<EnemyModel> listEnemiesInRange = GameTools.GetListEnemiesInRange(base.gameObject, commonAttackDamage);
			for (int num = listEnemiesInRange.Count - 1; num >= 0; num--)
			{
				Enemy originalParameter = listEnemiesInRange[num].OriginalParameter;
				if (originalParameter.isBoss)
				{
					listEnemiesInRange[num].ProcessDamage(DamageType.Range, commonAttackDamage, effectAttackOnBoss);
				}
				else if (listEnemiesInRange[num].Id == 18)
				{
					listEnemiesInRange[num].ProcessDamage(DamageType.Range, commonAttackDamage, effectAttackSuper);
				}
				else
				{
					listEnemiesInRange[num].ProcessDamage(DamageType.Range, commonAttackDamage, effectAttack);
				}
			}
			timeTracking = cooldownTime;
			if (listEnemiesInRange.Count > 0 != showingSlowEffect)
			{
				showingSlowEffect = (listEnemiesInRange.Count > 0);
				slowEffect.SetActive(showingSlowEffect);
			}
		}
	}
}

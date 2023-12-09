using MyCustom;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class Tower2Ultimate1Bullet : CustomMonoBehaviour
	{
		private BulletModel bulletModel;

		private Tower2Ultimate0Skill1 tower2UltimateSkill1;

		private string burningBuffKey = "Burning";

		private Vector3 scaleVector = new Vector3(1.3f, 1.3f, 1.3f);

		public void Awake()
		{
			bulletModel = GetComponent<BulletModel>();
			bulletModel.onDamageAoe += BulletModel_onDamageAoe;
			bulletModel.OnInitialized += BulletModel_OnInitialized;
		}

		private void BulletModel_OnInitialized()
		{
			tower2UltimateSkill1 = bulletModel.towerModel.GetComponentInChildren<Tower2Ultimate0Skill1>();
			if ((bool)tower2UltimateSkill1)
			{
				if (tower2UltimateSkill1.unlock)
				{
					base.gameObject.transform.localScale = scaleVector;
				}
				else
				{
					base.gameObject.transform.localScale = Vector3.one;
				}
			}
		}

		private void BulletModel_onDamageAoe(List<EnemyModel> aoeTargets)
		{
			if ((bool)tower2UltimateSkill1 && tower2UltimateSkill1.unlock)
			{
				if (tower2UltimateSkill1.firstTimeUpgrade)
				{
					AttackInRangeEnemies(aoeTargets);
					tower2UltimateSkill1.firstTimeUpgrade = false;
				}
				else if (tower2UltimateSkill1.ChanceToCastSkill > 0 && Random.Range(0, 100) < tower2UltimateSkill1.ChanceToCastSkill)
				{
					AttackInRangeEnemies(aoeTargets);
				}
			}
		}

		private void AttackInRangeEnemies(List<EnemyModel> aoeTargets)
		{
			foreach (EnemyModel aoeTarget in aoeTargets)
			{
				aoeTarget.BuffsHolder.AddBuff(burningBuffKey, new Buff(isPositive: false, tower2UltimateSkill1.DamageBurn, tower2UltimateSkill1.Duration), BuffStackLogic.ChooseMax, BuffStackLogic.ChooseMax);
				BurningEffect(aoeTarget, tower2UltimateSkill1.Duration);
			}
			BurningGround(tower2UltimateSkill1.Duration);
		}

		private void BurningEffect(EnemyModel enemy, float burningTime)
		{
			enemy.EnemyEffectController.PlayFXBurning(burningTime);
		}

		private void BurningGround(float burningTime)
		{
			EffectController effect = SingletonMonoBehaviour<SpawnFX>.Instance.GetEffect(SpawnFX.METEOR_EXPLOSION);
			effect.transform.position = base.gameObject.transform.position;
			effect.Init(burningTime);
		}

		private void OnDestroy()
		{
			bulletModel.OnInitialized -= BulletModel_OnInitialized;
		}
	}
}

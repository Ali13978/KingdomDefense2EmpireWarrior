using Parameter;
using System;
using System.Collections;
using System.Threading;
using UnityEngine;

namespace Gameplay
{
	public class TowerAttackSingleTargetCommonController : TowerAttackSingleTargetController
	{
		[SerializeField]
		[Range(1f, 5f)]
		private int turnCount = 1;

		[SerializeField]
		private float turnInterval;

		[SerializeField]
		private TowerSkillScaleDamageByRange towerSkillScaleDamageByRange;

		private BulletModel bullet;

		private EffectAttack effectAttack;

		public BulletModel Bullet
		{
			get
			{
				return bullet;
			}
			set
			{
				bullet = value;
			}
		}

		public override event Action<BulletModel, EnemyModel> OnFireBullet;

		public override void OnAppear()
		{
			base.OnAppear();
			SpawnBullet instance = SingletonMonoBehaviour<SpawnBullet>.Instance;
			Tower originalParameter = base.TowerModel.OriginalParameter;
			int id = originalParameter.id;
			Tower originalParameter2 = base.TowerModel.OriginalParameter;
			instance.InitBulletsFromTower(id, originalParameter2.level);
		}

		public override void StopAttack()
		{
		}

		protected override void OnStartAttack()
		{
			StartCoroutine(Fire());
		}

		private IEnumerator Fire()
		{
			for (int i = 0; i < turnCount; i++)
			{
				yield return new WaitForSeconds(turnInterval);
				CreateTurn();
			}
		}

		private void CreateTurn()
		{
			int count = bulletParametersInOneTurn.Count;
			for (int i = 0; i < count; i++)
			{
				StartCoroutine(CreateBullet(bulletParametersInOneTurn[i]));
			}
		}

		private IEnumerator CreateBullet(BulletParameter bulletParameter)
		{
			yield return new WaitForSeconds(bulletParameter.delayTime - 0.2f);
			bulletParameter.OnCreateBullet.Invoke();
			yield return new WaitForSeconds(0.2f);
			Tower originalParameter = base.TowerModel.OriginalParameter;
			Bullet = SingletonMonoBehaviour<SpawnBullet>.Instance.GetForTower(originalParameter.id, originalParameter.level);
			CommonAttackDamage commonAttackDamage = new CommonAttackDamage
			{
				physicsDamage = base.BuffedDamagePhysics,
				magicDamage = base.BuffedDamageMagic,
				instantKillChance = base.BuffedInstantKillChance,
				criticalStrikeChance = originalParameter.criticalStrikeChance,
				ignoreArmorChance = originalParameter.ignoreArmorChance,
				bulletDirection = bulletParameter.bulletDirection,
				aoeRange = (float)originalParameter.bulletAoe / GameData.PIXEL_PER_UNIT,
				maxRange = (float)originalParameter.attackRangeMax / GameData.PIXEL_PER_UNIT
			};
			if ((bool)towerSkillScaleDamageByRange)
			{
				commonAttackDamage.physicsDamage = towerSkillScaleDamageByRange.GetScaledDamage(commonAttackDamage.physicsDamage, commonAttackDamage.maxRange, base.Target);
				commonAttackDamage.magicDamage = towerSkillScaleDamageByRange.GetScaledDamage(commonAttackDamage.magicDamage, commonAttackDamage.maxRange, base.Target);
			}
			effectAttack.buffKey = originalParameter.debuffKey;
			effectAttack.debuffEffectValue = originalParameter.debuffEffectValue;
			effectAttack.debuffEffectDuration = (float)originalParameter.debuffEffectDuration / 1000f;
			effectAttack.debuffChance = originalParameter.debuffChance;
			Bullet.gameObject.SetActive(value: true);
			Bullet.transform.position = bulletParameter.gunBarrel.position;
			Bullet.transform.eulerAngles = base.TowerModel.gun.transform.eulerAngles;
			Bullet.InitFromTower(base.TowerModel, commonAttackDamage, effectAttack, base.Target);
			if (OnFireBullet != null)
			{
				OnFireBullet(Bullet, base.Target);
			}
		}
	}
}

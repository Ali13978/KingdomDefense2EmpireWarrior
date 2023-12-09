using MyCustom;
using System.Collections;
using UnityEngine;

namespace Gameplay
{
	public class WeaponStation : CustomMonoBehaviour
	{
		[SerializeField]
		private Transform gunPos;

		[SerializeField]
		private GameObject bulletPrefab;

		[SerializeField]
		private string bulletName;

		private TowerModel sourceTower;

		private HeroModel sourceHero;

		private bool unlock;

		[Space]
		[SerializeField]
		private WeaponStationAnimator weaponStationAnimator;

		private int physicsDamage;

		private float attackRange;

		private float timeTracking;

		private float cooldownTime;

		private float lifeTime;

		[Space]
		[SerializeField]
		private float appearAnimDuration;

		[SerializeField]
		private float disAppearAnimDuration;

		[SerializeField]
		private float delayTimeCastSkill;

		public void Update()
		{
			if (SingletonMonoBehaviour<GameData>.Instance.IsGameStart && unlock)
			{
				if (IsCooldownDone())
				{
					StartCoroutine(CastFire());
				}
				timeTracking = Mathf.MoveTowards(timeTracking, 0f, Time.deltaTime);
			}
		}

		private bool IsCooldownDone()
		{
			return timeTracking == 0f;
		}

		public void InitFromHero(HeroModel sourceHero, int physicsDamage, float cooldownTime, float attackRange, float lifeTime)
		{
			this.sourceHero = sourceHero;
			this.cooldownTime = cooldownTime;
			this.attackRange = attackRange;
			this.physicsDamage = physicsDamage;
			timeTracking = cooldownTime;
			weaponStationAnimator.Reset();
			InitFXs();
			CustomInvoke(GetReady, appearAnimDuration);
			CustomInvoke(EndOfLifeTime, lifeTime + appearAnimDuration);
		}

		private void GetReady()
		{
			unlock = true;
		}

		public void OnDrawGizmosSelected()
		{
			if (unlock)
			{
				Gizmos.color = Color.yellow;
				Gizmos.DrawWireSphere(base.transform.position, attackRange);
			}
		}

		private void InitFXs()
		{
			SingletonMonoBehaviour<SpawnBullet>.Instance.InitExtendBullet(bulletPrefab);
		}

		private IEnumerator CastFire()
		{
			timeTracking = cooldownTime;
			EnemyModel target = GameTools.GetRandomEnemyInRange(base.gameObject, new CommonAttackDamage(0, 0, attackRange));
			if (GameTools.IsValidEnemy(target))
			{
				weaponStationAnimator.PlayAnimAttack();
				yield return new WaitForSeconds(delayTimeCastSkill);
				BulletModel bullet = SingletonMonoBehaviour<SpawnBullet>.Instance.GetBulletByName(bulletName);
				bullet.transform.position = gunPos.position;
				if (sourceHero != null)
				{
					bullet.InitFromHero(sourceHero, new CommonAttackDamage(physicsDamage, 0), target);
				}
				bullet.gameObject.SetActive(value: true);
			}
		}

		private void EndOfLifeTime()
		{
			unlock = false;
			weaponStationAnimator.PlayAnimDisappear();
			CustomInvoke(ReturnPool, disAppearAnimDuration);
		}

		private void ReturnPool()
		{
			SingletonMonoBehaviour<SpawnTower>.Instance.Push(base.gameObject);
		}
	}
}

using MyCustom;
using System.Collections;
using UnityEngine;

namespace Gameplay
{
	public class MiniDragonController : CustomMonoBehaviour
	{
		[SerializeField]
		private float delayTime;

		[SerializeField]
		private Transform gunPos;

		[SerializeField]
		private string bulletName;

		private Vector3 localScale = Vector3.one;

		private TowerModel towerModel;

		private Animator animator;

		private EnemyModel target;

		private int magicDamage;

		private void Awake()
		{
			animator = GetComponentInChildren<Animator>();
			localScale = base.transform.localScale;
		}

		public void Init(TowerModel towerModel, int damage)
		{
			this.towerModel = towerModel;
			magicDamage = damage;
		}

		public void StartAttack(EnemyModel target)
		{
			this.target = target;
			LookAtTarget();
			StartCoroutine(Attack(target));
		}

		private IEnumerator Attack(EnemyModel target)
		{
			yield return new WaitForSeconds(delayTime);
			PlayAttack();
			BulletModel bullet = SingletonMonoBehaviour<SpawnBullet>.Instance.GetBulletByName(bulletName);
			bullet.transform.position = gunPos.position;
			bullet.gameObject.SetActive(value: true);
			bullet.InitFromTower(towerModel, new CommonAttackDamage(0, magicDamage), target);
		}

		private void PlayAttack()
		{
			animator.Play("Attack");
		}

		public void LookAtTarget()
		{
			localScale.x = 0.5f * (float)(-GetDirection(target.gameObject));
			base.gameObject.transform.localScale = localScale;
		}

		private int GetDirection(GameObject target)
		{
			Vector3 position = target.transform.position;
			float x = position.x;
			Vector3 position2 = base.gameObject.transform.position;
			float num = x - position2.x;
			num = ((!(num > 0f)) ? (-1f) : 1f);
			return (int)num;
		}

		public void ReturnPool()
		{
			SingletonMonoBehaviour<SpawnTower>.Instance.Push(base.gameObject);
		}
	}
}

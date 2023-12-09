using Parameter;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class TowerAttackAnimLineController : TowerController
	{
		[Space]
		[Header("Line shot param")]
		[SerializeField]
		private LazeController lazeController;

		[SerializeField]
		private float lineShotSpeed;

		[SerializeField]
		private float lineShotMaxRange;

		public float bulletRadius = 0.45f;

		public GameObject behitEffectPrefab;

		private Vector3 rootPositionLineShot;

		private Vector3 moveDir;

		private float moveDisLeft;

		private Vector3 endPos;

		private HashSet<int> enemyFlag = new HashSet<int>();

		private float sqBulletRadius;

		private int towerSourceId;

		private int minMagicDam;

		private int maxMagicDam;

		[SerializeField]
		private TowerAttackSingleTargetCommonController towerAttackSingleTargetCommonController;

		private void Init()
		{
			towerSourceId = GameTools.GetTowerSourceId(base.TowerModel.Level, base.TowerModel.Id);
			sqBulletRadius = bulletRadius * bulletRadius;
			Tower originalParameter = base.TowerModel.OriginalParameter;
			minMagicDam = originalParameter.damage_Magic_min;
			Tower originalParameter2 = base.TowerModel.OriginalParameter;
			maxMagicDam = originalParameter2.damage_Magic_max;
			CalculateParameter();
		}

		private void CalculateParameter()
		{
			if (base.TowerModel.towerFindEnemyController.Targets.Count > 0)
			{
				EnemyModel target = GetTarget();
				towerAttackSingleTargetCommonController.Bullet.gameObject.SetActive(value: false);
				rootPositionLineShot = base.TowerModel.transform.position;
				moveDir = (target.transform.position - rootPositionLineShot).normalized;
				endPos = rootPositionLineShot + moveDir * 0.4f;
				moveDisLeft = lineShotMaxRange;
				enemyFlag.Clear();
				lazeController.Init();
				Show();
			}
		}

		private EnemyModel GetTarget()
		{
			List<EnemyModel> listActiveEnemy = SingletonMonoBehaviour<GameData>.Instance.ListActiveEnemy;
			float attackRangeMax = base.TowerModel.towerFindEnemyController.AttackRangeMax;
			float num = attackRangeMax * attackRangeMax;
			for (int num2 = listActiveEnemy.Count - 1; num2 >= 0; num2--)
			{
				if ((listActiveEnemy[num2].transform.position - rootPositionLineShot).sqrMagnitude <= num && listActiveEnemy[num2].Id == 22)
				{
					return listActiveEnemy[num2];
				}
			}
			return base.TowerModel.towerFindEnemyController.Targets[0];
		}

		private void OnLineShotComplete()
		{
			Hide();
		}

		public override void OnAppear()
		{
			base.OnAppear();
			Reset();
		}

		public override void OnReturnPool()
		{
			base.OnReturnPool();
			Reset();
		}

		public override void Update()
		{
			base.Update();
			if (!(moveDisLeft > 0f))
			{
				return;
			}
			float num = Time.deltaTime * lineShotSpeed;
			moveDisLeft -= num;
			endPos += moveDir * num;
			lazeController.Resize(endPos);
			List<EnemyModel> listActiveEnemy = SingletonMonoBehaviour<GameData>.Instance.ListActiveEnemy;
			for (int num2 = listActiveEnemy.Count - 1; num2 >= 0; num2--)
			{
				if (!enemyFlag.Contains(listActiveEnemy[num2].GetInstanceID()) && (listActiveEnemy[num2].transform.position - endPos).sqrMagnitude < sqBulletRadius)
				{
					enemyFlag.Add(listActiveEnemy[num2].GetInstanceID());
					ObjectPool.Spawn(behitEffectPrefab, listActiveEnemy[num2].transform.position);
					listActiveEnemy[num2].ProcessDamage(DamageType.Range, new CommonAttackDamage(CharacterType.Tower, towerSourceId, 0, Random.Range(minMagicDam, maxMagicDam), 0f, base.TowerModel.finalCriticalStrikeChange, 0));
				}
			}
			if (moveDisLeft <= 0f)
			{
				lazeController.StopImmediate();
			}
		}

		private void Awake()
		{
			towerAttackSingleTargetCommonController.OnFireBullet += TowerAttackSingleTargetCommonController_OnFireBullet;
		}

		private void TowerAttackSingleTargetCommonController_OnFireBullet(BulletModel arg1, EnemyModel arg2)
		{
			Init();
		}

		private void OnDestroy()
		{
			towerAttackSingleTargetCommonController.OnFireBullet -= TowerAttackSingleTargetCommonController_OnFireBullet;
		}

		private void Reset()
		{
			Hide();
		}

		private void Show()
		{
			lazeController.gameObject.SetActive(value: true);
		}

		private void Hide()
		{
			lazeController.gameObject.SetActive(value: false);
		}
	}
}

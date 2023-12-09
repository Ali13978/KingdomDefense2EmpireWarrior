using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class LogicEnemySkillExplosionItself : EnemyController
	{
		private Enemy originalParameter;

		private EnemyFindTargetController enemyFindTargetController;

		private EnemyMovementController enemyMovementController;

		private CharacterModel target;

		private float attackRangeAverage;

		private CommonAttackDamage commonAttackDamageSender;

		public override void OnAppear()
		{
			base.OnAppear();
		}

		public override void Initialize()
		{
			base.Initialize();
		}

		private void EnemyModel_OnStartRun(int obj)
		{
			SetParameter();
		}

		private void SetParameter()
		{
			originalParameter = base.EnemyModel.OriginalParameter;
			attackRangeAverage = (float)originalParameter.attack_range_average / GameData.PIXEL_PER_UNIT;
		}

		private void GetAllComponents()
		{
			enemyFindTargetController = base.EnemyModel.EnemyFindTargetController;
			enemyMovementController = base.EnemyModel.EnemyMovementController;
		}

		private void Awake()
		{
			GetAllComponents();
			base.EnemyModel.OnStartRun += EnemyModel_OnStartRun;
		}

		private void OnDestroy()
		{
			base.EnemyModel.OnStartRun -= EnemyModel_OnStartRun;
		}

		public override void Update()
		{
			base.Update();
			if (IsEnemyAlive() && !SingletonMonoBehaviour<GameData>.Instance.IsGameOver && IsAliveAndHaveTarget())
			{
				PrepareToExplode();
			}
		}

		private void PrepareToExplode()
		{
			if (IsCloseToTarget())
			{
				UnityEngine.Debug.Log("Close to explosion!");
				DamageToAlly(enemyFindTargetController.Target);
			}
		}

		private bool IsAliveAndHaveTarget()
		{
			if (!base.EnemyModel.IsAlive)
			{
				return false;
			}
			if (enemyFindTargetController.Target == null)
			{
				return false;
			}
			return true;
		}

		private bool IsCloseToTarget()
		{
			Vector3 position = enemyFindTargetController.Target.transform.position;
			float x = position.x;
			Vector3 position2 = base.gameObject.transform.position;
			float num = Mathf.Abs(x - position2.x);
			Vector3 position3 = enemyFindTargetController.Target.transform.position;
			float y = position3.y;
			Vector3 position4 = base.gameObject.transform.position;
			float num2 = Mathf.Abs(y - position4.y);
			if (num <= attackRangeAverage && num2 < 0.1f)
			{
				return true;
			}
			return false;
		}

		private void DamageToAlly(CharacterModel characterModel)
		{
			commonAttackDamageSender = new CommonAttackDamage();
			commonAttackDamageSender.physicsDamage = Random.Range(originalParameter.attack_physics_min, originalParameter.attack_physics_max);
			commonAttackDamageSender.magicDamage = Random.Range(originalParameter.attack_magic_min, originalParameter.attack_magic_max);
			commonAttackDamageSender.criticalStrikeChance = 0;
			characterModel.ProcessDamage(DamageType.Magic, commonAttackDamageSender);
			base.EnemyModel.EnemyAnimationController.ToDieState();
			base.EnemyModel.Dead();
			base.EnemyModel.ReturnPool(1f);
		}
	}
}

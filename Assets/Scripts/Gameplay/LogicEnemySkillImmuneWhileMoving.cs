using UnityEngine;

namespace Gameplay
{
	public class LogicEnemySkillImmuneWhileMoving : EnemyController
	{
		public GameObject immuneEffectPrefab;

		private bool isMoving;

		private float countdownShowEffect;

		private float countdownIncreaseIgnore;

		private GameObject effect;

		private int subscribeId;

		public override void Initialize()
		{
			base.Initialize();
			subscribeId = GameTools.GetUniqueId();
			GameEventCenter.Instance.Subscribe(GameEventType.OnBeforeCalculatePhysicsDamage, new DamageInfoSubscriberData(subscribeId, OnBeforeCalculateDamage));
			GameEventCenter.Instance.Subscribe(GameEventType.OnAfterCalculatePhysicsDamage, new DamageInfoSubscriberData(subscribeId, OnAfterCalculateDamage));
			GameEventCenter.Instance.Subscribe(GameEventType.OnAfterCalculateMagicDamage, new DamageInfoSubscriberData(subscribeId, OnAfterCalculateDamage));
		}

		public override void Update()
		{
			base.Update();
			isMoving = (base.EnemyModel.curState == EntityStateEnum.EnemyMove);
			countdownShowEffect -= Time.deltaTime;
			countdownIncreaseIgnore -= Time.deltaTime;
			if (countdownShowEffect > 0f)
			{
				effect.transform.position = base.EnemyModel.transform.position + new Vector3(0f, 0.25f, 0f);
			}
		}

		public void OnBeforeCalculateDamage(CommonAttackDamage damageInfo)
		{
			if (damageInfo.targetInstanceId != base.EnemyModel.gameObject.GetInstanceID() || countdownIncreaseIgnore > 0f || damageInfo.damageSource != 0 || damageInfo.sourceId / 1000 != 4)
			{
				return;
			}
			int num = damageInfo.sourceId % 10;
			int num2 = damageInfo.sourceId % 1000 / 10;
			if (num == 1 && num2 == 4)
			{
				countdownIncreaseIgnore = 1f;
				damageInfo.physicsDamage = (int)((float)damageInfo.physicsDamage * 2f);
				if (!damageInfo.isIgnoreArmor && Random.Range(0f, 1f) < 0.75f)
				{
					damageInfo.isIgnoreArmor = true;
				}
			}
		}

		public void OnAfterCalculateDamage(CommonAttackDamage damageInfo)
		{
			if (damageInfo.targetInstanceId != base.EnemyModel.gameObject.GetInstanceID() || !isMoving || damageInfo.damageSource != CharacterType.Tower)
			{
				return;
			}
			int num = damageInfo.sourceId % 10;
			if (num == 3 || num == 0)
			{
				damageInfo.magicDamage = 0;
				damageInfo.physicsDamage = 0;
				if (countdownShowEffect <= 0f)
				{
					countdownShowEffect = 1f;
					effect = ObjectPool.Spawn(immuneEffectPrefab, base.EnemyModel.transform.position + new Vector3(0f, 0.25f, 0f));
				}
			}
		}

		public override void OnReturnPool()
		{
			GameEventCenter.Instance.Unsubscribe(subscribeId, GameEventType.OnAfterCalculateMagicDamage);
			GameEventCenter.Instance.Unsubscribe(subscribeId, GameEventType.OnAfterCalculatePhysicsDamage);
			GameEventCenter.Instance.Unsubscribe(subscribeId, GameEventType.OnBeforeCalculatePhysicsDamage);
			base.OnReturnPool();
		}
	}
}

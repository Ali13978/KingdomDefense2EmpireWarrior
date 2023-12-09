using DG.Tweening;
using Parameter;
using System.Collections;
using UnityEngine;

namespace Gameplay
{
	public class LogicEnemySkill23 : EnemyController
	{
		public float delayAttack;

		public float detectRange;

		public GameObject projPrefab;

		public GameObject explodeEffectPrefab;

		private float countdown;

		private int subscribeId;

		public override void Initialize()
		{
			base.Initialize();
			countdown = 0f;
			subscribeId = GameTools.GetUniqueId();
			GameEventCenter.Instance.Subscribe(GameEventType.OnAfterCalculatePhysicsDamage, new DamageInfoSubscriberData(subscribeId, OnAfterCalculatePhysicsDamage));
		}

		public void OnAfterCalculatePhysicsDamage(CommonAttackDamage damageInfo)
		{
			if (damageInfo.targetInstanceId == base.EnemyModel.gameObject.GetInstanceID() && damageInfo.damageSource == CharacterType.Tower)
			{
				int num = damageInfo.sourceId % 10;
				int num2 = damageInfo.sourceId / 10;
				if (num == 0 && num2 == 4)
				{
					damageInfo.physicsDamage *= 3;
				}
			}
		}

		public override void Update()
		{
			base.Update();
			countdown -= Time.deltaTime;
			if (countdown <= 0f)
			{
				countdown = 0.4f;
				CharacterModel allyWithHighestScore = GameTools.GetAllyWithHighestScore(base.EnemyModel, (CharacterModel characterModel) => true, detectRange, CustomScoreCharacter);
				if (allyWithHighestScore != null && base.EnemyModel.IsAlive)
				{
					countdown = delayAttack;
					StartCoroutine(AttackSequence(allyWithHighestScore));
				}
			}
		}

		public override void OnReturnPool()
		{
			GameEventCenter.Instance.Unsubscribe(subscribeId, GameEventType.OnAfterCalculatePhysicsDamage);
			base.OnReturnPool();
		}

		public float CustomScoreCharacter(CharacterModel hero, EnemyModel enemy)
		{
			if (hero is AllyModel)
			{
				return 7f;
			}
			return 0f;
		}

		public IEnumerator AttackSequence(CharacterModel targetAlly)
		{
			base.EnemyModel.SetSpecialStateDuration(0.55f);
			base.EnemyModel.SetSpecialStateAnimationName(EnemyAnimationController.animSpecialAttack);
			base.EnemyModel.GetFSMController().GetCurrentState().OnInput(StateInputType.SpecialState, EnemyAnimationController.animSpecialAttack);
			base.EnemyModel.EnemyAnimationController.ToSpecialAttackState();
			Transform transform = base.EnemyModel.transform;
			Vector3 position = targetAlly.transform.position;
			float x = position.x;
			Vector3 position2 = base.EnemyModel.transform.position;
			transform.localScale = new Vector3((x > position2.x) ? 1 : (-1), 1f, 1f);
			yield return new WaitForSeconds(0.4f);
			GameObject projectile = ObjectPool.Spawn(projPrefab);
			projectile.transform.position = base.transform.position;
			projectile.transform.right = (targetAlly.transform.position - projectile.transform.position).normalized;
			yield return null;
			Vector3 targetPos = targetAlly.transform.position;
			float shootingDur = 0.15f;
			if (GameTools.IsValidEnemy(base.EnemyModel))
			{
				projectile.transform.DOMove(targetPos, shootingDur);
			}
			yield return new WaitForSeconds(shootingDur);
			projectile.transform.DOKill();
			projectile.Recycle();
			ObjectPool.Spawn(explodeEffectPrefab, targetPos);
			Enemy originalParameter = base.EnemyModel.OriginalParameter;
			int minDamage = originalParameter.attack_magic_min;
			Enemy originalParameter2 = base.EnemyModel.OriginalParameter;
			int maxDamage = originalParameter2.attack_magic_max;
			int magicDamage = Random.Range(minDamage, maxDamage);
			if (GameTools.IsValidCharacter(targetAlly))
			{
				if (targetAlly is HeroModel)
				{
					targetAlly.ProcessDamage(DamageType.Magic, new CommonAttackDamage(0, magicDamage));
					yield break;
				}
				targetAlly.Dead();
				targetAlly.ReturnPool(0f);
			}
		}
	}
}

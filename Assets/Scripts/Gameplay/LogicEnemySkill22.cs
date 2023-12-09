using DG.Tweening;
using Parameter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class LogicEnemySkill22 : EnemyController
	{
		public float delayAttack;

		public float detectRange;

		public float explodeRange;

		private int minDamage;

		private int maxDamage;

		public GameObject bombPrefab;

		public GameObject explodeEffectPrefab;

		private float countdown;

		private int subscribeId;

		public override void Initialize()
		{
			minDamage = 50;
			maxDamage = 65;
			base.Initialize();
			countdown = 0f;
			subscribeId = GameTools.GetUniqueId();
			GameEventCenter.Instance.Subscribe(GameEventType.OnAfterCalculateMagicDamage, new DamageInfoSubscriberData(subscribeId, OnAfterCalculateMagicDamage));
		}

		public void OnAfterCalculateMagicDamage(CommonAttackDamage damageInfo)
		{
			if (damageInfo.targetInstanceId == base.EnemyModel.gameObject.GetInstanceID() && damageInfo.damageSource == CharacterType.Tower)
			{
				int num = damageInfo.sourceId % 10;
				int num2 = damageInfo.sourceId / 10;
				if (num == 3 && num2 == 4)
				{
					damageInfo.magicDamage = (int)((float)damageInfo.magicDamage * 2.1f);
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
			GameEventCenter.Instance.Unsubscribe(subscribeId, GameEventType.OnAfterCalculateMagicDamage);
			base.OnReturnPool();
		}

		public float CustomScoreCharacter(CharacterModel hero, EnemyModel enemy)
		{
			if (hero is AllyModel)
			{
				return 5f;
			}
			return 0f;
		}

		public IEnumerator AttackSequence(CharacterModel targetAlly)
		{
			Enemy originalParameter = base.EnemyModel.OriginalParameter;
			minDamage = originalParameter.attack_physics_min;
			Enemy originalParameter2 = base.EnemyModel.OriginalParameter;
			maxDamage = originalParameter2.attack_physics_max;
			base.EnemyModel.SetSpecialStateDuration(0.9f);
			base.EnemyModel.SetSpecialStateAnimationName(EnemyAnimationController.animSpecialAttack);
			base.EnemyModel.GetFSMController().GetCurrentState().OnInput(StateInputType.SpecialState, EnemyAnimationController.animSpecialAttack);
			base.EnemyModel.EnemyAnimationController.ToSpecialAttackState();
			Transform transform = base.EnemyModel.transform;
			Vector3 position = targetAlly.transform.position;
			float x = position.x;
			Vector3 position2 = base.EnemyModel.transform.position;
			transform.localScale = new Vector3((x > position2.x) ? 1 : (-1), 1f, 1f);
			yield return new WaitForSeconds(0.4f);
			GameObject bomb = ObjectPool.Spawn(bombPrefab);
			bomb.transform.position = base.transform.position;
			yield return null;
			Vector3 targetPos = targetAlly.transform.position;
			float throwBombDur = 0.6f;
			bomb.transform.DOJump(targetPos, 0.9f, 1, throwBombDur);
			yield return new WaitForSeconds(throwBombDur);
			bomb.transform.DOKill();
			bomb.Recycle();
			ObjectPool.Spawn(explodeEffectPrefab, targetPos);
			List<CharacterModel> alliesInRange = GameTools.GetAllyInRange(targetPos, (CharacterModel characterModel) => true, explodeRange);
			for (int num = alliesInRange.Count - 1; num >= 0; num--)
			{
				alliesInRange[num].ProcessDamage(DamageType.Magic, new CommonAttackDamage(Random.Range(minDamage, maxDamage), 0));
			}
		}
	}
}

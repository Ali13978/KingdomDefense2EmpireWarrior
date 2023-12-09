using MyCustom;
using Parameter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class StormController : CustomMonoBehaviour
	{
		[SerializeField]
		private float appearAnimDuration;

		[SerializeField]
		private float disAppearAnimDuration;

		private float aoeRange;

		private float activationTime;

		private float pushBackDistance;

		private int maxEnemyAffected;

		private bool isReady;

		private bool isCastingSkill;

		private float timeTracking;

		private float trackingDuration = 1f;

		private EnemyModel lastTarget;

		private EnemyModel currentTarget;

		[SerializeField]
		private float stormSpeed;

		private List<EnemyModel> pushedbackEnemies = new List<EnemyModel>();

		private HashSet<int> collidedFlag = new HashSet<int>();

		private int pushCount;

		private void Update()
		{
			if (isReady)
			{
				if (timeTracking == 0f)
				{
					TryToCastSkill();
				}
				timeTracking = Mathf.MoveTowards(timeTracking, 0f, Time.deltaTime);
			}
		}

		public void OnDrawGizmosSelected()
		{
			if (isReady)
			{
				Gizmos.color = Color.yellow;
				Gizmos.DrawWireSphere(base.transform.position, aoeRange);
			}
		}

		public void Init(float aoeRange, float activationTime, float pushBackDistance, int maxEnemyAffected)
		{
			this.aoeRange = aoeRange;
			this.activationTime = activationTime;
			this.pushBackDistance = pushBackDistance;
			this.maxEnemyAffected = maxEnemyAffected;
			currentTarget = null;
			CustomInvoke(GetReady, appearAnimDuration);
			CustomInvoke(EndOfLifeTime, activationTime + appearAnimDuration);
		}

		private void TryToCastSkill()
		{
			StartCoroutine(PushbackSequence());
			timeTracking = trackingDuration;
		}

		private IEnumerator PushbackSequence()
		{
			int lineIndex = 0;
			Vector3 offSet = Vector3.zero;
			MonsterPathAnchor monsterAnchor = new MonsterPathAnchor();
			LineManager.Current.FindNearestLine(base.gameObject.transform.position, out lineIndex, out offSet, out monsterAnchor);
			pushedbackEnemies.Clear();
			collidedFlag.Clear();
			pushCount = 0;
			float pushDistance = pushBackDistance;
			while (pushDistance > 0f)
			{
				float move = stormSpeed * Time.deltaTime;
				pushDistance -= move;
				LineManager.Current.MoveMonsterAnchor(monsterAnchor, 0f - move);
				base.gameObject.transform.position = monsterAnchor.pos;
				FindCollidedEnemies(pushDistance / stormSpeed, monsterAnchor.pos);
				for (int num = pushedbackEnemies.Count - 1; num >= 0; num--)
				{
					if (GameTools.IsValidEnemy(pushedbackEnemies[num]))
					{
						LineManager.Current.RequestMove(pushedbackEnemies[num], pushedbackEnemies[num].monsterPathData, 0f - move);
					}
					else
					{
						pushedbackEnemies.RemoveAt(num);
					}
				}
				yield return null;
			}
			if (pushDistance <= 0f)
			{
				ForceEndOfLifeTime();
			}
		}

		private void FindCollidedEnemies(float pushDuration, Vector3 dragonPos)
		{
			if (pushCount >= maxEnemyAffected)
			{
				return;
			}
			List<EnemyModel> listActiveEnemy = SingletonMonoBehaviour<GameData>.Instance.ListActiveEnemy;
			for (int num = listActiveEnemy.Count - 1; num >= 0; num--)
			{
				if (!collidedFlag.Contains(listActiveEnemy[num].GetInstanceID()) && (listActiveEnemy[num].transform.position - dragonPos).sqrMagnitude <= aoeRange && listActiveEnemy[num].curState != EntityStateEnum.EnemySpecialState)
				{
					Enemy originalParameter = listActiveEnemy[num].OriginalParameter;
					if (!originalParameter.isBoss)
					{
						pushedbackEnemies.Add(listActiveEnemy[num]);
						collidedFlag.Add(listActiveEnemy[num].GetInstanceID());
						listActiveEnemy[num].SetSpecialStateDuration(pushDuration);
						listActiveEnemy[num].enemyFsmController.GetCurrentState().OnInput(StateInputType.SpecialState);
						pushCount++;
					}
				}
			}
		}

		private void GetReady()
		{
			isReady = true;
		}

		private void ForceEndOfLifeTime()
		{
			CustomCancelInvoke(EndOfLifeTime);
			EndOfLifeTime();
		}

		private void EndOfLifeTime()
		{
			isReady = false;
			isCastingSkill = false;
			currentTarget = null;
			lastTarget = null;
			CustomInvoke(ReturnPool, disAppearAnimDuration);
		}

		private void ReturnPool()
		{
			SingletonMonoBehaviour<SpawnTower>.Instance.Push(base.gameObject);
		}
	}
}

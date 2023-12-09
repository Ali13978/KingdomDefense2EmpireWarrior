using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class BossW3_TurnAndSummonEnemies_P1 : BossW3BaseState
	{
		public BossW3_TurnAndSummonEnemies_P1(LogicEnemyBossW3 logicEnemy)
			: base(logicEnemy)
		{
			if (GameTools.IsValidEnemy(enemyModel))
			{
				enemyModel.SetSpecialStateDuration(999999f);
				enemyModel.GetFSMController().GetCurrentState().SetTransition(EntityStateEnum.EnemySpecialState);
				logicEnemy.summonRoutineCountdown--;
				logicEnemy.StartCoroutine(TurnAndSummonSequence());
			}
		}

		private IEnumerator TurnAndSummonSequence()
		{
			float castDuration = 0.9f;
			float transformDuration = 0.8f;
			if (Random.Range(0f, 1f) < 0.7f && HaveTowerBarrackAlly())
			{
				logicEnemy.animator.Play(LogicEnemyBossW3.animSpecialAttack);
				yield return new WaitForSeconds(castDuration);
				List<Vector3> spawnPos = new List<Vector3>();
				List<CharacterModel> listAlly = SingletonMonoBehaviour<GameData>.Instance.ListActiveAlly;
				for (int num = listAlly.Count - 1; num >= 0; num--)
				{
					if (listAlly[num] is AllyModel)
					{
						CharacterModel characterModel = listAlly[num];
						Vector3 position = characterModel.transform.position;
						spawnPos.Add(position);
						characterModel.Dead();
						characterModel.ReturnPool(0f);
						ObjectPool.Spawn(logicEnemy.behitTurnAllyPrefab, position);
						ObjectPool.Spawn(logicEnemy.monsterTransformationPrefab, position);
					}
				}
				yield return new WaitForSeconds(transformDuration);
				for (int i = 0; i < spawnPos.Count; i++)
				{
					EnemyModel enemy = GameTools.SummonEnemy(21, 0);
					enemy.transform.position = spawnPos[i];
					enemy.monsterPathData = new MonsterPathData(spawnPos[i], delegate
					{
						GameEventCenter.Instance.Trigger(GameEventType.OnEnemyMoveToEndPoint, enemy);
					});
				}
				yield return new WaitForSeconds(1f);
			}
			MonsterPathAnchor pathAnchor = logicEnemy.GetRandomPosOnRoad(0.45f, 0.65f);
			float chance = Random.Range(0f, 100f);
			int monsterId;
			int monsterQuantity;
			float disBwMonster;
			if (chance < 33f)
			{
				monsterId = 21;
				monsterQuantity = Random.Range(4, 8);
				disBwMonster = 0.55f;
			}
			else if (chance < 66f)
			{
				monsterId = 20;
				monsterQuantity = Random.Range(2, 4);
				disBwMonster = 1.2f;
			}
			else
			{
				monsterId = 18;
				monsterQuantity = Random.Range(6, 12);
				disBwMonster = 0.4f;
			}
			logicEnemy.animator.Play(LogicEnemyBossW3.animSpecialAttack);
			yield return new WaitForSeconds(castDuration);
			for (int j = 0; j < monsterQuantity; j++)
			{
				EnemyModel enemy2 = GameTools.SummonEnemy(monsterId, 0);
				Vector3 pos = pathAnchor.pos;
				ObjectPool.Spawn(logicEnemy.summonEffectPrefab, pos);
				enemy2.transform.position = pos;
				enemy2.monsterPathData = new MonsterPathData(pathAnchor, delegate
				{
					GameEventCenter.Instance.Trigger(GameEventType.OnEnemyMoveToEndPoint, enemy2);
				});
				LineManager.Current.MoveMonsterAnchor(pathAnchor, disBwMonster);
			}
			logicEnemy.curState = new BossW3_MoveCloseToGoal_P2(logicEnemy);
		}

		private bool HaveTowerBarrackAlly()
		{
			List<CharacterModel> listActiveAlly = SingletonMonoBehaviour<GameData>.Instance.ListActiveAlly;
			for (int i = 0; i < listActiveAlly.Count; i++)
			{
				if (listActiveAlly[i] is AllyModel)
				{
					return true;
				}
			}
			return false;
		}
	}
}

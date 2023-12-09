using Parameter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class Tower3Ultimate1Skill0 : TowerUltimateCommon
	{
		private int towerID = 3;

		private int ultimateBranch = 1;

		private int skillID;

		public float dragonSpd = 0.4f;

		public float dragonRadius = 0.65f;

		public GameObject spawnEffect;

		public GameObject dragonPrefab;

		private float sqDragonRadius;

		private TowerModel towerModel;

		private int maxEnemyAffected;

		private float distance;

		private float skillRange;

		private float cooldownTime;

		private float timeTracking;

		private List<EnemyModel> listNearbyEnemies = new List<EnemyModel>();

		private CommonAttackDamage commonAttackDamage;

		[SerializeField]
		private EffectCaster effectCaster;

		private List<EnemyModel> pushedbackEnemies = new List<EnemyModel>();

		private HashSet<int> collidedFlag = new HashSet<int>();

		private int pushCount;

		public override void InitTowerModel(TowerModel towerModel)
		{
			this.towerModel = towerModel;
			sqDragonRadius = dragonRadius * dragonRadius;
		}

		public override void UnlockUltimate(int ultiLevel)
		{
			base.UnlockUltimate(ultiLevel);
			ReadParameter(ultiLevel);
			TryToCastTeleportEnemiesBack();
		}

		public override void OnReturnPool()
		{
			base.OnReturnPool();
		}

		private void Update()
		{
			if (SingletonMonoBehaviour<GameData>.Instance.IsGameStart && unlock)
			{
				if (IsCooldownDone())
				{
					TryToCastTeleportEnemiesBack();
				}
				timeTracking = Mathf.MoveTowards(timeTracking, 0f, Time.deltaTime);
			}
		}

		public void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(base.transform.position, skillRange);
		}

		private bool IsCooldownDone()
		{
			return timeTracking == 0f;
		}

		private void ReadParameter(int currentSkillLevel)
		{
			maxEnemyAffected = TowerSkillParameter.Instance.GetParamBySkillLevel(towerID, ultimateBranch, skillID, currentSkillLevel, 0);
			cooldownTime = TowerSkillParameter.Instance.GetParamBySkillLevel(towerID, ultimateBranch, skillID, currentSkillLevel, 1);
			distance = (float)TowerSkillParameter.Instance.GetParamBySkillLevel(towerID, ultimateBranch, skillID, currentSkillLevel, 2) / GameData.PIXEL_PER_UNIT;
			skillRange = (float)TowerSkillParameter.Instance.GetParamBySkillLevel(towerID, ultimateBranch, skillID, currentSkillLevel, 3) / GameData.PIXEL_PER_UNIT;
			unlock = true;
			commonAttackDamage = new CommonAttackDamage();
			commonAttackDamage.aoeRange = skillRange;
		}

		private void InitFXs()
		{
			SingletonMonoBehaviour<SpawnFX>.Instance.InitFX(SpawnFX.EFFECT_SELL_TOWER_ON_ALLY);
		}

		private void TryToCastTeleportEnemiesBack()
		{
			listNearbyEnemies = GameTools.GetListEnemiesInRange(base.gameObject, commonAttackDamage);
			if (listNearbyEnemies.Count <= 0)
			{
				return;
			}
			float num = float.PositiveInfinity;
			EnemyModel enemyModel = null;
			for (int num2 = listNearbyEnemies.Count - 1; num2 >= 0; num2--)
			{
				if (listNearbyEnemies[num2].curState != EntityStateEnum.EnemySpecialState)
				{
					Enemy originalParameter = listNearbyEnemies[num2].OriginalParameter;
					if (!originalParameter.isBoss)
					{
						float sqrMagnitude = (base.transform.position - listNearbyEnemies[num2].transform.position).sqrMagnitude;
						if (sqrMagnitude < num)
						{
							num = sqrMagnitude;
							enemyModel = listNearbyEnemies[num2];
						}
					}
				}
			}
			if (enemyModel != null)
			{
				StartCoroutine(PushbackSequence(enemyModel));
			}
			timeTracking = cooldownTime;
			towerModel.towerSoundController.PlayCastSkillSound(skillID);
		}

		private IEnumerator PushbackSequence(EnemyModel target)
		{
			MonsterPathAnchor monsterAnchor = new MonsterPathAnchor(target.monsterPathData.secondAnchor);
			ObjectPool.Spawn(spawnEffect, monsterAnchor.pos);
			yield return new WaitForSeconds(0.1f);
			GameObject dragon = ObjectPool.Spawn(dragonPrefab, monsterAnchor.pos);
			pushedbackEnemies.Clear();
			collidedFlag.Clear();
			pushCount = 0;
			float pushDistance = distance;
			while (pushDistance > 0f)
			{
				float move = dragonSpd * Time.deltaTime;
				pushDistance -= move;
				LineManager.Current.MoveMonsterAnchor(monsterAnchor, 0f - move);
				dragon.transform.position = monsterAnchor.pos;
				FindCollidedEnemies(pushDistance / dragonSpd, monsterAnchor.pos);
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
			ObjectPool.Spawn(spawnEffect, monsterAnchor.pos);
			yield return new WaitForSeconds(0.1f);
			dragon.Recycle();
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
				if (!collidedFlag.Contains(listActiveEnemy[num].GetInstanceID()) && (listActiveEnemy[num].transform.position - dragonPos).sqrMagnitude <= sqDragonRadius && listActiveEnemy[num].curState != EntityStateEnum.EnemySpecialState)
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
	}
}

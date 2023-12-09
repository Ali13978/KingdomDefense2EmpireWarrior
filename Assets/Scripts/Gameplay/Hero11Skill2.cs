using Data;
using DG.Tweening;
using Parameter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class Hero11Skill2 : HeroSkillCommon
	{
		public GameObject minionPrefab;

		public GameObject spawnFxPrefab;

		public GameObject explodeFxPrefab;

		public float disTriggerAttack = 1f;

		public float delaySpawnMinion;

		public float jumpDuration = 0.3f;

		public float jumpHeight = 0.3f;

		private int heroID = 11;

		private int skillID = 2;

		private int petId = 1011;

		private int currentLevel;

		private int currentSkillLevel;

		private HeroModel heroModel;

		private Hero11Skill2Param skillParams;

		private float cooldownDuration;

		private float cooldownCountdown;

		private float sqDisTriggerAttack;

		private float explodeRange;

		private float minionLifetime;

		private int maxMinionQuantity;

		private int minionCountdown;

		private List<PhoenixMinionData> minionList = new List<PhoenixMinionData>();

		private float countdownDetectEnemy;

		private bool unlocked;

		public override void Init(HeroModel heroModel)
		{
			base.Init(heroModel);
			unlocked = true;
			this.heroModel = heroModel;
			currentLevel = ReadWriteDataHero.Instance.GetCurrentHeroLevel(heroID);
			currentSkillLevel = HeroParameter.Instance.GetSkillPoint(heroID, currentLevel, skillID);
			skillParams = (HeroSkillParameter.Instance.GetHeroSkillsParameter(heroID, skillID) as HeroSkillParameter_11_2).listParam[currentSkillLevel - 1];
			cooldownDuration = (float)skillParams.cooldown_time * 0.001f;
			cooldownCountdown = cooldownDuration * 0.35f;
			sqDisTriggerAttack = disTriggerAttack * disTriggerAttack;
			explodeRange = (float)skillParams.explode_range / GameData.PIXEL_PER_UNIT;
			minionLifetime = (float)skillParams.minion_lifetime * 0.001f;
			maxMinionQuantity = skillParams.minion_quantity;
			if (GameTools.IsUltimateHero(heroID))
			{
				maxMinionQuantity++;
			}
			minionCountdown = maxMinionQuantity;
			int uniqueId = GameTools.GetUniqueId();
			GameEventCenter.Instance.Subscribe(GameEventType.OnAfterCalculateMagicDamage, new DamageInfoSubscriberData(uniqueId, OnAfterCalculateDamage));
		}

		public void OnAfterCalculateDamage(CommonAttackDamage damageInfo)
		{
			if (!(cooldownCountdown > 0f) && (damageInfo.sourceId == heroModel.HeroID || damageInfo.sourceId == petId) && damageInfo.targetEnemyModel.EnemyHealthController.CurrentHealth <= damageInfo.magicDamage)
			{
				StartCoroutine(SpawnMinion(damageInfo.targetEnemyModel.transform.position));
			}
		}

		private IEnumerator SpawnMinion(Vector3 targetPos)
		{
			minionCountdown--;
			if (minionCountdown <= 0)
			{
				cooldownCountdown = cooldownDuration;
			}
			ObjectPool.Spawn(spawnFxPrefab, targetPos);
			yield return new WaitForSeconds(delaySpawnMinion);
			GameObject minion = ObjectPool.Spawn(minionPrefab, targetPos);
			minion.transform.localScale = new Vector3((Random.Range(0, 100) < 50) ? 1 : (-1), 1f, 1f);
			yield return new WaitForSeconds(2f);
			minionList.Add(new PhoenixMinionData(minion, minionLifetime));
		}

		public override void Update()
		{
			base.Update();
			if (!unlocked)
			{
				return;
			}
			cooldownCountdown -= Time.deltaTime;
			for (int num = minionList.Count - 1; num >= 0; num--)
			{
				if (!minionList[num].isAttacking)
				{
					minionList[num].lifetimeCountdown -= Time.deltaTime;
					if (minionList[num].lifetimeCountdown <= 0f)
					{
						minionList[num].isDestroyed = true;
					}
				}
				if (minionList[num].isDestroyed)
				{
					ObjectPool.Spawn(explodeFxPrefab, minionList[num].minion.transform.position);
					List<EnemyModel> listEnemiesInRange = GameTools.GetListEnemiesInRange(minionList[num].minion.transform.position, new CommonAttackDamage(0, 0, explodeRange));
					for (int j = 0; j < listEnemiesInRange.Count; j++)
					{
						listEnemiesInRange[j].ProcessDamage(DamageType.Magic, new CommonAttackDamage(0, skillParams.magic_damage));
					}
					minionList[num].minion.Recycle();
					minionList.RemoveAt(num);
				}
			}
			countdownDetectEnemy -= Time.deltaTime;
			if (!(countdownDetectEnemy <= 0f))
			{
				return;
			}
			countdownDetectEnemy = 0.5f;
			if (minionList.Count <= 0)
			{
				return;
			}
			List<EnemyModel> listActiveEnemy = SingletonMonoBehaviour<GameData>.Instance.ListActiveEnemy;
			for (int num2 = listActiveEnemy.Count - 1; num2 >= 0; num2--)
			{
				if (GameTools.IsValidEnemy(listActiveEnemy[num2]) && !listActiveEnemy[num2].IsInTunnel && !listActiveEnemy[num2].IsUnderground)
				{
					for (int num3 = minionList.Count - 1; num3 >= 0; num3--)
					{
						if (!minionList[num3].isAttacking && SingletonMonoBehaviour<GameData>.Instance.SqrDistance(minionList[num3].minion.transform.position, listActiveEnemy[num2].transform.position) < sqDisTriggerAttack)
						{
							PhoenixMinionData i = minionList[num3];
							i.isAttacking = true;
							Transform transform = i.minion.transform;
							Vector3 position = listActiveEnemy[num2].transform.position;
							float x = position.x;
							Vector3 position2 = minionList[num3].minion.transform.position;
							transform.localScale = new Vector3((x > position2.x) ? 1 : (-1), 1f, 1f);
							float magnitude = (listActiveEnemy[num2].transform.position - minionList[num3].minion.transform.position).magnitude;
							float duration = Mathf.Max(jumpDuration * magnitude / disTriggerAttack, 0.4f) + 0.1f;
							i.PlayJump();
							i.minion.transform.DOJump(listActiveEnemy[num2].transform.position, jumpHeight, 1, duration).OnComplete(delegate
							{
								i.isDestroyed = true;
							});
							break;
						}
					}
				}
			}
		}
	}
}

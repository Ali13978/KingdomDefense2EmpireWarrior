using SSR;
using SSR.Core.Architecture;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class LogicEnemySkillSealTowers : EnemyController
	{
		[SerializeField]
		private float coolDownTime;

		[SerializeField]
		private int maxTowers;

		[SerializeField]
		private float effectTime;

		[SerializeField]
		private float rangeInPixel;

		private string silentBuffKey = "Silent";

		[Header("Common setting")]
		[SerializeField]
		private bool activateAtStart;

		[SerializeField]
		private float attackAnimationDuration = 1f;

		[SerializeField]
		private float offsetTimeAfterAnim;

		[SerializeField]
		private float minSpeed = 0.05f;

		[SerializeField]
		private GameObject sealEffectPrefab;

		[SerializeField]
		private string sealEffectName;

		[Space]
		[SerializeField]
		private OrderedEventDispatcher onStartAttack = new OrderedEventDispatcher();

		[SerializeField]
		private OrderedEventDispatcher onFinishAttack = new OrderedEventDispatcher();

		[SerializeField]
		private OrderedEventDispatcher onCancelAttack = new OrderedEventDispatcher();

		private float timeTracking;

		private bool attacking;

		private List<TowerModel> inRangeTowers = new List<TowerModel>();

		private List<TowerModel> nearestTowers = new List<TowerModel>();

		private List<float> towersSquaredDistances = new List<float>();

		private WaitForSeconds waitForAnimation;

		public override void OnAppear()
		{
			base.OnAppear();
			timeTracking = ((!activateAtStart) ? (coolDownTime / 1000f) : 0f);
		}

		private void Start()
		{
			waitForAnimation = new WaitForSeconds(attackAnimationDuration);
			SingletonMonoBehaviour<SpawnFX>.Instance.InitExtendObject(sealEffectPrefab, 0);
		}

		public override void Update()
		{
			base.Update();
			if (attacking || !IsEnemyAlive() || SingletonMonoBehaviour<GameData>.Instance.IsGameOver)
			{
				return;
			}
			if (IsCooldownDone() && IsCurrentSpeedGreaterThanMinSpeed())
			{
				GetTowers();
				if (nearestTowers.Count > 0)
				{
					StartCoroutine(Attack(nearestTowers));
				}
			}
			timeTracking = Mathf.MoveTowards(timeTracking, 0f, Time.deltaTime);
		}

		public void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(base.transform.position, rangeInPixel / GameData.PIXEL_PER_UNIT);
		}

		private bool IsCooldownDone()
		{
			return timeTracking == 0f;
		}

		private IEnumerator Attack(List<TowerModel> towers)
		{
			attacking = true;
			onStartAttack.Dispatch();
			if (!IsCurrentSpeedGreaterThanMinSpeed())
			{
				yield return null;
			}
			if (!IsEnemyAlive())
			{
				yield return null;
			}
			base.EnemyModel.SetSpecialStateDuration(attackAnimationDuration);
			base.EnemyModel.SetSpecialStateAnimationName(EnemyAnimationController.animSpecialAttack);
			base.EnemyModel.GetFSMController().GetCurrentState().OnInput(StateInputType.SpecialState, EnemyAnimationController.animSpecialAttack);
			base.EnemyModel.EnemyAnimationController.ToSpecialAttackState();
			yield return new WaitForSeconds(attackAnimationDuration / 2f);
			SSRLog.Log($"Logic Boss1, attack {towers.Count} towers");
			bool isPositiveForTower = false;
			foreach (TowerModel tower in towers)
			{
				tower.BuffsHolder.AddBuff(silentBuffKey, new Buff(isPositiveForTower, 1f, effectTime / 1000f), BuffStackLogic.ChooseMax, BuffStackLogic.ChooseMax);
				AddEffectTowerSeal(tower);
			}
			timeTracking = coolDownTime / 1000f;
			attacking = false;
			onFinishAttack.Dispatch();
			yield return null;
		}

		private void AddEffectTowerSeal(TowerModel tower)
		{
			EffectController effect = SingletonMonoBehaviour<SpawnFX>.Instance.GetEffect(sealEffectName);
			effect.transform.position = tower.transform.position - new Vector3(0f, 0.05f, 0f);
			effect.SetLayerOverTarget(tower.transform);
			effect.Init(effectTime / 1000f);
		}

		private void GetTowers()
		{
			SingletonMonoBehaviour<GameData>.Instance.GetInRangeTowers(base.EnemyModel.transform.position, rangeInPixel / GameData.PIXEL_PER_UNIT, inRangeTowers);
			SingletonMonoBehaviour<GameData>.Instance.GetNearestTowers(base.EnemyModel.transform.position, maxTowers, inRangeTowers, nearestTowers, towersSquaredDistances);
		}
	}
}

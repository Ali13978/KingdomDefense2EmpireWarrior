using MyCustom;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class SpeedUpAuraController : CustomMonoBehaviour
	{
		private float aoeRange;

		private float activationTime;

		private int attackSpeedIncreasePercentage;

		private float timeTracking;

		private float trackingDuration = 1f;

		[SerializeField]
		private float appearAnimDuration;

		[SerializeField]
		private float disAppearAnimDuration;

		private bool isReady;

		[SerializeField]
		private Animator animator;

		private string buffKey = "IncreaseAttackSpeedByPercentage";

		private List<TowerModel> listInRangeTowers = new List<TowerModel>();

		private void Update()
		{
			if (isReady)
			{
				if (timeTracking == 0f)
				{
					TryToCastBuff();
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

		public void Init(float _aoeRange, float _activationTime, int _attackSpeedIncreasePercentage)
		{
			aoeRange = _aoeRange;
			activationTime = _activationTime;
			attackSpeedIncreasePercentage = _attackSpeedIncreasePercentage;
			CustomInvoke(GetReady, appearAnimDuration);
			CustomInvoke(EndOfLifeTime, activationTime + appearAnimDuration);
		}

		private void TryToCastBuff()
		{
			UnityEngine.Debug.Log("Try To cast buff speed!");
			GetInRangeTower();
			if (listInRangeTowers.Count > 0)
			{
				foreach (TowerModel listInRangeTower in listInRangeTowers)
				{
					listInRangeTower.BuffsHolder.AddBuff(buffKey, new Buff(isPositive: false, attackSpeedIncreasePercentage, trackingDuration), BuffStackLogic.ChooseMax, BuffStackLogic.ChooseMax);
					EffectController effect = SingletonMonoBehaviour<SpawnFX>.Instance.GetEffect(SpawnFX.BUFF_SPEED_ON_TOWER);
					effect.transform.position = listInRangeTower.transform.position;
					effect.Init(trackingDuration + Time.deltaTime);
				}
			}
			timeTracking = trackingDuration;
		}

		private void GetInRangeTower()
		{
			SingletonMonoBehaviour<GameData>.Instance.GetInRangeTowers(base.gameObject.transform.position, aoeRange, listInRangeTowers);
		}

		private void GetReady()
		{
			timeTracking = trackingDuration;
			isReady = true;
		}

		private void EndOfLifeTime()
		{
			isReady = false;
			listInRangeTowers.Clear();
			animator.Play("Disappear");
			CustomInvoke(ReturnPool, disAppearAnimDuration);
		}

		private void ReturnPool()
		{
			SingletonMonoBehaviour<SpawnTower>.Instance.Push(base.gameObject);
		}
	}
}

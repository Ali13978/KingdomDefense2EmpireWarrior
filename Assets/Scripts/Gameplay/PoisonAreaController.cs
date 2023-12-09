using MyCustom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class PoisonAreaController : CustomMonoBehaviour
	{
		private float aoeRange;

		private float activationTime;

		private string buffKey = "Burning";

		private int burnDamage;

		[SerializeField]
		private float appearAnimDuration;

		[SerializeField]
		private float disAppearAnimDuration;

		private bool isReady;

		private float timeTracking;

		private float trackingDuration = 1f;

		[SerializeField]
		private Animator animator;

		[SerializeField]
		private GameObject[] decorations;

		[SerializeField]
		private float delayTimeCreateDecoration;

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

		public void Init(float _aoeRange, float _activationTime, int _burnDamage)
		{
			aoeRange = _aoeRange;
			activationTime = _activationTime;
			burnDamage = _burnDamage;
			CustomInvoke(GetReady, appearAnimDuration);
			CustomInvoke(EndOfLifeTime, activationTime + appearAnimDuration);
		}

		private void TryToCastBuff()
		{
			TargetType targetType = default(TargetType);
			targetType.isAir = true;
			targetType.isGround = true;
			targetType.isUnderGround = false;
			targetType.isTunnel = false;
			List<EnemyModel> listEnemiesInRange = GameTools.GetListEnemiesInRange(base.gameObject, new CommonAttackDamage(targetType, aoeRange));
			if (listEnemiesInRange.Count > 0)
			{
				foreach (EnemyModel item in listEnemiesInRange)
				{
					if (GameTools.IsValidEnemy(item) && !item.IsInTunnel)
					{
						DamageEnemy(item);
					}
				}
			}
			timeTracking = trackingDuration;
		}

		private void DamageEnemy(EnemyModel enemyModel)
		{
			enemyModel.ProcessEffect(buffKey, burnDamage, trackingDuration + Time.deltaTime, DamageFXType.Poison1);
		}

		private void GetReady()
		{
			isReady = true;
			StartCoroutine(ShowDecorations());
		}

		private void EndOfLifeTime()
		{
			isReady = false;
			HideDecoration();
			animator.Play("Disappear");
			CustomInvoke(ReturnPool, disAppearAnimDuration);
		}

		private void ReturnPool()
		{
			SingletonMonoBehaviour<SpawnTower>.Instance.Push(base.gameObject);
		}

		private IEnumerator ShowDecorations()
		{
			for (int i = 0; i < decorations.Length; i++)
			{
				decorations[i].SetActive(value: true);
				yield return new WaitForSeconds(delayTimeCreateDecoration);
			}
		}

		private void HideDecoration()
		{
			for (int i = 0; i < decorations.Length; i++)
			{
				decorations[i].SetActive(value: false);
			}
		}
	}
}

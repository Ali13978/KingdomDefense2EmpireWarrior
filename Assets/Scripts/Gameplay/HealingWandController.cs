using MyCustom;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class HealingWandController : CustomMonoBehaviour
	{
		private float activationTime;

		private int hpPerSecond;

		private float healingRange;

		private float timeTracking;

		private float timeTrackingForHeal;

		private List<CharacterModel> allyInRange = new List<CharacterModel>();

		private bool isReady;

		private Animator animator;

		private void Awake()
		{
			animator = GetComponent<Animator>();
		}

		public void Init(float _activationTime, int _hpPerSecond, float _healingRange, float _timeTracking)
		{
			isReady = true;
			activationTime = _activationTime;
			hpPerSecond = _hpPerSecond;
			healingRange = _healingRange;
			timeTracking = _timeTracking;
			timeTrackingForHeal = timeTracking;
			CustomInvoke(PlayAnimEnd, activationTime - 0.5f);
		}

		private void PlayAnimEnd()
		{
			animator.SetTrigger("End");
		}

		public void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(base.transform.position, healingRange);
		}

		private void Update()
		{
			if (base.gameObject.activeSelf && isReady)
			{
				if (timeTrackingForHeal == 0f)
				{
					Healing();
				}
				timeTrackingForHeal = Mathf.MoveTowards(timeTrackingForHeal, 0f, Time.deltaTime);
			}
		}

		private void Healing()
		{
			GetAlliesInRange();
			for (int i = 0; i < allyInRange.Count; i++)
			{
				HealingAlly(allyInRange[i]);
			}
			timeTrackingForHeal = timeTracking;
		}

		private void HealingAlly(CharacterModel ally)
		{
			if (ally.IsAlive)
			{
				ally.IncreaseHealth(hpPerSecond);
				EffectController effect = SingletonMonoBehaviour<SpawnFX>.Instance.GetEffect(SpawnFX.EFFECT_HEAL_2);
				effect.transform.position = ally.transform.position;
				effect.Init(timeTracking + 0.5f, ally.BuffsHolder.transform, ally.GetComponent<SpriteRenderer>().sprite.rect.width);
			}
		}

		private void GetAlliesInRange()
		{
			allyInRange.Clear();
			List<CharacterModel> listActiveAlly = SingletonMonoBehaviour<GameData>.Instance.ListActiveAlly;
			for (int i = 0; i < listActiveAlly.Count; i++)
			{
				CharacterModel characterModel = listActiveAlly[i];
				float num = SingletonMonoBehaviour<GameData>.Instance.SqrDistance(base.gameObject, characterModel.gameObject);
				if (num <= healingRange * healingRange)
				{
					allyInRange.Add(characterModel);
				}
			}
		}
	}
}

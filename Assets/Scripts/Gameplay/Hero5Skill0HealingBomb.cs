using MyCustom;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class Hero5Skill0HealingBomb : CustomMonoBehaviour
	{
		private int healAmount;

		private float skillRange;

		private List<CharacterModel> allyInRange = new List<CharacterModel>();

		public void Init(int healAmount, float skillRange)
		{
			this.healAmount = healAmount;
			this.skillRange = skillRange;
			Healing();
			CustomInvoke(ReturnPool, 3f);
		}

		public void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(base.transform.position, skillRange);
		}

		private void Healing()
		{
			GetAlliesInRange();
			for (int i = 0; i < allyInRange.Count; i++)
			{
				HealingAlly(allyInRange[i]);
			}
		}

		private void HealingAlly(CharacterModel ally)
		{
			if (ally.IsAlive)
			{
				ally.IncreaseHealth(healAmount);
				EffectController effect = SingletonMonoBehaviour<SpawnFX>.Instance.GetEffect(SpawnFX.EFFECT_HEAL_0);
				effect.transform.position = ally.transform.position;
				effect.Init(1.5f, ally.BuffsHolder.transform, ally.GetComponent<SpriteRenderer>().sprite.rect.width);
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
				if (num <= skillRange * skillRange)
				{
					allyInRange.Add(characterModel);
				}
			}
		}

		private void ReturnPool()
		{
			SingletonMonoBehaviour<SpawnBullet>.Instance.Push(base.gameObject);
		}
	}
}

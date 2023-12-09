using GeneralVariable;
using MyCustom;
using UnityEngine;

namespace Gameplay
{
	public class Hero3Skill2IceTrap : CustomMonoBehaviour
	{
		private int burnDamage;

		private string buffKey;

		private int slowPercent;

		private float duration;

		private string burningBuffKey = "Burning";

		[SerializeField]
		private CircleCollider2D collider;

		public void Init(int burnDamage, string buffKey, int slowPercent, float duration)
		{
			this.burnDamage = burnDamage;
			this.buffKey = buffKey;
			this.slowPercent = slowPercent;
			this.duration = duration;
			collider.enabled = true;
			CustomInvoke(ReturnPool, duration);
		}

		private void OnTriggerEnter2D(Collider2D other)
		{
			if (other.gameObject.tag == GeneralVariable.GeneralVariable.ENEMY_TAG)
			{
				EnemyModel component = other.gameObject.GetComponent<EnemyModel>();
				if (!component.IsAir && component.IsAlive && !component.IsInTunnel && !component.IsUnderground)
				{
					DamageEnemy(component);
				}
			}
		}

		private void DamageEnemy(EnemyModel enemyModel)
		{
			enemyModel.ProcessEffect(buffKey, slowPercent, duration, DamageFXType.Slow);
			enemyModel.BuffsHolder.AddBuff(burningBuffKey, new Buff(isPositive: false, burnDamage, duration), BuffStackLogic.ChooseMax, BuffStackLogic.ChooseMax);
		}

		private void ReturnPool()
		{
			collider.enabled = false;
			SingletonMonoBehaviour<SpawnBullet>.Instance.Push(base.gameObject);
		}
	}
}

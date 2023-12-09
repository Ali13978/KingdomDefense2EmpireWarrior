using MyCustom;
using Parameter;
using System.Collections;
using UnityEngine;

namespace Gameplay
{
	public class ItemMeteorStrikeController : CustomMonoBehaviour
	{
		private int powerUpItemID;

		private PowerUpItem powerUpItem;

		private int damage;

		private float delayTime;

		private float cooldownTime;

		private int amount;

		private float aoeRange;

		[SerializeField]
		private float singleAoeRange;

		[SerializeField]
		private MeteorController meteorPrefab;

		[SerializeField]
		private float offsetHigh;

		[SerializeField]
		private float timeStep;

		private void Awake()
		{
			powerUpItem = GetComponent<PowerUpItem>();
			powerUpItemID = powerUpItem.powerUpItemID;
		}

		private void Start()
		{
			int[] customValue = Singleton<PowerUpItemParameter>.Instance.GetCustomValue(powerUpItemID);
			SingletonMonoBehaviour<SpawnBullet>.Instance.InitExtendBullet(meteorPrefab.gameObject);
			aoeRange = (float)customValue[0] / GameData.PIXEL_PER_UNIT;
			damage = customValue[1];
			amount = customValue[2];
			delayTime = (float)customValue[3] / 1000f;
			cooldownTime = (float)Singleton<PowerUpItemParameter>.Instance.GetCooldownTime(powerUpItemID) / 1000f;
			powerUpItem.Init(cooldownTime);
		}

		public void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(getTargetVector(), aoeRange);
		}

		public void CastMeteorStrike()
		{
			SingletonMonoBehaviour<SpawnFX>.Instance.InitFX(SpawnFX.LIGHTNING_PROJECTILE_SHADOW);
			SingletonMonoBehaviour<SpawnFX>.Instance.InitFX(SpawnFX.LIGHTNING_PROJECTILE_RANGE);
			SingletonMonoBehaviour<SpawnFX>.Instance.InitFX(SpawnFX.METEOR_EXPLOSION);
			StartCoroutine(DoCastSkill(getTargetVector()));
		}

		private IEnumerator DoCastSkill(Vector2 targetPosition)
		{
			CastEffectSkillRange(targetPosition);
			GameEventCenter.Instance.Trigger(GameEventType.EventUseItem, new EventTriggerData(EventTriggerType.UseItem, 1, 1, forceSaveProgress: true));
			for (int i = 0; i < amount; i++)
			{
				yield return new WaitForSeconds(delayTime);
				MeteorController bullet = SingletonMonoBehaviour<SpawnBullet>.Instance.GetMeteorController();
				bullet.transform.position = new Vector2(targetPosition.x, offsetHigh) + Random.insideUnitCircle * aoeRange;
				bullet.Init(damage, singleAoeRange, timeStep * (offsetHigh - targetPosition.y), offsetHigh - targetPosition.y);
			}
		}

		private void CastEffectSkillRange(Vector2 targetPosition)
		{
			EffectController effect = SingletonMonoBehaviour<SpawnFX>.Instance.GetEffect(SpawnFX.LIGHTNING_PROJECTILE_RANGE);
			effect.transform.position = targetPosition;
			effect.Init(0.75f);
		}

		private Vector2 getTargetVector()
		{
			Vector3 vector = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
			return new Vector2(vector.x, vector.y);
		}
	}
}

using MyCustom;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class ItemHealingWandController : CustomMonoBehaviour
	{
		private int powerUpItemID;

		private PowerUpItem powerUpItem;

		private int hpPerSecond;

		private float activationTime;

		private float healingRange;

		private float timeTracking;

		private float cooldownTime;

		private void Awake()
		{
			powerUpItem = GetComponent<PowerUpItem>();
			powerUpItemID = powerUpItem.powerUpItemID;
		}

		private void Start()
		{
			int[] customValue = Singleton<PowerUpItemParameter>.Instance.GetCustomValue(powerUpItemID);
			activationTime = (float)Singleton<PowerUpItemParameter>.Instance.GetWeaponActivationTime(powerUpItemID) / 1000f;
			hpPerSecond = customValue[1];
			cooldownTime = (float)Singleton<PowerUpItemParameter>.Instance.GetCooldownTime(powerUpItemID) / 1000f;
			healingRange = (float)customValue[0] / GameData.PIXEL_PER_UNIT;
			timeTracking = 1f;
			powerUpItem.Init(cooldownTime);
			InitFXs();
		}

		private void InitFXs()
		{
			SingletonMonoBehaviour<SpawnFX>.Instance.InitFX(SpawnFX.HEALING_WAND);
			SingletonMonoBehaviour<SpawnFX>.Instance.InitFX(SpawnFX.EFFECT_HEAL_2);
		}

		public void CastHealingWand()
		{
			CreateHealingWand(getTargetVector(), activationTime);
			GameEventCenter.Instance.Trigger(GameEventType.EventUseItem, new EventTriggerData(EventTriggerType.UseItem, 2, 1, forceSaveProgress: true));
		}

		private void CreateHealingWand(Vector2 targetPosition, float activationTime)
		{
			EffectController effect = SingletonMonoBehaviour<SpawnFX>.Instance.GetEffect(SpawnFX.HEALING_WAND);
			effect.transform.position = targetPosition;
			effect.Init(activationTime);
			effect.GetComponent<HealingWandController>().Init(activationTime, hpPerSecond, healingRange, timeTracking);
		}

		private Vector2 getTargetVector()
		{
			Vector3 vector = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
			return new Vector2(vector.x, vector.y);
		}
	}
}

using MyCustom;
using Parameter;
using System.Collections;
using UnityEngine;

namespace Gameplay
{
	public class ItemStormController : CustomMonoBehaviour
	{
		private int powerUpItemID;

		private PowerUpItem powerUpItem;

		private float activationTime;

		private float cooldownTime;

		private float pushBackDistance;

		private float aoeRange;

		private int maxEnemyAffected;

		private string buffkey = "Slow";

		private void Awake()
		{
			powerUpItem = GetComponent<PowerUpItem>();
			powerUpItemID = powerUpItem.powerUpItemID;
		}

		private void Start()
		{
			int[] customValue = Singleton<PowerUpItemParameter>.Instance.GetCustomValue(powerUpItemID);
			pushBackDistance = (float)customValue[0] / GameData.PIXEL_PER_UNIT;
			aoeRange = (float)customValue[1] / GameData.PIXEL_PER_UNIT;
			maxEnemyAffected = customValue[2];
			activationTime = (float)Singleton<PowerUpItemParameter>.Instance.GetWeaponActivationTime(powerUpItemID) / 1000f;
			cooldownTime = (float)Singleton<PowerUpItemParameter>.Instance.GetCooldownTime(powerUpItemID) / 1000f;
			powerUpItem.Init(cooldownTime);
			InitFXs();
		}

		private void InitFXs()
		{
			SingletonMonoBehaviour<SpawnFX>.Instance.InitFX(SpawnFX.STORM);
		}

		public void CastSkill()
		{
			StartCoroutine(DoCastSkill(getTargetVector()));
		}

		private IEnumerator DoCastSkill(Vector2 targetPosition)
		{
			GameEventCenter.Instance.Trigger(GameEventType.EventUseItem, new EventTriggerData(EventTriggerType.UseItem, 8, 1, forceSaveProgress: true));
			yield return null;
			UnityEngine.Debug.Log("Cast skill storm!");
			EffectController storm = SingletonMonoBehaviour<SpawnFX>.Instance.GetEffect(SpawnFX.STORM);
			storm.transform.position = targetPosition;
			storm.gameObject.SetActive(value: true);
			storm.GetComponent<StormController>().Init(aoeRange, activationTime, pushBackDistance, maxEnemyAffected);
		}

		private Vector2 getTargetVector()
		{
			Vector3 vector = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
			return new Vector2(vector.x, vector.y);
		}
	}
}

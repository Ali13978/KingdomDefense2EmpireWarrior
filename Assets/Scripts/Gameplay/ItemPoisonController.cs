using MyCustom;
using Parameter;
using System.Collections;
using UnityEngine;

namespace Gameplay
{
	public class ItemPoisonController : CustomMonoBehaviour
	{
		private int powerUpItemID;

		private PowerUpItem powerUpItem;

		private float activationTime;

		private int burnDamage;

		private float aoeRange;

		private float cooldownTime;

		private void Awake()
		{
			powerUpItem = GetComponent<PowerUpItem>();
			powerUpItemID = powerUpItem.powerUpItemID;
		}

		private void Start()
		{
			int[] customValue = Singleton<PowerUpItemParameter>.Instance.GetCustomValue(powerUpItemID);
			burnDamage = customValue[0];
			aoeRange = (float)customValue[1] / GameData.PIXEL_PER_UNIT;
			activationTime = (float)Singleton<PowerUpItemParameter>.Instance.GetWeaponActivationTime(powerUpItemID) / 1000f;
			cooldownTime = (float)Singleton<PowerUpItemParameter>.Instance.GetCooldownTime(powerUpItemID) / 1000f;
			powerUpItem.Init(cooldownTime);
			InitFXs();
		}

		private void InitFXs()
		{
			SingletonMonoBehaviour<SpawnFX>.Instance.InitFX(SpawnFX.POISON_AREA);
			SingletonMonoBehaviour<SpawnFX>.Instance.InitFX(SpawnFX.EFFECT_POISON1);
		}

		public void CastSkill()
		{
			StartCoroutine(DoCastSkill(getTargetVector()));
		}

		private IEnumerator DoCastSkill(Vector2 targetPosition)
		{
			GameEventCenter.Instance.Trigger(GameEventType.EventUseItem, new EventTriggerData(EventTriggerType.UseItem, 5, 1, forceSaveProgress: true));
			yield return null;
			UnityEngine.Debug.Log("Cast skill poison!");
			EffectController poisonArea = SingletonMonoBehaviour<SpawnFX>.Instance.GetEffect(SpawnFX.POISON_AREA);
			poisonArea.transform.position = targetPosition;
			poisonArea.gameObject.SetActive(value: true);
			poisonArea.GetComponent<PoisonAreaController>().Init(aoeRange, activationTime, burnDamage);
		}

		private Vector2 getTargetVector()
		{
			Vector3 vector = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
			return new Vector2(vector.x, vector.y);
		}
	}
}

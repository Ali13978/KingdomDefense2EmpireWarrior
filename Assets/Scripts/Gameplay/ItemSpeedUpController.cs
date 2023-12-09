using MyCustom;
using Parameter;
using System.Collections;
using UnityEngine;

namespace Gameplay
{
	public class ItemSpeedUpController : CustomMonoBehaviour
	{
		private int powerUpItemID;

		private PowerUpItem powerUpItem;

		private float activationTime;

		private float cooldownTime;

		private int attackSpeedIncreasePercentage;

		private float aoeRange;

		private void Awake()
		{
			powerUpItem = GetComponent<PowerUpItem>();
			powerUpItemID = powerUpItem.powerUpItemID;
		}

		private void Start()
		{
			int[] customValue = Singleton<PowerUpItemParameter>.Instance.GetCustomValue(powerUpItemID);
			attackSpeedIncreasePercentage = customValue[0];
			aoeRange = (float)customValue[1] / GameData.PIXEL_PER_UNIT;
			activationTime = (float)Singleton<PowerUpItemParameter>.Instance.GetWeaponActivationTime(powerUpItemID) / 1000f;
			cooldownTime = (float)Singleton<PowerUpItemParameter>.Instance.GetCooldownTime(powerUpItemID) / 1000f;
			powerUpItem.Init(cooldownTime);
			InitFXs();
		}

		private void InitFXs()
		{
			SingletonMonoBehaviour<SpawnFX>.Instance.InitFX(SpawnFX.BUFF_SPEED_AURA);
			SingletonMonoBehaviour<SpawnFX>.Instance.InitFX(SpawnFX.BUFF_SPEED_ON_TOWER);
		}

		public void CastSkill()
		{
			StartCoroutine(DoCastSkill(getTargetVector()));
		}

		private IEnumerator DoCastSkill(Vector2 targetPosition)
		{
			yield return null;
			UnityEngine.Debug.Log(">>>>>Cast skill speed up!");
			GameEventCenter.Instance.Trigger(GameEventType.EventUseItem, new EventTriggerData(EventTriggerType.UseItem, 4, 1, forceSaveProgress: true));
			EffectController speedAura = SingletonMonoBehaviour<SpawnFX>.Instance.GetEffect(SpawnFX.BUFF_SPEED_AURA);
			speedAura.transform.position = targetPosition;
			speedAura.gameObject.SetActive(value: true);
			speedAura.GetComponent<SpeedUpAuraController>().Init(aoeRange, activationTime, attackSpeedIncreasePercentage);
		}

		private Vector2 getTargetVector()
		{
			Vector3 vector = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
			return new Vector2(vector.x, vector.y);
		}
	}
}

using MyCustom;
using Parameter;
using UnityEngine;

namespace Gameplay
{
	public class ItemIncreaseGoldController : CustomMonoBehaviour
	{
		[SerializeField]
		private float delaytimeFX;

		private int powerUpItemID;

		private PowerUpItem powerUpItem;

		private int goldAmount;

		private float cooldownTime;

		private void Awake()
		{
			powerUpItem = GetComponent<PowerUpItem>();
			powerUpItemID = powerUpItem.powerUpItemID;
		}

		private void Start()
		{
			int[] customValue = Singleton<PowerUpItemParameter>.Instance.GetCustomValue(powerUpItemID);
			goldAmount = customValue[0];
			cooldownTime = (float)Singleton<PowerUpItemParameter>.Instance.GetCooldownTime(powerUpItemID) / 1000f;
			powerUpItem.Init(cooldownTime);
		}

		public void IncreaseGold()
		{
			SingletonMonoBehaviour<SpawnFX>.Instance.InitFX(SpawnFX.GOLD_CHEST);
			EffectController effect = SingletonMonoBehaviour<SpawnFX>.Instance.GetEffect(SpawnFX.GOLD_CHEST);
			effect.transform.position = Vector2.zero;
			effect.Init(delaytimeFX);
			effect.GetComponent<SpawnExtendFX>().Init();
			CustomInvoke(DoIncreaseGold, delaytimeFX);
		}

		private void DoIncreaseGold()
		{
			GameEventCenter.Instance.Trigger(GameEventType.EventUseItem, new EventTriggerData(EventTriggerType.UseItem, 3, 1, forceSaveProgress: true));
			SingletonMonoBehaviour<GameData>.Instance.IncreaseMoney(goldAmount);
		}

		private Vector2 getTargetVector()
		{
			Vector3 vector = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
			return new Vector2(vector.x, vector.y);
		}
	}
}

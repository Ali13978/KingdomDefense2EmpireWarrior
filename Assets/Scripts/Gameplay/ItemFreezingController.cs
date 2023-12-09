using MyCustom;
using Parameter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Gameplay
{
	public class ItemFreezingController : CustomMonoBehaviour
	{
		private int powerUpItemID;

		private PowerUpItem powerUpItem;

		private float activationTime;

		private float cooldownTime;

		private int slowPercent;

		private string buffkey = "Slow";

		private void Awake()
		{
			powerUpItem = GetComponent<PowerUpItem>();
			powerUpItemID = powerUpItem.powerUpItemID;
		}

		private void Start()
		{
			activationTime = (float)Singleton<PowerUpItemParameter>.Instance.GetWeaponActivationTime(powerUpItemID) / 1000f;
			cooldownTime = (float)Singleton<PowerUpItemParameter>.Instance.GetCooldownTime(powerUpItemID) / 1000f;
			slowPercent = 100;
			powerUpItem.Init(cooldownTime);
		}

		public void FreezeAllEnemy()
		{
			SingletonMonoBehaviour<SpawnFX>.Instance.InitFX(SpawnFX.EFFECT_ITEM_FREEZE);
			StopAllCoroutines();
			StartCoroutine(DoFreeze());
		}

		private IEnumerator DoFreeze()
		{
			GameEventCenter.Instance.Trigger(GameEventType.EventUseItem, new EventTriggerData(EventTriggerType.UseItem, 0, 1, forceSaveProgress: true));
			List<EnemyModel> ListActiveEnemy = SingletonMonoBehaviour<GameData>.Instance.ListActiveEnemy;
			FrostEffect fxCamera = SingletonMonoBehaviour<SpawnFX>.Instance.GetFrostEffectOnCamera();
			fxCamera.Init(2f * activationTime);
			fxCamera.DoEffectIn(activationTime, 0.25f);
			foreach (EnemyModel item in ListActiveEnemy)
			{
				item.ProcessEffect(buffkey, slowPercent, activationTime, DamageFXType.Freezing);
			}
			yield return new WaitForSeconds(activationTime);
			fxCamera.DoEffectOut(activationTime, 0f);
		}
	}
}

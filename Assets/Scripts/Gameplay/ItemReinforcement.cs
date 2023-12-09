using MyCustom;
using Parameter;
using System.Collections;
using UnityEngine;

namespace Gameplay
{
	public class ItemReinforcement : CustomMonoBehaviour
	{
		[SerializeField]
		private int allyID;

		private int powerUpItemID;

		private PowerUpItem powerUpItem;

		private int reinforcementAmount;

		private float duration;

		private float cooldownTime;

		private string buffkey = "Slow";

		private Hero heroParameter = default(Hero);

		private float parameterScale = 1f;

		private Vector2 vectorXUnit = new Vector2(0.3f, 0f);

		private void Awake()
		{
			powerUpItem = GetComponent<PowerUpItem>();
			powerUpItemID = powerUpItem.powerUpItemID;
		}

		private void Start()
		{
			int[] customValue = Singleton<PowerUpItemParameter>.Instance.GetCustomValue(powerUpItemID);
			heroParameter.attack_physics_min = customValue[0];
			heroParameter.attack_physics_max = customValue[1];
			heroParameter.attack_range_min = customValue[2];
			heroParameter.attack_range_average = customValue[3];
			heroParameter.attack_range_max = customValue[4];
			heroParameter.health = customValue[5];
			heroParameter.armor_physics = customValue[6];
			heroParameter.speed = customValue[7];
			heroParameter.attack_cooldown = customValue[8];
			reinforcementAmount = customValue[9];
			duration = (float)Singleton<PowerUpItemParameter>.Instance.GetWeaponActivationTime(powerUpItemID) / 1000f;
			cooldownTime = (float)Singleton<PowerUpItemParameter>.Instance.GetCooldownTime(powerUpItemID) / 1000f;
			UnityEngine.Debug.Log(heroParameter);
			powerUpItem.Init(cooldownTime);
			InitFXs();
		}

		private void InitFXs()
		{
			SingletonMonoBehaviour<SpawnAlly>.Instance.PushAlliesToPool(allyID, allyID, 0);
			if (heroParameter.attack_range_max > 100)
			{
				SingletonMonoBehaviour<SpawnBullet>.Instance.InitBulletsFromTower(allyID, allyID);
			}
		}

		public void CastSkill()
		{
			StartCoroutine(DoCastSkill(getTargetVector()));
		}

		private IEnumerator DoCastSkill(Vector2 targetPosition)
		{
			yield return null;
			UnityEngine.Debug.Log("Call reforcement!");
			if (allyID == 1000)
			{
				UnityEngine.Debug.Log("_____>>> trigger use ground reinforce");
				GameEventCenter.Instance.Trigger(GameEventType.EventUseItem, new EventTriggerData(EventTriggerType.UseItem, 6, 1, forceSaveProgress: true));
			}
			else if (allyID == 1001)
			{
				UnityEngine.Debug.Log("_____>>> trigger use ssky reinforce");
				GameEventCenter.Instance.Trigger(GameEventType.EventUseItem, new EventTriggerData(EventTriggerType.UseItem, 7, 1, forceSaveProgress: true));
			}
			int cloneIndex = 0;
			for (int i = 0; i < reinforcementAmount; i++)
			{
				AllyModel allyModel = SingletonMonoBehaviour<SpawnAlly>.Instance.Get(allyID, allyID);
				if (reinforcementAmount == 1)
				{
					allyModel.transform.position = targetPosition;
				}
				if (reinforcementAmount > 1)
				{
					if (cloneIndex == 0)
					{
						allyModel.transform.position = targetPosition - vectorXUnit;
					}
					else
					{
						allyModel.transform.position = targetPosition + vectorXUnit;
					}
				}
				allyModel.InitFromHero(heroParameter, parameterScale, duration);
				allyModel.gameObject.SetActive(value: true);
				if (heroParameter.attack_range_max > 100)
				{
					allyModel.AllyAttackController.rangeAttack = true;
				}
				cloneIndex++;
			}
		}

		private Vector2 getTargetVector()
		{
			Vector3 vector = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
			return new Vector2(vector.x, vector.y);
		}
	}
}

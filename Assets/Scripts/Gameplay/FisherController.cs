using UnityEngine;

namespace Gameplay
{
	public class FisherController : MonoBehaviour
	{
		private static string BONUSMONEY_OBJECT_NAME = "BonusMoney";

		private Animator animator;

		[SerializeField]
		private int chanceToSuccessFishing;

		[SerializeField]
		private int goldAmount;

		[SerializeField]
		private Transform goldPosition;

		[SerializeField]
		private float cooldownTime;

		private float cooldownTimeTracking;

		private void Awake()
		{
			animator = GetComponent<Animator>();
		}

		private void Start()
		{
			cooldownTimeTracking = cooldownTime;
		}

		private void Update()
		{
			if (SingletonMonoBehaviour<GameData>.Instance.IsGameStart)
			{
				if (cooldownTimeTracking == 0f)
				{
					ProcessFishing();
				}
				cooldownTimeTracking = Mathf.MoveTowards(cooldownTimeTracking, 0f, Time.deltaTime);
			}
		}

		private void ProcessFishing()
		{
			if (chanceToSuccessFishing < UnityEngine.Random.Range(0, 100))
			{
				animator.SetTrigger("Catch");
			}
			else
			{
				animator.SetTrigger("Miss");
			}
			cooldownTimeTracking = cooldownTime;
		}

		public void OnCatch()
		{
			DroppedGoldController droppedGold = SingletonMonoBehaviour<SpawnFX>.Instance.GetDroppedGold();
			droppedGold.gameObject.SetActive(value: true);
			droppedGold.transform.position = goldPosition.position;
			droppedGold.Init(goldAmount);
			SingletonMonoBehaviour<GameData>.Instance.IncreaseMoney(goldAmount);
		}

		public void OnMiss()
		{
		}
	}
}

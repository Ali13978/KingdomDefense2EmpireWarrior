using System.Collections;
using UnityEngine;

namespace Gameplay
{
	public class SpawnExtendFX : MonoBehaviour
	{
		[SerializeField]
		private string effectName;

		[SerializeField]
		private int amountOfEffect;

		[SerializeField]
		private float timeStepBetweenEffect;

		[SerializeField]
		private float delayTime;

		[SerializeField]
		private float effectLifeTime;

		private GameObject targetPosition;

		private void Start()
		{
			SingletonMonoBehaviour<SpawnFX>.Instance.InitFX(effectName);
			targetPosition = GameObject.FindGameObjectWithTag("CoinPosition");
		}

		public void Init()
		{
			StartCoroutine(CreateFlyingCoin());
		}

		private IEnumerator CreateFlyingCoin()
		{
			yield return new WaitForSeconds(delayTime);
			for (int i = 0; i < amountOfEffect; i++)
			{
				EffectController coin = SingletonMonoBehaviour<SpawnFX>.Instance.GetEffect(SpawnFX.FLYING_COIN);
				coin.transform.position = base.gameObject.transform.position;
				coin.Init(effectLifeTime);
				coin.GetComponent<FlyingCoinController>().Init(targetPosition.transform.position);
				yield return new WaitForSeconds(timeStepBetweenEffect);
			}
		}
	}
}

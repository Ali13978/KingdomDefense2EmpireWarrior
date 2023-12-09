using SSR.Core.Architecture;
using UnityEngine;

namespace SSR.Core.Gameplay.HelperComponent
{
	public class RandomTimeTriggerer : MonoBehaviour
	{
		[SerializeField]
		private float minTimeInterval;

		[SerializeField]
		private float maxTimeInterval;

		[SerializeField]
		private OrderedEventDispatcher onTrigger = new OrderedEventDispatcher();

		private float timeTracker;

		private float currentInterval;

		public void Awake()
		{
			StartNewCounter();
		}

		private void StartNewCounter()
		{
			timeTracker = 0f;
			currentInterval = GetRandomInterval();
		}

		public void Update()
		{
			timeTracker += Time.deltaTime;
			if (timeTracker >= currentInterval)
			{
				StartNewCounter();
				onTrigger.Dispatch();
			}
		}

		private float GetRandomInterval()
		{
			return UnityEngine.Random.Range(minTimeInterval, maxTimeInterval);
		}
	}
}

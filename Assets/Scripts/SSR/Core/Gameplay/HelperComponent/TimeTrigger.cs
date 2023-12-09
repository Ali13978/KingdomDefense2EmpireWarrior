using SSR.Core.Architecture;
using UnityEngine;
using UnityEngine.Serialization;

namespace SSR.Core.Gameplay.HelperComponent
{
	public class TimeTrigger : MonoBehaviour
	{
		[SerializeField]
		private float countDownTime = 2f;

		[SerializeField]
		[FormerlySerializedAs("timeOutEvenent")]
		private OrderedEventDispatcher timeOutEvent = new OrderedEventDispatcher();

		private float currentTime;

		private bool counting;

		public float CountDownTime
		{
			get
			{
				return countDownTime;
			}
			set
			{
				countDownTime = value;
			}
		}

		private void Awake()
		{
			if (!counting)
			{
				base.enabled = false;
			}
		}

		private void Update()
		{
			currentTime -= Time.deltaTime;
			if (currentTime <= 0f)
			{
				timeOutEvent.Dispatch();
				base.enabled = false;
			}
		}

		public void StartCountDown()
		{
			base.enabled = true;
			currentTime = CountDownTime;
			counting = true;
		}

		public void OnDisable()
		{
			counting = false;
		}
	}
}

using SSR.Core.Architecture;
using System;
using UnityEngine;

namespace SSR.Core.HelperComponent
{
	public class NextDayTriggerer : MonoBehaviour
	{
		[SerializeField]
		private OrderedEventDispatcher onNewDay = new OrderedEventDispatcher();

		private DateTime lastSavedDate;

		public void OnEnable()
		{
			lastSavedDate = DateTime.Today;
		}

		public void Update()
		{
			if (DateTime.Today.Date > lastSavedDate)
			{
				onNewDay.Dispatch();
				lastSavedDate = DateTime.Today;
			}
		}
	}
}

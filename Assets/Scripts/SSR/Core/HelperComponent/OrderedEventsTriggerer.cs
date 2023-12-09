using SSR.Core.Architecture;
using UnityEngine;

namespace SSR.Core.HelperComponent
{
	public class OrderedEventsTriggerer : MonoBehaviour
	{
		[SerializeField]
		private OrderedEventDispatcher events = new OrderedEventDispatcher();

		[ContextMenu("Trigger")]
		public void Trigger()
		{
			events.Dispatch();
		}
	}
}

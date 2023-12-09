using UnityEngine;

namespace SSR.Core.Architecture.UI
{
	public class UIRefresherEvent : MonoBehaviour, IUIRefresher
	{
		[SerializeField]
		private OrderedEventDispatcher onRefresh = new OrderedEventDispatcher();

		public void Refresh()
		{
			onRefresh.Dispatch();
		}
	}
}

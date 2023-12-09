using SSR.Core.Architecture;
using UnityEngine;

namespace SSR.Core.HelperComponent
{
	public class ApplicationLostFocusTriggerer : MonoBehaviour
	{
		[SerializeField]
		private bool ignoreInEditor;

		[SerializeField]
		private OrderedEventDispatcher onLost = new OrderedEventDispatcher();

		[SerializeField]
		private OrderedEventDispatcher onGot = new OrderedEventDispatcher();

		public void OnApplicationFocus(bool focus)
		{
			if (!focus)
			{
				SSRLog.Log("App lost focus", this);
				onLost.Dispatch();
			}
			else
			{
				SSRLog.Log("App got focus", this);
				onGot.Dispatch();
			}
		}
	}
}

using SSR.Core.Architecture;
using UnityEngine;

namespace Notify
{
	public abstract class NotifyUnit : MonoBehaviour
	{
		[Space]
		[SerializeField]
		private OrderedEventDispatcher onShowNotify;

		[SerializeField]
		private OrderedEventDispatcher onNotShowNotify;

		protected abstract bool ShouldShowNotify();

		public void CheckCondition()
		{
			if (ShouldShowNotify())
			{
				onShowNotify.Dispatch();
			}
			else
			{
				onNotShowNotify.Dispatch();
			}
		}
	}
}

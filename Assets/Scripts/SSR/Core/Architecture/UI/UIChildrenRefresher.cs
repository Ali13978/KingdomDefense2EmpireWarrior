using System;
using System.Collections;
using UnityEngine;

namespace SSR.Core.Architecture.UI
{
	[DisallowMultipleComponent]
	public class UIChildrenRefresher : MonoBehaviour, IUIRefresher
	{
		[ContextMenu("Refresh")]
		public void Refresh()
		{
			IEnumerator enumerator = base.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Transform transform = (Transform)enumerator.Current;
					transform.GetComponent<IUIRefresher>()?.Refresh();
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
		}
	}
}

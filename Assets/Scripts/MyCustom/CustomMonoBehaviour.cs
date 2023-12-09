using System;
using UnityEngine;

namespace MyCustom
{
	public class CustomMonoBehaviour : MonoBehaviour
	{
		public void CustomInvoke(Action a, float delayTime)
		{
			Invoke(a.Method.Name, delayTime);
		}

		public void CustomCancelInvoke(Action a)
		{
			CancelInvoke(a.Method.Name);
		}

		public void CustomCancelInvoke()
		{
			CancelInvoke();
		}

		public void CustomInvokeRepeating(Action a, float delayTime, float repeatRate)
		{
			InvokeRepeating(a.Method.Name, delayTime, repeatRate);
		}
	}
}

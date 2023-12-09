using SSR.Core.Architecture;
using SSR.Core.Architecture.UI;
using System.Collections;
using UnityEngine;

namespace SSR.Core.HelperComponent
{
	public class CountDownText : TextContentSetter
	{
		[SerializeField]
		private int startNumber = 3;

		[SerializeField]
		private int endNumber = 1;

		[SerializeField]
		private float interval = 1f;

		[SerializeField]
		private OrderedEventDispatcher onFinish = new OrderedEventDispatcher();

		private int currentNumber;

		private bool counting;

		protected override void SetContent()
		{
			base.Text.text = currentNumber.ToString();
		}

		[ContextMenu("StartCountDown")]
		public void StartCountDown()
		{
			if (!counting)
			{
				StartCoroutine(CountDownAsync());
			}
		}

		public void StopCounting()
		{
			counting = false;
		}

		private IEnumerator CountDownAsync()
		{
			counting = true;
			float timeTracking2 = 0f;
			currentNumber = startNumber;
			while (currentNumber != endNumber && counting)
			{
				yield return new WaitForEndOfFrame();
				timeTracking2 += Time.unscaledDeltaTime;
				if (timeTracking2 >= interval)
				{
					timeTracking2 = 0f;
					currentNumber = (int)Mathf.MoveTowards(currentNumber, endNumber, 1f);
				}
			}
			for (timeTracking2 = 0f; timeTracking2 < interval; timeTracking2 += Time.unscaledDeltaTime)
			{
				if (!counting)
				{
					break;
				}
				yield return new WaitForEndOfFrame();
			}
			if (counting)
			{
				onFinish.Dispatch();
				counting = false;
			}
		}

		public void OnValidate()
		{
			setContentAtUpdate = true;
		}
	}
}

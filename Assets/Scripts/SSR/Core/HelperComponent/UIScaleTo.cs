using SSR.Core.Architecture;
using System.Collections;
using UnityEngine;

namespace SSR.Core.HelperComponent
{
	public class UIScaleTo : OutsiteTargetRectTransform
	{
		[SerializeField]
		private OrderedEventDispatcher onFinishScaling;

		[Space]
		[SerializeField]
		private Vector3 desireTargetScale;

		[SerializeField]
		private float duration = 0.5f;

		[SerializeField]
		private bool useX;

		[SerializeField]
		private bool useY;

		[SerializeField]
		private bool useZ;

		[ContextMenu("Scale")]
		public void Scale()
		{
			if (duration > 0f)
			{
				StartCoroutine(ScaleAsync());
			}
			else
			{
				base.TargetRectTransform.localScale = GetTargetScale();
			}
		}

		private IEnumerator ScaleAsync()
		{
			float currentTime = 0f;
			Vector3 startScale = base.TargetRectTransform.localScale;
			Vector3 targetScale = GetTargetScale();
			while (currentTime < duration)
			{
				currentTime = Mathf.MoveTowards(currentTime, duration, Time.deltaTime);
				base.TargetRectTransform.localScale = Vector3.Lerp(startScale, targetScale, currentTime / duration);
				yield return new WaitForEndOfFrame();
			}
			onFinishScaling.Dispatch();
			yield return null;
		}

		private Vector3 GetTargetScale()
		{
			Vector3 localScale = base.TargetRectTransform.localScale;
			if (useX)
			{
				localScale.x = desireTargetScale.x;
			}
			if (useY)
			{
				localScale.y = desireTargetScale.y;
			}
			if (useZ)
			{
				localScale.z = desireTargetScale.z;
			}
			return localScale;
		}
	}
}

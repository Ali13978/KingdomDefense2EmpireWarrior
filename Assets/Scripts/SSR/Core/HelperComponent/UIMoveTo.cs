using SSR.Core.Architecture;
using System.Collections;
using UnityEngine;

namespace SSR.Core.HelperComponent
{
	public class UIMoveTo : OutsiteTargetRectTransform
	{
		[SerializeField]
		private OrderedEventDispatcher onFinishMoving;

		[Space]
		[SerializeField]
		private Vector3 desireTargetPosition;

		[SerializeField]
		private float duration = 0.5f;

		[SerializeField]
		private bool useX;

		[SerializeField]
		private bool useY;

		[SerializeField]
		private bool useZ;

		[ContextMenu("Move")]
		public void Move()
		{
			if (duration > 0f)
			{
				StartCoroutine(MoveAsync());
				return;
			}
			base.TargetRectTransform.anchoredPosition = GetFinalTargetPosition(base.TargetRectTransform.anchoredPosition);
			onFinishMoving.Dispatch();
		}

		private IEnumerator MoveAsync()
		{
			float currentTime = 0f;
			Vector3 startPosition = base.TargetRectTransform.anchoredPosition;
			Vector3 targetPosition = GetFinalTargetPosition(startPosition);
			while (currentTime <= duration)
			{
				currentTime = Mathf.MoveTowards(currentTime, duration, Time.unscaledDeltaTime);
				base.TargetRectTransform.anchoredPosition = Vector3.Lerp(startPosition, targetPosition, currentTime / duration);
				if (currentTime == duration)
				{
					break;
				}
				yield return new WaitForEndOfFrame();
			}
			onFinishMoving.Dispatch();
			yield return null;
		}

		private Vector3 GetFinalTargetPosition(Vector3 startPositon)
		{
			if (useX)
			{
				startPositon.x = desireTargetPosition.x;
			}
			if (useY)
			{
				startPositon.y = desireTargetPosition.y;
			}
			if (useZ)
			{
				startPositon.z = desireTargetPosition.z;
			}
			return startPositon;
		}
	}
}

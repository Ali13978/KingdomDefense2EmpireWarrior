using System.Collections;
using UnityEngine;

namespace SSR.Core.HelperComponent
{
	public class UIMoveBy : OutsiteTargetRectTransform
	{
		[SerializeField]
		private Vector2 distance;

		[SerializeField]
		private float duration;

		[ContextMenu("Move")]
		public void Move()
		{
			StartCoroutine(MoveAsync(distance));
		}

		[ContextMenu("MoveReverse")]
		public void MoveReverse()
		{
			StartCoroutine(MoveAsync(-distance));
		}

		private IEnumerator MoveAsync(Vector2 distance)
		{
			float t = 0f;
			Vector3 start = base.TargetRectTransform.anchoredPosition;
			Vector2 target = base.TargetRectTransform.anchoredPosition + distance;
			while (t <= duration)
			{
				base.TargetRectTransform.anchoredPosition = Vector2.Lerp(start, target, t / duration);
				t = Mathf.MoveTowards(t, duration, Time.deltaTime);
				yield return new WaitForEndOfFrame();
			}
			yield return null;
		}
	}
}

using System.Collections;
using UnityEngine;

namespace SSR.Core.Gameplay.HelperComponent
{
	[RequireComponent(typeof(QuadStretcher))]
	public class QuadStretchTo : MonoBehaviour
	{
		[SerializeField]
		[HideInInspector]
		private QuadStretcher quadStretcher;

		[SerializeField]
		private Vector4 targetStretch;

		[SerializeField]
		private float duration;

		private bool stopFlag;

		private bool stretching;

		[ContextMenu("Stretch")]
		public void Stretch()
		{
			if (duration == 0f)
			{
				quadStretcher.Stretch = targetStretch;
			}
			else
			{
				StartCoroutine(StretchAsync());
			}
		}

		public void StopStretching()
		{
			if (stretching)
			{
				stopFlag = true;
			}
		}

		private IEnumerator StretchAsync()
		{
			stretching = true;
			float t = 0f;
			Vector4 startStretch = quadStretcher.Stretch;
			while (t <= duration)
			{
				if (stopFlag)
				{
					stopFlag = false;
					break;
				}
				quadStretcher.Stretch = Vector4.Lerp(startStretch, targetStretch, t / duration);
				t += Time.deltaTime;
				yield return new WaitForEndOfFrame();
			}
			stretching = false;
		}

		public void Reset()
		{
			quadStretcher = GetComponent<QuadStretcher>();
		}
	}
}

using UnityEngine;

namespace SSR.Core.HelperComponent
{
	public abstract class OutsiteTargetRectTransform : MonoBehaviour
	{
		[SerializeField]
		private RectTransform targetRectTransform;

		protected RectTransform TargetRectTransform
		{
			get
			{
				return targetRectTransform;
			}
			private set
			{
				targetRectTransform = value;
			}
		}

		public void Reset()
		{
			TargetRectTransform = GetComponent<RectTransform>();
		}

		public void OnValidate()
		{
		}
	}
}

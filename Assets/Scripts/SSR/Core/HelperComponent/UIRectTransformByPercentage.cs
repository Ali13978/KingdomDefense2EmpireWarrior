using SSR.Core.Architecture.UI;
using UnityEngine;

namespace SSR.Core.HelperComponent
{
	[RequireComponent(typeof(RectTransform))]
	public abstract class UIRectTransformByPercentage : ContentSetter
	{
		[SerializeField]
		[HideInNormalInspector]
		protected RectTransform targetRectTransform;

		[SerializeField]
		protected float minValue;

		[SerializeField]
		protected float maxValue;

		[Space]
		[SerializeField]
		[Range(0f, 1f)]
		private float currentPercentage = 0.5f;

		public float CurrentPercentage
		{
			get
			{
				return currentPercentage;
			}
			set
			{
				currentPercentage = value;
			}
		}

		protected float CurrentValue => minValue + (maxValue - minValue) * currentPercentage;

		public void OnDrawGizmos()
		{
			SetContent();
		}

		public void Reset()
		{
			targetRectTransform = GetComponent<RectTransform>();
		}

		protected abstract override void SetContent();
	}
}

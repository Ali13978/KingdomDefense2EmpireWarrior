using UnityEngine;

namespace SSR.Core.HelperComponent
{
	public class UISetWidthByPercentage : UIRectTransformByPercentage
	{
		protected override void SetContent()
		{
			Vector2 sizeDelta = targetRectTransform.sizeDelta;
			sizeDelta.x = base.CurrentValue;
			targetRectTransform.sizeDelta = sizeDelta;
		}
	}
}

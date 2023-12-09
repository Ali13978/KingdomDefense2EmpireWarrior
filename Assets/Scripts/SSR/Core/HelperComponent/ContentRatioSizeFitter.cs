using System;
using UnityEngine;
using UnityEngine.UI;

namespace SSR.Core.HelperComponent
{
	public class ContentRatioSizeFitter : MonoBehaviour
	{
		[SerializeField]
		private float maxWidth;

		[SerializeField]
		private float maxHeight;

		[ContextMenu("SetSize")]
		public void SetSize()
		{
			if (maxWidth <= 0f || maxHeight <= 0f)
			{
				throw new Exception();
			}
			ILayoutElement component = GetComponent<ILayoutElement>();
			RectTransform component2 = GetComponent<RectTransform>();
			if (component != null && !(component2 == null))
			{
				float num = component.preferredWidth / component.preferredHeight;
				float num2 = component.preferredWidth;
				float num3 = component.preferredHeight;
				float num4 = 1f;
				while (num2 > maxWidth || num3 > maxHeight)
				{
					num4 = ((!(num2 > maxWidth)) ? (maxHeight / num3) : (maxWidth / num2));
					num3 *= num4;
					num2 *= num4;
				}
				component2.sizeDelta = new Vector2(num2, num3);
			}
		}
	}
}

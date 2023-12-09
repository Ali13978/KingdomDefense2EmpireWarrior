using UnityEngine;
using UnityEngine.UI;

namespace SSR.DevTool
{
	public class TextUiAllCapp : MonoBehaviour
	{
		[ContextMenu("ToAllCap")]
		public void ToAllCap()
		{
			Text component = GetComponent<Text>();
			if (component != null)
			{
				component.text = component.text.ToUpper();
			}
		}
	}
}

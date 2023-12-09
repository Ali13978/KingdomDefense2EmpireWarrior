using UnityEngine;
using UnityEngine.UI;

public class TourTabManager : MonoBehaviour
{
	public Image tabBg;

	public Text tabTitleText;

	[HideInInspector]
	public bool isFocused;

	public void SetFocus(bool isFocused)
	{
		this.isFocused = isFocused;
		if (isFocused)
		{
			tabBg.color = Color.white;
			Color color = tabTitleText.color;
			color.a = 1f;
			tabTitleText.color = color;
		}
		else
		{
			tabBg.color = Color.gray;
			Color color2 = tabTitleText.color;
			color2.a = 0.5f;
			tabTitleText.color = color2;
		}
	}
}

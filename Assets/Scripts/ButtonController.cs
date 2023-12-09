using MyCustom;
using SSR.Core.Architecture;
using UnityEngine;

public class ButtonController : CustomMonoBehaviour
{
	[SerializeField]
	private OrderedEventDispatcher onClick = new OrderedEventDispatcher();

	public virtual void OnClick()
	{
		onClick.Dispatch();
		UISoundManager.Instance.PlayClick();
	}

	public virtual void UpdateButtonStatus()
	{
	}
}

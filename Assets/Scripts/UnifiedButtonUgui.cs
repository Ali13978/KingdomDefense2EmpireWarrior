using SSR;
using UnityEngine.EventSystems;

public class UnifiedButtonUgui : UnifiedButton, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler, IEventSystemHandler
{
	public void OnPointerClick(PointerEventData eventData)
	{
		SetMouseUpAsButton();
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		SSRLog.Log("UnifiedButtonCollider.OnPointerDown");
		SetMouseDown();
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		SetMouseUp();
	}
}

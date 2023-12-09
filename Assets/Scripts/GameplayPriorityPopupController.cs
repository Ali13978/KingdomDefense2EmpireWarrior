using Gameplay;
using UnityEngine;

public class GameplayPriorityPopupController : GameplayPopupController
{
	private bool isInQueue;

	[HideInInspector]
	public PopupPriorityEnum priority;

	private bool isRecyclable;

	private RectTransform rectTrans;

	public virtual void InitPriority(PopupPriorityEnum priority)
	{
		if (rectTrans == null)
		{
			rectTrans = GetComponent<RectTransform>();
		}
		if (rectTrans != null)
		{
			rectTrans.offsetMax = Vector2.zero;
			rectTrans.offsetMin = Vector2.zero;
		}
		AddPriority(priority);
		isRecyclable = true;
	}

	public void AddPriority(PopupPriorityEnum priority)
	{
		if (!isInQueue)
		{
			isInQueue = true;
			this.priority = priority;
			PriorityPopupManager.Instance.AddPopup(this);
		}
	}

	public override void OnCloseAnimationComplete()
	{
		base.OnCloseAnimationComplete();
		if (isInQueue)
		{
			isInQueue = false;
			PriorityPopupManager.Instance.RemoveCurrentPopup(this);
		}
		if (isRecyclable)
		{
			this.Recycle();
		}
	}
}

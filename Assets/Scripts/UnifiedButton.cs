using SSR;
using SSR.Core.Architecture;
using UnityEngine;

public abstract class UnifiedButton : MonoBehaviour
{
	[SerializeField]
	private int sortingOrder;

	[SerializeField]
	private OrderedEventDispatcher onClick = new OrderedEventDispatcher();

	[SerializeField]
	private OrderedEventDispatcher onMouseDown = new OrderedEventDispatcher();

	private bool isHolding;

	private bool clearFirstHoldFrame;

	private static bool hasButtonHold;

	private static bool firstHoldFrame;

	private static int currentHoldButtonOrder;

	public int SortingOrder => sortingOrder;

	protected void SetMouseDown()
	{
		isHolding = true;
		if (!hasButtonHold)
		{
			hasButtonHold = true;
			firstHoldFrame = true;
			currentHoldButtonOrder = sortingOrder;
		}
		else
		{
			currentHoldButtonOrder = Mathf.Max(sortingOrder, currentHoldButtonOrder);
		}
	}

	protected void SetMouseUpAsButton()
	{
		if (currentHoldButtonOrder == sortingOrder)
		{
			onClick.Dispatch();
			hasButtonHold = false;
			isHolding = false;
		}
	}

	protected void SetMouseUp()
	{
		isHolding = false;
		hasButtonHold = false;
	}

	public void Update()
	{
		if (clearFirstHoldFrame)
		{
			firstHoldFrame = false;
			clearFirstHoldFrame = false;
		}
	}

	public void LateUpdate()
	{
		if (firstHoldFrame)
		{
			SSRLog.Log("UnifiedButtonCollider.Update: firstHoldFrame");
			if (sortingOrder == currentHoldButtonOrder && isHolding)
			{
				firstHoldFrame = false;
				onMouseDown.Dispatch();
			}
			clearFirstHoldFrame = true;
		}
	}
}

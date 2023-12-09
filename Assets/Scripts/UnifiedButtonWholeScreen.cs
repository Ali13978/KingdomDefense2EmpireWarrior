using UnityEngine;

public class UnifiedButtonWholeScreen : UnifiedButton
{
	public new void Update()
	{
		base.Update();
		if (Input.GetMouseButtonDown(0))
		{
			SetMouseDown();
		}
		if (Input.GetMouseButtonUp(0))
		{
			SetMouseUpAsButton();
		}
	}
}

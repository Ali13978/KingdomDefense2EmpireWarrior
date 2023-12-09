using SSR;

public class UnifiedButtonCollider : UnifiedButton
{
	public void OnMouseDown()
	{
		SSRLog.Log("UnifiedButtonCollider.OnMouseDown");
		SetMouseDown();
	}

	public void OnMouseUp()
	{
		SetMouseUp();
	}

	public void OnMouseUpAsButton()
	{
		SetMouseUpAsButton();
	}
}

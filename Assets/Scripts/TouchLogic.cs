using UnityEngine;

public class TouchLogic : MonoBehaviour
{
	public static int currTouch;

	[HideInInspector]
	public int touch2Watch = 64;

	private GUITexture guiTexture;

	public void Awake()
	{
		guiTexture = GetComponent<GUITexture>();
	}

	public virtual void Update()
	{
		if (UnityEngine.Input.touchCount <= 0)
		{
			return;
		}
		for (int i = 0; i < UnityEngine.Input.touchCount; i++)
		{
			Touch touch = UnityEngine.Input.GetTouch(i);
			currTouch = touch.fingerId;
			if (guiTexture != null && guiTexture.HitTest(touch.position))
			{
				if (touch.phase == TouchPhase.Began)
				{
					OnTouchBegan();
					touch2Watch = currTouch;
				}
				if (touch.phase == TouchPhase.Ended)
				{
					OnTouchEnded();
				}
				if (touch.phase == TouchPhase.Moved)
				{
					OnTouchMoved();
				}
				if (touch.phase == TouchPhase.Stationary)
				{
					OnTouchStayed();
				}
			}
			switch (touch.phase)
			{
			case TouchPhase.Began:
				OnTouchBeganAnywhere();
				break;
			case TouchPhase.Ended:
				OnTouchEndedAnywhere();
				break;
			case TouchPhase.Moved:
				OnTouchMovedAnywhere();
				break;
			case TouchPhase.Stationary:
				OnTouchStayedAnywhere();
				break;
			}
		}
	}

	public virtual void OnTouchBegan()
	{
	}

	public virtual void OnTouchEnded()
	{
	}

	public virtual void OnTouchMoved()
	{
	}

	public virtual void OnTouchStayed()
	{
	}

	public virtual void OnTouchBeganAnywhere()
	{
	}

	public virtual void OnTouchEndedAnywhere()
	{
	}

	public virtual void OnTouchMovedAnywhere()
	{
	}

	public virtual void OnTouchStayedAnywhere()
	{
	}
}

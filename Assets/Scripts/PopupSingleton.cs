using Gameplay;
using UnityEngine;

public class PopupSingleton : SingletonMonoBehaviour<PopupSingleton>
{
	[HideInInspector]
	public bool isOpen;

	public virtual void Update()
	{
		if (!SingletonMonoBehaviour<GameData>.Instance.IsAnyTutorialPopupOpen)
		{
			UpdateHideIfClickedOutside();
		}
	}

	private void UpdateHideIfClickedOutside()
	{
		if (Input.GetMouseButtonDown(0) && base.gameObject.activeSelf && !RectTransformUtility.RectangleContainsScreenPoint(base.gameObject.GetComponent<RectTransform>(), UnityEngine.Input.mousePosition, Camera.main))
		{
			OnClickOutsideDown();
		}
		if (Input.GetMouseButtonUp(0) && base.gameObject.activeSelf)
		{
			if (RectTransformUtility.RectangleContainsScreenPoint(base.gameObject.GetComponent<RectTransform>(), UnityEngine.Input.mousePosition, Camera.main))
			{
				OnClick();
			}
			else
			{
				OnClickOutsideUp();
			}
		}
	}

	protected virtual void OnClickOutsideUp()
	{
	}

	protected virtual void OnClickOutsideDown()
	{
	}

	protected virtual void OnClick()
	{
	}

	public virtual void Toggle()
	{
		if (isOpen)
		{
			Close();
		}
		else
		{
			Open();
		}
	}

	public virtual void Open()
	{
	}

	public virtual void Close()
	{
	}

	public virtual void OnClickOutSide()
	{
	}
}

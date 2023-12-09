using DG.Tweening;
using Gameplay;
using MyCustom;
using UnityEngine;

public class PopupController : CustomMonoBehaviour, IPopup
{
	[Space]
	[Header("Tween open n close")]
	public Tweener tween;

	public float timeToOpen = 0.2f;

	public float timeToClose = 0.1f;

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

	public virtual void Open()
	{
		isOpen = true;
	}

	public virtual void Close()
	{
		isOpen = false;
	}
}

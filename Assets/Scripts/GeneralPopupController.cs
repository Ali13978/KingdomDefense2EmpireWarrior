using MyCustom;
using UnityEngine;

public class GeneralPopupController : CustomMonoBehaviour
{
	[HideInInspector]
	public bool isOpen;

	public virtual void OnClick()
	{
	}

	public virtual void Open()
	{
		isOpen = true;
		base.gameObject.SetActive(value: true);
		UISoundManager.Instance.PlayOpenPopup();
	}

	public virtual void Close()
	{
		isOpen = false;
		base.gameObject.SetActive(value: false);
		UISoundManager.Instance.PlayClosePopup();
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
}

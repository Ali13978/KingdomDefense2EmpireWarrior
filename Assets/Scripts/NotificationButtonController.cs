using Middle;
using UnityEngine;

public class NotificationButtonController : ButtonController
{
	[SerializeField]
	private GameObject imageOff;

	private bool isOn;

	private void OnEnable()
	{
		isOn = Config.Instance.AllowPushNoti;
		ViewButton();
	}

	public override void OnClick()
	{
		base.OnClick();
		Config.Instance.AllowPushNoti = !Config.Instance.AllowPushNoti;
		isOn = Config.Instance.AllowPushNoti;
		ViewButton();
	}

	private void ViewButton()
	{
		if (isOn)
		{
			imageOff.SetActive(value: false);
		}
		else
		{
			imageOff.SetActive(value: true);
		}
	}
}

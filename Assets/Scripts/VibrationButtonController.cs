using Middle;
using Services.PlatformSpecific;
using UnityEngine;

public class VibrationButtonController : ButtonController
{
	[SerializeField]
	private GameObject imageOff;

	private bool isOn;

	private void OnEnable()
	{
		isOn = Config.Instance.Vibration;
		ViewButton();
	}

	public override void OnClick()
	{
		base.OnClick();
		Config.Instance.Vibration = !Config.Instance.Vibration;
		isOn = Config.Instance.Vibration;
		ViewButton();
		SendEventSetting();
	}

	private void SendEventSetting()
	{
		int num = -1;
		num = (isOn ? 1 : 0);
		PlatformSpecificServicesProvider.Services.Analytics.SendEvent_UserSetting_Vibrate(num);
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

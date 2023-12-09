using Middle;
using Services.PlatformSpecific;
using UnityEngine;

public class SoundButtonController : ButtonController
{
	[SerializeField]
	private GameObject imageOff;

	private bool isOn;

	private void OnEnable()
	{
		isOn = Config.Instance.Sound;
		ViewButton();
	}

	public override void OnClick()
	{
		base.OnClick();
		Config.Instance.Sound = !Config.Instance.Sound;
		isOn = Config.Instance.Sound;
		ViewButton();
		SendEventSetting();
	}

	private void SendEventSetting()
	{
		int num = -1;
		num = (isOn ? 1 : 0);
		PlatformSpecificServicesProvider.Services.Analytics.SendEvent_UserSetting_Sound(num);
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

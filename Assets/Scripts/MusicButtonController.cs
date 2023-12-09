using Middle;
using Services.PlatformSpecific;
using UnityEngine;

public class MusicButtonController : ButtonController
{
	[SerializeField]
	private GameObject imageOff;

	private bool isOn;

	private void OnEnable()
	{
		isOn = Config.Instance.Music;
		ViewButton();
	}

	public override void OnClick()
	{
		base.OnClick();
		Config.Instance.Music = !Config.Instance.Music;
		isOn = Config.Instance.Music;
		ViewButton();
		SendEventSetting();
	}

	private void SendEventSetting()
	{
		int num = -1;
		num = (isOn ? 1 : 0);
		PlatformSpecificServicesProvider.Services.Analytics.SendEvent_UserSetting_Music(num);
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

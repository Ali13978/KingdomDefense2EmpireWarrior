using UnityEngine;

public class OpenFBFanpageButtonController : ButtonController
{
	private void OnEnable()
	{
	}

	public override void OnClick()
	{
		Application.OpenURL(MarketingConfig.fbFanpageLinkWeb);
	}
}

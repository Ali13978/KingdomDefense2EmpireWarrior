using UnityEngine;

public class RateGameButtonController : ButtonController
{
	private void OnEnable()
	{
	}

	public override void OnClick()
	{
		Application.OpenURL(MarketingConfig.rateGameLink);
	}
}

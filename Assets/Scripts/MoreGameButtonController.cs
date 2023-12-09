using UnityEngine;

public class MoreGameButtonController : ButtonController
{
	private void OnEnable()
	{
	}

	public override void OnClick()
	{
		Application.OpenURL(MarketingConfig.moreGameLink);
	}
}

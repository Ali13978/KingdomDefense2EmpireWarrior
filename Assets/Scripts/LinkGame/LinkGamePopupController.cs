using Gameplay;
using UnityEngine;

namespace LinkGame
{
	public class LinkGamePopupController : GameplayPopupController
	{
		public GameObject linkWithoutGems;

		public GameObject linkWithGems;

		public void Init()
		{
			OpenWithScaleAnimation();
		}

		public void DirectLinkToGame()
		{
			UnityEngine.Debug.Log("Link to store!");
			Application.OpenURL(MarketingConfig.goe_trackingLink);
			CloseWithScaleAnimation();
		}
	}
}

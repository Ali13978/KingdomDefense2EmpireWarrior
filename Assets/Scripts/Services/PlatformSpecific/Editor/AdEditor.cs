using UnityEngine;

namespace Services.PlatformSpecific.Editor
{
	public class AdEditor : MonoBehaviour, IAd
	{
		public bool IsOfferVideoAvailable => true;

		public void RequestAds()
		{
			UnityEngine.Debug.Log("Editor Request all ads services!");
		}

		public void ShowInterstitial()
		{
			UnityEngine.Debug.Log("Show Editor Ads Interstitial Success!");
			PlatformSpecificServicesProvider.Services.Analytics.SendEvent_WatchAds("FB/Admob Ads");
		}

		public void ShowOfferVideo(OfferVideoCallback offerVideoCallback)
		{
			if (offerVideoCallback != null)
			{
				offerVideoCallback(completed: true);
				PlatformSpecificServicesProvider.Services.Analytics.SendEvent_WatchAds("Unity Ads");
				PlatformSpecificServicesProvider.Services.Analytics.SendEvent_WatchedVideoReward();
			}
		}
	}
}

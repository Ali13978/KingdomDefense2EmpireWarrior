using ApplicationEntry;
using Data;
using Services.PlatformSpecific;
using UnityEngine;

namespace MainMenu
{
	public class VideoPlayerManager
	{
		private static VideoPlayerManager instance;

		public static VideoPlayerManager Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new VideoPlayerManager();
				}
				return instance;
			}
			set
			{
				instance = value;
			}
		}

		public void TryToShowInterstitialAds_Loading()
		{
			int num = ApplicationEntry.ApplicationEntry.Instance.ReadWriteRemoteSettingData.ChanceToShowInterAds_Loading();
			if (Random.Range(0, 100) < num && ReadWriteData.Instance.GetPlayCount() >= 2)
			{
				PlatformSpecificServicesProvider.Services.Ad.ShowInterstitial();
				UnityEngine.Debug.Log("Show Interstitial Ads");
			}
		}
	}
}

using ApplicationEntry;
using System;
using UnityEngine;

namespace Services.PlatformSpecific.Android
{
	public class AdAndroid : MonoBehaviour, IAd
	{
		[Space]
		[Header("IronSource Ads")]
		[SerializeField]
		private string ironSourceAppID;

		[SerializeField]
		private string ironSourceInterstitialID;

		[SerializeField]
		private string ironSourcevideoRewardID;

		private bool isAudienceLoaded;

		private OfferVideoCallback offerVideoCallback;

		private bool eventRewardedCalled;

		private string keyPrior = "ironsource";

		public bool IsOfferVideoAvailable => IronSource.Agent.isRewardedVideoAvailable();

		private void Start()
		{
			InitAds();
			RequestAds();
		}

		private bool IsAbleToRequestAds()
		{
			bool flag = false;
			return ApplicationEntry.ApplicationEntry.Instance.ReadWriteRemoteSettingData.ChanceToShowInterAds_EndGame() > 0 || ApplicationEntry.ApplicationEntry.Instance.ReadWriteRemoteSettingData.ChanceToShowInterAds_Loading() > 0;
		}

		private void InitAds()
		{
			InitIronSourceAd();
		}

		public void RequestAds()
		{
			if (IsAbleToRequestAds())
			{
				RequestIronSourceInterstitialAds();
			}
		}

		public void ShowInterstitial()
		{
			TryToShow_IronSourceInterstitialAds();
		}

		public void ShowOfferVideo(OfferVideoCallback offerVideoCallback)
		{
			this.offerVideoCallback = offerVideoCallback;
			IronSource.Agent.showRewardedVideo();
		}

		private void InitIronSourceAd()
		{
			IronSource.Agent.init(ironSourceAppID);
			IronSource.Agent.shouldTrackNetworkState(track: true);
			initIronSourceRewardedVideo();
			InitIronSourceInterstitialAds();
		}

		private void InitIronSourceInterstitialAds()
		{
			IronSourceEvents.onInterstitialAdReadyEvent += InterstitialAdReadyEvent;
			IronSourceEvents.onInterstitialAdLoadFailedEvent += InterstitialAdLoadFailedEvent;
			IronSourceEvents.onInterstitialAdShowSucceededEvent += InterstitialAdShowSucceededEvent;
			IronSourceEvents.onInterstitialAdShowFailedEvent += InterstitialAdShowFailedEvent;
			IronSourceEvents.onInterstitialAdClickedEvent += InterstitialAdClickedEvent;
			IronSourceEvents.onInterstitialAdOpenedEvent += InterstitialAdOpenedEvent;
			IronSourceEvents.onInterstitialAdClosedEvent += InterstitialAdClosedEvent;
		}

		private void RequestIronSourceInterstitialAds()
		{
			IronSource.Agent.loadInterstitial();
		}

		private void TryToShow_InterstitialAds()
		{
			try
			{
				if (IronSource.Agent.isInterstitialReady())
				{
					UnityEngine.Debug.Log("Admob ad loaded!");
					ShowIronSourceInterstitialAds();
				}
				else
				{
					UnityEngine.Debug.Log("Admob ad unloaded!");
				}
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogError(ex.ToString());
			}
		}

		private void ShowIronSourceInterstitialAds()
		{
			IronSource.Agent.showInterstitial();
		}

		private void TryToShow_IronSourceInterstitialAds()
		{
			try
			{
				if (IronSource.Agent.isInterstitialReady())
				{
					UnityEngine.Debug.Log("IronSource Interstitial show!");
					ShowIronSourceInterstitialAds();
				}
				else
				{
					UnityEngine.Debug.Log("IronSource Interstitial unloaded!");
				}
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogError(ex.ToString());
			}
		}

		private void InterstitialAdLoadFailedEvent(IronSourceError error)
		{
		}

		private void InterstitialAdShowSucceededEvent()
		{
		}

		private void InterstitialAdShowFailedEvent(IronSourceError error)
		{
		}

		private void InterstitialAdClickedEvent()
		{
		}

		private void InterstitialAdClosedEvent()
		{
			RequestIronSourceInterstitialAds();
		}

		private void InterstitialAdReadyEvent()
		{
		}

		private void InterstitialAdOpenedEvent()
		{
		}

		private void initIronSourceRewardedVideo()
		{
			IronSourceEvents.onRewardedVideoAdOpenedEvent += RewardedVideoAdOpenedEvent;
			IronSourceEvents.onRewardedVideoAdClosedEvent += RewardedVideoAdClosedEvent;
			IronSourceEvents.onRewardedVideoAvailabilityChangedEvent += RewardedVideoAvailabilityChangedEvent;
			IronSourceEvents.onRewardedVideoAdStartedEvent += RewardedVideoAdStartedEvent;
			IronSourceEvents.onRewardedVideoAdEndedEvent += RewardedVideoAdEndedEvent;
			IronSourceEvents.onRewardedVideoAdRewardedEvent += RewardedVideoAdRewardedEvent;
			IronSourceEvents.onRewardedVideoAdShowFailedEvent += RewardedVideoAdShowFailedEvent;
		}

		public void showIronSourceVideoRewarded()
		{
			IronSource.Agent.showRewardedVideo();
		}

		private void RewardedVideoAdOpenedEvent()
		{
			UnityEngine.Debug.Log("RewardedVideoAdOpenedEvent");
		}

		private void RewardedVideoAdClosedEvent()
		{
			UnityEngine.Debug.Log("RewardedVideoAdClosedEvent");
		}

		private void RewardedVideoAvailabilityChangedEvent(bool available)
		{
			UnityEngine.Debug.Log("RewardedVideoAvailabilityChangedEvent");
		}

		private void RewardedVideoAdStartedEvent()
		{
			UnityEngine.Debug.Log("RewardedVideoAdStartedEvent");
		}

		private void RewardedVideoAdEndedEvent()
		{
			UnityEngine.Debug.Log("RewardedVideoAdEndedEvent");
		}

		private void RewardedVideoAdRewardedEvent(IronSourcePlacement placement)
		{
			UnityEngine.Debug.Log("RewardedVideoAdRewardedEvent");
			eventRewardedCalled = true;
		}

		private void RewardedVideoAdShowFailedEvent(IronSourceError error)
		{
			UnityEngine.Debug.Log("RewardedVideoAdShowFailedEvent");
		}

		private void OnApplicationPause(bool isPaused)
		{
			IronSource.Agent.onApplicationPause(isPaused);
		}

		private void Update()
		{
			if (eventRewardedCalled)
			{
				if (offerVideoCallback != null)
				{
					offerVideoCallback(completed: true);
				}
				eventRewardedCalled = false;
			}
		}
	}
}

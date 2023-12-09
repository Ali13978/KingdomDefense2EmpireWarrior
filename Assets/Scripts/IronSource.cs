using System.Collections.Generic;
using UnityEngine;

public class IronSource : IronSourceIAgent
{
	private IronSourceIAgent _platformAgent;

	private static IronSource _instance;

	private const string UNITY_PLUGIN_VERSION = "6.7.9.3";

	public const string GENDER_MALE = "male";

	public const string GENDER_FEMALE = "female";

	public const string GENDER_UNKNOWN = "unknown";

	public static IronSource Agent
	{
		get
		{
			if (_instance == null)
			{
				_instance = new IronSource();
			}
			return _instance;
		}
	}

	private IronSource()
	{
		_platformAgent = new AndroidAgent();
	}

	public static string pluginVersion()
	{
		return "6.7.9.3";
	}

	public static string unityVersion()
	{
		return Application.unityVersion;
	}

	public void reportAppStarted()
	{
		_platformAgent.reportAppStarted();
	}

	public void onApplicationPause(bool pause)
	{
		_platformAgent.onApplicationPause(pause);
	}

	public void setAge(int age)
	{
		_platformAgent.setAge(age);
	}

	public void setGender(string gender)
	{
		if (gender.Equals("male"))
		{
			_platformAgent.setGender("male");
		}
		else if (gender.Equals("female"))
		{
			_platformAgent.setGender("female");
		}
		else if (gender.Equals("unknown"))
		{
			_platformAgent.setGender("unknown");
		}
	}

	public void setMediationSegment(string segment)
	{
		_platformAgent.setMediationSegment(segment);
	}

	public string getAdvertiserId()
	{
		return _platformAgent.getAdvertiserId();
	}

	public void validateIntegration()
	{
		_platformAgent.validateIntegration();
	}

	public void shouldTrackNetworkState(bool track)
	{
		_platformAgent.shouldTrackNetworkState(track);
	}

	public bool setDynamicUserId(string dynamicUserId)
	{
		return _platformAgent.setDynamicUserId(dynamicUserId);
	}

	public void setAdaptersDebug(bool enabled)
	{
		_platformAgent.setAdaptersDebug(enabled);
	}

	public void setUserId(string userId)
	{
		_platformAgent.setUserId(userId);
	}

	public void init(string appKey)
	{
		_platformAgent.init(appKey);
	}

	public void init(string appKey, params string[] adUnits)
	{
		_platformAgent.init(appKey, adUnits);
	}

	public void initISDemandOnly(string appKey, params string[] adUnits)
	{
		_platformAgent.initISDemandOnly(appKey, adUnits);
	}

	public void showRewardedVideo()
	{
		_platformAgent.showRewardedVideo();
	}

	public void showRewardedVideo(string placementName)
	{
		_platformAgent.showRewardedVideo(placementName);
	}

	public IronSourcePlacement getPlacementInfo(string placementName)
	{
		return _platformAgent.getPlacementInfo(placementName);
	}

	public bool isRewardedVideoAvailable()
	{
		return _platformAgent.isRewardedVideoAvailable();
	}

	public bool isRewardedVideoPlacementCapped(string placementName)
	{
		return _platformAgent.isRewardedVideoPlacementCapped(placementName);
	}

	public void setRewardedVideoServerParams(Dictionary<string, string> parameters)
	{
		_platformAgent.setRewardedVideoServerParams(parameters);
	}

	public void clearRewardedVideoServerParams()
	{
		_platformAgent.clearRewardedVideoServerParams();
	}

	public void showISDemandOnlyRewardedVideo(string instanceId)
	{
		_platformAgent.showISDemandOnlyRewardedVideo(instanceId);
	}

	public void showISDemandOnlyRewardedVideo(string instanceId, string placementName)
	{
		_platformAgent.showISDemandOnlyRewardedVideo(instanceId, placementName);
	}

	public bool isISDemandOnlyRewardedVideoAvailable(string instanceId)
	{
		return _platformAgent.isISDemandOnlyRewardedVideoAvailable(instanceId);
	}

	public void loadInterstitial()
	{
		_platformAgent.loadInterstitial();
	}

	public void showInterstitial()
	{
		_platformAgent.showInterstitial();
	}

	public void showInterstitial(string placementName)
	{
		_platformAgent.showInterstitial(placementName);
	}

	public bool isInterstitialReady()
	{
		return _platformAgent.isInterstitialReady();
	}

	public bool isInterstitialPlacementCapped(string placementName)
	{
		return _platformAgent.isInterstitialPlacementCapped(placementName);
	}

	public void loadISDemandOnlyInterstitial(string instanceId)
	{
		_platformAgent.loadISDemandOnlyInterstitial(instanceId);
	}

	public void showISDemandOnlyInterstitial(string instanceId)
	{
		_platformAgent.showISDemandOnlyInterstitial(instanceId);
	}

	public void showISDemandOnlyInterstitial(string instanceId, string placementName)
	{
		_platformAgent.showISDemandOnlyInterstitial(instanceId, placementName);
	}

	public bool isISDemandOnlyInterstitialReady(string instanceId)
	{
		return _platformAgent.isISDemandOnlyInterstitialReady(instanceId);
	}

	public void showOfferwall()
	{
		_platformAgent.showOfferwall();
	}

	public void showOfferwall(string placementName)
	{
		_platformAgent.showOfferwall(placementName);
	}

	public void getOfferwallCredits()
	{
		_platformAgent.getOfferwallCredits();
	}

	public bool isOfferwallAvailable()
	{
		return _platformAgent.isOfferwallAvailable();
	}

	public void loadBanner(IronSourceBannerSize size, IronSourceBannerPosition position)
	{
		_platformAgent.loadBanner(size, position);
	}

	public void loadBanner(IronSourceBannerSize size, IronSourceBannerPosition position, string placementName)
	{
		_platformAgent.loadBanner(size, position, placementName);
	}

	public void destroyBanner()
	{
		_platformAgent.destroyBanner();
	}

	public void displayBanner()
	{
		_platformAgent.displayBanner();
	}

	public void hideBanner()
	{
		_platformAgent.hideBanner();
	}

	public bool isBannerPlacementCapped(string placementName)
	{
		return _platformAgent.isBannerPlacementCapped(placementName);
	}

	public void setSegment(IronSourceSegment segment)
	{
		_platformAgent.setSegment(segment);
	}

	public void setConsent(bool consent)
	{
		_platformAgent.setConsent(consent);
	}
}

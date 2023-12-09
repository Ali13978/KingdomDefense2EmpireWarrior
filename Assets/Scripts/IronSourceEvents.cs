using IronSourceJSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IronSourceEvents : MonoBehaviour
{
	private const string ERROR_CODE = "error_code";

	private const string ERROR_DESCRIPTION = "error_description";

	private const string INSTANCE_ID_KEY = "instanceId";

	private const string PLACEMENT_KEY = "placement";

	private static event Action<IronSourceError> _onRewardedVideoAdShowFailedEvent;

	public static event Action<IronSourceError> onRewardedVideoAdShowFailedEvent;

	private static event Action _onRewardedVideoAdOpenedEvent;

	public static event Action onRewardedVideoAdOpenedEvent;

	private static event Action _onRewardedVideoAdClosedEvent;

	public static event Action onRewardedVideoAdClosedEvent;

	private static event Action _onRewardedVideoAdStartedEvent;

	public static event Action onRewardedVideoAdStartedEvent;

	private static event Action _onRewardedVideoAdEndedEvent;

	public static event Action onRewardedVideoAdEndedEvent;

	private static event Action<IronSourcePlacement> _onRewardedVideoAdRewardedEvent;

	public static event Action<IronSourcePlacement> onRewardedVideoAdRewardedEvent;

	private static event Action<IronSourcePlacement> _onRewardedVideoAdClickedEvent;

	public static event Action<IronSourcePlacement> onRewardedVideoAdClickedEvent;

	private static event Action<bool> _onRewardedVideoAvailabilityChangedEvent;

	public static event Action<bool> onRewardedVideoAvailabilityChangedEvent;

	private static event Action<string, bool> _onRewardedVideoAvailabilityChangedDemandOnlyEvent;

	public static event Action<string, bool> onRewardedVideoAvailabilityChangedDemandOnlyEvent;

	private static event Action<string> _onRewardedVideoAdOpenedDemandOnlyEvent;

	public static event Action<string> onRewardedVideoAdOpenedDemandOnlyEvent;

	private static event Action<string> _onRewardedVideoAdClosedDemandOnlyEvent;

	public static event Action<string> onRewardedVideoAdClosedDemandOnlyEvent;

	private static event Action<string, IronSourcePlacement> _onRewardedVideoAdRewardedDemandOnlyEvent;

	public static event Action<string, IronSourcePlacement> onRewardedVideoAdRewardedDemandOnlyEvent;

	private static event Action<string, IronSourceError> _onRewardedVideoAdShowFailedDemandOnlyEvent;

	public static event Action<string, IronSourceError> onRewardedVideoAdShowFailedDemandOnlyEvent;

	private static event Action<string, IronSourcePlacement> _onRewardedVideoAdClickedDemandOnlyEvent;

	public static event Action<string, IronSourcePlacement> onRewardedVideoAdClickedDemandOnlyEvent;

	private static event Action _onInterstitialAdReadyEvent;

	public static event Action onInterstitialAdReadyEvent;

	private static event Action<IronSourceError> _onInterstitialAdLoadFailedEvent;

	public static event Action<IronSourceError> onInterstitialAdLoadFailedEvent;

	private static event Action _onInterstitialAdOpenedEvent;

	public static event Action onInterstitialAdOpenedEvent;

	private static event Action _onInterstitialAdClosedEvent;

	public static event Action onInterstitialAdClosedEvent;

	private static event Action _onInterstitialAdShowSucceededEvent;

	public static event Action onInterstitialAdShowSucceededEvent;

	private static event Action<IronSourceError> _onInterstitialAdShowFailedEvent;

	public static event Action<IronSourceError> onInterstitialAdShowFailedEvent;

	private static event Action _onInterstitialAdClickedEvent;

	public static event Action onInterstitialAdClickedEvent;

	private static event Action<string> _onInterstitialAdReadyDemandOnlyEvent;

	public static event Action<string> onInterstitialAdReadyDemandOnlyEvent;

	private static event Action<string, IronSourceError> _onInterstitialAdLoadFailedDemandOnlyEvent;

	public static event Action<string, IronSourceError> onInterstitialAdLoadFailedDemandOnlyEvent;

	private static event Action<string> _onInterstitialAdOpenedDemandOnlyEvent;

	public static event Action<string> onInterstitialAdOpenedDemandOnlyEvent;

	private static event Action<string> _onInterstitialAdClosedDemandOnlyEvent;

	public static event Action<string> onInterstitialAdClosedDemandOnlyEvent;

	private static event Action<string> _onInterstitialAdShowSucceededDemandOnlyEvent;

	public static event Action<string> onInterstitialAdShowSucceededDemandOnlyEvent;

	private static event Action<string, IronSourceError> _onInterstitialAdShowFailedDemandOnlyEvent;

	public static event Action<string, IronSourceError> onInterstitialAdShowFailedDemandOnlyEvent;

	private static event Action<string> _onInterstitialAdClickedDemandOnlyEvent;

	public static event Action<string> onInterstitialAdClickedDemandOnlyEvent;

	private static event Action _onInterstitialAdRewardedEvent;

	public static event Action onInterstitialAdRewardedEvent;

	private static event Action _onOfferwallOpenedEvent;

	public static event Action onOfferwallOpenedEvent;

	private static event Action<IronSourceError> _onOfferwallShowFailedEvent;

	public static event Action<IronSourceError> onOfferwallShowFailedEvent;

	private static event Action _onOfferwallClosedEvent;

	public static event Action onOfferwallClosedEvent;

	private static event Action<IronSourceError> _onGetOfferwallCreditsFailedEvent;

	public static event Action<IronSourceError> onGetOfferwallCreditsFailedEvent;

	private static event Action<Dictionary<string, object>> _onOfferwallAdCreditedEvent;

	public static event Action<Dictionary<string, object>> onOfferwallAdCreditedEvent;

	private static event Action<bool> _onOfferwallAvailableEvent;

	public static event Action<bool> onOfferwallAvailableEvent;

	private static event Action _onBannerAdLoadedEvent;

	public static event Action onBannerAdLoadedEvent;

	private static event Action<IronSourceError> _onBannerAdLoadFailedEvent;

	public static event Action<IronSourceError> onBannerAdLoadFailedEvent;

	private static event Action _onBannerAdClickedEvent;

	public static event Action onBannerAdClickedEvent;

	private static event Action _onBannerAdScreenPresentedEvent;

	public static event Action onBannerAdScreenPresentedEvent;

	private static event Action _onBannerAdScreenDismissedEvent;

	public static event Action onBannerAdScreenDismissedEvent;

	private static event Action _onBannerAdLeftApplicationEvent;

	public static event Action onBannerAdLeftApplicationEvent;

	private static event Action<string> _onSegmentReceivedEvent;

	public static event Action<string> onSegmentReceivedEvent;

	private void Awake()
	{
		base.gameObject.name = "IronSourceEvents";
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	public void onRewardedVideoAdShowFailed(string description)
	{
		if (IronSourceEvents._onRewardedVideoAdShowFailedEvent != null)
		{
			IronSourceError errorFromErrorObject = getErrorFromErrorObject(description);
			IronSourceEvents._onRewardedVideoAdShowFailedEvent(errorFromErrorObject);
		}
	}

	public void onRewardedVideoAdOpened(string empty)
	{
		if (IronSourceEvents._onRewardedVideoAdOpenedEvent != null)
		{
			IronSourceEvents._onRewardedVideoAdOpenedEvent();
		}
	}

	public void onRewardedVideoAdClosed(string empty)
	{
		if (IronSourceEvents._onRewardedVideoAdClosedEvent != null)
		{
			IronSourceEvents._onRewardedVideoAdClosedEvent();
		}
	}

	public void onRewardedVideoAdStarted(string empty)
	{
		if (IronSourceEvents._onRewardedVideoAdStartedEvent != null)
		{
			IronSourceEvents._onRewardedVideoAdStartedEvent();
		}
	}

	public void onRewardedVideoAdEnded(string empty)
	{
		if (IronSourceEvents._onRewardedVideoAdEndedEvent != null)
		{
			IronSourceEvents._onRewardedVideoAdEndedEvent();
		}
	}

	public void onRewardedVideoAdRewarded(string description)
	{
		if (IronSourceEvents._onRewardedVideoAdRewardedEvent != null)
		{
			IronSourcePlacement placementFromObject = getPlacementFromObject(description);
			IronSourceEvents._onRewardedVideoAdRewardedEvent(placementFromObject);
		}
	}

	public void onRewardedVideoAdClicked(string description)
	{
		if (IronSourceEvents._onRewardedVideoAdClickedEvent != null)
		{
			IronSourcePlacement placementFromObject = getPlacementFromObject(description);
			IronSourceEvents._onRewardedVideoAdClickedEvent(placementFromObject);
		}
	}

	public void onRewardedVideoAvailabilityChanged(string stringAvailable)
	{
		bool obj = (stringAvailable == "true") ? true : false;
		if (IronSourceEvents._onRewardedVideoAvailabilityChangedEvent != null)
		{
			IronSourceEvents._onRewardedVideoAvailabilityChangedEvent(obj);
		}
	}

	public void onRewardedVideoAvailabilityChangedDemandOnly(string args)
	{
		if (IronSourceEvents._onRewardedVideoAvailabilityChangedDemandOnlyEvent != null && !string.IsNullOrEmpty(args))
		{
			List<object> list = Json.Deserialize(args) as List<object>;
			bool arg = (list[1].ToString().ToLower() == "true") ? true : false;
			string arg2 = list[0].ToString();
			IronSourceEvents._onRewardedVideoAvailabilityChangedDemandOnlyEvent(arg2, arg);
		}
	}

	public void onRewardedVideoAdOpenedDemandOnly(string instanceId)
	{
		if (IronSourceEvents._onRewardedVideoAdOpenedDemandOnlyEvent != null)
		{
			IronSourceEvents._onRewardedVideoAdOpenedDemandOnlyEvent(instanceId);
		}
	}

	public void onRewardedVideoAdClosedDemandOnly(string instanceId)
	{
		if (IronSourceEvents._onRewardedVideoAdClosedDemandOnlyEvent != null)
		{
			IronSourceEvents._onRewardedVideoAdClosedDemandOnlyEvent(instanceId);
		}
	}

	public void onRewardedVideoAdRewardedDemandOnly(string args)
	{
		if (IronSourceEvents._onRewardedVideoAdRewardedDemandOnlyEvent != null && !string.IsNullOrEmpty(args))
		{
			List<object> list = Json.Deserialize(args) as List<object>;
			string arg = list[0].ToString();
			IronSourcePlacement placementFromObject = getPlacementFromObject(list[1]);
			IronSourceEvents._onRewardedVideoAdRewardedDemandOnlyEvent(arg, placementFromObject);
		}
	}

	public void onRewardedVideoAdShowFailedDemandOnly(string args)
	{
		if (IronSourceEvents._onRewardedVideoAdShowFailedDemandOnlyEvent != null && !string.IsNullOrEmpty(args))
		{
			List<object> list = Json.Deserialize(args) as List<object>;
			IronSourceError errorFromErrorObject = getErrorFromErrorObject(list[1]);
			string arg = list[0].ToString();
			IronSourceEvents._onRewardedVideoAdShowFailedDemandOnlyEvent(arg, errorFromErrorObject);
		}
	}

	public void onRewardedVideoAdClickedDemandOnly(string args)
	{
		if (IronSourceEvents._onRewardedVideoAdClickedDemandOnlyEvent != null && !string.IsNullOrEmpty(args))
		{
			List<object> list = Json.Deserialize(args) as List<object>;
			string arg = list[0].ToString();
			IronSourcePlacement placementFromObject = getPlacementFromObject(list[1]);
			IronSourceEvents._onRewardedVideoAdClickedDemandOnlyEvent(arg, placementFromObject);
		}
	}

	public void onInterstitialAdReady()
	{
		if (IronSourceEvents._onInterstitialAdReadyEvent != null)
		{
			IronSourceEvents._onInterstitialAdReadyEvent();
		}
	}

	public void onInterstitialAdLoadFailed(string description)
	{
		if (IronSourceEvents._onInterstitialAdLoadFailedEvent != null)
		{
			IronSourceError errorFromErrorObject = getErrorFromErrorObject(description);
			IronSourceEvents._onInterstitialAdLoadFailedEvent(errorFromErrorObject);
		}
	}

	public void onInterstitialAdOpened(string empty)
	{
		if (IronSourceEvents._onInterstitialAdOpenedEvent != null)
		{
			IronSourceEvents._onInterstitialAdOpenedEvent();
		}
	}

	public void onInterstitialAdClosed(string empty)
	{
		if (IronSourceEvents._onInterstitialAdClosedEvent != null)
		{
			IronSourceEvents._onInterstitialAdClosedEvent();
		}
	}

	public void onInterstitialAdShowSucceeded(string empty)
	{
		if (IronSourceEvents._onInterstitialAdShowSucceededEvent != null)
		{
			IronSourceEvents._onInterstitialAdShowSucceededEvent();
		}
	}

	public void onInterstitialAdShowFailed(string description)
	{
		if (IronSourceEvents._onInterstitialAdShowFailedEvent != null)
		{
			IronSourceError errorFromErrorObject = getErrorFromErrorObject(description);
			IronSourceEvents._onInterstitialAdShowFailedEvent(errorFromErrorObject);
		}
	}

	public void onInterstitialAdClicked(string empty)
	{
		if (IronSourceEvents._onInterstitialAdClickedEvent != null)
		{
			IronSourceEvents._onInterstitialAdClickedEvent();
		}
	}

	public void onInterstitialAdReadyDemandOnly(string instanceId)
	{
		if (IronSourceEvents._onInterstitialAdReadyDemandOnlyEvent != null)
		{
			IronSourceEvents._onInterstitialAdReadyDemandOnlyEvent(instanceId);
		}
	}

	public void onInterstitialAdLoadFailedDemandOnly(string args)
	{
		if (IronSourceEvents._onInterstitialAdLoadFailedDemandOnlyEvent != null && !string.IsNullOrEmpty(args))
		{
			List<object> list = Json.Deserialize(args) as List<object>;
			IronSourceError errorFromErrorObject = getErrorFromErrorObject(list[1]);
			string arg = list[0].ToString();
			IronSourceEvents._onInterstitialAdLoadFailedDemandOnlyEvent(arg, errorFromErrorObject);
		}
	}

	public void onInterstitialAdOpenedDemandOnly(string instanceId)
	{
		if (IronSourceEvents._onInterstitialAdOpenedDemandOnlyEvent != null)
		{
			IronSourceEvents._onInterstitialAdOpenedDemandOnlyEvent(instanceId);
		}
	}

	public void onInterstitialAdClosedDemandOnly(string instanceId)
	{
		if (IronSourceEvents._onInterstitialAdClosedDemandOnlyEvent != null)
		{
			IronSourceEvents._onInterstitialAdClosedDemandOnlyEvent(instanceId);
		}
	}

	public void onInterstitialAdShowSucceededDemandOnly(string instanceId)
	{
		if (IronSourceEvents._onInterstitialAdShowSucceededDemandOnlyEvent != null)
		{
			IronSourceEvents._onInterstitialAdShowSucceededDemandOnlyEvent(instanceId);
		}
	}

	public void onInterstitialAdShowFailedDemandOnly(string args)
	{
		if (IronSourceEvents._onInterstitialAdLoadFailedDemandOnlyEvent != null && !string.IsNullOrEmpty(args))
		{
			List<object> list = Json.Deserialize(args) as List<object>;
			IronSourceError errorFromErrorObject = getErrorFromErrorObject(list[1]);
			string arg = list[0].ToString();
			IronSourceEvents._onInterstitialAdShowFailedDemandOnlyEvent(arg, errorFromErrorObject);
		}
	}

	public void onInterstitialAdClickedDemandOnly(string instanceId)
	{
		if (IronSourceEvents._onInterstitialAdClickedDemandOnlyEvent != null)
		{
			IronSourceEvents._onInterstitialAdClickedDemandOnlyEvent(instanceId);
		}
	}

	public void onInterstitialAdRewarded(string empty)
	{
		if (IronSourceEvents._onInterstitialAdRewardedEvent != null)
		{
			IronSourceEvents._onInterstitialAdRewardedEvent();
		}
	}

	public void onOfferwallOpened(string empty)
	{
		if (IronSourceEvents._onOfferwallOpenedEvent != null)
		{
			IronSourceEvents._onOfferwallOpenedEvent();
		}
	}

	public void onOfferwallShowFailed(string description)
	{
		if (IronSourceEvents._onOfferwallShowFailedEvent != null)
		{
			IronSourceError errorFromErrorObject = getErrorFromErrorObject(description);
			IronSourceEvents._onOfferwallShowFailedEvent(errorFromErrorObject);
		}
	}

	public void onOfferwallClosed(string empty)
	{
		if (IronSourceEvents._onOfferwallClosedEvent != null)
		{
			IronSourceEvents._onOfferwallClosedEvent();
		}
	}

	public void onGetOfferwallCreditsFailed(string description)
	{
		if (IronSourceEvents._onGetOfferwallCreditsFailedEvent != null)
		{
			IronSourceError errorFromErrorObject = getErrorFromErrorObject(description);
			IronSourceEvents._onGetOfferwallCreditsFailedEvent(errorFromErrorObject);
		}
	}

	public void onOfferwallAdCredited(string json)
	{
		if (IronSourceEvents._onOfferwallAdCreditedEvent != null)
		{
			IronSourceEvents._onOfferwallAdCreditedEvent(Json.Deserialize(json) as Dictionary<string, object>);
		}
	}

	public void onOfferwallAvailable(string stringAvailable)
	{
		bool obj = (stringAvailable == "true") ? true : false;
		if (IronSourceEvents._onOfferwallAvailableEvent != null)
		{
			IronSourceEvents._onOfferwallAvailableEvent(obj);
		}
	}

	public void onBannerAdLoaded()
	{
		if (IronSourceEvents._onBannerAdLoadedEvent != null)
		{
			IronSourceEvents._onBannerAdLoadedEvent();
		}
	}

	public void onBannerAdLoadFailed(string description)
	{
		if (IronSourceEvents._onBannerAdLoadFailedEvent != null)
		{
			IronSourceError errorFromErrorObject = getErrorFromErrorObject(description);
			IronSourceEvents._onBannerAdLoadFailedEvent(errorFromErrorObject);
		}
	}

	public void onBannerAdClicked()
	{
		if (IronSourceEvents._onBannerAdClickedEvent != null)
		{
			IronSourceEvents._onBannerAdClickedEvent();
		}
	}

	public void onBannerAdScreenPresented()
	{
		if (IronSourceEvents._onBannerAdScreenPresentedEvent != null)
		{
			IronSourceEvents._onBannerAdScreenPresentedEvent();
		}
	}

	public void onBannerAdScreenDismissed()
	{
		if (IronSourceEvents._onBannerAdScreenDismissedEvent != null)
		{
			IronSourceEvents._onBannerAdScreenDismissedEvent();
		}
	}

	public void onBannerAdLeftApplication()
	{
		if (IronSourceEvents._onBannerAdLeftApplicationEvent != null)
		{
			IronSourceEvents._onBannerAdLeftApplicationEvent();
		}
	}

	public void onSegmentReceived(string segmentName)
	{
		if (IronSourceEvents._onSegmentReceivedEvent != null)
		{
			IronSourceEvents._onSegmentReceivedEvent(segmentName);
		}
	}

	private IronSourceError getErrorFromErrorObject(object descriptionObject)
	{
		Dictionary<string, object> dictionary = null;
		if (descriptionObject is IDictionary)
		{
			dictionary = (descriptionObject as Dictionary<string, object>);
		}
		else if (descriptionObject is string && !string.IsNullOrEmpty(descriptionObject.ToString()))
		{
			dictionary = (Json.Deserialize(descriptionObject.ToString()) as Dictionary<string, object>);
		}
		IronSourceError result = new IronSourceError(-1, string.Empty);
		if (dictionary != null && dictionary.Count > 0)
		{
			int errorCode = Convert.ToInt32(dictionary["error_code"].ToString());
			string errorDescription = dictionary["error_description"].ToString();
			result = new IronSourceError(errorCode, errorDescription);
		}
		return result;
	}

	private IronSourcePlacement getPlacementFromObject(object placementObject)
	{
		Dictionary<string, object> dictionary = null;
		if (placementObject is IDictionary)
		{
			dictionary = (placementObject as Dictionary<string, object>);
		}
		else if (placementObject is string)
		{
			dictionary = (Json.Deserialize(placementObject.ToString()) as Dictionary<string, object>);
		}
		IronSourcePlacement result = null;
		if (dictionary != null && dictionary.Count > 0)
		{
			int rewardAmount = Convert.ToInt32(dictionary["placement_reward_amount"].ToString());
			string rewardName = dictionary["placement_reward_name"].ToString();
			string placementName = dictionary["placement_name"].ToString();
			result = new IronSourcePlacement(placementName, rewardName, rewardAmount);
		}
		return result;
	}
}

using Firebase.Analytics;
using UnityEngine;

public class FirebaseAnalyticsAndroid : MonoBehaviour
{
	public void FirebaseInit()
	{
		UnityEngine.Debug.Log("Enabling data collection.");
		FirebaseAnalytics.SetAnalyticsCollectionEnabled(enabled: true);
	}

	public void LogEvent(string eventName)
	{
		FirebaseAnalytics.LogEvent(eventName);
	}

	public void LogEventBeginCheckout(decimal value, string currency)
	{
		FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventBeginCheckout, new Firebase.Analytics.Parameter(FirebaseAnalytics.ParameterValue, value.ToString()), new Firebase.Analytics.Parameter(FirebaseAnalytics.ParameterCurrency, currency));
	}
}

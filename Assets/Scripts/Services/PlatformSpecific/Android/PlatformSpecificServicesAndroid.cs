using UnityEngine;

namespace Services.PlatformSpecific.Android
{
	public class PlatformSpecificServicesAndroid : MonoBehaviour, IPlatformSpecificServices
	{
		[SerializeField]
		private AnalyticsAndroid analyticsAndroid;

		[SerializeField]
		private InappPurchaseAndroid inappPurchaseAndroid;

		[SerializeField]
		private FacebookServicesAndroid facebookServicesAndroid;

		[SerializeField]
		private AdAndroid adAndroid;

		[SerializeField]
		private UserProfileAndroid userProfileAndroid;

		[SerializeField]
		private DataCloudSaverAndroid dataCloudSaverAndroid;

		[SerializeField]
		private NotificationAndroid notificationAndroid;

		public IAnalytics Analytics => analyticsAndroid;

		public IInappPurchase InApPurchase => inappPurchaseAndroid;

		public IFacebookServices FacebookServices => facebookServicesAndroid;

		public IAd Ad => adAndroid;

		public IUserProfile UserProfile => userProfileAndroid;

		public IDataCloudSaver DataCloudSaver => dataCloudSaverAndroid;

		public INotification GameNotification => notificationAndroid;

		public string StoreLink => "market://details?id=com.zonmob.HeroLegend.KingdomDefense.TowerGame";

		private void Awake()
		{
			if (PlatformSpecificServicesProvider.Services != null)
			{
				UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			Object.DontDestroyOnLoad(base.gameObject);
			PlatformSpecificServicesProvider.Services = this;
		}
	}
}

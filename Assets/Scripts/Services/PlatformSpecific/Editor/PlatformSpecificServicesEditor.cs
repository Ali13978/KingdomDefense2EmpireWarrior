using UnityEngine;

namespace Services.PlatformSpecific.Editor
{
	public class PlatformSpecificServicesEditor : MonoBehaviour, IPlatformSpecificServices
	{
		[SerializeField]
		private AnalyticsEditor analyticsEditor;

		[SerializeField]
		private InappPurchaseEditor inappPurchaseEditor;

		[SerializeField]
		private FacebookServicesEditor facebookServicesEditor;

		[SerializeField]
		private AdEditor adEditor;

		[SerializeField]
		private UserProfileEditor userProfileEditor;

		[SerializeField]
		private DataCloudSaverEditor dataCloudSaverEditor;

		[SerializeField]
		private NotificationEditor gameNotificationEditor;

		public IAnalytics Analytics => analyticsEditor;

		public IInappPurchase InApPurchase => inappPurchaseEditor;

		public IFacebookServices FacebookServices => facebookServicesEditor;

		public IAd Ad => adEditor;

		public IUserProfile UserProfile => userProfileEditor;

		public IDataCloudSaver DataCloudSaver => dataCloudSaverEditor;

		public INotification GameNotification => gameNotificationEditor;

		public string StoreLink => "market://details?id=com.zonmob.HeroLegend.KingdomDefense.TowerGame";

		public void Awake()
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

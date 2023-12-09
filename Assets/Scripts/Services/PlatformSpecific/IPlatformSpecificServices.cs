namespace Services.PlatformSpecific
{
	public interface IPlatformSpecificServices
	{
		string StoreLink
		{
			get;
		}

		IAnalytics Analytics
		{
			get;
		}

		IInappPurchase InApPurchase
		{
			get;
		}

		IFacebookServices FacebookServices
		{
			get;
		}

		IAd Ad
		{
			get;
		}

		IUserProfile UserProfile
		{
			get;
		}

		IDataCloudSaver DataCloudSaver
		{
			get;
		}

		INotification GameNotification
		{
			get;
		}
	}
}

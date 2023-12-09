using Data;

namespace Services.PlatformSpecific
{
	public class DataRestoreDeliver
	{
		public UserData_Hero userData_Hero;

		public UserData_GlobalUpgrade userData_GlobalUpgrade;

		public UserData_Map userData_Map;

		public UserData_Theme userData_Theme;

		public UserData_PowerupItem userData_PowerupItem;

		public UserData_UserProfile userData_UserProfile;

		public UserData_Tutorial userData_Tutorial;

		public UserData_DailyTrial userData_DailyTrial;

		public UserData_Offer userData_Offer;

		public UserData_FreeResources userData_FreeResources;

		public UserData_SaleBundle userData_SaleBundle;

		public UserData_DailyReward userData_DailyReward;

		public void DispatchToAllDataWriter(DataRestoreDeliver data)
		{
			ReadWriteDataHero.Instance.RestoreDataFromCloud(data.userData_Hero);
			ReadWriteDataGlobalUpgrade.Instance.RestoreDataFromCloud(data.userData_GlobalUpgrade);
			ReadWriteDataMap.Instance.RestoreDataFromCloud(data.userData_Map);
			ReadWriteDataTheme.Instance.RestoreDataFromCloud(data.userData_Theme);
			ReadWriteDataPowerUpItem.Instance.RestoreDataFromCloud(data.userData_PowerupItem);
			ReadWriteDataUserProfile.Instance.RestoreDataFromCloud(data.userData_UserProfile);
			ReadWriteDataPlayerCurrency.Instance.RestoreDataFromCloud(data.userData_UserProfile);
			ReadWriteDataTutorial.Instance.RestoreDataFromCloud(data.userData_Tutorial);
			ReadWriteDataDailyTrial.Instance.RestoreDataFromCloud(data.userData_DailyTrial);
			ReadWriteDataOffers.Instance.RestoreDataFromCloud(data.userData_Offer);
			ReadWriteDataFreeResources.Instance.RestoreDataFromCloud(data.userData_FreeResources);
			ReadWriteDataSaleBundle.Instance.RestoreDataFromCloud(data.userData_SaleBundle);
			ReadWriteDataDailyReward.Instance.RestoreDataFromCloud(data.userData_DailyReward);
		}
	}
}

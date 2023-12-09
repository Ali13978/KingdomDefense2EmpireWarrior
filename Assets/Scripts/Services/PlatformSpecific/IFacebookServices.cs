namespace Services.PlatformSpecific
{
	public interface IFacebookServices
	{
		void LogIn();

		void LogOut();

		void LikeFanpage();

		void InviteFriend();

		void ShareFanpage();

		void ShareLinkGame(SceneName sceneName, int currentMapID);

		void SharePromotionImage(int imageID);

		void ShareScreenShot();

		void InviteToGroup();

		int GetFreeResources(string rewardID);

		void SendEvent_GetFreeResourcesComplete(FreeResourcesType freeResourcesType);
	}
}

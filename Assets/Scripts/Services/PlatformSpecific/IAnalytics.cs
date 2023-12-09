namespace Services.PlatformSpecific
{
	public interface IAnalytics
	{
		void SendEvent_OpenSceneMainMenu();

		void SendEvent_OpenSceneWorldMap(int totalGem, int totalStar, int maxMapIDUnlocked);

		void SendEvent_OpenGlobalUpgrade(int remainingStar, int totalStar);

		void SendEvent_OpenHeroCamp(int totalGem, int maxMapIDUnlocked);

		void SendEvent_OpenStore(int totalGem, int maxMapIDUnlocked);

		void SendEvent_OpenGuide(int maxMapIDUnlocked);

		void SendEvent_OpenMapLevelSelect(int mapID);

		void SendEvent_ChooseHeroAtMapLevelSelect(string heroName);

		void SendEvent_StartGame_MapLevel(int mapID);

		void SendEvent_BuyPowerupItem(string itemName, int itemPrice);

		void SendEvent_ResetGlobalUpgrade(int remainingStar, int totalStar);

		void SendEvent_UnlockedHero(string heroName);

		void SendEvent_ClickButtonBuyHero();

		void SendEvent_BoughtHero(int heroID, string heroName, int previousGem, int currentGem, int maxMapIDUnlocked, int heroOwnedAmount);

		void SendEvent_UpgradeHeroLevel(string heroName, int previousGem, int currentGem, int currentHeroLevel, int maxMapIDUnlocked, int heroOwnedAmount);

		void SendEvent_StartGame(int mapID, string hero1Name, int hero1Level, string hero2Name, int hero2Level, string hero3Name, int hero3Level, int puItem0Quantity, int puItem1Quantity, int puItem2Quantity, int puItem3Quantity);

		void SendEvent_EndGame();

		void SendEvent_EndGame(int mapID, int starEarned, int gemCollectInMap, int playedAmount);

		void SendEvent_RestartGame_Setting(int mapID, int previousStarEarned);

		void SendEvent_RestartGame_EndGame(int mapID, int previousStarEarned);

		void SendEvent_CallEarlyEnemies(int mapID, int currentWave, string callType);

		void SendEvent_ShowTipsButton(string tipType, int id);

		void SendEvent_OpenTipsPopup(string tipType, int id);

		void SendEvent_UserSetting_Music(int music);

		void SendEvent_UserSetting_Sound(int sound);

		void SendEvent_UserSetting_Vibrate(int vibrate);

		void SendEvent_DoneTutorial(int step);

		void SendEvent_BuyItem(string productID);

		void SendEvent_UsePowerupItem(string itemName);

		void SendEvent_WatchGameplayVideoRewardComplete(int currentMapID, string rewardType);

		void SendEvent_ShareLinkGameComplete(SceneName sceneName, int mapID);

		void SendEvent_GetFreeResourcesComplete(FreeResourcesType freeResourcesType, int currentMapIDUnlocked);

		void SendEvent_FreeChestOffer(int mapID, FreeChestOfferType type, int offerCount);

		void SendEvent_EndGameDailyTrial(int currentDay, int wavePassed, int playCount, int mapIDCampaignUnlocked);

		void SendEvent_UnlockPet(int heroOwnedAmount, int petOwnedAmount, string petBoughtName);

		void SendEvent_WatchAds(string adsName);

		void SendEvent_WatchedVideoReward();

		void SendEvent_EndGameTournament();

		void SendEvent_ReceiveFriendReward();

		void SendEvent_CompleteEvent(int eventId);

		void SendEvent_BeginCheckout(decimal value, string currency);
	}
}

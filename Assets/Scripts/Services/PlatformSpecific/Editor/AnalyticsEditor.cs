using UnityEngine;

namespace Services.PlatformSpecific.Editor
{
	public class AnalyticsEditor : MonoBehaviour, IAnalytics
	{
		public void SendEvent_OpenGlobalUpgrade(int remainingStar, int totalStar)
		{
			UnityEngine.Debug.Log($"AnalyticsEditor.SendEvent: Open GlobalUpgrade: remainingStar: {remainingStar}, totalStar: {totalStar}");
		}

		public void SendEvent_OpenGuide(int maxMapIDUnlocked)
		{
			UnityEngine.Debug.Log($"AnalyticsEditor.SendEvent: Open Guide: max mapID Unlocked: {maxMapIDUnlocked}");
		}

		public void SendEvent_OpenHeroCamp(int totalGem, int maxMapIDUnlocked)
		{
			UnityEngine.Debug.Log($"AnalyticsEditor.SendEvent: Open HeroCamp: totalGem: {totalGem}, max mapID Unlocked: {maxMapIDUnlocked}");
		}

		public void SendEvent_OpenStore(int totalGem, int maxMapIDUnlocked)
		{
			UnityEngine.Debug.Log($"AnalyticsEditor.SendEvent: Open Store: totalGem: {totalGem}, max mapID Unlocked: {maxMapIDUnlocked}");
		}

		public void SendEvent_OpenSceneMainMenu()
		{
			UnityEngine.Debug.Log($"AnalyticsEditor.SendEvent: Open Scene MainMenu");
		}

		public void SendEvent_OpenSceneWorldMap(int totalGem, int totalStar, int maxMapIDUnlocked)
		{
			UnityEngine.Debug.Log($"AnalyticsEditor.SendEvent: Open Scene WorldMap: totalGem: {totalGem}, totalStar: {totalStar}, maxMapIdUnlocked: {maxMapIDUnlocked}");
		}

		public void SendEvent_OpenMapLevelSelect(int mapID)
		{
			UnityEngine.Debug.Log($"AnalyticsEditor.SendEvent: Open Map level select at mapID : {mapID}");
		}

		public void SendEvent_ChooseHeroAtMapLevelSelect(string heroName)
		{
			UnityEngine.Debug.Log($"AnalyticsEditor.SendEvent: Map level choose hero : {heroName}");
		}

		public void SendEvent_StartGame_MapLevel(int mapID)
		{
			UnityEngine.Debug.Log($"AnalyticsEditor.SendEvent: Map level start at mapID: {mapID}");
		}

		public void SendEvent_BuyPowerupItem(string itemName, int itemPrice)
		{
			UnityEngine.Debug.Log($"AnalyticsEditor.SendEvent: Buy Powerup Item: item name: {itemName}, price: {itemPrice}");
		}

		public void SendEvent_ResetGlobalUpgrade(int remainingStar, int totalStar)
		{
		}

		public void SendEvent_UnlockedHero(string heroName)
		{
			UnityEngine.Debug.Log($"AnalyticsEditor.SendEvent: Unlock Hero: {heroName}");
		}

		public void SendEvent_ClickButtonBuyHero()
		{
			UnityEngine.Debug.Log("AnalyticsEditor.SendEvent: Click button buy Hero");
		}

		public void SendEvent_BoughtHero(int heroID, string heroName, int previousGem, int currentGem, int maxMapIDUnlocked, int heroOwnedAmount)
		{
			UnityEngine.Debug.Log($"AnalyticsEditor.SendEvent: Bought Hero ID {heroID} {heroName}, previous gem: {previousGem}, current gem: {currentGem}, max mapID unlocked: {maxMapIDUnlocked}, hero owned amound: {heroOwnedAmount}");
		}

		public void SendEvent_UpgradeHeroLevel(string heroName, int previousGem, int currentGem, int currentHeroLevel, int maxMapIDUnlocked, int heroOwnedAmount)
		{
			UnityEngine.Debug.Log($"AnalyticsEditor.SendEvent: Upgrade level Hero: {heroName}, previous gem: {previousGem}, current gem: {currentGem}, current hero level: {currentHeroLevel}, max mapID unlocked : {maxMapIDUnlocked}, hero owned amount : {heroOwnedAmount}");
		}

		public void SendEvent_StartGame(int mapID, string hero1Name, int hero1Level, string hero2Name, int hero2Level, string hero3Name, int hero3Level, int puItem0Quantity, int puItem1Quantity, int puItem2Quantity, int puItem3Quantity)
		{
			UnityEngine.Debug.Log("AnalyticsEditor.SendEvent: Start Game:Map " + mapID + "\n hero1 name " + hero1Name + " hero1 level " + hero1Level + "\n hero2 name " + hero2Name + " hero2 level " + hero2Level + "\n hero3 name " + hero3Name + " hero3 level " + hero3Name + "\n item quantity = " + puItem0Quantity + "_" + puItem1Quantity + "_" + puItem2Quantity + "_" + puItem3Quantity);
		}

		public void SendEvent_EndGame(int mapID, int starEarned, int gemCollectInMap, int playedAmount)
		{
			UnityEngine.Debug.Log("AnalyticsEditor.SendEvent: End Game:Map " + mapID + "\n Star Earned = " + starEarned + "\n Gem collected = " + gemCollectInMap + "\n Played count = " + playedAmount);
		}

		public void SendEvent_RestartGame_Setting(int mapID, int previousStarEarned)
		{
			UnityEngine.Debug.Log($"AnalyticsEditor.SendEvent: Restart Game at Setting: mapID: {mapID}, previous star earned: {previousStarEarned}");
		}

		public void SendEvent_RestartGame_EndGame(int mapID, int previousStarEarned)
		{
			UnityEngine.Debug.Log($"AnalyticsEditor.SendEvent: Restart Game at End Game: mapID: {mapID}, previous star earned: {previousStarEarned}");
		}

		public void SendEvent_CallEarlyEnemies(int mapID, int currentWave, string callType)
		{
		}

		public void SendEvent_ShowTipsButton(string tipType, int id)
		{
		}

		public void SendEvent_OpenTipsPopup(string tipType, int id)
		{
		}

		public void SendEvent_UserSetting_Music(int music)
		{
			UnityEngine.Debug.Log($"AnalyticsEditor.SendEvent: User Setting Music: {music}");
		}

		public void SendEvent_UserSetting_Sound(int sound)
		{
			UnityEngine.Debug.Log($"AnalyticsEditor.SendEvent: User Setting Sound: {sound}");
		}

		public void SendEvent_UserSetting_Vibrate(int vibrate)
		{
			UnityEngine.Debug.Log($"AnalyticsEditor.SendEvent: User Setting Vibrate: {vibrate}");
		}

		public void SendEvent_DoneTutorial(int step)
		{
			UnityEngine.Debug.Log($"AnalyticsEditor.SendEvent: Done Tut Start Game Step: {step}");
		}

		public void SendEvent_BuyItem(string productID)
		{
			UnityEngine.Debug.Log($"AnalyticsEditor.SendEvent: Buy Item: {productID}");
		}

		public void SendEvent_UsePowerupItem(string itemName)
		{
			UnityEngine.Debug.Log($"AnalyticsEditor.SendEvent: User Item: {itemName}");
		}

		public void SendEvent_WatchGameplayVideoRewardComplete(int currentMapID, string rewardType)
		{
			UnityEngine.Debug.Log($"AnalyticsEditor.SendEvent: Get video reward : {rewardType} at map : {currentMapID}");
		}

		public void SendEvent_ShareLinkGameComplete(SceneName sceneName, int mapID)
		{
			UnityEngine.Debug.Log($"AnalyticsEditor.SendEvent: Share link game complete at scene: {sceneName.ToString()} mapID : {mapID}");
		}

		public void SendEvent_GetFreeResourcesComplete(FreeResourcesType freeResourcesType, int maxMapIDUnlocked)
		{
			UnityEngine.Debug.Log($"AnalyticsEditor.SendEvent: Get free gem Type: {freeResourcesType.ToString()} mapID unlocked : {maxMapIDUnlocked}");
		}

		public void SendEvent_FreeChestOffer(int mapID, FreeChestOfferType type, int offerCount)
		{
			UnityEngine.Debug.Log($"AnalyticsEditor.SendEvent:Offer Free Chest: mapID - {mapID} , kiểu offer - {type.ToString()} , lần thứ - {offerCount} ");
		}

		public void SendEvent_EndGameDailyTrial(int currentDay, int wavePassed, int playCount, int mapIDCampaignUnlocked)
		{
			UnityEngine.Debug.Log("AnalyticsEditor.SendEvent: End Game Daily Trial:Current day = " + currentDay + "\n wavePassed = " + wavePassed + "\n Playcount = " + playCount + "\n mapIdUnlock = " + mapIDCampaignUnlocked);
		}

		public void SendEvent_UnlockPet(int heroOwnedAmount, int petOwnedAmount, string petBoughtName)
		{
			UnityEngine.Debug.Log("AnalyticsEditor.SendEvent: Unlock Pet:Hero Owned Amount = " + heroOwnedAmount + "\n pet Owned Amount = " + petOwnedAmount + "\n pet Bought Name = " + petBoughtName);
		}

		public void SendEvent_WatchAds(string adsName)
		{
			UnityEngine.Debug.Log("AnalyticsEditor.SendEvent: Watch Ads:Ads name = " + adsName);
		}

		public void SendEvent_WatchedVideoReward()
		{
			UnityEngine.Debug.Log("AnalyticsEditor.SendEvent: Watch Video Reward");
		}

		public void SendEvent_EndGame()
		{
			UnityEngine.Debug.Log("AnalyticsEditor.SendEvent: End Game Campaign");
		}

		public void SendEvent_EndGameTournament()
		{
			UnityEngine.Debug.Log("AnalyticsEditor.SendEvent: End Game Tournament");
		}

		public void SendEvent_CompleteEvent(int eventId)
		{
			UnityEngine.Debug.Log("AnalyticsEditor.SendEvent: complete quest");
		}

		public void SendEvent_ReceiveFriendReward()
		{
			UnityEngine.Debug.Log("AnalyticsEditor.SendEvent: ReceiveFriendReward");
		}

		public void SendEvent_BeginCheckout(decimal value, string currency)
		{
			UnityEngine.Debug.Log("AnalyticsEditor.SendEvent: Begin Checkout  Value = " + value + " Currency = " + currency);
		}
	}
}

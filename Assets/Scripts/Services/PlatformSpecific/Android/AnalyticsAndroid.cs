using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

namespace Services.PlatformSpecific.Android
{
	public class AnalyticsAndroid : MonoBehaviour, IAnalytics
	{
		[SerializeField]
		private FirebaseAnalyticsAndroid firebaseAnalyticsAndroid;

		public void SendEvent_OpenGlobalUpgrade(int remainingStar, int totalStar)
		{
			Analytics.CustomEvent("Open Global Upgrade", new Dictionary<string, object>
			{
				{
					"Remaining Star",
					remainingStar
				},
				{
					"Total Star",
					totalStar
				}
			});
		}

		public void SendEvent_OpenGuide(int maxMapIDUnlocked)
		{
			Analytics.CustomEvent("Open Guide", new Dictionary<string, object>
			{
				{
					"Max Map ID Unlocked",
					maxMapIDUnlocked
				}
			});
		}

		public void SendEvent_OpenHeroCamp(int totalGem, int maxMapIDUnlocked)
		{
			Analytics.CustomEvent("Open HeroCamp", new Dictionary<string, object>
			{
				{
					"Current Gem",
					totalGem
				},
				{
					"Max Map ID Unlocked",
					maxMapIDUnlocked
				}
			});
		}

		public void SendEvent_OpenSceneMainMenu()
		{
			Analytics.CustomEvent("Open Main Menu");
		}

		public void SendEvent_OpenSceneWorldMap(int totalGem, int totalStar, int maxMapIDUnlocked)
		{
			Analytics.CustomEvent("Open WorldMap", new Dictionary<string, object>
			{
				{
					"Current Gem",
					totalGem
				},
				{
					"Total Star",
					totalStar
				},
				{
					"Max Map ID Unlocked",
					maxMapIDUnlocked
				}
			});
		}

		public void SendEvent_OpenStore(int totalGem, int maxMapIDUnlocked)
		{
			Analytics.CustomEvent("Open Store", new Dictionary<string, object>
			{
				{
					"Current Gem",
					totalGem
				},
				{
					"Max Map ID Unlocked",
					maxMapIDUnlocked
				}
			});
		}

		public void SendEvent_OpenMapLevelSelect(int mapID)
		{
			Analytics.CustomEvent("Open Map Level Select", new Dictionary<string, object>
			{
				{
					"Current Map ID",
					mapID
				}
			});
		}

		public void SendEvent_ChooseHeroAtMapLevelSelect(string heroName)
		{
			Analytics.CustomEvent("Select Hero At Map Level", new Dictionary<string, object>
			{
				{
					"Hero Name",
					heroName
				}
			});
		}

		public void SendEvent_StartGame_MapLevel(int mapID)
		{
			Analytics.CustomEvent("Start Game At Map Level", new Dictionary<string, object>
			{
				{
					"Current Map ID",
					mapID
				}
			});
		}

		public void SendEvent_OpenTipsPopup(string tipType, int id)
		{
		}

		public void SendEvent_ResetGlobalUpgrade(int remainingStar, int totalStar)
		{
		}

		public void SendEvent_RestartGame_EndGame(int mapID, int previousStarEarned)
		{
			Analytics.CustomEvent("Replay Game At EndGame", new Dictionary<string, object>
			{
				{
					"Map ID",
					mapID
				},
				{
					"Previous Star Earned",
					previousStarEarned
				}
			});
		}

		public void SendEvent_RestartGame_Setting(int mapID, int previousStarEarned)
		{
			Analytics.CustomEvent("Replay Game In Setting", new Dictionary<string, object>
			{
				{
					"Map ID",
					mapID
				},
				{
					"Previous Star Earned",
					previousStarEarned
				}
			});
		}

		public void SendEvent_ShowTipsButton(string tipType, int id)
		{
		}

		public void SendEvent_StartGame(int mapID, string hero1Name, int hero1Level, string hero2Name, int hero2Level, string hero3Name, int hero3Level, int puItem0Quantity, int puItem1Quantity, int puItem2Quantity, int puItem3Quantity)
		{
			Analytics.CustomEvent("Start Game " + mapID, new Dictionary<string, object>
			{
				{
					"Hero 1 Name",
					hero1Name
				},
				{
					"Hero 1 Level",
					hero1Level
				},
				{
					"Hero 2 Name",
					hero2Name
				},
				{
					"Hero 2 Level",
					hero2Level
				},
				{
					"Hero 3 Name",
					hero3Name
				},
				{
					"Hero 3 Level",
					hero3Level
				},
				{
					"Item Frost Gem Quantity",
					puItem0Quantity
				},
				{
					"Item Meteor Strike Quantity",
					puItem1Quantity
				},
				{
					"Item Healing Ward Quantity",
					puItem2Quantity
				},
				{
					"Item Gold Chest Quantity",
					puItem3Quantity
				}
			});
		}

		public void SendEvent_UnlockedHero(string heroName)
		{
			Analytics.CustomEvent("Unlock Hero", new Dictionary<string, object>
			{
				{
					"Hero Name",
					heroName
				}
			});
		}

		public void SendEvent_UpgradeHeroLevel(string heroName, int previousGem, int currentGem, int currentHeroLevel, int maxMapIDUnlocked, int heroOwnedAmount)
		{
			Analytics.CustomEvent("Upgrade Hero Level", new Dictionary<string, object>
			{
				{
					"Hero Name",
					heroName
				},
				{
					"Previous Gem",
					previousGem
				},
				{
					"Current Gem",
					currentGem
				},
				{
					"Current Hero Level",
					currentHeroLevel
				},
				{
					"Max MapId Unlocked",
					maxMapIDUnlocked
				},
				{
					"Hero Owned Amount",
					heroOwnedAmount
				}
			});
		}

		public void SendEvent_BoughtHero(int heroID, string heroName, int previousGem, int currentGem, int maxMapIDUnlocked, int heroOwnedAmount)
		{
			Analytics.CustomEvent("Bought Hero", new Dictionary<string, object>
			{
				{
					"Hero ID",
					heroID
				},
				{
					"Hero Name",
					heroName
				},
				{
					"Previous Gem",
					previousGem
				},
				{
					"Current Gem",
					currentGem
				},
				{
					"Max MapId Unlocked",
					maxMapIDUnlocked
				},
				{
					"Hero Owned Amount",
					heroOwnedAmount
				}
			});
		}

		public void SendEvent_BuyPowerupItem(string itemName, int itemPrice)
		{
			Analytics.CustomEvent("Bought Powerup Item", new Dictionary<string, object>
			{
				{
					"Item Name",
					itemName
				},
				{
					"Item Price",
					itemPrice
				}
			});
		}

		public void SendEvent_CallEarlyEnemies(int mapID, int currentWave, string callType)
		{
		}

		public void SendEvent_EndGame(int mapID, int starEarned, int gemCollectInMap, int playedAmount)
		{
			Analytics.CustomEvent("End Game " + mapID, new Dictionary<string, object>
			{
				{
					"Star Earned",
					starEarned
				},
				{
					"Gem Collected",
					gemCollectInMap
				},
				{
					"Played Amount",
					playedAmount
				}
			});
		}

		public void SendEvent_TimeLoading(int timeLoadingMilisecond)
		{
			Analytics.CustomEvent("Scene Loading Time", new Dictionary<string, object>
			{
				{
					"Total Time in Milisecond",
					timeLoadingMilisecond
				}
			});
		}

		public void SendEvent_UserSetting_Music(int music)
		{
			Analytics.CustomEvent("Music Setting", new Dictionary<string, object>
			{
				{
					"Set",
					music
				}
			});
		}

		public void SendEvent_UserSetting_Sound(int sound)
		{
			Analytics.CustomEvent("Sound Setting", new Dictionary<string, object>
			{
				{
					"Set",
					sound
				}
			});
		}

		public void SendEvent_UserSetting_Vibrate(int vibrate)
		{
			Analytics.CustomEvent("Vibrate Setting", new Dictionary<string, object>
			{
				{
					"Set",
					vibrate
				}
			});
		}

		public void SendEvent_DoneTutorial(int step)
		{
			Analytics.CustomEvent("Start Game Tutorial", new Dictionary<string, object>
			{
				{
					"Step",
					step
				}
			});
		}

		public void SendEvent_BuyItem(string productID)
		{
			Analytics.CustomEvent("Purchase Item", new Dictionary<string, object>
			{
				{
					"Product ID",
					productID
				}
			});
			PlatformSpecificServicesProvider.Services.UserProfile.TakePremiumUserInfor();
		}

		public void SendEvent_UsePowerupItem(string itemName)
		{
			Analytics.CustomEvent("Use Item", new Dictionary<string, object>
			{
				{
					"Item Name",
					itemName
				}
			});
		}

		public void SendEvent_WatchGameplayVideoRewardComplete(int currentMapID, string rewardType)
		{
			Analytics.CustomEvent("Watch Gameplay Video Reward Completed!", new Dictionary<string, object>
			{
				{
					"Current mapID",
					currentMapID
				},
				{
					"Reward Type",
					rewardType
				}
			});
		}

		public void SendEvent_ShareLinkGameComplete(SceneName sceneName, int mapID)
		{
			Analytics.CustomEvent("Share Link Game Completed!", new Dictionary<string, object>
			{
				{
					"Scene name",
					sceneName.ToString()
				},
				{
					"Current mapID",
					mapID
				}
			});
		}

		public void SendEvent_GetFreeResourcesComplete(FreeResourcesType freeResourcesType, int currentMapIDUnlocked)
		{
			Analytics.CustomEvent("Get Free Resouces Completed!", new Dictionary<string, object>
			{
				{
					"Resource type",
					freeResourcesType.ToString()
				},
				{
					"Current mapID Unlocked",
					currentMapIDUnlocked
				}
			});
		}

		public void SendEvent_FreeChestOffer(int mapID, FreeChestOfferType type, int offerCount)
		{
			Analytics.CustomEvent("Free chest offer!", new Dictionary<string, object>
			{
				{
					"Map ID",
					mapID
				},
				{
					"Offer type",
					type.ToString()
				},
				{
					"Turn",
					offerCount
				}
			});
		}

		public void SendEvent_EndGameDailyTrial(int currentDay, int wavePassed, int playCount, int mapIDCampaignUnlocked)
		{
			Analytics.CustomEvent("End Game Daily Trial Day: " + currentDay, new Dictionary<string, object>
			{
				{
					"Wave passed",
					wavePassed
				},
				{
					"Play count",
					playCount
				},
				{
					"Map ID Campaign Unlocked",
					mapIDCampaignUnlocked
				}
			});
		}

		public void SendEvent_UnlockPet(int heroOwnedAmount, int petOwnedAmount, string petBoughtName)
		{
			Analytics.CustomEvent("Unlock pet!", new Dictionary<string, object>
			{
				{
					"Hero owned amount",
					heroOwnedAmount
				},
				{
					"Pet owned amount",
					petOwnedAmount
				},
				{
					"Pet name",
					petBoughtName
				}
			});
		}

		public void SendEvent_WatchAds(string adsName)
		{
			Analytics.CustomEvent("Watch Ads", new Dictionary<string, object>
			{
				{
					"Ads Provider Type",
					adsName
				}
			});
		}

		public void SendEvent_ClickButtonBuyHero()
		{
			firebaseAnalyticsAndroid.LogEvent("Click button Buy Hero");
		}

		public void SendEvent_EndGame()
		{
			firebaseAnalyticsAndroid.LogEvent("End Game Campaign Mode");
		}

		public void SendEvent_WatchedVideoReward()
		{
			firebaseAnalyticsAndroid.LogEvent("Watch Video Reward");
		}

		public void SendEvent_EndGameTournament()
		{
			firebaseAnalyticsAndroid.LogEvent("End Game Tournament");
		}

		public void SendEvent_ReceiveFriendReward()
		{
			firebaseAnalyticsAndroid.LogEvent("Receive Friend Reward");
		}

		public void SendEvent_BeginCheckout(decimal value, string currency)
		{
			firebaseAnalyticsAndroid.LogEventBeginCheckout(value, currency);
		}

		public void SendEvent_CompleteEvent(int eventId)
		{
		}
	}
}

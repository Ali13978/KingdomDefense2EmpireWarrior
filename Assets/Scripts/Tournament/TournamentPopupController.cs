using Data;
using FreeResources;
using Gameplay;
using LifetimePopup;
using MyCustom;
using Newtonsoft.Json;
using Parameter;
using Services.PlatformSpecific;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WorldMap;

namespace Tournament
{
	public class TournamentPopupController : GameplayPopupController
	{
		[SerializeField]
		private StartGamePopupController startGamePopupController;

		private GameplayPopupController tournamentRulePopup;

		private TourReceiveRewardPopup tourReceiveRewardPopup;

		public GameplayPopupController tourRulePopupPrefab;

		public TourReceiveRewardPopup tourReceiveRewardPrefab;

		private TourAllPrizePopupController tourAllPrizePopup;

		public TourAllPrizePopupController allprizePopupPrefab;

		public TourTabManager groupTab;

		public TourTabManager friendTab;

		public GameObject friendRewardNotify;

		public Text seasonInfoText;

		public Text seasonTimeLeftText;

		public TourRankEntryManager sampleRankEntry;

		public RectTransform scrollContent;

		public RectTransform scrollHandle;

		public Text leagueTitleText;

		public List<GameObject> leagueIcons;

		public TourRankPrizeManager curPrizeBlock;

		public TourRankPrizeManager nextPrizeBlock;

		public Image blessedHeroIcon;

		public Text gemQuantityToStartGameText;

		public GameObject availableAdBtnBg;

		public GameObject unavailableAdBtnBg;

		public Text nextFreeEntryText;

		public float heightOfRankEntry;

		public GameObject loadingIcon;

		public GameObject prepareObject;

		public GameObject inviteFbObj;

		public GameObject loginFbObj;

		public Text inviteFriendDescText;

		public Text requireLoginFBDescText;

		private List<TourRankEntryManager> rankEntries = new List<TourRankEntryManager>();

		private List<TourUsers_UserData> friendsUserData = new List<TourUsers_UserData>();

		private List<TourPlayerInfo> groupPlayers;

		private List<TourPlayerInfo> friendPlayers;

		private TourSeasonInfo seasonInfo;

		private TourUserSelfInfo userSelfInfo;

		private float countdownUpdate;

		private float loadingTimeoutCountdown;

		private bool isStartTourWithAdsAvai;

		private DateTime nextFreeEntryTime;

		private bool isInited;

		private bool isSeasonInfoRetrieved;

		private bool isUserSelfInfoRetrieved;

		private bool isGroupPlayerRetrieved;

		private bool isBlessedHeroRetrieved;

		private bool isPriceConstantsRetrieved;

		private bool notifyFlag;

		private string notifyContent = string.Empty;

		private Action notifyCallback;

		public static string NEXT_FREE_TIME_KEY = "NEXTFTKEY";

		public static string END_SEASON_TIME_KEY = "ENDSSTKEY";

		private int maxFriendNumber = 20;

		private bool isReadingFbFriendScores;

		public void Init()
		{
			OpenWithScaleAnimation();
			countdownUpdate = 50f;
			RectTransform rectTransform = scrollHandle;
			Vector2 offsetMin = scrollHandle.offsetMin;
			rectTransform.offsetMin = new Vector2(offsetMin.x, 0f);
			RectTransform rectTransform2 = scrollHandle;
			Vector2 offsetMax = scrollHandle.offsetMax;
			rectTransform2.offsetMax = new Vector2(offsetMax.x, 0f);
			if (!isInited)
			{
				isUserSelfInfoRetrieved = false;
				isGroupPlayerRetrieved = false;
				isBlessedHeroRetrieved = false;
				isSeasonInfoRetrieved = false;
				isPriceConstantsRetrieved = false;
				notifyFlag = false;
				loadingTimeoutCountdown = 20f;
				prepareObject.SetActive(value: true);
				friendRewardNotify.SetActive(value: false);
				GameEventCenter.Instance.Subscribe(GameEventType.OnTournamentMapRuleReceived, new SimpleSubscriberData(GameTools.GetUniqueId(), OnFinishReadingTournamentMapRule));
				GameEventCenter.Instance.Subscribe(GameEventType.OnTournamentPriceConstantsReceived, new SimpleSubscriberData(GameTools.GetUniqueId(), OnFinishReadingTournamentPriceConstants));
				rankEntries.Add(sampleRankEntry);
				WorldMapData.Instance.ReadDataMapRule.ReadTournamentMapRule();
				WorldMapData.Instance.ReadDataMapRule.ReadTournamentPriceConstants();
				ReadSeasonInfoData();
				ReadUserSelfData();
			}
			else
			{
				prepareObject.SetActive(value: false);
			}
			OnGroupTabClicked();
			isStartTourWithAdsAvai = IsStartTourWithAdsAvailable();
			availableAdBtnBg.SetActive(isStartTourWithAdsAvai);
			unavailableAdBtnBg.SetActive(!isStartTourWithAdsAvai);
			SendEvent_EndGameTournament();
		}

		private void SendEvent_EndGameTournament()
		{
			PlatformSpecificServicesProvider.Services.Analytics.SendEvent_EndGameTournament();
		}

		public override void Update()
		{
			base.Update();
			if (!isInited)
			{
				if (loadingTimeoutCountdown > 0f)
				{
					loadingTimeoutCountdown -= Time.deltaTime;
					if (loadingTimeoutCountdown <= 0f)
					{
						CloseWithScaleAnimation();
						SingletonMonoBehaviour<LifetimeCanvas>.Instance.NotifyPopupController.Init("No response from server. Please check your connection and come back later!", "OK", null);
					}
				}
				if (!isUserSelfInfoRetrieved || !isGroupPlayerRetrieved || !isBlessedHeroRetrieved || !isSeasonInfoRetrieved || !isPriceConstantsRetrieved)
				{
					return;
				}
				loadingTimeoutCountdown = -1f;
				if (notifyFlag)
				{
					if (!Debug.isDebugBuild)
					{
						CloseWithScaleAnimation();
					}
					else
					{
						isInited = true;
						SetupUI();
					}
					SingletonMonoBehaviour<LifetimeCanvas>.Instance.NotifyPopupController.Init(notifyContent, "OK", notifyCallback);
				}
				else
				{
					isInited = true;
					SetupUI();
				}
			}
			countdownUpdate -= Time.deltaTime;
			if (countdownUpdate <= 0f)
			{
				countdownUpdate = 50f;
				SetupSeasonTimeLeft();
				SetupNextFreeEntryTimeLeft();
			}
		}

		private void SetupUI()
		{
			prepareObject.SetActive(value: false);
			MapRuleParameter.Instance.SetCurrentSeasonID(seasonInfo.seasonNumber);
			GameTools.blessedHeroId = MapRuleParameter.Instance.GetBlessedHeroID();
			GameTools.SetRewardSprite(new RewardItem(RewardType.SingleHero, GameTools.blessedHeroId, 1, isDisplayQuantity: false), blessedHeroIcon);
			seasonInfoText.text = string.Format("{0} : {1}", seasonInfo.seasonNumber + 1, seasonInfo.seasonStartDate.ToString("dd.MM.yyyy"));
			SetupSeasonTimeLeft();
			GameTools.WriteTimeStamp(END_SEASON_TIME_KEY, seasonInfo.seasonEndDate);
			SetupCurrentLeague(userSelfInfo);
			nextFreeEntryTime = GetNextFreeEntryTime();
			SetupNextFreeEntryTimeLeft();
			gemQuantityToStartGameText.text = GameTools.GetGemQuantityTostartTour(increasePlayCount: false).ToString();
			if (userSelfInfo.recSeasonReward < seasonInfo.seasonNumber - 1)
			{
				OnReceiveLastSeasonReward();
			}
			if (userSelfInfo.recFriendReward < seasonInfo.seasonNumber - 1 && PlatformSpecificServicesProvider.Services.UserProfile.IsLoggedIn_Facebook())
			{
				friendRewardNotify.SetActive(value: true);
			}
			int yourIndex = GetYourIndex(groupPlayers);
			if (!groupPlayers[yourIndex].isYou)
			{
				groupPlayers.Add(new TourPlayerInfo(userSelfInfo.name, ReadWriteDataHero.Instance.GetListHeroIDOwned(), new TimeSpan(0L), isYou: true, userSelfInfo.countryCode));
				SortList(groupPlayers);
			}
			if (groupTab.isFocused)
			{
				OnGroupTabClicked();
			}
			SetupCurrentPrizeAndNextPrize();
			List<TournamentPrizeConfigData> leagueAllPrize = GameTools.GetLeagueAllPrize(1000);
			int num = leagueAllPrize[0].Itemquantities[0];
			inviteFriendDescText.text = string.Format(GameTools.GetLocalization("INVITE_FRIEND_REQUIRE"), num);
			requireLoginFBDescText.text = string.Format(GameTools.GetLocalization("FB_LOGIN_REQUIRE"), num);
			UnityEngine.Debug.LogFormat("+++userselfinfo tier: {0} gameTools {1}", userSelfInfo.curtier, GameTools.tourUserSelfInfo.curtier);
			GameEventCenter.Instance.Trigger(GameEventType.OnTournamentTierUp, null);
		}

		public void SetupCurrentLeague(TourUserSelfInfo userData)
		{
			int curtier = userData.curtier;
			leagueTitleText.text = GameTools.GetLocalization("LEAGUE_" + curtier);
			for (int num = leagueIcons.Count - 1; num >= 0; num--)
			{
				leagueIcons[num].SetActive(value: false);
			}
			leagueIcons[curtier].SetActive(value: true);
		}

		public void SetupCurrentPrizeAndNextPrize()
		{
			List<RewardItem> curReward = null;
			List<RewardItem> nextReward = null;
			GetPrizeData(out int curLowerRange, out int curUpperRange, out curReward, out int nextLowerRange, out int nextUpperRange, out nextReward);
			curPrizeBlock.Init(curUpperRange, curLowerRange, curReward);
			nextPrizeBlock.Init(nextUpperRange, nextLowerRange, nextReward);
		}

		public void SetupSeasonTimeLeft()
		{
			if (isSeasonInfoRetrieved)
			{
				TimeSpan timeSpan = seasonInfo.seasonEndDate - GetNow();
				seasonTimeLeftText.text = $"{timeSpan.Days}d{timeSpan.Hours}h{timeSpan.Minutes}m";
			}
		}

		public void SetupNextFreeEntryTimeLeft()
		{
			if (isUserSelfInfoRetrieved)
			{
				TimeSpan timeSpan = nextFreeEntryTime - GetNow();
				if (timeSpan.TotalSeconds < 0.0)
				{
					nextFreeEntryText.text = string.Empty;
					GameTools.SaveNumOfPlayTourCount(0);
				}
				else
				{
					nextFreeEntryText.text = string.Format(GameTools.GetLocalization("FREE_ENTRY_TEXT"), $"{timeSpan.Hours}h{timeSpan.Minutes}m");
				}
			}
		}

		public void SetupLeaderboard(List<TourPlayerInfo> players)
		{
			if (players != null)
			{
				int count = players.Count;
				RectTransform rectTransform = scrollContent;
				Vector2 sizeDelta = scrollContent.sizeDelta;
				rectTransform.sizeDelta = new Vector2(sizeDelta.x, (float)count * heightOfRankEntry + 50f);
				for (int i = rankEntries.Count; i < count; i++)
				{
					TourRankEntryManager tourRankEntryManager = UnityEngine.Object.Instantiate(sampleRankEntry, sampleRankEntry.transform.parent);
					tourRankEntryManager.transform.localPosition = sampleRankEntry.transform.localPosition + new Vector3(0f, (float)(-i) * heightOfRankEntry, 0f);
					rankEntries.Add(tourRankEntryManager);
				}
				for (int j = count; j < rankEntries.Count; j++)
				{
					rankEntries[j].gameObject.SetActive(value: false);
				}
				for (int k = 0; k < count; k++)
				{
					rankEntries[k].gameObject.SetActive(value: true);
					rankEntries[k].Init(players[k]);
				}
				int yourIndex = GetYourIndex(players);
				Vector3 localPosition = scrollContent.localPosition;
				localPosition.y = heightOfRankEntry * (float)(yourIndex - 3);
				scrollContent.localPosition = localPosition;
			}
		}

		public static int GetYourIndex(List<TourPlayerInfo> players)
		{
			for (int num = players.Count - 1; num >= 0; num--)
			{
				if (players[num].isYou)
				{
					return num;
				}
			}
			return players.Count - 1;
		}

		private void OpenStartGamePopup()
		{
			GameEventCenter.Instance.Trigger(GameEventType.EventPlayTournament, new EventTriggerData(EventTriggerType.PlayTournament, 1, forceSaveProgress: true));
			startGamePopupController.Init();
		}

		public void OnGroupTabClicked()
		{
			if (groupPlayers == null)
			{
				scrollContent.gameObject.SetActive(value: false);
				loadingIcon.SetActive(value: true);
			}
			else
			{
				scrollContent.gameObject.SetActive(value: true);
				loadingIcon.SetActive(value: false);
			}
			groupTab.SetFocus(isFocused: true);
			friendTab.SetFocus(isFocused: false);
			loginFbObj.SetActive(value: false);
			inviteFbObj.SetActive(value: false);
			SetupLeaderboard(groupPlayers);
		}

		public void OnFriendTabClicked()
		{
			if (!PlatformSpecificServicesProvider.Services.UserProfile.IsLoggedIn_Facebook())
			{
				scrollContent.gameObject.SetActive(value: false);
				loadingIcon.SetActive(value: false);
				loginFbObj.SetActive(value: true);
				inviteFbObj.SetActive(value: false);
			}
			else
			{
				loginFbObj.SetActive(value: false);
				if (friendPlayers == null)
				{
					scrollContent.gameObject.SetActive(value: false);
					loadingIcon.SetActive(value: true);
					inviteFbObj.SetActive(value: false);
					if (!isReadingFbFriendScores)
					{
						isReadingFbFriendScores = true;
						ReadTournamentFriends(userSelfInfo);
					}
				}
				else
				{
					scrollContent.gameObject.SetActive(value: true);
					loadingIcon.SetActive(value: false);
					inviteFbObj.SetActive(friendPlayers.Count < GameTools.requiredNumOfTourFriend);
					if (seasonInfo.seasonNumber - 1 >= 0 && userSelfInfo.recFriendReward < seasonInfo.seasonNumber - 1 && userSelfInfo.lastscore > 0)
					{
						List<TourPlayerInfo> list = new List<TourPlayerInfo>();
						list.Add(new TourPlayerInfo(userSelfInfo.name, GameTools.DecodeHeroList(userSelfInfo.heroesCode), new TimeSpan(0, 0, 0, 0, userSelfInfo.lastscore), isYou: true, userSelfInfo.countryCode));
						for (int num = friendsUserData.Count - 1; num >= 0; num--)
						{
							if (friendsUserData[num].lastscore > 0)
							{
								list.Add(new TourPlayerInfo(friendsUserData[num].name, GameTools.DecodeHeroList(friendsUserData[num].heroes), new TimeSpan(0, 0, 0, 0, friendsUserData[num].lastscore), isYou: false, friendsUserData[num].country));
							}
						}
						if (list.Count >= GameTools.requiredNumOfTourFriend)
						{
							if (tourReceiveRewardPopup == null)
							{
								tourReceiveRewardPopup = UnityEngine.Object.Instantiate(tourReceiveRewardPrefab, base.transform);
							}
							tourReceiveRewardPopup.InitFriendReward(userSelfInfo, seasonInfo.seasonNumber - 1, seasonInfo, list);
						}
					}
				}
			}
			friendTab.SetFocus(isFocused: true);
			groupTab.SetFocus(isFocused: false);
			friendRewardNotify.SetActive(value: false);
			SetupLeaderboard(friendPlayers);
		}

		public void OnLoginFbButtonClicked()
		{
			CloseWithScaleAnimation();
			SingletonMonoBehaviour<WorldMap.UIRootController>.Instance.userProfilePopupController.Init();
		}

		public void OnInviteFriendClicked()
		{
			PlatformSpecificServicesProvider.Services.UserProfile.InviteFriend_Facebook();
		}

		public void OnTourRuleButtonClicked()
		{
			if (tournamentRulePopup == null)
			{
				tournamentRulePopup = UnityEngine.Object.Instantiate(tourRulePopupPrefab, base.transform);
			}
			tournamentRulePopup.OpenWithScaleAnimation();
		}

		public void OnReceiveLastSeasonReward()
		{
			if (seasonInfo.seasonNumber - 1 >= 0 && userSelfInfo.lastgroupid >= 0)
			{
				if (tourReceiveRewardPopup == null)
				{
					tourReceiveRewardPopup = UnityEngine.Object.Instantiate(tourReceiveRewardPrefab, base.transform);
				}
				tourReceiveRewardPopup.Init(userSelfInfo, seasonInfo.seasonNumber - 1, seasonInfo);
			}
		}

		public void OnChatButtonClicked()
		{
			Application.OpenURL("https://www.facebook.com/KingdomDefense.BestTDGame/");
		}

		public void OnBestPlayerButtonClicked()
		{
		}

		public bool IsReadyToStart()
		{
			if (!StaticMethod.IsInternetConnectionAvailable())
			{
				string notiContent = Singleton<NotificationDescription>.Instance.GetNotiContent(119);
				SingletonMonoBehaviour<LifetimeCanvas>.Instance.NotifyPopupController.Init(notiContent, isShowButtonFreeResources: false, isShowButtonGoToStore: false);
				return false;
			}
			return isGroupPlayerRetrieved && isBlessedHeroRetrieved && isInited;
		}

		public void OnStartWithGemButtonClicked()
		{
			if (IsReadyToStart())
			{
				int currentGem = ReadWriteDataPlayerCurrency.Instance.GetCurrentGem();
				int gemQuantityTostartTour = GameTools.GetGemQuantityTostartTour(increasePlayCount: false);
				if (gemQuantityTostartTour <= currentGem)
				{
					ReadWriteDataPlayerCurrency.Instance.ChangeGem(-gemQuantityTostartTour, isDispatchEventChange: true);
					IncreaseNumOfPlayTime();
					OpenStartGamePopup();
				}
				else
				{
					UnityEngine.Debug.Log("Không đủ Gem!");
					string notiContent = Singleton<NotificationDescription>.Instance.GetNotiContent(20);
					SingletonMonoBehaviour<LifetimeCanvas>.Instance.NotifyPopupController.Init(notiContent, isShowButtonFreeResources: true, isShowButtonGoToStore: true);
				}
			}
		}

		public void OnStartWithAdsButtonClicked()
		{
			if (IsReadyToStart())
			{
				if (isStartTourWithAdsAvai)
				{
					FreeResources.VideoPlayerManager.Instance.PlayRewardVideo(delegate(bool completed)
					{
						if (completed)
						{
							IncreaseNumOfPlayTime();
							OpenStartGamePopup();
						}
					});
					return;
				}
				string notiContent = Singleton<NotificationDescription>.Instance.GetNotiContent(19);
				SingletonMonoBehaviour<LifetimeCanvas>.Instance.NotifyPopupController.Init(notiContent, isShowButtonFreeResources: false, isShowButtonGoToStore: false);
			}
		}

		public void OnAllPrizeBtnClicked()
		{
			if (tourAllPrizePopup == null)
			{
				tourAllPrizePopup = UnityEngine.Object.Instantiate(allprizePopupPrefab, base.transform);
			}
			tourAllPrizePopup.Init(GetLeagueIndex());
		}

		public void OnFinishReadingTournamentMapRule()
		{
			try
			{
				isBlessedHeroRetrieved = true;
			}
			catch (Exception message)
			{
				UnityEngine.Debug.LogError(message);
				isBlessedHeroRetrieved = true;
				OnErrorOccur("(Error 100) Galahad gets lost :( Where am I?");
			}
		}

		public void OnFinishReadingTournamentPriceConstants()
		{
			isPriceConstantsRetrieved = true;
		}

		public void ReadSeasonInfoData()
		{
			PlatformSpecificServicesProvider.Services.DataCloudSaver.RetrieveDataWithMainThreadCallback("Tournament/Seasoninfo", delegate(IDataSnapshot task)
			{
				try
				{
					TourSeason_SeasonInfo tourSeason_SeasonInfo;
					if (!task.IsFaulted())
					{
						string rawJsonValue = task.GetRawJsonValue();
						if (string.IsNullOrEmpty(rawJsonValue))
						{
							int num = PlayerPrefs.GetInt("TESTtourDefaultMap", 0) % 6;
							UnityEngine.Debug.Log("test default map id " + num);
							seasonInfo = new TourSeasonInfo(num, GetNow(), GetNow().AddDays(6.0));
							goto IL_00d1;
						}
						tourSeason_SeasonInfo = JsonConvert.DeserializeObject<TourSeason_SeasonInfo>(rawJsonValue);
						if (!tourSeason_SeasonInfo.lockTournament)
						{
							goto IL_009e;
						}
						OnLockTournament(tourSeason_SeasonInfo.lockReason);
						if (Debug.isDebugBuild)
						{
							goto IL_009e;
						}
					}
					goto end_IL_0000;
					IL_009e:
					seasonInfo = new TourSeasonInfo(tourSeason_SeasonInfo.number, GameTools.FromUnixTimeToDateTime(tourSeason_SeasonInfo.startTime), GameTools.FromUnixTimeToDateTime(tourSeason_SeasonInfo.endTime), tourSeason_SeasonInfo.minVersion, tourSeason_SeasonInfo.groupBaseonTier);
					goto IL_00d1;
					IL_00d1:
					UnityEngine.Debug.LogFormat("cur ver {0} min ver {1}", Application.version, seasonInfo.minVersion);
					if (!seasonInfo.IsCurVersionUptodate())
					{
						AddSimpleNotification("Please update to the newest version on store", delegate
						{
							Application.OpenURL(PlatformSpecificServicesProvider.Services.StoreLink);
						});
					}
					GameTools.tourSeasonInfo = seasonInfo;
					isSeasonInfoRetrieved = true;
					end_IL_0000:;
				}
				catch (Exception message)
				{
					UnityEngine.Debug.LogError(message);
					OnErrorOccur("(Error 105) Galahad loses track of time :( What time is it?");
					isSeasonInfoRetrieved = true;
				}
			});
		}

		public void ReadUserSelfData()
		{
			string uid = ReadWriteDataUserProfile.Instance.GetUserID();
			PlatformSpecificServicesProvider.Services.DataCloudSaver.RetrieveDataWithMainThreadCallback("Tournament/Users/" + uid, delegate(IDataSnapshot task)
			{
				try
				{
					if (!task.IsFaulted())
					{
						UnityEngine.Debug.Log("++++ read user info  successful !!!!! " + uid);
						string rawJsonValue = task.GetRawJsonValue();
						if (string.IsNullOrEmpty(rawJsonValue))
						{
							userSelfInfo = new TourUserSelfInfo(-999, 0, -1, -1, -1, ReadWriteDataUserProfile.Instance.GetUserName(), 0, -1, GameTools.GetEncodedHeroList(ReadWriteDataHero.Instance.GetListHeroIDOwned()), ReadWriteDataUserProfile.Instance.GetUserRegionCode(), tierup: false);
							GameTools.WriteTimeStamp(NEXT_FREE_TIME_KEY, GetNow());
							goto IL_01b5;
						}
						TourUsers_UserData tourUsers_UserData = JsonConvert.DeserializeObject<TourUsers_UserData>(rawJsonValue);
						if (tourUsers_UserData.curgroupid >= -1)
						{
							userSelfInfo = new TourUserSelfInfo(tourUsers_UserData.curgroupid, tourUsers_UserData.curtier, tourUsers_UserData.lastgroupid, tourUsers_UserData.recFriendReward, tourUsers_UserData.recSeasonReward, tourUsers_UserData.name, tourUsers_UserData.score, tourUsers_UserData.lastscore, tourUsers_UserData.heroes, tourUsers_UserData.country, tourUsers_UserData.tierup);
							if (userSelfInfo.curgroupid >= 0)
							{
								ReadTournamentGroup(userSelfInfo);
							}
							if (GameTools.isTestingSeasonReward)
							{
								GameTools.isTestingSeasonReward = false;
								userSelfInfo.recSeasonReward = -1;
							}
							if (GameTools.isTestingFriendReward || GameTools.isTestingFriendRewardNoFakeUser)
							{
								userSelfInfo.recFriendReward = -1;
								if (userSelfInfo.lastscore <= 0)
								{
									userSelfInfo.lastscore = 123;
								}
								GameTools.isTestingFriendRewardNoFakeUser = false;
							}
							goto IL_01b5;
						}
						CustomInvoke(ReadUserSelfData, 1f);
					}
					goto end_IL_0000;
					IL_01b5:
					GameTools.tourUserSelfInfo = userSelfInfo;
					if (userSelfInfo.curgroupid < 0)
					{
						groupPlayers = new List<TourPlayerInfo>
						{
							new TourPlayerInfo(userSelfInfo.name, ReadWriteDataHero.Instance.GetListHeroIDOwned(), new TimeSpan(0L), isYou: true, userSelfInfo.countryCode)
						};
						GameTools.tourplayers = groupPlayers;
						ReadTournamentAllGroupInfo();
					}
					isUserSelfInfoRetrieved = true;
					end_IL_0000:;
				}
				catch (Exception message)
				{
					UnityEngine.Debug.LogError(message);
					OnErrorOccur("(Error 110) Galahad misses his mom :( Oh no, He's about to cry!!");
					isUserSelfInfoRetrieved = true;
				}
			});
		}

		public void ReadTournamentAllGroupInfo()
		{
			PlatformSpecificServicesProvider.Services.DataCloudSaver.RetrieveDataWithMainThreadCallback("Tournament/Groupinfo", delegate(IDataSnapshot task)
			{
				try
				{
					if (!task.IsFaulted())
					{
						string rawJsonValue = task.GetRawJsonValue();
						if ((GameTools.allGroupInfos = JsonConvert.DeserializeObject<Dictionary<string, Tour_GroupInfo>>(rawJsonValue)) == null)
						{
							GameTools.allGroupInfos = new Dictionary<string, Tour_GroupInfo>();
						}
						isGroupPlayerRetrieved = true;
					}
				}
				catch (Exception message)
				{
					UnityEngine.Debug.LogError(message);
					OnErrorOccur("(Error 130) Galahad is shy :( Is it because of that girl in the tournament group?");
					isGroupPlayerRetrieved = true;
				}
			});
		}

		public void ReadTournamentGroup(TourUserSelfInfo userData)
		{
			int curgroupid = userData.curgroupid;
			PlatformSpecificServicesProvider.Services.DataCloudSaver.RetrieveDataWithMainThreadCallback("Tournament/Curseasongroups/" + curgroupid, delegate(IDataSnapshot task)
			{
				try
				{
					if (!task.IsFaulted())
					{
						UnityEngine.Debug.Log("++++ read Group data successful !!!!! ");
						string rawJsonValue = task.GetRawJsonValue();
						if (string.IsNullOrEmpty(rawJsonValue))
						{
							groupPlayers = new List<TourPlayerInfo>
							{
								new TourPlayerInfo(userSelfInfo.name, ReadWriteDataHero.Instance.GetListHeroIDOwned(), new TimeSpan(0L), isYou: true, userSelfInfo.countryCode)
							};
							GameTools.tourplayers = groupPlayers;
							isGroupPlayerRetrieved = true;
						}
						else
						{
							Dictionary<string, TourSeasonGroup_Member> entries = JsonConvert.DeserializeObject<Dictionary<string, TourSeasonGroup_Member>>(rawJsonValue);
							groupPlayers = GetTourGroupList(entries);
							GameTools.tourplayers = groupPlayers;
							isGroupPlayerRetrieved = true;
						}
					}
				}
				catch (Exception message)
				{
					UnityEngine.Debug.LogError(message);
					OnErrorOccur("(Error 120) Galahad is shy :( Is it because of that girl in the tournament group?");
					isGroupPlayerRetrieved = true;
				}
			});
		}

		private IEnumerator RetryReadTournamentGroup(TourUserSelfInfo userData)
		{
			yield return new WaitForSeconds(2f);
			ReadTournamentGroup(userData);
		}

		public void ReadTournamentFriends(TourUserSelfInfo userData)
		{
			UnityEngine.Debug.Log("start read tour friends");
			if (PlatformSpecificServicesProvider.Services.UserProfile.IsLoggedIn_Facebook())
			{
				PlatformSpecificServicesProvider.Services.UserProfile.GetUidsOfUserFriends(delegate(List<string> friendList)
				{
					string text = "++Friends num: " + friendList.Count + ",";
					int num = Mathf.Min(friendList.Count, 10);
					for (int i = 0; i < num; i++)
					{
						text = text + friendList[i] + "__";
					}
					UnityEngine.Debug.Log(text);
					StartCoroutine(GetTourFriendList(friendList));
				});
			}
		}

		public static List<TourPlayerInfo> GetTourGroupList(Dictionary<string, TourSeasonGroup_Member> entries)
		{
			List<TourPlayerInfo> list = new List<TourPlayerInfo>();
			string userID = ReadWriteDataUserProfile.Instance.GetUserID();
			foreach (KeyValuePair<string, TourSeasonGroup_Member> entry in entries)
			{
				list.Add(new TourPlayerInfo(entry, userID));
			}
			SortList(list);
			return list;
		}

		public IEnumerator GetTourFriendList(List<string> fbIds)
		{
			friendsUserData.Clear();
			List<TourPlayerInfo> rtn = new List<TourPlayerInfo>();
			rtn.Add(new TourPlayerInfo(userSelfInfo.name, GameTools.DecodeHeroList(userSelfInfo.heroesCode), new TimeSpan(0, 0, 0, 0, userSelfInfo.score), isYou: true, userSelfInfo.countryCode));
			int friendCountdown = fbIds.Count;
			for (int num = fbIds.Count - 1; num >= 0; num--)
			{
				PlatformSpecificServicesProvider.Services.DataCloudSaver.RetrieveData("Tournament/FBToUid/" + fbIds[num], delegate(IDataSnapshot task)
				{
					if (task.IsCompleted())
					{
						string rawJsonValue = task.GetRawJsonValue();
						UnityEngine.Debug.Log("++fb id to uid json: " + rawJsonValue);
						if (string.IsNullOrEmpty(rawJsonValue))
						{
							friendCountdown--;
						}
						else
						{
							string str = JsonConvert.DeserializeObject<string>(rawJsonValue);
							PlatformSpecificServicesProvider.Services.DataCloudSaver.RetrieveData("Tournament/Users/" + str, delegate(IDataSnapshot secondTask)
							{
								string rawJsonValue2 = secondTask.GetRawJsonValue();
								UnityEngine.Debug.Log("___ new friend user data json: " + rawJsonValue2);
								if (!string.IsNullOrEmpty(rawJsonValue2))
								{
									TourUsers_UserData tourUsers_UserData = JsonConvert.DeserializeObject<TourUsers_UserData>(rawJsonValue2);
									friendsUserData.Add(tourUsers_UserData);
									if (tourUsers_UserData.score > 0)
									{
										rtn.Add(new TourPlayerInfo(tourUsers_UserData.name, GameTools.DecodeHeroList(tourUsers_UserData.heroes), new TimeSpan(0, 0, 0, 0, tourUsers_UserData.score), isYou: false, tourUsers_UserData.country));
									}
								}
								friendCountdown--;
							});
						}
					}
					else
					{
						friendCountdown--;
					}
				});
			}
			while (friendCountdown > 0)
			{
				yield return null;
			}
			if (GameTools.isTestingFriendReward)
			{
				GameTools.isTestingFriendReward = false;
				AddTestFriendDataForTesting();
			}
			SortList(rtn);
			friendPlayers = rtn;
			if (friendTab.isFocused)
			{
				OnFriendTabClicked();
			}
		}

		private void AddTestFriendDataForTesting()
		{
			for (int i = 0; i < 5; i++)
			{
				TourUsers_UserData tourUsers_UserData = new TourUsers_UserData();
				tourUsers_UserData.name = "tester" + i;
				tourUsers_UserData.heroes = 3;
				tourUsers_UserData.lastscore = (i + 1) * 100;
				tourUsers_UserData.score = i + 1;
				tourUsers_UserData.country = "us";
				friendsUserData.Add(tourUsers_UserData);
			}
		}

		public static void SortList(List<TourPlayerInfo> players)
		{
			int count = players.Count;
			for (int i = 0; i < count - 1; i++)
			{
				for (int j = i + 1; j < count; j++)
				{
					if (players[i].time.TotalMilliseconds < players[j].time.TotalMilliseconds)
					{
						TourPlayerInfo value = players[i];
						players[i] = players[j];
						players[j] = value;
					}
				}
			}
			for (int k = 0; k < count; k++)
			{
				players[k].rank = k;
			}
		}

		public int GetLeagueIndex()
		{
			if (userSelfInfo != null)
			{
				return userSelfInfo.curtier;
			}
			return 0;
		}

		public static DateTime GetNow()
		{
			return DateTime.Now;
		}

		public void GetPrizeData(out int curLowerRange, out int curUpperRange, out List<RewardItem> curReward, out int nextLowerRange, out int nextUpperRange, out List<RewardItem> nextReward)
		{
			int yourIndex = GetYourIndex(groupPlayers);
			List<TournamentPrizeConfigData> leagueAllPrize = GameTools.GetLeagueAllPrize(GetLeagueIndex());
			int index = 1;
			int index2 = 0;
			int count = leagueAllPrize.Count;
			for (int i = 0; i < count; i++)
			{
				if (leagueAllPrize[i].Rankrangelower >= yourIndex + 1 && yourIndex + 1 >= leagueAllPrize[i].Rankrangeupper)
				{
					index = i;
					index2 = ((i <= 0) ? i : (i - 1));
					break;
				}
			}
			curLowerRange = leagueAllPrize[index].Rankrangelower;
			curUpperRange = leagueAllPrize[index].Rankrangeupper;
			curReward = GameTools.GetTournamentRewardList(leagueAllPrize[index]);
			nextLowerRange = leagueAllPrize[index2].Rankrangelower;
			nextUpperRange = leagueAllPrize[index2].Rankrangeupper;
			nextReward = GameTools.GetTournamentRewardList(leagueAllPrize[index2]);
		}

		public int GetBlessedHeroId()
		{
			return 0;
		}

		public bool IsStartTourWithAdsAvailable()
		{
			return FreeResources.VideoPlayerManager.Instance.CheckIfVideoExits();
		}

		public DateTime GetNextFreeEntryTime()
		{
			return GameTools.ReadTimeStamp(NEXT_FREE_TIME_KEY);
		}

		public void IncreaseNumOfPlayTime()
		{
			int numOfPlayTourCount = GameTools.GetNumOfPlayTourCount();
			if (numOfPlayTourCount == 0)
			{
				GameTools.WriteTimeStamp(NEXT_FREE_TIME_KEY, GetNow().AddHours(12.0));
			}
			GameTools.SaveNumOfPlayTourCount(numOfPlayTourCount + 1);
		}

		public void OnErrorOccur(string errorMessage)
		{
			notifyContent = $"{errorMessage}\nPlease reload or come back later.";
			notifyFlag = true;
		}

		public void OnLockTournament(string lockReason)
		{
			notifyContent = $"{lockReason}\nPlease comeback in a few minutes.";
			notifyFlag = true;
		}

		public void AddSimpleNotification(string notification, Action callback = null)
		{
			notifyContent = notification;
			notifyFlag = true;
			notifyCallback = callback;
		}
	}
}

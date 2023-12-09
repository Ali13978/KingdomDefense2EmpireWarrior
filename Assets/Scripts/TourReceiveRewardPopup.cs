using Data;
using Gameplay;
using LifetimePopup;
using Newtonsoft.Json;
using Services.PlatformSpecific;
using System;
using System.Collections.Generic;
using Tournament;
using UnityEngine;
using UnityEngine.UI;

public class TourReceiveRewardPopup : GameplayPopupController
{
	public GameObject GetBtnObj;

	public TourRankEntryManager sampleRankEntry;

	public RectTransform scrollContent;

	public RectTransform scrollHandle;

	public float heightOfRankEntry = 90f;

	public GameObject loadingIcon;

	public Text seasonInfoText;

	private List<TourRankEntryManager> rankEntries = new List<TourRankEntryManager>();

	private TourUserSelfInfo userData;

	private List<RewardItem> curReward;

	private int lastSeasonNumber;

	private TournamentRewardType tourRewardType;

	private bool isInited;

	public void Init(TourUserSelfInfo userData, int lastSeasonNumber, TourSeasonInfo seasonInfo)
	{
		if (lastSeasonNumber < 0)
		{
			base.gameObject.SetActive(value: false);
			return;
		}
		tourRewardType = TournamentRewardType.SeasonReward;
		this.userData = userData;
		GetBtnObj.SetActive(value: false);
		this.lastSeasonNumber = lastSeasonNumber;
		seasonInfoText.text = $"SEASON {seasonInfo.seasonNumber + 1 - 1}";
		OpenWithScaleAnimation();
		ReadLastTournamentGroup();
	}

	public void InitFriendReward(TourUserSelfInfo userData, int lastSeasonNumber, TourSeasonInfo seasonInfo, List<TourPlayerInfo> friendPlayers)
	{
		if (lastSeasonNumber < 0)
		{
			base.gameObject.SetActive(value: false);
			return;
		}
		tourRewardType = TournamentRewardType.FriendReward;
		this.userData = userData;
		this.lastSeasonNumber = lastSeasonNumber;
		loadingIcon.SetActive(value: false);
		seasonInfoText.text = $"SEASON {seasonInfo.seasonNumber + 1 - 1}";
		OpenWithScaleAnimation();
		SetupLeaderboard(GameTools.GetLeagueAllPrize(1000), friendPlayers);
		int yourIndex = TournamentPopupController.GetYourIndex(friendPlayers);
		if (yourIndex < GameTools.requiredNumOfTourFriend)
		{
			PlatformSpecificServicesProvider.Services.Analytics.SendEvent_ReceiveFriendReward();
		}
	}

	public void ReadLastTournamentGroup()
	{
		int lastgroupid = userData.lastgroupid;
		PlatformSpecificServicesProvider.Services.DataCloudSaver.RetrieveDataWithMainThreadCallback("Tournament/Lastseasongroups/" + lastgroupid, delegate(IDataSnapshot task)
		{
			if (!task.IsFaulted())
			{
				string rawJsonValue = task.GetRawJsonValue();
				UnityEngine.Debug.Log("++last group json: " + rawJsonValue);
				loadingIcon.SetActive(value: false);
				if (string.IsNullOrEmpty(rawJsonValue))
				{
					Close();
				}
				else
				{
					Dictionary<string, TourSeasonGroup_Member> entries = JsonConvert.DeserializeObject<Dictionary<string, TourSeasonGroup_Member>>(rawJsonValue);
					List<TourPlayerInfo> tourGroupList = TournamentPopupController.GetTourGroupList(entries);
					int num = (!userData.tierup) ? userData.curtier : (userData.curtier - 1);
					if (num < 0)
					{
						num = 0;
					}
					List<TournamentPrizeConfigData> leagueAllPrize = GameTools.GetLeagueAllPrize(num);
					SetupLeaderboard(leagueAllPrize, tourGroupList);
					GetBtnObj.SetActive(value: true);
				}
			}
		});
	}

	public void SetupLeaderboard(List<TournamentPrizeConfigData> prizeData, List<TourPlayerInfo> playerList)
	{
		if (!isInited)
		{
			isInited = true;
			rankEntries.Add(sampleRankEntry);
			RectTransform rectTransform = scrollHandle;
			Vector2 offsetMin = scrollHandle.offsetMin;
			rectTransform.offsetMin = new Vector2(offsetMin.x, 0f);
			RectTransform rectTransform2 = scrollHandle;
			Vector2 offsetMax = scrollHandle.offsetMax;
			rectTransform2.offsetMax = new Vector2(offsetMax.x, 0f);
		}
		int yourIndex = TournamentPopupController.GetYourIndex(playerList);
		if (!playerList[yourIndex].isYou)
		{
			playerList.Add(new TourPlayerInfo(userData.name, new List<int>
			{
				2
			}, new TimeSpan(0L), isYou: true, userData.countryCode));
		}
		TournamentPopupController.SortList(playerList);
		yourIndex = TournamentPopupController.GetYourIndex(playerList);
		int count = playerList.Count;
		for (int i = rankEntries.Count; i < count; i++)
		{
			TourRankEntryManager tourRankEntryManager = UnityEngine.Object.Instantiate(sampleRankEntry, sampleRankEntry.transform.parent);
			tourRankEntryManager.transform.localPosition = sampleRankEntry.transform.localPosition + new Vector3(0f, (float)(-i) * heightOfRankEntry, 0f);
			rankEntries.Add(tourRankEntryManager);
		}
		for (int j = 0; j < count; j++)
		{
			rankEntries[j].gameObject.SetActive(value: true);
		}
		for (int k = count; k < rankEntries.Count; k++)
		{
			rankEntries[k].gameObject.SetActive(value: false);
		}
		RectTransform rectTransform3 = scrollContent;
		Vector2 sizeDelta = scrollContent.sizeDelta;
		rectTransform3.sizeDelta = new Vector2(sizeDelta.x, (float)playerList.Count * heightOfRankEntry + 50f);
		for (int l = 0; l < count; l++)
		{
			rankEntries[l].Init(playerList[l]);
		}
		for (int m = 1; m < count; m++)
		{
			rankEntries[m].transform.localPosition = sampleRankEntry.transform.localPosition + new Vector3(0f, (float)(-m) * heightOfRankEntry, 0f);
		}
		Vector3 localPosition = scrollContent.localPosition;
		localPosition.y = heightOfRankEntry * (float)(yourIndex - 3);
		scrollContent.localPosition = localPosition;
		count = prizeData.Count;
		curReward = null;
		for (int n = 0; n < count; n++)
		{
			if (prizeData[n].Rankrangelower >= yourIndex + 1 && yourIndex + 1 >= prizeData[n].Rankrangeupper)
			{
				curReward = GameTools.GetTournamentRewardList(prizeData[n]);
				break;
			}
		}
		if (curReward == null)
		{
			curReward = GameTools.GetTournamentRewardList(prizeData[count - 1]);
		}
	}

	public void OnGetRewardBtnClicked()
	{
		Dictionary<string, object> dictionary = new Dictionary<string, object>();
		string userID = ReadWriteDataUserProfile.Instance.GetUserID();
		dictionary.Add(string.Format("Tournament/Users/{0}/{1}", userID, (tourRewardType != 0) ? "recFriendReward" : "recSeasonReward"), lastSeasonNumber);
		PlatformSpecificServicesProvider.Services.DataCloudSaver.UpdateData(dictionary);
		for (int i = 0; i < curReward.Count; i++)
		{
			if (curReward[i].rewardType == RewardType.Gem)
			{
				ReadWriteDataPlayerCurrency.Instance.ChangeGem(curReward[i].value, isDispatchEventChange: true);
			}
			else if (curReward[i].rewardType == RewardType.Item)
			{
				ReadWriteDataPowerUpItem.Instance.ChangeItemQuantity(curReward[i].itemID, curReward[i].value);
			}
		}
		if (SingletonMonoBehaviour<LifetimeCanvas>.Instance.RewardPopupController == null)
		{
			UnityEngine.Debug.LogError("null reward popup");
		}
		SingletonMonoBehaviour<LifetimeCanvas>.Instance.RewardPopupController.Init(curReward.ToArray());
		CloseWithScaleAnimation();
	}
}

using Data;
using LifetimePopup;
using MyCustom;
using Services.PlatformSpecific;
using System;
using System.Collections.Generic;
using Tournament;
using UnityEngine;
using UnityEngine.UI;

namespace Gameplay
{
	public class TournamentResultPopupController : GameplayPopupController
	{
		[SerializeField]
		private Text timeResult;

		public TourRankEntryManager sampleRankEntry;

		public float heightOfRankEntry;

		public RectTransform scrollContent;

		public RectTransform scrollHandle;

		private List<TourRankEntryManager> rankEntries = new List<TourRankEntryManager>();

		private bool isInited;

		public void Init()
		{
			OpenWithScaleAnimation();
			RectTransform rectTransform = scrollHandle;
			Vector2 offsetMin = scrollHandle.offsetMin;
			rectTransform.offsetMin = new Vector2(offsetMin.x, 0f);
			RectTransform rectTransform2 = scrollHandle;
			Vector2 offsetMax = scrollHandle.offsetMax;
			rectTransform2.offsetMax = new Vector2(offsetMax.x, 0f);
			if (!isInited)
			{
				isInited = true;
				rankEntries.Add(sampleRankEntry);
				for (int i = 1; i < GameTools.tourplayers.Count; i++)
				{
					TourRankEntryManager tourRankEntryManager = UnityEngine.Object.Instantiate(sampleRankEntry, sampleRankEntry.transform.parent);
					tourRankEntryManager.transform.localPosition = sampleRankEntry.transform.localPosition + new Vector3(0f, (float)(-i) * heightOfRankEntry, 0f);
					rankEntries.Add(tourRankEntryManager);
				}
				RectTransform rectTransform3 = scrollContent;
				Vector2 sizeDelta = scrollContent.sizeDelta;
				rectTransform3.sizeDelta = new Vector2(sizeDelta.x, (float)GameTools.tourplayers.Count * heightOfRankEntry + 50f);
			}
			if (!StaticMethod.IsInternetConnectionAvailable())
			{
				string content = "Can't summit result! Please check your internet connection!";
				SingletonMonoBehaviour<LifetimeCanvas>.Instance.NotifyPopupController.Init(content, "Retry", UpdateResultToServer);
			}
			else
			{
				UpdateResultToServer();
			}
			InitResult();
		}

		private void UpdateResultToServer()
		{
			if (GameTools.tourUserSelfInfo.curgroupid >= -1)
			{
				SendResult();
			}
			else
			{
				InitTournamentUserDataOnDb();
			}
		}

		private void InitResult()
		{
			int milliseconds = (int)(SingletonMonoBehaviour<GameData>.Instance.tournamentBattleTime * 1000f);
			TimeSpan time = new TimeSpan(0, 0, 0, 0, milliseconds);
			timeResult.text = $"{(int)time.TotalMinutes}:{time.Seconds:00}.{time.Milliseconds:000}";
			int yourIndex = TournamentPopupController.GetYourIndex(GameTools.tourplayers);
			if (time.TotalMilliseconds > GameTools.tourplayers[yourIndex].time.TotalMilliseconds)
			{
				GameTools.tourplayers[yourIndex].time = time;
				GameTools.tourplayers[yourIndex].heroIds = SingletonMonoBehaviour<GameData>.Instance.ListHeroesIdsSelected;
				TournamentPopupController.SortList(GameTools.tourplayers);
				yourIndex = TournamentPopupController.GetYourIndex(GameTools.tourplayers);
			}
			int count = GameTools.tourplayers.Count;
			for (int i = 0; i < count; i++)
			{
				rankEntries[i].Init(GameTools.tourplayers[i]);
			}
			Vector3 localPosition = scrollContent.localPosition;
			localPosition.y = heightOfRankEntry * (float)(yourIndex - 3);
			scrollContent.localPosition = localPosition;
			yourIndex = TournamentPopupController.GetYourIndex(GameTools.tourplayers);
		}

		private void SendResult()
		{
			int num = (int)(SingletonMonoBehaviour<GameData>.Instance.tournamentBattleTime * 1000f);
			TimeSpan timeSpan = new TimeSpan(0, 0, 0, 0, num);
			int yourIndex = TournamentPopupController.GetYourIndex(GameTools.tourplayers);
			if (timeSpan.TotalMilliseconds <= GameTools.tourplayers[yourIndex].time.TotalMilliseconds)
			{
				UnityEngine.Debug.LogFormat("not the highest score, yours {0} score {1} highest {2} ", timeSpan.TotalMilliseconds, num, GameTools.tourplayers[yourIndex].time.TotalMilliseconds);
				return;
			}
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			string userID = ReadWriteDataUserProfile.Instance.GetUserID();
			if (string.IsNullOrEmpty(userID))
			{
				return;
			}
			int encodedHeroList = GameTools.GetEncodedHeroList(SingletonMonoBehaviour<GameData>.Instance.ListHeroesIdsSelected);
			int result = GameTools.tourUserSelfInfo.curgroupid;
			if (result < 0)
			{
				int.TryParse(PickNewGroup(SingletonMonoBehaviour<GameData>.Instance.ListHeroesIdsSelected), out result);
				dictionary[$"Tournament/Users/{userID}/curgroupid"] = result;
				UnityEngine.Debug.LogFormat("+_+_+ decided to join group {0}", result);
			}
			dictionary[$"Tournament/Curseasongroups/{result}/{userID}/heroes"] = encodedHeroList;
			dictionary[$"Tournament/Curseasongroups/{result}/{userID}/score"] = num;
			if (!string.IsNullOrEmpty(GameTools.tourUserSelfInfo.name))
			{
				dictionary[$"Tournament/Curseasongroups/{result}/{userID}/name"] = GameTools.tourUserSelfInfo.name;
			}
			dictionary[$"Tournament/Curseasongroups/{result}/{userID}/country"] = GameTools.tourUserSelfInfo.countryCode;
			dictionary[$"Tournament/Users/{userID}/score"] = num;
			dictionary[$"Tournament/Users/{userID}/heroes"] = encodedHeroList;
			if (PlatformSpecificServicesProvider.Services.UserProfile.IsLoggedIn_Facebook())
			{
				string uidOfUser = PlatformSpecificServicesProvider.Services.UserProfile.GetUidOfUser();
				if (!string.IsNullOrEmpty(uidOfUser))
				{
					dictionary[$"Tournament/FBToUid/{uidOfUser}"] = userID;
				}
			}
			UnityEngine.Debug.Log("1111 update score to " + num);
			PlatformSpecificServicesProvider.Services.DataCloudSaver.UpdateData(dictionary);
		}

		public TourUsers_UserData InitTournamentUserDataOnDb()
		{
			string userID = ReadWriteDataUserProfile.Instance.GetUserID();
			if (string.IsNullOrEmpty(userID))
			{
				return null;
			}
			int num = (int)(SingletonMonoBehaviour<GameData>.Instance.tournamentBattleTime * 1000f);
			TourUsers_UserData tourUsers_UserData = new TourUsers_UserData();
			string text = PickNewGroup(SingletonMonoBehaviour<GameData>.Instance.ListHeroesIdsSelected);
			int.TryParse(text, out tourUsers_UserData.curgroupid);
			tourUsers_UserData.heroes = GameTools.GetEncodedHeroList(SingletonMonoBehaviour<GameData>.Instance.ListHeroesIdsSelected);
			tourUsers_UserData.lastgroupid = -1;
			tourUsers_UserData.name = ReadWriteDataUserProfile.Instance.GetUserName();
			tourUsers_UserData.recFriendReward = GameTools.tourSeasonInfo.seasonNumber - 1;
			tourUsers_UserData.recFriendReward = GameTools.tourSeasonInfo.seasonNumber - 1;
			tourUsers_UserData.score = num;
			tourUsers_UserData.country = ReadWriteDataUserProfile.Instance.GetUserRegionCode();
			PlatformSpecificServicesProvider.Services.DataCloudSaver.WriteData(tourUsers_UserData, "Tournament/Users/" + userID);
			UnityEngine.Debug.Log("___11___ add new tour user to group " + tourUsers_UserData.curgroupid);
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary[$"Tournament/Curseasongroups/{text}/{userID}/heroes"] = tourUsers_UserData.heroes;
			dictionary[$"Tournament/Curseasongroups/{text}/{userID}/score"] = num;
			dictionary[$"Tournament/Curseasongroups/{text}/{userID}/name"] = tourUsers_UserData.name;
			dictionary[$"Tournament/Curseasongroups/{text}/{userID}/country"] = tourUsers_UserData.country;
			PlatformSpecificServicesProvider.Services.DataCloudSaver.UpdateData(dictionary);
			if (PlatformSpecificServicesProvider.Services.UserProfile.IsLoggedIn_Facebook())
			{
				string uidOfUser = PlatformSpecificServicesProvider.Services.UserProfile.GetUidOfUser();
				if (!string.IsNullOrEmpty(uidOfUser))
				{
					PlatformSpecificServicesProvider.Services.DataCloudSaver.WriteData(userID, "Tournament/FBToUid/" + uidOfUser);
				}
			}
			return tourUsers_UserData;
		}

		private string PickNewGroup(List<int> heroes)
		{
			int thresQuantity = (int)((float)GameTools.maxUserPerTourGroup * 0.75f);
			int num = (int)((float)GameTools.maxUserPerTourGroup * 0.88f);
			bool flag = IsMyTeamPremium(heroes);
			int num2 = 0;
			int num3 = 0;
			foreach (KeyValuePair<string, Tour_GroupInfo> allGroupInfo in GameTools.allGroupInfos)
			{
				num3++;
				if (allGroupInfo.Value.quantity >= num)
				{
					num2++;
				}
			}
			int result = 0;
			if (num2 >= num3)
			{
				result = GetNewGroupId(num3);
				PlatformSpecificServicesProvider.Services.DataCloudSaver.WriteNewGroupInfoTransaction(result, flag, 0);
			}
			else
			{
				float num4 = -9999f;
				KeyValuePair<string, Tour_GroupInfo> keyValuePair = default(KeyValuePair<string, Tour_GroupInfo>);
				foreach (KeyValuePair<string, Tour_GroupInfo> allGroupInfo2 in GameTools.allGroupInfos)
				{
					if (allGroupInfo2.Value.quantity < GameTools.maxUserPerTourGroup && IsGroupSuitable(allGroupInfo2.Key))
					{
						float groupScore = GetGroupScore(allGroupInfo2, thresQuantity, flag);
						UnityEngine.Debug.LogFormat("____Group {0} has score of {1}", allGroupInfo2.Key, groupScore);
						if (groupScore > num4)
						{
							num4 = groupScore;
							keyValuePair = allGroupInfo2;
						}
					}
				}
				if (num4 > -9000f)
				{
					int.TryParse(keyValuePair.Key, out result);
					PlatformSpecificServicesProvider.Services.DataCloudSaver.WriteGroupInfoTransaction("Tournament/Groupinfo/" + result, flag);
				}
				else
				{
					result = GetNewGroupId(num3);
					PlatformSpecificServicesProvider.Services.DataCloudSaver.WriteNewGroupInfoTransaction(result, flag, 0);
				}
			}
			return result.ToString();
		}

		private bool IsGroupSuitable(string groupKey)
		{
			if (!GameTools.tourSeasonInfo.isChoosingGroupBaseOnTier)
			{
				return true;
			}
			int.TryParse(groupKey, out int result);
			if (result % 100 == GameTools.tourUserSelfInfo.curtier)
			{
				return true;
			}
			return false;
		}

		private int GetNewGroupId(int curNumOfGroup)
		{
			for (int i = 0; i <= curNumOfGroup; i++)
			{
				int result = i;
				if (GameTools.tourSeasonInfo.isChoosingGroupBaseOnTier)
				{
					result = i * 100 + GameTools.tourUserSelfInfo.curtier;
				}
				if (!GameTools.allGroupInfos.ContainsKey(result.ToString()))
				{
					return result;
				}
			}
			return 10000;
		}

		private bool IsMyTeamPremium(List<int> heroes)
		{
			for (int num = heroes.Count - 1; num >= 0; num--)
			{
				if (heroes[num] != 1 && heroes[num] != 2)
				{
					return true;
				}
			}
			return false;
		}

		private float GetGroupScore(KeyValuePair<string, Tour_GroupInfo> group, int thresQuantity, bool isMyTeamPremium)
		{
			float num = 0f;
			num = ((group.Value.quantity >= thresQuantity) ? (num + (10f - (float)(group.Value.quantity - thresQuantity) * 1f / (float)(GameTools.maxUserPerTourGroup - thresQuantity))) : (num + (10f - (float)(thresQuantity - group.Value.quantity) * 1f / (float)thresQuantity * 10f)));
			if (isMyTeamPremium)
			{
				return num + (float)(GameTools.maxUserPerTourGroup - group.Value.premiumCount) * 10f / (float)GameTools.maxUserPerTourGroup;
			}
			return num + (10f - (float)(GameTools.maxUserPerTourGroup - group.Value.premiumCount) * 10f / (float)GameTools.maxUserPerTourGroup);
		}
	}
}

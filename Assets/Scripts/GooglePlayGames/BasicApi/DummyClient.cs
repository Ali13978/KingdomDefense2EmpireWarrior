using GooglePlayGames.BasicApi.Events;
using GooglePlayGames.BasicApi.Multiplayer;
using GooglePlayGames.BasicApi.SavedGame;
using GooglePlayGames.BasicApi.Video;
using GooglePlayGames.OurUtils;
using System;
using UnityEngine.SocialPlatforms;

namespace GooglePlayGames.BasicApi
{
	public class DummyClient : IPlayGamesClient
	{
		public void Authenticate(Action<bool, string> callback, bool silent)
		{
			LogUsage();
			callback?.Invoke(arg1: false, "Not implemented on this platform");
		}

		public bool IsAuthenticated()
		{
			LogUsage();
			return false;
		}

		public void SignOut()
		{
			LogUsage();
		}

		public string GetIdToken()
		{
			LogUsage();
			return null;
		}

		public string GetUserId()
		{
			LogUsage();
			return "DummyID";
		}

		public string GetServerAuthCode()
		{
			LogUsage();
			return null;
		}

		public void GetAnotherServerAuthCode(bool reAuthenticateIfNeeded, Action<string> callback)
		{
			LogUsage();
			callback(null);
		}

		public string GetUserEmail()
		{
			return string.Empty;
		}

		public void GetPlayerStats(Action<CommonStatusCodes, PlayerStats> callback)
		{
			LogUsage();
			callback(CommonStatusCodes.ApiNotConnected, new PlayerStats());
		}

		public string GetUserDisplayName()
		{
			LogUsage();
			return "Player";
		}

		public string GetUserImageUrl()
		{
			LogUsage();
			return null;
		}

		public void LoadUsers(string[] userIds, Action<IUserProfile[]> callback)
		{
			LogUsage();
			callback?.Invoke(null);
		}

		public void LoadAchievements(Action<Achievement[]> callback)
		{
			LogUsage();
			callback?.Invoke(null);
		}

		public Achievement GetAchievement(string achId)
		{
			LogUsage();
			return null;
		}

		public void UnlockAchievement(string achId, Action<bool> callback)
		{
			LogUsage();
			callback?.Invoke(obj: false);
		}

		public void RevealAchievement(string achId, Action<bool> callback)
		{
			LogUsage();
			callback?.Invoke(obj: false);
		}

		public void IncrementAchievement(string achId, int steps, Action<bool> callback)
		{
			LogUsage();
			callback?.Invoke(obj: false);
		}

		public void SetStepsAtLeast(string achId, int steps, Action<bool> callback)
		{
			LogUsage();
			callback?.Invoke(obj: false);
		}

		public void ShowAchievementsUI(Action<UIStatus> callback)
		{
			LogUsage();
			callback?.Invoke(UIStatus.VersionUpdateRequired);
		}

		public void ShowLeaderboardUI(string leaderboardId, LeaderboardTimeSpan span, Action<UIStatus> callback)
		{
			LogUsage();
			callback?.Invoke(UIStatus.VersionUpdateRequired);
		}

		public int LeaderboardMaxResults()
		{
			return 25;
		}

		public void LoadScores(string leaderboardId, LeaderboardStart start, int rowCount, LeaderboardCollection collection, LeaderboardTimeSpan timeSpan, Action<LeaderboardScoreData> callback)
		{
			LogUsage();
			callback?.Invoke(new LeaderboardScoreData(leaderboardId, ResponseStatus.LicenseCheckFailed));
		}

		public void LoadMoreScores(ScorePageToken token, int rowCount, Action<LeaderboardScoreData> callback)
		{
			LogUsage();
			callback?.Invoke(new LeaderboardScoreData(token.LeaderboardId, ResponseStatus.LicenseCheckFailed));
		}

		public void SubmitScore(string leaderboardId, long score, Action<bool> callback)
		{
			LogUsage();
			callback?.Invoke(obj: false);
		}

		public void SubmitScore(string leaderboardId, long score, string metadata, Action<bool> callback)
		{
			LogUsage();
			callback?.Invoke(obj: false);
		}

		public IRealTimeMultiplayerClient GetRtmpClient()
		{
			LogUsage();
			return null;
		}

		public ITurnBasedMultiplayerClient GetTbmpClient()
		{
			LogUsage();
			return null;
		}

		public ISavedGameClient GetSavedGameClient()
		{
			LogUsage();
			return null;
		}

		public IEventsClient GetEventsClient()
		{
			LogUsage();
			return null;
		}

		public IVideoClient GetVideoClient()
		{
			LogUsage();
			return null;
		}

		public void RegisterInvitationDelegate(InvitationReceivedDelegate invitationDelegate)
		{
			LogUsage();
		}

		public Invitation GetInvitationFromNotification()
		{
			LogUsage();
			return null;
		}

		public bool HasInvitationFromNotification()
		{
			LogUsage();
			return false;
		}

		public void LoadFriends(Action<bool> callback)
		{
			LogUsage();
			callback(obj: false);
		}

		public IUserProfile[] GetFriends()
		{
			LogUsage();
			return new IUserProfile[0];
		}

		public IntPtr GetApiClient()
		{
			LogUsage();
			return IntPtr.Zero;
		}

		public void SetGravityForPopups(Gravity gravity)
		{
			LogUsage();
		}

		private static void LogUsage()
		{
			Logger.d("Received method call on DummyClient - using stub implementation.");
		}
	}
}

using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.Events;
using GooglePlayGames.BasicApi.Multiplayer;
using GooglePlayGames.BasicApi.SavedGame;
using GooglePlayGames.BasicApi.Video;
using GooglePlayGames.Native.Cwrapper;
using GooglePlayGames.Native.PInvoke;
using GooglePlayGames.OurUtils;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace GooglePlayGames.Native
{
	public class NativeClient : IPlayGamesClient
	{
		private enum AuthState
		{
			Unauthenticated,
			Authenticated,
			SilentPending
		}

		private readonly IClientImpl clientImpl;

		private readonly object GameServicesLock = new object();

		private readonly object AuthStateLock = new object();

		private readonly PlayGamesClientConfiguration mConfiguration;

		private GooglePlayGames.Native.PInvoke.GameServices mServices;

		private volatile NativeTurnBasedMultiplayerClient mTurnBasedClient;

		private volatile NativeRealtimeMultiplayerClient mRealTimeClient;

		private volatile ISavedGameClient mSavedGameClient;

		private volatile IEventsClient mEventsClient;

		private volatile IVideoClient mVideoClient;

		private volatile TokenClient mTokenClient;

		private volatile Action<Invitation, bool> mInvitationDelegate;

		private volatile Dictionary<string, GooglePlayGames.BasicApi.Achievement> mAchievements;

		private volatile GooglePlayGames.BasicApi.Multiplayer.Player mUser;

		private volatile List<GooglePlayGames.BasicApi.Multiplayer.Player> mFriends;

		private volatile Action<bool, string> mPendingAuthCallbacks;

		private volatile Action<bool, string> mSilentAuthCallbacks;

		private volatile AuthState mAuthState;

		private volatile uint mAuthGeneration;

		private volatile bool mSilentAuthFailed;

		private volatile bool friendsLoading;

		internal NativeClient(PlayGamesClientConfiguration configuration, IClientImpl clientImpl)
		{
			PlayGamesHelperObject.CreateObject();
			mConfiguration = Misc.CheckNotNull(configuration);
			this.clientImpl = clientImpl;
		}

		private GooglePlayGames.Native.PInvoke.GameServices GameServices()
		{
			lock (GameServicesLock)
			{
				return mServices;
			}
		}

		public void Authenticate(Action<bool, string> callback, bool silent)
		{
			lock (AuthStateLock)
			{
				if (mAuthState == AuthState.Authenticated)
				{
					InvokeCallbackOnGameThread(callback, data: true, null);
					return;
				}
				if (mSilentAuthFailed && silent)
				{
					InvokeCallbackOnGameThread(callback, data: false, "silent auth failed");
					return;
				}
				if (callback != null)
				{
					if (silent)
					{
						mSilentAuthCallbacks = (Action<bool, string>)Delegate.Combine(mSilentAuthCallbacks, callback);
					}
					else
					{
						mPendingAuthCallbacks = (Action<bool, string>)Delegate.Combine(mPendingAuthCallbacks, callback);
					}
				}
			}
			friendsLoading = false;
			InitializeTokenClient();
			if (mTokenClient.NeedsToRun())
			{
				UnityEngine.Debug.Log("Starting Auth with token client.");
				mTokenClient.FetchTokens(delegate(int result)
				{
					InitializeGameServices();
					if (result == 0)
					{
						GameServices().StartAuthorizationUI();
					}
					else
					{
						HandleAuthTransition(GooglePlayGames.Native.Cwrapper.Types.AuthOperation.SIGN_IN, (CommonErrorStatus.AuthStatus)result);
					}
				});
				return;
			}
			InitializeGameServices();
			if (!silent)
			{
				GameServices().StartAuthorizationUI();
			}
		}

		private static Action<T> AsOnGameThreadCallback<T>(Action<T> callback)
		{
			if (callback == null)
			{
				return delegate
				{
				};
			}
			return delegate(T result)
			{
				InvokeCallbackOnGameThread(callback, result);
			};
		}

		private static void InvokeCallbackOnGameThread<T, S>(Action<T, S> callback, T data, S msg)
		{
			if (callback != null)
			{
				PlayGamesHelperObject.RunOnGameThread(delegate
				{
					GooglePlayGames.OurUtils.Logger.d("Invoking user callback on game thread");
					callback(data, msg);
				});
			}
		}

		private static void InvokeCallbackOnGameThread<T>(Action<T> callback, T data)
		{
			if (callback != null)
			{
				PlayGamesHelperObject.RunOnGameThread(delegate
				{
					GooglePlayGames.OurUtils.Logger.d("Invoking user callback on game thread");
					callback(data);
				});
			}
		}

		private void InitializeGameServices()
		{
			lock (GameServicesLock)
			{
				if (mServices == null)
				{
					using (GameServicesBuilder gameServicesBuilder = GameServicesBuilder.Create())
					{
						using (PlatformConfiguration configRef = clientImpl.CreatePlatformConfiguration(mConfiguration))
						{
							RegisterInvitationDelegate(mConfiguration.InvitationDelegate);
							gameServicesBuilder.SetOnAuthFinishedCallback(HandleAuthTransition);
							gameServicesBuilder.SetOnTurnBasedMatchEventCallback(delegate(GooglePlayGames.Native.Cwrapper.Types.MultiplayerEvent eventType, string matchId, NativeTurnBasedMatch match)
							{
								mTurnBasedClient.HandleMatchEvent(eventType, matchId, match);
							});
							gameServicesBuilder.SetOnMultiplayerInvitationEventCallback(HandleInvitation);
							if (mConfiguration.EnableSavedGames)
							{
								gameServicesBuilder.EnableSnapshots();
							}
							string[] scopes = mConfiguration.Scopes;
							for (int i = 0; i < scopes.Length; i++)
							{
								gameServicesBuilder.AddOauthScope(scopes[i]);
							}
							if (mConfiguration.IsHidingPopups)
							{
								gameServicesBuilder.SetShowConnectingPopup(flag: false);
							}
							UnityEngine.Debug.Log("Building GPG services, implicitly attempts silent auth");
							mAuthState = AuthState.SilentPending;
							mServices = gameServicesBuilder.Build(configRef);
							mEventsClient = new NativeEventClient(new GooglePlayGames.Native.PInvoke.EventManager(mServices));
							mVideoClient = new NativeVideoClient(new GooglePlayGames.Native.PInvoke.VideoManager(mServices));
							mTurnBasedClient = new NativeTurnBasedMultiplayerClient(this, new TurnBasedManager(mServices));
							mTurnBasedClient.RegisterMatchDelegate(mConfiguration.MatchDelegate);
							mRealTimeClient = new NativeRealtimeMultiplayerClient(this, new RealtimeManager(mServices));
							if (mConfiguration.EnableSavedGames)
							{
								mSavedGameClient = new NativeSavedGameClient(new GooglePlayGames.Native.PInvoke.SnapshotManager(mServices));
							}
							else
							{
								mSavedGameClient = new UnsupportedSavedGamesClient("You must enable saved games before it can be used. See PlayGamesClientConfiguration.Builder.EnableSavedGames.");
							}
							mAuthState = AuthState.SilentPending;
							InitializeTokenClient();
						}
					}
				}
			}
		}

		private void InitializeTokenClient()
		{
			if (mTokenClient == null)
			{
				mTokenClient = clientImpl.CreateTokenClient(reset: true);
				if (!GameInfo.WebClientIdInitialized() && (mConfiguration.IsRequestingIdToken || mConfiguration.IsRequestingAuthCode))
				{
					GooglePlayGames.OurUtils.Logger.e("Server Auth Code and ID Token require web clientId to configured.");
				}
				string[] scopes = mConfiguration.Scopes;
				mTokenClient.SetWebClientId("462144861492-rmse7dtl29cbk599ru830pr73u40a55b.apps.googleusercontent.com");
				mTokenClient.SetRequestAuthCode(mConfiguration.IsRequestingAuthCode, mConfiguration.IsForcingRefresh);
				mTokenClient.SetRequestEmail(mConfiguration.IsRequestingEmail);
				mTokenClient.SetRequestIdToken(mConfiguration.IsRequestingIdToken);
				mTokenClient.SetHidePopups(mConfiguration.IsHidingPopups);
				mTokenClient.AddOauthScopes(scopes);
				mTokenClient.SetAccountName(mConfiguration.AccountName);
			}
		}

		internal void HandleInvitation(GooglePlayGames.Native.Cwrapper.Types.MultiplayerEvent eventType, string invitationId, GooglePlayGames.Native.PInvoke.MultiplayerInvitation invitation)
		{
			Action<Invitation, bool> currentHandler = mInvitationDelegate;
			if (currentHandler == null)
			{
				GooglePlayGames.OurUtils.Logger.d("Received " + eventType + " for invitation " + invitationId + " but no handler was registered.");
				return;
			}
			if (eventType == GooglePlayGames.Native.Cwrapper.Types.MultiplayerEvent.REMOVED)
			{
				GooglePlayGames.OurUtils.Logger.d("Ignoring REMOVED for invitation " + invitationId);
				return;
			}
			bool shouldAutolaunch = eventType == GooglePlayGames.Native.Cwrapper.Types.MultiplayerEvent.UPDATED_FROM_APP_LAUNCH;
			Invitation invite = invitation.AsInvitation();
			PlayGamesHelperObject.RunOnGameThread(delegate
			{
				currentHandler(invite, shouldAutolaunch);
			});
		}

		public string GetUserEmail()
		{
			if (!IsAuthenticated())
			{
				UnityEngine.Debug.Log("Cannot get API client - not authenticated");
				return null;
			}
			return mTokenClient.GetEmail();
		}

		public string GetIdToken()
		{
			if (!IsAuthenticated())
			{
				UnityEngine.Debug.Log("Cannot get API client - not authenticated");
				return null;
			}
			return mTokenClient.GetIdToken();
		}

		public string GetServerAuthCode()
		{
			if (!IsAuthenticated())
			{
				UnityEngine.Debug.Log("Cannot get API client - not authenticated");
				return null;
			}
			return mTokenClient.GetAuthCode();
		}

		public void GetAnotherServerAuthCode(bool reAuthenticateIfNeeded, Action<string> callback)
		{
			mTokenClient.GetAnotherServerAuthCode(reAuthenticateIfNeeded, callback);
		}

		public bool IsAuthenticated()
		{
			lock (AuthStateLock)
			{
				return mAuthState == AuthState.Authenticated;
			}
		}

		public void LoadFriends(Action<bool> callback)
		{
			if (!IsAuthenticated())
			{
				GooglePlayGames.OurUtils.Logger.d("Cannot loadFriends when not authenticated");
				PlayGamesHelperObject.RunOnGameThread(delegate
				{
					callback(obj: false);
				});
			}
			else if (mFriends != null)
			{
				PlayGamesHelperObject.RunOnGameThread(delegate
				{
					callback(obj: true);
				});
			}
			else
			{
				mServices.PlayerManager().FetchFriends(delegate(ResponseStatus status, List<GooglePlayGames.BasicApi.Multiplayer.Player> players)
				{
					if (status == ResponseStatus.Success || status == ResponseStatus.SuccessWithStale)
					{
						mFriends = players;
						PlayGamesHelperObject.RunOnGameThread(delegate
						{
							callback(obj: true);
						});
					}
					else
					{
						mFriends = new List<GooglePlayGames.BasicApi.Multiplayer.Player>();
						GooglePlayGames.OurUtils.Logger.e("Got " + status + " loading friends");
						PlayGamesHelperObject.RunOnGameThread(delegate
						{
							callback(obj: false);
						});
					}
				});
			}
		}

		public IUserProfile[] GetFriends()
		{
			if (mFriends == null && !friendsLoading)
			{
				GooglePlayGames.OurUtils.Logger.w("Getting friends before they are loaded!!!");
				friendsLoading = true;
				LoadFriends(delegate(bool ok)
				{
					GooglePlayGames.OurUtils.Logger.d("loading: " + ok + " mFriends = " + mFriends);
					if (!ok)
					{
						GooglePlayGames.OurUtils.Logger.e("Friends list did not load successfully.  Disabling loading until re-authenticated");
					}
					friendsLoading = !ok;
				});
			}
			return (mFriends != null) ? mFriends.ToArray() : new IUserProfile[0];
		}

		private void PopulateAchievements(uint authGeneration, GooglePlayGames.Native.PInvoke.AchievementManager.FetchAllResponse response)
		{
			if (authGeneration != mAuthGeneration)
			{
				GooglePlayGames.OurUtils.Logger.d("Received achievement callback after signout occurred, ignoring");
				return;
			}
			GooglePlayGames.OurUtils.Logger.d("Populating Achievements, status = " + response.Status());
			lock (AuthStateLock)
			{
				if (response.Status() != CommonErrorStatus.ResponseStatus.VALID && response.Status() != CommonErrorStatus.ResponseStatus.VALID_BUT_STALE)
				{
					GooglePlayGames.OurUtils.Logger.e("Error retrieving achievements - check the log for more information. Failing signin.");
					Action<bool, string> action = mPendingAuthCallbacks;
					mPendingAuthCallbacks = null;
					if (action != null)
					{
						InvokeCallbackOnGameThread(action, data: false, "Cannot load achievements, Authenication failing");
					}
					SignOut();
					return;
				}
				Dictionary<string, GooglePlayGames.BasicApi.Achievement> dictionary = new Dictionary<string, GooglePlayGames.BasicApi.Achievement>();
				foreach (NativeAchievement item in response)
				{
					using (item)
					{
						dictionary[item.Id()] = item.AsAchievement();
					}
				}
				GooglePlayGames.OurUtils.Logger.d("Found " + dictionary.Count + " Achievements");
				mAchievements = dictionary;
			}
			GooglePlayGames.OurUtils.Logger.d("Maybe finish for Achievements");
			MaybeFinishAuthentication();
		}

		private void MaybeFinishAuthentication()
		{
			Action<bool, string> action = null;
			lock (AuthStateLock)
			{
				if (mUser == null || mAchievements == null)
				{
					GooglePlayGames.OurUtils.Logger.d("Auth not finished. User=" + mUser + " achievements=" + mAchievements);
					return;
				}
				GooglePlayGames.OurUtils.Logger.d("Auth finished. Proceeding.");
				action = mPendingAuthCallbacks;
				mPendingAuthCallbacks = null;
				mAuthState = AuthState.Authenticated;
			}
			if (action != null)
			{
				GooglePlayGames.OurUtils.Logger.d("Invoking Callbacks: " + action);
				InvokeCallbackOnGameThread(action, data: true, null);
			}
		}

		private void PopulateUser(uint authGeneration, GooglePlayGames.Native.PInvoke.PlayerManager.FetchSelfResponse response)
		{
			GooglePlayGames.OurUtils.Logger.d("Populating User");
			if (authGeneration != mAuthGeneration)
			{
				GooglePlayGames.OurUtils.Logger.d("Received user callback after signout occurred, ignoring");
				return;
			}
			lock (AuthStateLock)
			{
				if (response.Status() != CommonErrorStatus.ResponseStatus.VALID && response.Status() != CommonErrorStatus.ResponseStatus.VALID_BUT_STALE)
				{
					GooglePlayGames.OurUtils.Logger.e("Error retrieving user, signing out");
					Action<bool, string> action = mPendingAuthCallbacks;
					mPendingAuthCallbacks = null;
					if (action != null)
					{
						InvokeCallbackOnGameThread(action, data: false, "Cannot load user profile");
					}
					SignOut();
					return;
				}
				mUser = response.Self().AsPlayer();
				mFriends = null;
			}
			GooglePlayGames.OurUtils.Logger.d("Found User: " + mUser);
			GooglePlayGames.OurUtils.Logger.d("Maybe finish for User");
			MaybeFinishAuthentication();
		}

		private void HandleAuthTransition(GooglePlayGames.Native.Cwrapper.Types.AuthOperation operation, CommonErrorStatus.AuthStatus status)
		{
			GooglePlayGames.OurUtils.Logger.d("Starting Auth Transition. Op: " + operation + " status: " + status);
			lock (AuthStateLock)
			{
				switch (operation)
				{
				case GooglePlayGames.Native.Cwrapper.Types.AuthOperation.SIGN_IN:
					if (status == CommonErrorStatus.AuthStatus.VALID)
					{
						if (mSilentAuthCallbacks != null)
						{
							mPendingAuthCallbacks = (Action<bool, string>)Delegate.Combine(mPendingAuthCallbacks, mSilentAuthCallbacks);
							mSilentAuthCallbacks = null;
						}
						uint currentAuthGeneration = mAuthGeneration;
						mServices.AchievementManager().FetchAll(delegate(GooglePlayGames.Native.PInvoke.AchievementManager.FetchAllResponse results)
						{
							PopulateAchievements(currentAuthGeneration, results);
						});
						mServices.PlayerManager().FetchSelf(delegate(GooglePlayGames.Native.PInvoke.PlayerManager.FetchSelfResponse results)
						{
							PopulateUser(currentAuthGeneration, results);
						});
					}
					else if (mAuthState == AuthState.SilentPending)
					{
						mSilentAuthFailed = true;
						mAuthState = AuthState.Unauthenticated;
						Action<bool, string> callback = mSilentAuthCallbacks;
						mSilentAuthCallbacks = null;
						GooglePlayGames.OurUtils.Logger.d("Invoking callbacks, AuthState changed from silentPending to Unauthenticated.");
						InvokeCallbackOnGameThread(callback, data: false, "silent auth failed");
						if (mPendingAuthCallbacks != null)
						{
							GooglePlayGames.OurUtils.Logger.d("there are pending auth callbacks - starting AuthUI");
							GameServices().StartAuthorizationUI();
						}
					}
					else
					{
						GooglePlayGames.OurUtils.Logger.d("AuthState == " + mAuthState + " calling auth callbacks with failure");
						UnpauseUnityPlayer();
						Action<bool, string> callback2 = mPendingAuthCallbacks;
						mPendingAuthCallbacks = null;
						InvokeCallbackOnGameThread(callback2, data: false, "Authentication failed");
					}
					break;
				case GooglePlayGames.Native.Cwrapper.Types.AuthOperation.SIGN_OUT:
					ToUnauthenticated();
					break;
				default:
					GooglePlayGames.OurUtils.Logger.e("Unknown AuthOperation " + operation);
					break;
				}
			}
		}

		private void UnpauseUnityPlayer()
		{
		}

		private void ToUnauthenticated()
		{
			lock (AuthStateLock)
			{
				mUser = null;
				mFriends = null;
				mAchievements = null;
				mAuthState = AuthState.Unauthenticated;
				mTokenClient = clientImpl.CreateTokenClient(reset: true);
				mAuthGeneration++;
			}
		}

		public void SignOut()
		{
			ToUnauthenticated();
			if (GameServices() != null)
			{
				mTokenClient.Signout();
				GameServices().SignOut();
			}
		}

		public string GetUserId()
		{
			if (mUser == null)
			{
				return null;
			}
			return mUser.id;
		}

		public string GetUserDisplayName()
		{
			if (mUser == null)
			{
				return null;
			}
			return mUser.userName;
		}

		public string GetUserImageUrl()
		{
			if (mUser == null)
			{
				return null;
			}
			return mUser.AvatarURL;
		}

		public void SetGravityForPopups(Gravity gravity)
		{
			PlayGamesHelperObject.RunOnGameThread(delegate
			{
				clientImpl.SetGravityForPopups(GetApiClient(), gravity);
			});
		}

		public void GetPlayerStats(Action<CommonStatusCodes, GooglePlayGames.BasicApi.PlayerStats> callback)
		{
			PlayGamesHelperObject.RunOnGameThread(delegate
			{
				clientImpl.GetPlayerStats(GetApiClient(), callback);
			});
		}

		public void LoadUsers(string[] userIds, Action<IUserProfile[]> callback)
		{
			mServices.PlayerManager().FetchList(userIds, delegate(NativePlayer[] nativeUsers)
			{
				IUserProfile[] users = new IUserProfile[nativeUsers.Length];
				for (int i = 0; i < users.Length; i++)
				{
					users[i] = nativeUsers[i].AsPlayer();
				}
				PlayGamesHelperObject.RunOnGameThread(delegate
				{
					callback(users);
				});
			});
		}

		public GooglePlayGames.BasicApi.Achievement GetAchievement(string achId)
		{
			if (mAchievements == null || !mAchievements.ContainsKey(achId))
			{
				return null;
			}
			return mAchievements[achId];
		}

		public void LoadAchievements(Action<GooglePlayGames.BasicApi.Achievement[]> callback)
		{
			GooglePlayGames.BasicApi.Achievement[] data = new GooglePlayGames.BasicApi.Achievement[mAchievements.Count];
			mAchievements.Values.CopyTo(data, 0);
			PlayGamesHelperObject.RunOnGameThread(delegate
			{
				callback(data);
			});
		}

		public void UnlockAchievement(string achId, Action<bool> callback)
		{
			UpdateAchievement("Unlock", achId, callback, (GooglePlayGames.BasicApi.Achievement a) => a.IsUnlocked, delegate(GooglePlayGames.BasicApi.Achievement a)
			{
				a.IsUnlocked = true;
				GameServices().AchievementManager().Unlock(achId);
			});
		}

		public void RevealAchievement(string achId, Action<bool> callback)
		{
			UpdateAchievement("Reveal", achId, callback, (GooglePlayGames.BasicApi.Achievement a) => a.IsRevealed, delegate(GooglePlayGames.BasicApi.Achievement a)
			{
				a.IsRevealed = true;
				GameServices().AchievementManager().Reveal(achId);
			});
		}

		private void UpdateAchievement(string updateType, string achId, Action<bool> callback, Predicate<GooglePlayGames.BasicApi.Achievement> alreadyDone, Action<GooglePlayGames.BasicApi.Achievement> updateAchievment)
		{
			callback = AsOnGameThreadCallback(callback);
			Misc.CheckNotNull(achId);
			InitializeGameServices();
			GooglePlayGames.BasicApi.Achievement achievement = GetAchievement(achId);
			if (achievement == null)
			{
				GooglePlayGames.OurUtils.Logger.d("Could not " + updateType + ", no achievement with ID " + achId);
				callback(obj: false);
			}
			else if (alreadyDone(achievement))
			{
				GooglePlayGames.OurUtils.Logger.d("Did not need to perform " + updateType + ": on achievement " + achId);
				callback(obj: true);
			}
			else
			{
				GooglePlayGames.OurUtils.Logger.d("Performing " + updateType + " on " + achId);
				updateAchievment(achievement);
				GameServices().AchievementManager().Fetch(achId, delegate(GooglePlayGames.Native.PInvoke.AchievementManager.FetchResponse rsp)
				{
					if (rsp.Status() == CommonErrorStatus.ResponseStatus.VALID)
					{
						mAchievements.Remove(achId);
						mAchievements.Add(achId, rsp.Achievement().AsAchievement());
						callback(obj: true);
					}
					else
					{
						GooglePlayGames.OurUtils.Logger.e("Cannot refresh achievement " + achId + ": " + rsp.Status());
						callback(obj: false);
					}
				});
			}
		}

		public void IncrementAchievement(string achId, int steps, Action<bool> callback)
		{
			Misc.CheckNotNull(achId);
			callback = AsOnGameThreadCallback(callback);
			InitializeGameServices();
			GooglePlayGames.BasicApi.Achievement achievement = GetAchievement(achId);
			if (achievement == null)
			{
				GooglePlayGames.OurUtils.Logger.e("Could not increment, no achievement with ID " + achId);
				callback(obj: false);
			}
			else if (!achievement.IsIncremental)
			{
				GooglePlayGames.OurUtils.Logger.e("Could not increment, achievement with ID " + achId + " was not incremental");
				callback(obj: false);
			}
			else if (steps < 0)
			{
				GooglePlayGames.OurUtils.Logger.e("Attempted to increment by negative steps");
				callback(obj: false);
			}
			else
			{
				GameServices().AchievementManager().Increment(achId, Convert.ToUInt32(steps));
				GameServices().AchievementManager().Fetch(achId, delegate(GooglePlayGames.Native.PInvoke.AchievementManager.FetchResponse rsp)
				{
					if (rsp.Status() == CommonErrorStatus.ResponseStatus.VALID)
					{
						mAchievements.Remove(achId);
						mAchievements.Add(achId, rsp.Achievement().AsAchievement());
						callback(obj: true);
					}
					else
					{
						GooglePlayGames.OurUtils.Logger.e("Cannot refresh achievement " + achId + ": " + rsp.Status());
						callback(obj: false);
					}
				});
			}
		}

		public void SetStepsAtLeast(string achId, int steps, Action<bool> callback)
		{
			Misc.CheckNotNull(achId);
			callback = AsOnGameThreadCallback(callback);
			InitializeGameServices();
			GooglePlayGames.BasicApi.Achievement achievement = GetAchievement(achId);
			if (achievement == null)
			{
				GooglePlayGames.OurUtils.Logger.e("Could not increment, no achievement with ID " + achId);
				callback(obj: false);
			}
			else if (!achievement.IsIncremental)
			{
				GooglePlayGames.OurUtils.Logger.e("Could not increment, achievement with ID " + achId + " is not incremental");
				callback(obj: false);
			}
			else if (steps < 0)
			{
				GooglePlayGames.OurUtils.Logger.e("Attempted to increment by negative steps");
				callback(obj: false);
			}
			else
			{
				GameServices().AchievementManager().SetStepsAtLeast(achId, Convert.ToUInt32(steps));
				GameServices().AchievementManager().Fetch(achId, delegate(GooglePlayGames.Native.PInvoke.AchievementManager.FetchResponse rsp)
				{
					if (rsp.Status() == CommonErrorStatus.ResponseStatus.VALID)
					{
						mAchievements.Remove(achId);
						mAchievements.Add(achId, rsp.Achievement().AsAchievement());
						callback(obj: true);
					}
					else
					{
						GooglePlayGames.OurUtils.Logger.e("Cannot refresh achievement " + achId + ": " + rsp.Status());
						callback(obj: false);
					}
				});
			}
		}

		public void ShowAchievementsUI(Action<UIStatus> cb)
		{
			if (IsAuthenticated())
			{
				Action<CommonErrorStatus.UIStatus> callback = Callbacks.NoopUICallback;
				if (cb != null)
				{
					callback = delegate(CommonErrorStatus.UIStatus result)
					{
						cb((UIStatus)result);
					};
				}
				callback = AsOnGameThreadCallback(callback);
				GameServices().AchievementManager().ShowAllUI(callback);
			}
		}

		public int LeaderboardMaxResults()
		{
			return GameServices().LeaderboardManager().LeaderboardMaxResults;
		}

		public void ShowLeaderboardUI(string leaderboardId, LeaderboardTimeSpan span, Action<UIStatus> cb)
		{
			if (IsAuthenticated())
			{
				Action<CommonErrorStatus.UIStatus> callback = Callbacks.NoopUICallback;
				if (cb != null)
				{
					callback = delegate(CommonErrorStatus.UIStatus result)
					{
						cb((UIStatus)result);
					};
				}
				callback = AsOnGameThreadCallback(callback);
				if (leaderboardId == null)
				{
					GameServices().LeaderboardManager().ShowAllUI(callback);
				}
				else
				{
					GameServices().LeaderboardManager().ShowUI(leaderboardId, span, callback);
				}
			}
		}

		public void LoadScores(string leaderboardId, LeaderboardStart start, int rowCount, LeaderboardCollection collection, LeaderboardTimeSpan timeSpan, Action<LeaderboardScoreData> callback)
		{
			callback = AsOnGameThreadCallback(callback);
			GameServices().LeaderboardManager().LoadLeaderboardData(leaderboardId, start, rowCount, collection, timeSpan, mUser.id, callback);
		}

		public void LoadMoreScores(ScorePageToken token, int rowCount, Action<LeaderboardScoreData> callback)
		{
			callback = AsOnGameThreadCallback(callback);
			GameServices().LeaderboardManager().LoadScorePage(null, rowCount, token, callback);
		}

		public void SubmitScore(string leaderboardId, long score, Action<bool> callback)
		{
			callback = AsOnGameThreadCallback(callback);
			if (!IsAuthenticated())
			{
				callback(obj: false);
			}
			InitializeGameServices();
			if (leaderboardId == null)
			{
				throw new ArgumentNullException("leaderboardId");
			}
			GameServices().LeaderboardManager().SubmitScore(leaderboardId, score, null);
			callback(obj: true);
		}

		public void SubmitScore(string leaderboardId, long score, string metadata, Action<bool> callback)
		{
			callback = AsOnGameThreadCallback(callback);
			if (!IsAuthenticated())
			{
				callback(obj: false);
			}
			InitializeGameServices();
			if (leaderboardId == null)
			{
				throw new ArgumentNullException("leaderboardId");
			}
			GameServices().LeaderboardManager().SubmitScore(leaderboardId, score, metadata);
			callback(obj: true);
		}

		public IRealTimeMultiplayerClient GetRtmpClient()
		{
			if (!IsAuthenticated())
			{
				return null;
			}
			lock (GameServicesLock)
			{
				return mRealTimeClient;
			}
		}

		public ITurnBasedMultiplayerClient GetTbmpClient()
		{
			lock (GameServicesLock)
			{
				return mTurnBasedClient;
			}
		}

		public ISavedGameClient GetSavedGameClient()
		{
			lock (GameServicesLock)
			{
				return mSavedGameClient;
			}
		}

		public IEventsClient GetEventsClient()
		{
			lock (GameServicesLock)
			{
				return mEventsClient;
			}
		}

		public IVideoClient GetVideoClient()
		{
			lock (GameServicesLock)
			{
				return mVideoClient;
			}
		}

		public void RegisterInvitationDelegate(InvitationReceivedDelegate invitationDelegate)
		{
			if (invitationDelegate == null)
			{
				mInvitationDelegate = null;
			}
			else
			{
				mInvitationDelegate = Callbacks.AsOnGameThreadCallback(delegate(Invitation invitation, bool autoAccept)
				{
					invitationDelegate(invitation, autoAccept);
				});
			}
		}

		public IntPtr GetApiClient()
		{
			return InternalHooks.InternalHooks_GetApiClient(mServices.AsHandle());
		}
	}
}

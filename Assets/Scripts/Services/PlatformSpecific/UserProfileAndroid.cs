using Data;
using Facebook.MiniJSON;
using Facebook.Unity;
using Firebase;
using Firebase.Auth;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using LifetimePopup;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Services.PlatformSpecific
{
	public class UserProfileAndroid : MonoBehaviour, IUserProfile
	{
		private List<string> listPermissionLogin = new List<string>
		{
			"public_profile",
			"email",
			"user_friends"
		};

		private const string FIREBASE_PROVIDER_ID = "firebase";

		private const string GOOGLE_PROVIDER_ID = "google.com";

		private const string FACEBOOK_PROVIDER_ID = "facebook.com";

		private DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;

		private FirebaseAuth auth;

		private LoginState loginState;

		private bool isNewUser;

		private string tokenID = string.Empty;

		private string usedUserID;

		private bool isPlayingAsGuest;

		private Sprite currentUserAvatar;

		public bool IsSignInAnonymous
		{
			get
			{
				if (auth == null)
				{
					return false;
				}
				if (auth.CurrentUser == null)
				{
					return false;
				}
				IEnumerable<IUserInfo> providerData = auth.CurrentUser.ProviderData;
				foreach (IUserInfo item in providerData)
				{
					if (!string.Equals(item.ProviderId, "firebase"))
					{
						return false;
					}
				}
				return true;
			}
		}

		public bool IsAuthenticatedWithFacebook
		{
			get
			{
				if (auth == null)
				{
					UnityEngine.Debug.Log("auth object null");
					return false;
				}
				if (auth.CurrentUser == null)
				{
					return false;
				}
				IEnumerable<IUserInfo> providerData = auth.CurrentUser.ProviderData;
				foreach (IUserInfo item in providerData)
				{
					if (string.Equals(item.ProviderId, "facebook.com"))
					{
						return true;
					}
				}
				return false;
			}
		}

		public bool IsAuthenticatedWithGoogle
		{
			get
			{
				if (auth.CurrentUser == null)
				{
					return false;
				}
				IEnumerable<IUserInfo> providerData = auth.CurrentUser.ProviderData;
				foreach (IUserInfo item in providerData)
				{
					if (string.Equals(item.ProviderId, "google.com"))
					{
						return true;
					}
				}
				return false;
			}
		}

		public event Action OnLogStatusChangeEvent;

		public void Start()
		{
			TryInitializeFacebook();
			TryInitPlaygamePlatform();
		}

		private void OnDestroy()
		{
			if (auth != null)
			{
				auth.StateChanged -= AuthStateChanged;
				auth = null;
			}
		}

		private void TryInitializeFacebook()
		{
			if (!FB.IsInitialized)
			{
				FB.Init(InitCallback, OnHideUnity);
			}
			else
			{
				FB.ActivateApp();
			}
		}

		private void InitCallback()
		{
			if (FB.IsInitialized)
			{
				FB.ActivateApp();
			}
			else
			{
				UnityEngine.Debug.Log("Failed to Initialize the Facebook SDK");
			}
		}

		private void OnHideUnity(bool isGameShown)
		{
			if (!isGameShown)
			{
				Time.timeScale = 0f;
			}
			else
			{
				Time.timeScale = 1f;
			}
		}

		private void SendEvent_LogStatusChange()
		{
			if (this.OnLogStatusChangeEvent != null)
			{
				this.OnLogStatusChangeEvent();
			}
		}

		private void TryInitPlaygamePlatform()
		{
			PlayGamesClientConfiguration configuration = new PlayGamesClientConfiguration.Builder().RequestIdToken().RequestEmail().AddOauthScope("email")
				.Build();
			PlayGamesPlatform.InitializeInstance(configuration);
			PlayGamesPlatform.DebugLogEnabled = true;
			PlayGamesPlatform.Activate();
		}

		public void FirebaseInit()
		{
			auth = FirebaseAuth.DefaultInstance;
			UnityEngine.Debug.Log("auth setup " + auth);
			auth.StateChanged += AuthStateChanged;
			auth.IdTokenChanged += Auth_IdTokenChanged;
			if (auth.CurrentUser != null)
			{
				usedUserID = auth.CurrentUser.UserId;
				UnityEngine.Debug.Log("Init Firebase with userID" + usedUserID);
				isPlayingAsGuest = false;
			}
			else
			{
				isPlayingAsGuest = true;
			}
		}

		private void Auth_IdTokenChanged(object sender, EventArgs e)
		{
			if (auth.CurrentUser != null)
			{
				auth.CurrentUser.TokenAsync(forceRefresh: true).ContinueWith(delegate(Task<string> task)
				{
					tokenID = task.Result;
				});
			}
		}

		private void AuthStateChanged(object sender, EventArgs eventArgs)
		{
			if (auth.CurrentUser != null)
			{
				if (IsSignInAnonymous)
				{
					usedUserID = string.Empty;
				}
				isPlayingAsGuest = string.IsNullOrEmpty(usedUserID);
				isNewUser = (usedUserID != auth.CurrentUser.UserId);
				usedUserID = auth.CurrentUser.UserId;
				if (IsSignInAnonymous)
				{
					usedUserID = string.Empty;
				}
			}
			else
			{
				usedUserID = string.Empty;
				isNewUser = true;
				isPlayingAsGuest = true;
				loginState = LoginState.None;
			}
			SendEvent_LogStatusChange();
		}

		private void LoginFirebaseWithGoogleCredential()
		{
			loginState = LoginState.GoogleLogin;
			if (IsAuthenticatedWithFacebook)
			{
				LinkingAccount();
				UnityEngine.Debug.Log("Da dang nhap FB truoc do, linking!");
			}
			else
			{
				UnityEngine.Debug.Log("Chua dang nhap FB truoc do, new user!");
				Credential credential = GoogleAuthProvider.GetCredential(tokenID, null);
				auth.SignInWithCredentialAsync(credential).ContinueWith(delegate(Task<FirebaseUser> task)
				{
					if (task.IsCompleted)
					{
						UnityEngine.Debug.Log("Log in Firebase with GG userID" + auth.CurrentUser.UserId);
						SaveUserID();
						SaveUserRegionCode();
						SendEvent_LogStatusChange();
						UnityEngine.Debug.Log("End Progress");
						SingletonMonoBehaviour<LifetimeCanvas>.Instance.LoadingProgressPopupController.Close();
					}
				});
			}
		}

		private void LoginFirebaseWithFacebookCredential()
		{
			loginState = LoginState.FacebookLogin;
			if (IsAuthenticatedWithGoogle)
			{
				LinkingAccount();
				UnityEngine.Debug.Log("Da dang nhap GG truoc do, linking!");
			}
			else
			{
				UnityEngine.Debug.Log("Chua dang nhap GG truoc do, new user!");
				Credential credential = FacebookAuthProvider.GetCredential(tokenID);
				auth.SignInWithCredentialAsync(credential).ContinueWith(delegate(Task<FirebaseUser> task)
				{
					if (task.IsCompleted)
					{
						UnityEngine.Debug.Log("Log in Firebase with FB userID" + auth.CurrentUser.UserId);
						SaveUserID();
						SaveUserRegionCode();
						SendEvent_LogStatusChange();
						UnityEngine.Debug.Log("End Progress");
						SingletonMonoBehaviour<LifetimeCanvas>.Instance.LoadingProgressPopupController.Close();
					}
				});
			}
		}

		private void SaveUserID()
		{
			ReadWriteDataUserProfile.Instance.SetUserID(auth.CurrentUser.UserId);
		}

		private void SaveUserRegionCode()
		{
			string region = PreciseLocale.GetRegion();
			region = region.ToLower();
			UnityEngine.Debug.Log("Region code  = " + region);
			ReadWriteDataUserProfile.Instance.SetRegionCode(region);
		}

		public void FirebaseSignOut()
		{
			if (auth != null)
			{
				auth.SignOut();
			}
			usedUserID = string.Empty;
			isPlayingAsGuest = true;
			ReadWriteDataUserProfile.Instance.ClearUserID();
		}

		public void BackupData()
		{
			PlatformSpecificServicesProvider.Services.DataCloudSaver.BackupData();
		}

		public void RestoreData()
		{
			PlatformSpecificServicesProvider.Services.DataCloudSaver.RestoreData();
		}

		public void LogIn_Facebook()
		{
			SingletonMonoBehaviour<LifetimeCanvas>.Instance.LoadingProgressPopupController.Open();
			if (!FB.IsLoggedIn)
			{
				FB.LogInWithReadPermissions(listPermissionLogin, LogInFacebookCallBack);
			}
			else
			{
				DoLoginFirebaseWithFB();
			}
		}

		private void LogInFacebookCallBack(ILoginResult result)
		{
			if (FB.IsLoggedIn)
			{
				DoLoginFirebaseWithFB();
			}
			else
			{
				UnityEngine.Debug.Log("333");
			}
		}

		private void DoLoginFirebaseWithFB()
		{
			AccessToken currentAccessToken = AccessToken.CurrentAccessToken;
			string userId = currentAccessToken.UserId;
			tokenID = currentAccessToken.TokenString;
			UnityEngine.Debug.Log("Sign in FB success with userID = " + userId + " token = " + currentAccessToken);
			FB.API("/me?fields=name", HttpMethod.GET, GetUserNameCallback);
			LoginFirebaseWithFacebookCredential();
		}

		private void GetUserNameCallback(IResult result)
		{
			if (result.Error == null)
			{
				ReadWriteDataUserProfile.Instance.SetUserName(result.ResultDictionary["name"].ToString());
			}
		}

		public bool IsLoggedIn_Facebook()
		{
			bool flag = false;
			return IsAuthenticatedWithFacebook;
		}

		public void LogOut_Facebook()
		{
			if (IsLoggedIn_Facebook())
			{
				FB.LogOut();
			}
			loginState = LoginState.None;
			SendEvent_LogStatusChange();
		}

		public string GetUidOfUser()
		{
			if (FB.IsLoggedIn)
			{
				return AccessToken.CurrentAccessToken.UserId;
			}
			return null;
		}

		public void GetUidsOfUserFriends(Action<List<string>> callback)
		{
			List<string> rtn = new List<string>();
			string query = "/me/friends";
			FB.API(query, HttpMethod.GET, delegate(IGraphResult result)
			{
				Dictionary<string, object> dictionary = (Dictionary<string, object>)Json.Deserialize(result.RawResult);
				List<object> list = (List<object>)dictionary["data"];
				foreach (object item in list)
				{
					rtn.Add(((Dictionary<string, object>)item)["id"] as string);
				}
				callback(rtn);
			});
		}

		public void InviteFriend_Facebook()
		{
			PlatformSpecificServicesProvider.Services.FacebookServices.InviteFriend();
		}

		public void LogIn_Google()
		{
			SingletonMonoBehaviour<LifetimeCanvas>.Instance.LoadingProgressPopupController.Open();
			Social.localUser.Authenticate(delegate(bool success)
			{
				if (success)
				{
					string id = Social.localUser.id;
					tokenID = PlayGamesPlatform.Instance.GetIdToken();
					UnityEngine.Debug.Log("Sign in google success with userID = " + id + " token = " + tokenID);
					LoginFirebaseWithGoogleCredential();
				}
				else
				{
					UnityEngine.Debug.Log("Sign in google fail!");
				}
			});
		}

		public bool IsLoggedIn_Google()
		{
			bool flag = false;
			return IsAuthenticatedWithGoogle;
		}

		public void LogOut_Google()
		{
			if (IsLoggedIn_Google())
			{
				PlayGamesPlatform.Instance.SignOut();
			}
			loginState = LoginState.None;
			SendEvent_LogStatusChange();
		}

		private void LinkingAccount()
		{
			switch (loginState)
			{
			case LoginState.GoogleLogin:
				if (IsLoggedIn_Facebook())
				{
					tokenID = PlayGamesPlatform.Instance.GetIdToken();
					Credential credential2 = GoogleAuthProvider.GetCredential(tokenID, null);
					auth.CurrentUser.LinkWithCredentialAsync(credential2).ContinueWith(delegate(Task<FirebaseUser> task)
					{
						if (task.IsCanceled)
						{
							UnityEngine.Debug.LogError("LinkWithCredentialAsync was canceled.");
						}
						else if (task.IsFaulted)
						{
							UnityEngine.Debug.LogError("LinkWithCredentialAsync encountered an error: " + task.Exception);
							string key = "TITLE_DATABASE_EXIST";
							string localization = GameTools.GetLocalization(key);
							SingletonMonoBehaviour<LifetimeCanvas>.Instance.NotifyPopupController.Init(localization, isShowButtonFreeResources: false, isShowButtonGoToStore: false);
						}
						else
						{
							SendEvent_LogStatusChange();
							loginState = LoginState.None;
							UnityEngine.Debug.Log("Linking GG with existing FB account!");
						}
					});
					UnityEngine.Debug.Log("End Progress");
					SingletonMonoBehaviour<LifetimeCanvas>.Instance.LoadingProgressPopupController.Close();
				}
				break;
			case LoginState.FacebookLogin:
				if (IsLoggedIn_Google())
				{
					tokenID = AccessToken.CurrentAccessToken.TokenString;
					Credential credential = FacebookAuthProvider.GetCredential(tokenID);
					auth.CurrentUser.LinkWithCredentialAsync(credential).ContinueWith(delegate(Task<FirebaseUser> task)
					{
						if (task.IsCanceled)
						{
							UnityEngine.Debug.LogError("LinkWithCredentialAsync was canceled.");
						}
						else if (task.IsFaulted)
						{
							UnityEngine.Debug.LogError("LinkWithCredentialAsync encountered an error: " + task.Exception);
							string key2 = "TITLE_DATABASE_EXIST";
							string localization2 = GameTools.GetLocalization(key2);
							SingletonMonoBehaviour<LifetimeCanvas>.Instance.NotifyPopupController.Init(localization2, isShowButtonFreeResources: false, isShowButtonGoToStore: false);
						}
						else
						{
							SendEvent_LogStatusChange();
							loginState = LoginState.None;
							UnityEngine.Debug.Log("Linking FB with existing GG account!");
						}
					});
					UnityEngine.Debug.Log("End Progress");
					SingletonMonoBehaviour<LifetimeCanvas>.Instance.LoadingProgressPopupController.Close();
				}
				break;
			}
		}

		public string GetFireseBaseUserID()
		{
			string result = string.Empty;
			if (auth.CurrentUser != null)
			{
				result = auth.CurrentUser.UserId;
			}
			return result;
		}

		public string GetFireseBaseUserName()
		{
			string text = string.Empty;
			if (auth.CurrentUser != null)
			{
				text = auth.CurrentUser.DisplayName;
			}
			if (string.IsNullOrEmpty(text) && Social.localUser != null)
			{
				text = ((PlayGamesLocalUser)Social.localUser).userName;
			}
			return text;
		}

		public string GetFireseBaseUserEmail()
		{
			string text = string.Empty;
			if (auth.CurrentUser != null)
			{
				text = auth.CurrentUser.Email;
			}
			if (string.IsNullOrEmpty(text) && Social.localUser != null)
			{
				text = ((PlayGamesLocalUser)Social.localUser).Email;
			}
			return text;
		}

		public string GetFireseBaseUserPhoneNumber()
		{
			string result = string.Empty;
			if (auth.CurrentUser != null)
			{
				result = auth.CurrentUser.PhoneNumber;
			}
			return result;
		}

		public void TakePremiumUserInfor()
		{
			string empty = string.Empty;
			string empty2 = string.Empty;
			string empty3 = string.Empty;
			string empty4 = string.Empty;
			empty = GetFireseBaseUserID();
			empty2 = GetFireseBaseUserName();
			empty3 = GetFireseBaseUserEmail();
			empty4 = GetFireseBaseUserPhoneNumber();
			if (!string.IsNullOrEmpty(empty))
			{
				UnityEngine.Debug.Log("Claim user infor //id = " + empty + " //name = " + empty2 + " //email = " + empty3 + " //phonenumber = " + empty4);
				PlatformSpecificServicesProvider.Services.DataCloudSaver.ClaimPremiumUserInfor(empty, empty2, empty3, empty4);
			}
		}

		public Sprite GetUserAvatar()
		{
			if (FB.IsLoggedIn)
			{
				FB.API("me/picture?type=square&height=128&width=128", HttpMethod.GET, GetPicture);
			}
			return currentUserAvatar;
		}

		private void GetPicture(IGraphResult result)
		{
			if (result.Error == null && result.Texture != null)
			{
				currentUserAvatar = Sprite.Create(result.Texture, new Rect(0f, 0f, 128f, 128f), default(Vector2));
			}
		}
	}
}

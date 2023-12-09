using GooglePlayGames.OurUtils;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace GooglePlayGames
{
	public class PlayGamesUserProfile : IUserProfile
	{
		private string mDisplayName;

		private string mPlayerId;

		private string mAvatarUrl;

		private volatile bool mImageLoading;

		private Texture2D mImage;

		public string userName => mDisplayName;

		public string id => mPlayerId;

		public bool isFriend => true;

		public UserState state => UserState.Online;

		public Texture2D image
		{
			get
			{
				if (!mImageLoading && mImage == null && !string.IsNullOrEmpty(AvatarURL))
				{
					UnityEngine.Debug.Log("Starting to load image: " + AvatarURL);
					mImageLoading = true;
					PlayGamesHelperObject.RunCoroutine(LoadImage());
				}
				return mImage;
			}
		}

		public string AvatarURL => mAvatarUrl;

		internal PlayGamesUserProfile(string displayName, string playerId, string avatarUrl)
		{
			mDisplayName = displayName;
			mPlayerId = playerId;
			mAvatarUrl = avatarUrl;
			mImageLoading = false;
		}

		protected void ResetIdentity(string displayName, string playerId, string avatarUrl)
		{
			mDisplayName = displayName;
			mPlayerId = playerId;
			if (mAvatarUrl != avatarUrl)
			{
				mImage = null;
				mAvatarUrl = avatarUrl;
			}
			mImageLoading = false;
		}

		internal IEnumerator LoadImage()
		{
			if (!string.IsNullOrEmpty(AvatarURL))
			{
				WWW www = new WWW(AvatarURL);
				while (!www.isDone)
				{
					yield return null;
				}
				if (www.error == null)
				{
					mImage = www.texture;
				}
				else
				{
					mImage = Texture2D.blackTexture;
					UnityEngine.Debug.Log("Error downloading image: " + www.error);
				}
				mImageLoading = false;
			}
			else
			{
				UnityEngine.Debug.Log("No URL found.");
				mImage = Texture2D.blackTexture;
				mImageLoading = false;
			}
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (object.ReferenceEquals(this, obj))
			{
				return true;
			}
			PlayGamesUserProfile playGamesUserProfile = obj as PlayGamesUserProfile;
			if (playGamesUserProfile == null)
			{
				return false;
			}
			return StringComparer.Ordinal.Equals(mPlayerId, playGamesUserProfile.mPlayerId);
		}

		public override int GetHashCode()
		{
			return typeof(PlayGamesUserProfile).GetHashCode() ^ mPlayerId.GetHashCode();
		}

		public override string ToString()
		{
			return $"[Player: '{mDisplayName}' (id {mPlayerId})]";
		}
	}
}

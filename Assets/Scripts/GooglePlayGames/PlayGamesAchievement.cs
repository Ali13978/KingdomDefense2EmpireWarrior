using GooglePlayGames.BasicApi;
using System;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace GooglePlayGames
{
	internal class PlayGamesAchievement : IAchievement, IAchievementDescription
	{
		private readonly ReportProgress mProgressCallback;

		private string mId = string.Empty;

		private bool mIsIncremental;

		private int mCurrentSteps;

		private int mTotalSteps;

		private double mPercentComplete;

		private bool mCompleted;

		private bool mHidden;

		private DateTime mLastModifiedTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);

		private string mTitle = string.Empty;

		private string mRevealedImageUrl = string.Empty;

		private string mUnlockedImageUrl = string.Empty;

		private WWW mImageFetcher;

		private Texture2D mImage;

		private string mDescription = string.Empty;

		private ulong mPoints;

		public string id
		{
			get
			{
				return mId;
			}
			set
			{
				mId = value;
			}
		}

		public bool isIncremental => mIsIncremental;

		public int currentSteps => mCurrentSteps;

		public int totalSteps => mTotalSteps;

		public double percentCompleted
		{
			get
			{
				return mPercentComplete;
			}
			set
			{
				mPercentComplete = value;
			}
		}

		public bool completed => mCompleted;

		public bool hidden => mHidden;

		public DateTime lastReportedDate => mLastModifiedTime;

		public string title => mTitle;

		public Texture2D image => LoadImage();

		public string achievedDescription => mDescription;

		public string unachievedDescription => mDescription;

		public int points => (int)mPoints;

		internal PlayGamesAchievement()
			: this(PlayGamesPlatform.Instance.ReportProgress)
		{
		}

		internal PlayGamesAchievement(ReportProgress progressCallback)
		{
			mProgressCallback = progressCallback;
		}

		internal PlayGamesAchievement(Achievement ach)
			: this()
		{
			mId = ach.Id;
			mIsIncremental = ach.IsIncremental;
			mCurrentSteps = ach.CurrentSteps;
			mTotalSteps = ach.TotalSteps;
			if (ach.IsIncremental)
			{
				if (ach.TotalSteps > 0)
				{
					mPercentComplete = (double)ach.CurrentSteps / (double)ach.TotalSteps * 100.0;
				}
				else
				{
					mPercentComplete = 0.0;
				}
			}
			else
			{
				mPercentComplete = ((!ach.IsUnlocked) ? 0.0 : 100.0);
			}
			mCompleted = ach.IsUnlocked;
			mHidden = !ach.IsRevealed;
			mLastModifiedTime = ach.LastModifiedTime;
			mTitle = ach.Name;
			mDescription = ach.Description;
			mPoints = ach.Points;
			mRevealedImageUrl = ach.RevealedImageUrl;
			mUnlockedImageUrl = ach.UnlockedImageUrl;
		}

		public void ReportProgress(Action<bool> callback)
		{
			mProgressCallback(mId, mPercentComplete, callback);
		}

		private Texture2D LoadImage()
		{
			if (hidden)
			{
				return null;
			}
			string text = (!completed) ? mRevealedImageUrl : mUnlockedImageUrl;
			if (!string.IsNullOrEmpty(text))
			{
				if (mImageFetcher == null || mImageFetcher.url != text)
				{
					mImageFetcher = new WWW(text);
					mImage = null;
				}
				if (mImage != null)
				{
					return mImage;
				}
				if (mImageFetcher.isDone)
				{
					mImage = mImageFetcher.texture;
					return mImage;
				}
			}
			return null;
		}
	}
}

namespace GooglePlayGames.BasicApi.Video
{
	public class VideoCaptureState
	{
		private bool mIsCapturing;

		private VideoCaptureMode mCaptureMode;

		private VideoQualityLevel mQualityLevel;

		private bool mIsOverlayVisible;

		private bool mIsPaused;

		public bool IsCapturing => mIsCapturing;

		public VideoCaptureMode CaptureMode => mCaptureMode;

		public VideoQualityLevel QualityLevel => mQualityLevel;

		public bool IsOverlayVisible => mIsOverlayVisible;

		public bool IsPaused => mIsPaused;

		internal VideoCaptureState(bool isCapturing, VideoCaptureMode captureMode, VideoQualityLevel qualityLevel, bool isOverlayVisible, bool isPaused)
		{
			mIsCapturing = isCapturing;
			mCaptureMode = captureMode;
			mQualityLevel = qualityLevel;
			mIsOverlayVisible = isOverlayVisible;
			mIsPaused = isPaused;
		}

		public override string ToString()
		{
			return $"[VideoCaptureState: mIsCapturing={mIsCapturing}, mCaptureMode={mCaptureMode.ToString()}, mQualityLevel={mQualityLevel.ToString()}, mIsOverlayVisible={mIsOverlayVisible}, mIsPaused={mIsPaused}]";
		}
	}
}

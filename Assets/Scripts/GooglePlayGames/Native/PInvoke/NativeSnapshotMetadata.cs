using GooglePlayGames.BasicApi.SavedGame;
using GooglePlayGames.Native.Cwrapper;
using System;
using System.Runtime.InteropServices;

namespace GooglePlayGames.Native.PInvoke
{
	internal class NativeSnapshotMetadata : BaseReferenceHolder, ISavedGameMetadata
	{
		public bool IsOpen => SnapshotMetadata.SnapshotMetadata_IsOpen(SelfPtr());

		public string Filename => PInvokeUtilities.OutParamsToString((byte[] out_string, UIntPtr out_size) => SnapshotMetadata.SnapshotMetadata_FileName(SelfPtr(), out_string, out_size));

		public string Description => PInvokeUtilities.OutParamsToString((byte[] out_string, UIntPtr out_size) => SnapshotMetadata.SnapshotMetadata_Description(SelfPtr(), out_string, out_size));

		public string CoverImageURL => PInvokeUtilities.OutParamsToString((byte[] out_string, UIntPtr out_size) => SnapshotMetadata.SnapshotMetadata_CoverImageURL(SelfPtr(), out_string, out_size));

		public TimeSpan TotalTimePlayed
		{
			get
			{
				long num = SnapshotMetadata.SnapshotMetadata_PlayedTime(SelfPtr());
				if (num < 0)
				{
					return TimeSpan.FromMilliseconds(0.0);
				}
				return TimeSpan.FromMilliseconds(num);
			}
		}

		public DateTime LastModifiedTimestamp => PInvokeUtilities.FromMillisSinceUnixEpoch(SnapshotMetadata.SnapshotMetadata_LastModifiedTime(SelfPtr()));

		internal NativeSnapshotMetadata(IntPtr selfPointer)
			: base(selfPointer)
		{
		}

		public override string ToString()
		{
			if (IsDisposed())
			{
				return "[NativeSnapshotMetadata: DELETED]";
			}
			return $"[NativeSnapshotMetadata: IsOpen={IsOpen}, Filename={Filename}, Description={Description}, CoverImageUrl={CoverImageURL}, TotalTimePlayed={TotalTimePlayed}, LastModifiedTimestamp={LastModifiedTimestamp}]";
		}

		protected override void CallDispose(HandleRef selfPointer)
		{
			SnapshotMetadata.SnapshotMetadata_Dispose(SelfPtr());
		}
	}
}

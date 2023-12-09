using GooglePlayGames.BasicApi.SavedGame;
using GooglePlayGames.Native.Cwrapper;
using GooglePlayGames.OurUtils;
using System;
using System.Runtime.InteropServices;

namespace GooglePlayGames.Native.PInvoke
{
	internal class NativeSnapshotMetadataChange : BaseReferenceHolder
	{
		internal class Builder : BaseReferenceHolder
		{
			internal Builder()
				: base(SnapshotMetadataChangeBuilder.SnapshotMetadataChange_Builder_Construct())
			{
			}

			protected override void CallDispose(HandleRef selfPointer)
			{
				SnapshotMetadataChangeBuilder.SnapshotMetadataChange_Builder_Dispose(selfPointer);
			}

			internal Builder SetDescription(string description)
			{
				SnapshotMetadataChangeBuilder.SnapshotMetadataChange_Builder_SetDescription(SelfPtr(), description);
				return this;
			}

			internal Builder SetPlayedTime(ulong playedTime)
			{
				SnapshotMetadataChangeBuilder.SnapshotMetadataChange_Builder_SetPlayedTime(SelfPtr(), playedTime);
				return this;
			}

			internal Builder SetCoverImageFromPngData(byte[] pngData)
			{
				Misc.CheckNotNull(pngData);
				SnapshotMetadataChangeBuilder.SnapshotMetadataChange_Builder_SetCoverImageFromPngData(SelfPtr(), pngData, new UIntPtr((ulong)pngData.LongLength));
				return this;
			}

			internal Builder From(SavedGameMetadataUpdate update)
			{
				Builder builder = this;
				if (update.IsDescriptionUpdated)
				{
					builder = builder.SetDescription(update.UpdatedDescription);
				}
				if (update.IsCoverImageUpdated)
				{
					builder = builder.SetCoverImageFromPngData(update.UpdatedPngCoverImage);
				}
				if (update.IsPlayedTimeUpdated)
				{
					builder = builder.SetPlayedTime((ulong)update.UpdatedPlayedTime.Value.TotalMilliseconds);
				}
				return builder;
			}

			internal NativeSnapshotMetadataChange Build()
			{
				return FromPointer(SnapshotMetadataChangeBuilder.SnapshotMetadataChange_Builder_Create(SelfPtr()));
			}
		}

		internal NativeSnapshotMetadataChange(IntPtr selfPointer)
			: base(selfPointer)
		{
		}

		protected override void CallDispose(HandleRef selfPointer)
		{
			SnapshotMetadataChange.SnapshotMetadataChange_Dispose(selfPointer);
		}

		internal static NativeSnapshotMetadataChange FromPointer(IntPtr pointer)
		{
			if (pointer.Equals(IntPtr.Zero))
			{
				return null;
			}
			return new NativeSnapshotMetadataChange(pointer);
		}
	}
}

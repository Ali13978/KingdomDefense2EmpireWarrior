using AOT;
using GooglePlayGames.Native.Cwrapper;
using GooglePlayGames.OurUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace GooglePlayGames.Native.PInvoke
{
	internal class RealtimeManager
	{
		internal class RealTimeRoomResponse : BaseReferenceHolder
		{
			internal RealTimeRoomResponse(IntPtr selfPointer)
				: base(selfPointer)
			{
			}

			internal CommonErrorStatus.MultiplayerStatus ResponseStatus()
			{
				return RealTimeMultiplayerManager.RealTimeMultiplayerManager_RealTimeRoomResponse_GetStatus(SelfPtr());
			}

			internal bool RequestSucceeded()
			{
				return ResponseStatus() > (CommonErrorStatus.MultiplayerStatus)0;
			}

			internal NativeRealTimeRoom Room()
			{
				if (!RequestSucceeded())
				{
					return null;
				}
				return new NativeRealTimeRoom(RealTimeMultiplayerManager.RealTimeMultiplayerManager_RealTimeRoomResponse_GetRoom(SelfPtr()));
			}

			protected override void CallDispose(HandleRef selfPointer)
			{
				RealTimeMultiplayerManager.RealTimeMultiplayerManager_RealTimeRoomResponse_Dispose(selfPointer);
			}

			internal static RealTimeRoomResponse FromPointer(IntPtr pointer)
			{
				if (pointer.Equals(IntPtr.Zero))
				{
					return null;
				}
				return new RealTimeRoomResponse(pointer);
			}
		}

		internal class RoomInboxUIResponse : BaseReferenceHolder
		{
			internal RoomInboxUIResponse(IntPtr selfPointer)
				: base(selfPointer)
			{
			}

			internal CommonErrorStatus.UIStatus ResponseStatus()
			{
				return RealTimeMultiplayerManager.RealTimeMultiplayerManager_RoomInboxUIResponse_GetStatus(SelfPtr());
			}

			internal MultiplayerInvitation Invitation()
			{
				if (ResponseStatus() != CommonErrorStatus.UIStatus.VALID)
				{
					return null;
				}
				return new MultiplayerInvitation(RealTimeMultiplayerManager.RealTimeMultiplayerManager_RoomInboxUIResponse_GetInvitation(SelfPtr()));
			}

			protected override void CallDispose(HandleRef selfPointer)
			{
				RealTimeMultiplayerManager.RealTimeMultiplayerManager_RoomInboxUIResponse_Dispose(selfPointer);
			}

			internal static RoomInboxUIResponse FromPointer(IntPtr pointer)
			{
				if (PInvokeUtilities.IsNull(pointer))
				{
					return null;
				}
				return new RoomInboxUIResponse(pointer);
			}
		}

		internal class WaitingRoomUIResponse : BaseReferenceHolder
		{
			internal WaitingRoomUIResponse(IntPtr selfPointer)
				: base(selfPointer)
			{
			}

			internal CommonErrorStatus.UIStatus ResponseStatus()
			{
				return RealTimeMultiplayerManager.RealTimeMultiplayerManager_WaitingRoomUIResponse_GetStatus(SelfPtr());
			}

			internal NativeRealTimeRoom Room()
			{
				if (ResponseStatus() != CommonErrorStatus.UIStatus.VALID)
				{
					return null;
				}
				return new NativeRealTimeRoom(RealTimeMultiplayerManager.RealTimeMultiplayerManager_WaitingRoomUIResponse_GetRoom(SelfPtr()));
			}

			protected override void CallDispose(HandleRef selfPointer)
			{
				RealTimeMultiplayerManager.RealTimeMultiplayerManager_WaitingRoomUIResponse_Dispose(selfPointer);
			}

			internal static WaitingRoomUIResponse FromPointer(IntPtr pointer)
			{
				if (PInvokeUtilities.IsNull(pointer))
				{
					return null;
				}
				return new WaitingRoomUIResponse(pointer);
			}
		}

		internal class FetchInvitationsResponse : BaseReferenceHolder
		{
			internal FetchInvitationsResponse(IntPtr selfPointer)
				: base(selfPointer)
			{
			}

			internal bool RequestSucceeded()
			{
				return ResponseStatus() > (CommonErrorStatus.ResponseStatus)0;
			}

			internal CommonErrorStatus.ResponseStatus ResponseStatus()
			{
				return RealTimeMultiplayerManager.RealTimeMultiplayerManager_FetchInvitationsResponse_GetStatus(SelfPtr());
			}

			internal IEnumerable<MultiplayerInvitation> Invitations()
			{
				return PInvokeUtilities.ToEnumerable(RealTimeMultiplayerManager.RealTimeMultiplayerManager_FetchInvitationsResponse_GetInvitations_Length(SelfPtr()), (UIntPtr index) => new MultiplayerInvitation(RealTimeMultiplayerManager.RealTimeMultiplayerManager_FetchInvitationsResponse_GetInvitations_GetElement(SelfPtr(), index)));
			}

			protected override void CallDispose(HandleRef selfPointer)
			{
				RealTimeMultiplayerManager.RealTimeMultiplayerManager_FetchInvitationsResponse_Dispose(selfPointer);
			}

			internal static FetchInvitationsResponse FromPointer(IntPtr pointer)
			{
				if (PInvokeUtilities.IsNull(pointer))
				{
					return null;
				}
				return new FetchInvitationsResponse(pointer);
			}
		}

		private readonly GameServices mGameServices;

		[CompilerGenerated]
		private static RealTimeMultiplayerManager.RealTimeRoomCallback _003C_003Ef__mg_0024cache0;

		[CompilerGenerated]
		private static Func<IntPtr, PlayerSelectUIResponse> _003C_003Ef__mg_0024cache1;

		[CompilerGenerated]
		private static RealTimeMultiplayerManager.PlayerSelectUICallback _003C_003Ef__mg_0024cache2;

		[CompilerGenerated]
		private static Func<IntPtr, RoomInboxUIResponse> _003C_003Ef__mg_0024cache3;

		[CompilerGenerated]
		private static RealTimeMultiplayerManager.RoomInboxUICallback _003C_003Ef__mg_0024cache4;

		[CompilerGenerated]
		private static Func<IntPtr, WaitingRoomUIResponse> _003C_003Ef__mg_0024cache5;

		[CompilerGenerated]
		private static RealTimeMultiplayerManager.WaitingRoomUICallback _003C_003Ef__mg_0024cache6;

		[CompilerGenerated]
		private static Func<IntPtr, FetchInvitationsResponse> _003C_003Ef__mg_0024cache7;

		[CompilerGenerated]
		private static RealTimeMultiplayerManager.FetchInvitationsCallback _003C_003Ef__mg_0024cache8;

		[CompilerGenerated]
		private static RealTimeMultiplayerManager.LeaveRoomCallback _003C_003Ef__mg_0024cache9;

		[CompilerGenerated]
		private static RealTimeMultiplayerManager.RealTimeRoomCallback _003C_003Ef__mg_0024cacheA;

		[CompilerGenerated]
		private static RealTimeMultiplayerManager.SendReliableMessageCallback _003C_003Ef__mg_0024cacheB;

		[CompilerGenerated]
		private static Func<IntPtr, RealTimeRoomResponse> _003C_003Ef__mg_0024cacheC;

		internal RealtimeManager(GameServices gameServices)
		{
			mGameServices = Misc.CheckNotNull(gameServices);
		}

		internal void CreateRoom(RealtimeRoomConfig config, RealTimeEventListenerHelper helper, Action<RealTimeRoomResponse> callback)
		{
			RealTimeMultiplayerManager.RealTimeMultiplayerManager_CreateRealTimeRoom(mGameServices.AsHandle(), config.AsPointer(), helper.AsPointer(), InternalRealTimeRoomCallback, ToCallbackPointer(callback));
		}

		internal void ShowPlayerSelectUI(uint minimumPlayers, uint maxiumPlayers, bool allowAutomatching, Action<PlayerSelectUIResponse> callback)
		{
			RealTimeMultiplayerManager.RealTimeMultiplayerManager_ShowPlayerSelectUI(mGameServices.AsHandle(), minimumPlayers, maxiumPlayers, allowAutomatching, InternalPlayerSelectUIcallback, Callbacks.ToIntPtr(callback, PlayerSelectUIResponse.FromPointer));
		}

		[MonoPInvokeCallback(typeof(RealTimeMultiplayerManager.PlayerSelectUICallback))]
		internal static void InternalPlayerSelectUIcallback(IntPtr response, IntPtr data)
		{
			Callbacks.PerformInternalCallback("RealtimeManager#PlayerSelectUICallback", Callbacks.Type.Temporary, response, data);
		}

		[MonoPInvokeCallback(typeof(RealTimeMultiplayerManager.RealTimeRoomCallback))]
		internal static void InternalRealTimeRoomCallback(IntPtr response, IntPtr data)
		{
			Callbacks.PerformInternalCallback("RealtimeManager#InternalRealTimeRoomCallback", Callbacks.Type.Temporary, response, data);
		}

		[MonoPInvokeCallback(typeof(RealTimeMultiplayerManager.RoomInboxUICallback))]
		internal static void InternalRoomInboxUICallback(IntPtr response, IntPtr data)
		{
			Callbacks.PerformInternalCallback("RealtimeManager#InternalRoomInboxUICallback", Callbacks.Type.Temporary, response, data);
		}

		internal void ShowRoomInboxUI(Action<RoomInboxUIResponse> callback)
		{
			RealTimeMultiplayerManager.RealTimeMultiplayerManager_ShowRoomInboxUI(mGameServices.AsHandle(), InternalRoomInboxUICallback, Callbacks.ToIntPtr(callback, RoomInboxUIResponse.FromPointer));
		}

		internal void ShowWaitingRoomUI(NativeRealTimeRoom room, uint minimumParticipantsBeforeStarting, Action<WaitingRoomUIResponse> callback)
		{
			Misc.CheckNotNull(room);
			RealTimeMultiplayerManager.RealTimeMultiplayerManager_ShowWaitingRoomUI(mGameServices.AsHandle(), room.AsPointer(), minimumParticipantsBeforeStarting, InternalWaitingRoomUICallback, Callbacks.ToIntPtr(callback, WaitingRoomUIResponse.FromPointer));
		}

		[MonoPInvokeCallback(typeof(RealTimeMultiplayerManager.WaitingRoomUICallback))]
		internal static void InternalWaitingRoomUICallback(IntPtr response, IntPtr data)
		{
			Callbacks.PerformInternalCallback("RealtimeManager#InternalWaitingRoomUICallback", Callbacks.Type.Temporary, response, data);
		}

		[MonoPInvokeCallback(typeof(RealTimeMultiplayerManager.FetchInvitationsCallback))]
		internal static void InternalFetchInvitationsCallback(IntPtr response, IntPtr data)
		{
			Callbacks.PerformInternalCallback("RealtimeManager#InternalFetchInvitationsCallback", Callbacks.Type.Temporary, response, data);
		}

		internal void FetchInvitations(Action<FetchInvitationsResponse> callback)
		{
			RealTimeMultiplayerManager.RealTimeMultiplayerManager_FetchInvitations(mGameServices.AsHandle(), InternalFetchInvitationsCallback, Callbacks.ToIntPtr(callback, FetchInvitationsResponse.FromPointer));
		}

		[MonoPInvokeCallback(typeof(RealTimeMultiplayerManager.LeaveRoomCallback))]
		internal static void InternalLeaveRoomCallback(CommonErrorStatus.ResponseStatus response, IntPtr data)
		{
			Logger.d("Entering internal callback for InternalLeaveRoomCallback");
			Action<CommonErrorStatus.ResponseStatus> action = Callbacks.IntPtrToTempCallback<Action<CommonErrorStatus.ResponseStatus>>(data);
			if (action != null)
			{
				try
				{
					action(response);
				}
				catch (Exception arg)
				{
					Logger.e("Error encountered executing InternalLeaveRoomCallback. Smothering to avoid passing exception into Native: " + arg);
				}
			}
		}

		internal void LeaveRoom(NativeRealTimeRoom room, Action<CommonErrorStatus.ResponseStatus> callback)
		{
			RealTimeMultiplayerManager.RealTimeMultiplayerManager_LeaveRoom(mGameServices.AsHandle(), room.AsPointer(), InternalLeaveRoomCallback, Callbacks.ToIntPtr(callback));
		}

		internal void AcceptInvitation(MultiplayerInvitation invitation, RealTimeEventListenerHelper listener, Action<RealTimeRoomResponse> callback)
		{
			RealTimeMultiplayerManager.RealTimeMultiplayerManager_AcceptInvitation(mGameServices.AsHandle(), invitation.AsPointer(), listener.AsPointer(), InternalRealTimeRoomCallback, ToCallbackPointer(callback));
		}

		internal void DeclineInvitation(MultiplayerInvitation invitation)
		{
			RealTimeMultiplayerManager.RealTimeMultiplayerManager_DeclineInvitation(mGameServices.AsHandle(), invitation.AsPointer());
		}

		internal void SendReliableMessage(NativeRealTimeRoom room, MultiplayerParticipant participant, byte[] data, Action<CommonErrorStatus.MultiplayerStatus> callback)
		{
			RealTimeMultiplayerManager.RealTimeMultiplayerManager_SendReliableMessage(mGameServices.AsHandle(), room.AsPointer(), participant.AsPointer(), data, PInvokeUtilities.ArrayToSizeT(data), InternalSendReliableMessageCallback, Callbacks.ToIntPtr(callback));
		}

		[MonoPInvokeCallback(typeof(RealTimeMultiplayerManager.SendReliableMessageCallback))]
		internal static void InternalSendReliableMessageCallback(CommonErrorStatus.MultiplayerStatus response, IntPtr data)
		{
			Logger.d("Entering internal callback for InternalSendReliableMessageCallback " + response);
			Action<CommonErrorStatus.MultiplayerStatus> action = Callbacks.IntPtrToTempCallback<Action<CommonErrorStatus.MultiplayerStatus>>(data);
			if (action != null)
			{
				try
				{
					action(response);
				}
				catch (Exception arg)
				{
					Logger.e("Error encountered executing InternalSendReliableMessageCallback. Smothering to avoid passing exception into Native: " + arg);
				}
			}
		}

		internal void SendUnreliableMessageToAll(NativeRealTimeRoom room, byte[] data)
		{
			RealTimeMultiplayerManager.RealTimeMultiplayerManager_SendUnreliableMessageToOthers(mGameServices.AsHandle(), room.AsPointer(), data, PInvokeUtilities.ArrayToSizeT(data));
		}

		internal void SendUnreliableMessageToSpecificParticipants(NativeRealTimeRoom room, List<MultiplayerParticipant> recipients, byte[] data)
		{
			RealTimeMultiplayerManager.RealTimeMultiplayerManager_SendUnreliableMessage(mGameServices.AsHandle(), room.AsPointer(), (from r in recipients
				select r.AsPointer()).ToArray(), new UIntPtr((ulong)recipients.LongCount()), data, PInvokeUtilities.ArrayToSizeT(data));
		}

		private static IntPtr ToCallbackPointer(Action<RealTimeRoomResponse> callback)
		{
			return Callbacks.ToIntPtr(callback, RealTimeRoomResponse.FromPointer);
		}
	}
}

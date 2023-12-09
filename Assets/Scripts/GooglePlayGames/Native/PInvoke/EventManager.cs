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
	internal class EventManager
	{
		internal class FetchResponse : BaseReferenceHolder
		{
			internal FetchResponse(IntPtr selfPointer)
				: base(selfPointer)
			{
			}

			internal CommonErrorStatus.ResponseStatus ResponseStatus()
			{
				return GooglePlayGames.Native.Cwrapper.EventManager.EventManager_FetchResponse_GetStatus(SelfPtr());
			}

			internal bool RequestSucceeded()
			{
				return ResponseStatus() > (CommonErrorStatus.ResponseStatus)0;
			}

			internal NativeEvent Data()
			{
				if (!RequestSucceeded())
				{
					return null;
				}
				return new NativeEvent(GooglePlayGames.Native.Cwrapper.EventManager.EventManager_FetchResponse_GetData(SelfPtr()));
			}

			protected override void CallDispose(HandleRef selfPointer)
			{
				GooglePlayGames.Native.Cwrapper.EventManager.EventManager_FetchResponse_Dispose(selfPointer);
			}

			internal static FetchResponse FromPointer(IntPtr pointer)
			{
				if (pointer.Equals(IntPtr.Zero))
				{
					return null;
				}
				return new FetchResponse(pointer);
			}
		}

		internal class FetchAllResponse : BaseReferenceHolder
		{
			internal FetchAllResponse(IntPtr selfPointer)
				: base(selfPointer)
			{
			}

			internal CommonErrorStatus.ResponseStatus ResponseStatus()
			{
				return GooglePlayGames.Native.Cwrapper.EventManager.EventManager_FetchAllResponse_GetStatus(SelfPtr());
			}

			internal List<NativeEvent> Data()
			{
				IntPtr[] source = PInvokeUtilities.OutParamsToArray((IntPtr[] out_arg, UIntPtr out_size) => GooglePlayGames.Native.Cwrapper.EventManager.EventManager_FetchAllResponse_GetData(SelfPtr(), out_arg, out_size));
				return (from ptr in source
					select new NativeEvent(ptr)).ToList();
			}

			internal bool RequestSucceeded()
			{
				return ResponseStatus() > (CommonErrorStatus.ResponseStatus)0;
			}

			protected override void CallDispose(HandleRef selfPointer)
			{
				GooglePlayGames.Native.Cwrapper.EventManager.EventManager_FetchAllResponse_Dispose(selfPointer);
			}

			internal static FetchAllResponse FromPointer(IntPtr pointer)
			{
				if (pointer.Equals(IntPtr.Zero))
				{
					return null;
				}
				return new FetchAllResponse(pointer);
			}
		}

		private readonly GameServices mServices;

		[CompilerGenerated]
		private static Func<IntPtr, FetchAllResponse> _003C_003Ef__mg_0024cache0;

		[CompilerGenerated]
		private static GooglePlayGames.Native.Cwrapper.EventManager.FetchAllCallback _003C_003Ef__mg_0024cache1;

		[CompilerGenerated]
		private static Func<IntPtr, FetchResponse> _003C_003Ef__mg_0024cache2;

		[CompilerGenerated]
		private static GooglePlayGames.Native.Cwrapper.EventManager.FetchCallback _003C_003Ef__mg_0024cache3;

		internal EventManager(GameServices services)
		{
			mServices = Misc.CheckNotNull(services);
		}

		internal void FetchAll(Types.DataSource source, Action<FetchAllResponse> callback)
		{
			GooglePlayGames.Native.Cwrapper.EventManager.EventManager_FetchAll(mServices.AsHandle(), source, InternalFetchAllCallback, Callbacks.ToIntPtr(callback, FetchAllResponse.FromPointer));
		}

		[MonoPInvokeCallback(typeof(GooglePlayGames.Native.Cwrapper.EventManager.FetchAllCallback))]
		internal static void InternalFetchAllCallback(IntPtr response, IntPtr data)
		{
			Callbacks.PerformInternalCallback("EventManager#FetchAllCallback", Callbacks.Type.Temporary, response, data);
		}

		internal void Fetch(Types.DataSource source, string eventId, Action<FetchResponse> callback)
		{
			GooglePlayGames.Native.Cwrapper.EventManager.EventManager_Fetch(mServices.AsHandle(), source, eventId, InternalFetchCallback, Callbacks.ToIntPtr(callback, FetchResponse.FromPointer));
		}

		[MonoPInvokeCallback(typeof(GooglePlayGames.Native.Cwrapper.EventManager.FetchCallback))]
		internal static void InternalFetchCallback(IntPtr response, IntPtr data)
		{
			Callbacks.PerformInternalCallback("EventManager#FetchCallback", Callbacks.Type.Temporary, response, data);
		}

		internal void Increment(string eventId, uint steps)
		{
			GooglePlayGames.Native.Cwrapper.EventManager.EventManager_Increment(mServices.AsHandle(), eventId, steps);
		}
	}
}

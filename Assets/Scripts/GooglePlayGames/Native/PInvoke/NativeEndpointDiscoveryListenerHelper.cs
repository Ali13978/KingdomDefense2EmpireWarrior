using AOT;
using GooglePlayGames.Native.Cwrapper;
using GooglePlayGames.OurUtils;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace GooglePlayGames.Native.PInvoke
{
	internal class NativeEndpointDiscoveryListenerHelper : BaseReferenceHolder
	{
		[CompilerGenerated]
		private static Func<IntPtr, NativeEndpointDetails> _003C_003Ef__mg_0024cache0;

		[CompilerGenerated]
		private static EndpointDiscoveryListenerHelper.OnEndpointFoundCallback _003C_003Ef__mg_0024cache1;

		[CompilerGenerated]
		private static EndpointDiscoveryListenerHelper.OnEndpointLostCallback _003C_003Ef__mg_0024cache2;

		internal NativeEndpointDiscoveryListenerHelper()
			: base(EndpointDiscoveryListenerHelper.EndpointDiscoveryListenerHelper_Construct())
		{
		}

		protected override void CallDispose(HandleRef selfPointer)
		{
			EndpointDiscoveryListenerHelper.EndpointDiscoveryListenerHelper_Dispose(selfPointer);
		}

		internal void SetOnEndpointFound(Action<long, NativeEndpointDetails> callback)
		{
			EndpointDiscoveryListenerHelper.EndpointDiscoveryListenerHelper_SetOnEndpointFoundCallback(SelfPtr(), InternalOnEndpointFoundCallback, Callbacks.ToIntPtr(callback, NativeEndpointDetails.FromPointer));
		}

		[MonoPInvokeCallback(typeof(EndpointDiscoveryListenerHelper.OnEndpointFoundCallback))]
		private static void InternalOnEndpointFoundCallback(long id, IntPtr data, IntPtr userData)
		{
			Callbacks.PerformInternalCallback("NativeEndpointDiscoveryListenerHelper#InternalOnEndpointFoundCallback", Callbacks.Type.Permanent, id, data, userData);
		}

		internal void SetOnEndpointLostCallback(Action<long, string> callback)
		{
			EndpointDiscoveryListenerHelper.EndpointDiscoveryListenerHelper_SetOnEndpointLostCallback(SelfPtr(), InternalOnEndpointLostCallback, Callbacks.ToIntPtr(callback));
		}

		[MonoPInvokeCallback(typeof(EndpointDiscoveryListenerHelper.OnEndpointLostCallback))]
		private static void InternalOnEndpointLostCallback(long id, string lostEndpointId, IntPtr userData)
		{
			Action<long, string> action = Callbacks.IntPtrToPermanentCallback<Action<long, string>>(userData);
			if (action != null)
			{
				try
				{
					action(id, lostEndpointId);
				}
				catch (Exception arg)
				{
					Logger.e("Error encountered executing NativeEndpointDiscoveryListenerHelper#InternalOnEndpointLostCallback. Smothering to avoid passing exception into Native: " + arg);
				}
			}
		}
	}
}

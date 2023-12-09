using AOT;
using GooglePlayGames.Native.Cwrapper;
using GooglePlayGames.OurUtils;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace GooglePlayGames.Native.PInvoke
{
	internal class NativeMessageListenerHelper : BaseReferenceHolder
	{
		internal delegate void OnMessageReceived(long localClientId, string remoteEndpointId, byte[] data, bool isReliable);

		[CompilerGenerated]
		private static MessageListenerHelper.OnMessageReceivedCallback _003C_003Ef__mg_0024cache0;

		[CompilerGenerated]
		private static MessageListenerHelper.OnDisconnectedCallback _003C_003Ef__mg_0024cache1;

		internal NativeMessageListenerHelper()
			: base(MessageListenerHelper.MessageListenerHelper_Construct())
		{
		}

		protected override void CallDispose(HandleRef selfPointer)
		{
			MessageListenerHelper.MessageListenerHelper_Dispose(selfPointer);
		}

		internal void SetOnMessageReceivedCallback(OnMessageReceived callback)
		{
			MessageListenerHelper.MessageListenerHelper_SetOnMessageReceivedCallback(SelfPtr(), InternalOnMessageReceivedCallback, Callbacks.ToIntPtr(callback));
		}

		[MonoPInvokeCallback(typeof(MessageListenerHelper.OnMessageReceivedCallback))]
		private static void InternalOnMessageReceivedCallback(long id, string name, IntPtr data, UIntPtr dataLength, bool isReliable, IntPtr userData)
		{
			OnMessageReceived onMessageReceived = Callbacks.IntPtrToPermanentCallback<OnMessageReceived>(userData);
			if (onMessageReceived != null)
			{
				try
				{
					onMessageReceived(id, name, Callbacks.IntPtrAndSizeToByteArray(data, dataLength), isReliable);
				}
				catch (Exception arg)
				{
					Logger.e("Error encountered executing NativeMessageListenerHelper#InternalOnMessageReceivedCallback. Smothering to avoid passing exception into Native: " + arg);
				}
			}
		}

		internal void SetOnDisconnectedCallback(Action<long, string> callback)
		{
			MessageListenerHelper.MessageListenerHelper_SetOnDisconnectedCallback(SelfPtr(), InternalOnDisconnectedCallback, Callbacks.ToIntPtr(callback));
		}

		[MonoPInvokeCallback(typeof(MessageListenerHelper.OnDisconnectedCallback))]
		private static void InternalOnDisconnectedCallback(long id, string lostEndpointId, IntPtr userData)
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
					Logger.e("Error encountered executing NativeMessageListenerHelper#InternalOnDisconnectedCallback. Smothering to avoid passing exception into Native: " + arg);
				}
			}
		}
	}
}

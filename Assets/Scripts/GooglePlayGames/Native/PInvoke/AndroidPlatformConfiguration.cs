using AOT;
using GooglePlayGames.Native.Cwrapper;
using GooglePlayGames.OurUtils;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace GooglePlayGames.Native.PInvoke
{
	internal sealed class AndroidPlatformConfiguration : PlatformConfiguration
	{
		private delegate void IntentHandlerInternal(IntPtr intent, IntPtr userData);

		[CompilerGenerated]
		private static GooglePlayGames.Native.Cwrapper.AndroidPlatformConfiguration.IntentHandler _003C_003Ef__mg_0024cache0;

		private AndroidPlatformConfiguration(IntPtr selfPointer)
			: base(selfPointer)
		{
		}

		internal void SetActivity(IntPtr activity)
		{
			GooglePlayGames.Native.Cwrapper.AndroidPlatformConfiguration.AndroidPlatformConfiguration_SetActivity(SelfPtr(), activity);
		}

		internal void SetOptionalIntentHandlerForUI(Action<IntPtr> intentHandler)
		{
			Misc.CheckNotNull(intentHandler);
			GooglePlayGames.Native.Cwrapper.AndroidPlatformConfiguration.AndroidPlatformConfiguration_SetOptionalIntentHandlerForUI(SelfPtr(), InternalIntentHandler, Callbacks.ToIntPtr(intentHandler));
		}

		internal void SetOptionalViewForPopups(IntPtr view)
		{
			GooglePlayGames.Native.Cwrapper.AndroidPlatformConfiguration.AndroidPlatformConfiguration_SetOptionalViewForPopups(SelfPtr(), view);
		}

		protected override void CallDispose(HandleRef selfPointer)
		{
			GooglePlayGames.Native.Cwrapper.AndroidPlatformConfiguration.AndroidPlatformConfiguration_Dispose(selfPointer);
		}

		[MonoPInvokeCallback(typeof(IntentHandlerInternal))]
		private static void InternalIntentHandler(IntPtr intent, IntPtr userData)
		{
			Callbacks.PerformInternalCallback("AndroidPlatformConfiguration#InternalIntentHandler", Callbacks.Type.Permanent, intent, userData);
		}

		internal static AndroidPlatformConfiguration Create()
		{
			IntPtr selfPointer = GooglePlayGames.Native.Cwrapper.AndroidPlatformConfiguration.AndroidPlatformConfiguration_Construct();
			return new AndroidPlatformConfiguration(selfPointer);
		}
	}
}

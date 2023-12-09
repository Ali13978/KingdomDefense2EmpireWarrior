using AOT;
using GooglePlayGames.Native.Cwrapper;
using GooglePlayGames.OurUtils;
using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace GooglePlayGames.Native.PInvoke
{
	internal class NearbyConnectionsManagerBuilder : BaseReferenceHolder
	{
		[CompilerGenerated]
		private static NearbyConnectionsBuilder.OnInitializationFinishedCallback _003C_003Ef__mg_0024cache0;

		internal NearbyConnectionsManagerBuilder()
			: base(NearbyConnectionsBuilder.NearbyConnections_Builder_Construct())
		{
		}

		internal NearbyConnectionsManagerBuilder SetOnInitializationFinished(Action<NearbyConnectionsStatus.InitializationStatus> callback)
		{
			NearbyConnectionsBuilder.NearbyConnections_Builder_SetOnInitializationFinished(SelfPtr(), InternalOnInitializationFinishedCallback, Callbacks.ToIntPtr(callback));
			return this;
		}

		[MonoPInvokeCallback(typeof(NearbyConnectionsBuilder.OnInitializationFinishedCallback))]
		private static void InternalOnInitializationFinishedCallback(NearbyConnectionsStatus.InitializationStatus status, IntPtr userData)
		{
			Action<NearbyConnectionsStatus.InitializationStatus> action = Callbacks.IntPtrToPermanentCallback<Action<NearbyConnectionsStatus.InitializationStatus>>(userData);
			if (action == null)
			{
				Logger.w("Callback for Initialization is null. Received status: " + status);
			}
			else
			{
				try
				{
					action(status);
				}
				catch (Exception arg)
				{
					Logger.e("Error encountered executing NearbyConnectionsManagerBuilder#InternalOnInitializationFinishedCallback. Smothering exception: " + arg);
				}
			}
		}

		internal NearbyConnectionsManagerBuilder SetLocalClientId(long localClientId)
		{
			NearbyConnectionsBuilder.NearbyConnections_Builder_SetClientId(SelfPtr(), localClientId);
			return this;
		}

		internal NearbyConnectionsManagerBuilder SetDefaultLogLevel(Types.LogLevel minLevel)
		{
			NearbyConnectionsBuilder.NearbyConnections_Builder_SetDefaultOnLog(SelfPtr(), minLevel);
			return this;
		}

		internal NearbyConnectionsManager Build(PlatformConfiguration configuration)
		{
			return new NearbyConnectionsManager(NearbyConnectionsBuilder.NearbyConnections_Builder_Create(SelfPtr(), configuration.AsPointer()));
		}

		protected override void CallDispose(HandleRef selfPointer)
		{
			NearbyConnectionsBuilder.NearbyConnections_Builder_Dispose(selfPointer);
		}
	}
}

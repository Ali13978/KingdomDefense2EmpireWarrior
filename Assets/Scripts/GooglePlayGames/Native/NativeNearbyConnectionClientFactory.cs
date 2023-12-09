using GooglePlayGames.Android;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.Nearby;
using GooglePlayGames.Native.Cwrapper;
using GooglePlayGames.Native.PInvoke;
using GooglePlayGames.OurUtils;
using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace GooglePlayGames.Native
{
	public class NativeNearbyConnectionClientFactory
	{
		private static volatile NearbyConnectionsManager sManager;

		private static Action<INearbyConnectionClient> sCreationCallback;

		[CompilerGenerated]
		private static Action<NearbyConnectionsStatus.InitializationStatus> _003C_003Ef__mg_0024cache0;

		internal static NearbyConnectionsManager GetManager()
		{
			return sManager;
		}

		public static void Create(Action<INearbyConnectionClient> callback)
		{
			if (sManager == null)
			{
				sCreationCallback = callback;
				InitializeFactory();
			}
			else
			{
				callback(new NativeNearbyConnectionsClient(GetManager()));
			}
		}

		internal static void InitializeFactory()
		{
			PlayGamesHelperObject.CreateObject();
			NearbyConnectionsManager.ReadServiceId();
			NearbyConnectionsManagerBuilder nearbyConnectionsManagerBuilder = new NearbyConnectionsManagerBuilder();
			nearbyConnectionsManagerBuilder.SetOnInitializationFinished(OnManagerInitialized);
			PlatformConfiguration configuration = new AndroidClient().CreatePlatformConfiguration(PlayGamesClientConfiguration.DefaultConfiguration);
			UnityEngine.Debug.Log("Building manager Now");
			sManager = nearbyConnectionsManagerBuilder.Build(configuration);
		}

		internal static void OnManagerInitialized(NearbyConnectionsStatus.InitializationStatus status)
		{
			UnityEngine.Debug.Log("Nearby Init Complete: " + status + " sManager = " + sManager);
			if (status == NearbyConnectionsStatus.InitializationStatus.VALID)
			{
				if (sCreationCallback != null)
				{
					sCreationCallback(new NativeNearbyConnectionsClient(GetManager()));
					sCreationCallback = null;
				}
			}
			else
			{
				UnityEngine.Debug.LogError("ERROR: NearbyConnectionManager not initialized: " + status);
			}
		}
	}
}

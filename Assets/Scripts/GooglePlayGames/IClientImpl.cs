using GooglePlayGames.BasicApi;
using GooglePlayGames.Native.PInvoke;
using System;

namespace GooglePlayGames
{
	internal interface IClientImpl
	{
		PlatformConfiguration CreatePlatformConfiguration(PlayGamesClientConfiguration clientConfig);

		TokenClient CreateTokenClient(bool reset);

		void GetPlayerStats(IntPtr apiClientPtr, Action<CommonStatusCodes, PlayerStats> callback);

		void SetGravityForPopups(IntPtr apiClient, Gravity gravity);
	}
}

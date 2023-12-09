using System;
using System.Collections.Generic;
using UnityEngine;

namespace GooglePlayGames.BasicApi.Nearby
{
	public class DummyNearbyConnectionClient : INearbyConnectionClient
	{
		public int MaxUnreliableMessagePayloadLength()
		{
			return 1168;
		}

		public int MaxReliableMessagePayloadLength()
		{
			return 4096;
		}

		public void SendReliable(List<string> recipientEndpointIds, byte[] payload)
		{
			UnityEngine.Debug.LogError("SendReliable called from dummy implementation");
		}

		public void SendUnreliable(List<string> recipientEndpointIds, byte[] payload)
		{
			UnityEngine.Debug.LogError("SendUnreliable called from dummy implementation");
		}

		public void StartAdvertising(string name, List<string> appIdentifiers, TimeSpan? advertisingDuration, Action<AdvertisingResult> resultCallback, Action<ConnectionRequest> connectionRequestCallback)
		{
			AdvertisingResult obj = new AdvertisingResult(ResponseStatus.LicenseCheckFailed, string.Empty);
			resultCallback(obj);
		}

		public void StopAdvertising()
		{
			UnityEngine.Debug.LogError("StopAvertising in dummy implementation called");
		}

		public void SendConnectionRequest(string name, string remoteEndpointId, byte[] payload, Action<ConnectionResponse> responseCallback, IMessageListener listener)
		{
			UnityEngine.Debug.LogError("SendConnectionRequest called from dummy implementation");
			if (responseCallback != null)
			{
				ConnectionResponse obj = ConnectionResponse.Rejected(0L, string.Empty);
				responseCallback(obj);
			}
		}

		public void AcceptConnectionRequest(string remoteEndpointId, byte[] payload, IMessageListener listener)
		{
			UnityEngine.Debug.LogError("AcceptConnectionRequest in dummy implementation called");
		}

		public void StartDiscovery(string serviceId, TimeSpan? advertisingTimeout, IDiscoveryListener listener)
		{
			UnityEngine.Debug.LogError("StartDiscovery in dummy implementation called");
		}

		public void StopDiscovery(string serviceId)
		{
			UnityEngine.Debug.LogError("StopDiscovery in dummy implementation called");
		}

		public void RejectConnectionRequest(string requestingEndpointId)
		{
			UnityEngine.Debug.LogError("RejectConnectionRequest in dummy implementation called");
		}

		public void DisconnectFromEndpoint(string remoteEndpointId)
		{
			UnityEngine.Debug.LogError("DisconnectFromEndpoint in dummy implementation called");
		}

		public void StopAllConnections()
		{
			UnityEngine.Debug.LogError("StopAllConnections in dummy implementation called");
		}

		public string LocalEndpointId()
		{
			return string.Empty;
		}

		public string LocalDeviceId()
		{
			return "DummyDevice";
		}

		public string GetAppBundleId()
		{
			return "dummy.bundle.id";
		}

		public string GetServiceId()
		{
			return "dummy.service.id";
		}
	}
}

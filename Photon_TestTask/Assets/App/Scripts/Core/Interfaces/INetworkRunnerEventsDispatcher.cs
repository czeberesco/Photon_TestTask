using System;
using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;

namespace Core.Interfaces
{
	public interface INetworkRunnerEventsDispatcher
	{
		#region Events

		public event Action<NetworkObject, PlayerRef> ObjectExitAOI;
		public event Action<PlayerRef> PlayerJoined;
		public event Action<PlayerRef> PlayerLeft;
		public event Action<NetworkInput> Input;
		public event Action<PlayerRef, NetworkInput> InputMissing;
		public event Action<ShutdownReason> Shutdown;
		public event Action ConnectedToServer;
		public event Action<NetDisconnectReason> DisconnectedFromServer;
		public event Action<NetworkRunnerCallbackArgs.ConnectRequest, byte[]> ConnectRequest;
		public event Action<NetAddress, NetConnectFailedReason> ConnectFailed;
		public event Action<SimulationMessagePtr> UserSimulationMessage;
		public event Action<List<SessionInfo>> SessionListUpdated;
		public event Action<Dictionary<string, object>> CustomAuthenticationResponse;
		public event Action<HostMigrationToken> HostMigration;
		public event Action<PlayerRef, ReliableKey, ArraySegment<byte>> ReliableDataReceived;
		public event Action<PlayerRef, ReliableKey, float> ReliableDataProgress;
		public event Action SceneLoadDone;
		public event Action SceneLoadStart;

		#endregion
	}
}

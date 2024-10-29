using System;
using System.Collections.Generic;
using Core.Interfaces;
using Fusion;
using Fusion.Sockets;
using UnityEngine;
using Zenject;

namespace Core
{
	public class NetworkRunnerEventsDispatcher : INetworkRunnerEventsDispatcher, INetworkRunnerCallbacks, IInitializable, IDisposable
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

		#region PrivateFields

		private INetworkRunnerProvider m_networkRunnerProvider;
		private NetworkRunner m_currentRunner;

		#endregion

		#region Constructors

		public NetworkRunnerEventsDispatcher(INetworkRunnerProvider networkRunnerProvider)
		{
			m_networkRunnerProvider = networkRunnerProvider;
		}

		#endregion

		#region InterfaceImplementations

		public void Dispose()
		{
			Debug.Log($"Disposing {nameof(NetworkRunnerEventsDispatcher)}");
			
			UnregisterFromEvents();
			
			Debug.Log($"{nameof(NetworkRunnerEventsDispatcher)} disposed");
		}

		public void Initialize()
		{
			Debug.Log($"Initializing {nameof(NetworkRunnerEventsDispatcher)}");
			
			RegisterToEvents();
			
			Debug.Log($"{nameof(NetworkRunnerEventsDispatcher)} initialized");
		}

		public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
		{
			ObjectExitAOI?.Invoke(obj, player);
		}

		public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player)
		{
			ObjectExitAOI?.Invoke(obj, player);
		}

		public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
		{
			PlayerJoined?.Invoke(player);
		}

		public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
		{
			PlayerLeft?.Invoke(player);
		}

		public void OnInput(NetworkRunner runner, NetworkInput input)
		{
			Input?.Invoke(input);
		}

		public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
		{
			InputMissing?.Invoke(player, input);
		}

		public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
		{
			Shutdown?.Invoke(shutdownReason);
		}

		public void OnConnectedToServer(NetworkRunner runner)
		{
			ConnectedToServer?.Invoke();
		}

		public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason)
		{
			DisconnectedFromServer?.Invoke(reason);
		}

		public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
		{
			ConnectRequest?.Invoke(request, token);
		}

		public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
		{
			ConnectFailed?.Invoke(remoteAddress, reason);
		}

		public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
		{
			UserSimulationMessage?.Invoke(message);
		}

		public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
		{
			SessionListUpdated?.Invoke(sessionList);
		}

		public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
		{
			CustomAuthenticationResponse?.Invoke(data);
		}

		public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
		{
			HostMigration?.Invoke(hostMigrationToken);
		}

		public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data)
		{
			ReliableDataReceived?.Invoke(player, key, data);
		}

		public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress)
		{
			ReliableDataProgress?.Invoke(player, key, progress);
		}

		public void OnSceneLoadDone(NetworkRunner runner)
		{
			SceneLoadDone?.Invoke();
		}

		public void OnSceneLoadStart(NetworkRunner runner)
		{
			SceneLoadStart?.Invoke();
		}

		#endregion

		#region PrivateMethods

		private void RegisterToEvents()
		{
			m_networkRunnerProvider.RunnerInitialized += RegisterToRunner;
			m_networkRunnerProvider.RunnerWillBeDestroyed += UnregisterFromRunner;
		}

		private void UnregisterFromEvents()
		{
			m_networkRunnerProvider.RunnerWillBeDestroyed -= UnregisterFromRunner;
			m_networkRunnerProvider.RunnerInitialized -= RegisterToRunner;
		}

		private void RegisterToRunner(NetworkRunner runner)
		{
			if (m_currentRunner == runner)
			{
				Debug.LogWarning("Already registered to current runner");

				return;
			}

			m_currentRunner = runner;
			runner.AddCallbacks(this);
			
			Debug.Log($"{nameof(NetworkRunnerEventsDispatcher)} registered to {nameof(NetworkRunner)} {runner.name} callbacks");
		}

		private void UnregisterFromRunner(NetworkRunner runner)
		{
			runner.RemoveCallbacks(this);
			
			Debug.Log($"{nameof(NetworkRunnerEventsDispatcher)} unregistered from {nameof(NetworkRunner)} {runner.name} callbacks");
			
			m_currentRunner = null;
		}

		#endregion
	}
}

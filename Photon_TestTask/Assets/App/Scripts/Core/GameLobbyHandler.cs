using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Interfaces;
using Data;
using Fusion;
using UnityEngine;
using Utils;
using Zenject;

namespace Core
{
	public class GameLobbyHandler : IGameLobbyHandler, IInitializable, IDisposable
	{
		#region Events

		public event Action<StartGameResult, LobbyData> JoinLobbySuccess;
		public event Action<StartGameResult> JoinLobbyFailed;
		public event Action<List<SessionInfo>> SessionListUpdated;

		#endregion

		#region Properties

		public LobbyData CurrentLobby { get; private set; }

		#endregion

		#region PrivateFields

		private readonly INetworkRunnerProvider m_networkRunnerProvider;
		private readonly INetworkRunnerEventsDispatcher m_networkRunnerEventsDispatcher;
		private List<SessionInfo> m_currentSessions = new();

		#endregion

		#region Constructors

		public GameLobbyHandler(INetworkRunnerProvider networkRunnerProvider, INetworkRunnerEventsDispatcher networkRunnerEventsDispatcher)
		{
			m_networkRunnerProvider = networkRunnerProvider;
			m_networkRunnerEventsDispatcher = networkRunnerEventsDispatcher;
		}

		#endregion

		#region InterfaceImplementations

		public void Dispose()
		{
			Debug.Log($"Disposing {nameof(GameLobbyHandler)}");

			UnregisterFromEvents();

			Debug.Log($"{nameof(GameLobbyHandler)} disposed");
		}

		public List<SessionInfo> GetCurrentSessions()
		{
			return m_currentSessions;
		}

		public async void Initialize()
		{
			Debug.Log($"Initializing {nameof(GameLobbyHandler)}");

			RegisterToEvents();

			Debug.Log($"{nameof(GameLobbyHandler)} initialized");

			await JoinLobby(m_networkRunnerProvider.Runner, R.GAME_LOBBY_NAME);
		}

		#endregion

		#region PrivateMethods

		private async Task JoinLobby(NetworkRunner runner, string lobbyName)
		{
			Debug.Log($"Joining lobby {lobbyName} ...");

			StartGameResult result = await runner.JoinSessionLobby(SessionLobby.Shared, lobbyName);

			if (result.Ok)
			{
				Debug.Log($"Joined lobby {lobbyName}");
				CurrentLobby = new LobbyData(lobbyName);

				JoinLobbySuccess?.Invoke(result, CurrentLobby);

				return;
			}

			CurrentLobby = new LobbyData();
			Debug.LogError($"Failed to joined lobby: {result.ShutdownReason}");

			JoinLobbyFailed?.Invoke(result);
		}

		private void RegisterToEvents()
		{
			if (m_networkRunnerEventsDispatcher != null)
			{
				m_networkRunnerEventsDispatcher.SessionListUpdated += OnSessionListUpdated;
			}
		}

		private void UnregisterFromEvents()
		{
			if (m_networkRunnerEventsDispatcher != null)
			{
				m_networkRunnerEventsDispatcher.SessionListUpdated -= OnSessionListUpdated;
			}
		}

		private void OnSessionListUpdated(List<SessionInfo> sessionInfoList)
		{
			Debug.Log($"Lobby session list updated! Sessions found: {sessionInfoList.Count}");

			foreach (SessionInfo sessionInfo in sessionInfoList)
			{
				Debug.Log(
					$"Session name: [{sessionInfo.Name}]" +
					$", region: [{sessionInfo.Region}]" +
					$", max players: [{sessionInfo.MaxPlayers}]" +
					$", open status: [{sessionInfo.IsOpen}]"
				);
			}

			m_currentSessions = sessionInfoList;

			SessionListUpdated?.Invoke(m_currentSessions);
		}

		#endregion
	}
}

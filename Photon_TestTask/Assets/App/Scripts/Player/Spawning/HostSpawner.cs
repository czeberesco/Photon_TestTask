using System.Collections.Generic;
using Core.Interfaces;
using Data;
using Fusion;
using Player.Spawning.Data;
using UnityEngine;
using Zenject;

namespace Player.Spawning
{
	public class HostSpawner
	{
		#region PrivateFields

		private readonly INetworkRunnerProvider m_networkRunnerProvider;
		private readonly INetworkRunnerEventsDispatcher m_networkRunnerEventsDispatcher;
		private readonly PlayerPrefabsData m_playerPrefabsData;
		private readonly BasicSpawnStrategy m_spawnStrategy;
		private readonly Dictionary<PlayerRef, NetworkObject> m_players;

		#endregion

		#region Constructors

		public HostSpawner(
			INetworkRunnerProvider networkRunnerProvider,
			INetworkRunnerEventsDispatcher networkRunnerEventsDispatcher,
			PlayerPrefabsData playerPrefabsData,
			BasicSpawnStrategy spawnStrategy
		)
		{
			m_networkRunnerProvider = networkRunnerProvider;
			m_networkRunnerEventsDispatcher = networkRunnerEventsDispatcher;
			m_playerPrefabsData = playerPrefabsData;
			m_spawnStrategy = spawnStrategy;
			m_players = new Dictionary<PlayerRef, NetworkObject>();
		}

		#endregion

		#region PublicMethods

		public void Initialize()
		{
			RegisterToEvents();
		}

		public void Dispatch()
		{
			UnregisterFromEvents();
		}

		#endregion

		#region PrivateMethods

		private void RegisterToEvents()
		{
			m_networkRunnerEventsDispatcher.PlayerJoined += OnPlayerJoined;
			m_networkRunnerEventsDispatcher.PlayerLeft += OnPlayerLeft;
		}

		private void UnregisterFromEvents()
		{
			m_networkRunnerEventsDispatcher.PlayerLeft -= OnPlayerLeft;
			m_networkRunnerEventsDispatcher.PlayerJoined -= OnPlayerJoined;
		}

		private void OnPlayerJoined(PlayerRef playerRef)
		{
			Debug.Log($"{nameof(OnPlayerJoined)}. PlayerId: {playerRef.PlayerId}");

			NetworkRunner runner = m_networkRunnerProvider.Runner;
			PlayerSpawnData spawnData = m_spawnStrategy.GetSpawnData();

			NetworkObject networkPlayerObject = runner.Spawn(
				m_playerPrefabsData.PlayerNetworkPrefab,
				spawnData.Position,
				spawnData.Rotation,
				playerRef,
				OnBeforeSpawned
			);

			m_players.Add(playerRef, networkPlayerObject);
		}

		private void OnPlayerLeft(PlayerRef playerRef)
		{
			Debug.Log($"{nameof(OnPlayerLeft)}. PlayerId: {playerRef.PlayerId}");

			if (m_players.TryGetValue(playerRef, out NetworkObject networkObject))
			{
				NetworkRunner runner = m_networkRunnerProvider.Runner;

				runner.Despawn(networkObject);
				m_players.Remove(playerRef);
			}
		}

		private void OnBeforeSpawned(NetworkRunner runner, NetworkObject networkObject)
		{
			Debug.Log($"{nameof(OnBeforeSpawned)}, networkID: [{networkObject.Id}], name: [{networkObject.Name}]");
		}

		#endregion

		#region NestedTypes

		public class Factory : PlaceholderFactory<AbstractSpawnStrategy, HostSpawner> { }

		#endregion
	}
}

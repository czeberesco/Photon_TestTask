using Core.Interfaces;
using Data;
using Fusion;
using Player.Spawning.Data;
using UnityEngine;
using Zenject;

namespace Player.Spawning
{
	public class PlayerSpawner : SimulationBehaviour, IPlayerJoined
	{
		#region SerializeFields

		[SerializeField] private SpawnStrategy m_spawnStrategy;

		#endregion

		#region PrivateFields

		[Inject] private readonly PlayerPrefabsData m_playerPrefabsData;
		[Inject] private readonly INetworkRunnerProvider m_networkRunnerProvider;
		[Inject] private PlayerOfflineRig.Factory m_playerOfflineRigFactory;

		private PlayerOfflineRig m_playerOfflineRig;

		#endregion

		#region InterfaceImplementations

		public void PlayerJoined(PlayerRef player)
		{
		}

		#endregion

		#region PrivateMethods

		private void Awake()
		{
			RegisterToEvents();
			SpawnOfflineRig();
		}

		private void RegisterToEvents()
		{
			if (m_networkRunnerProvider.Runner.State == NetworkRunner.States.Running)
			{
				m_networkRunnerProvider?.Runner.AddGlobal(this);
			}
		}

		private void UnregisterFromEvents()
		{
			m_networkRunnerProvider?.Runner.RemoveGlobal(this);
		}

		private void SpawnOfflineRig()
		{
			if (m_playerOfflineRig != null)
			{
				Debug.LogWarning("Player offline rig already spawned");
			}

			PlayerSpawnData spawnData = m_spawnStrategy.GetSpawnData();
			m_playerOfflineRig = m_playerOfflineRigFactory.Create();
			m_playerOfflineRig.transform.SetPositionAndRotation(spawnData.Position, spawnData.Rotation);
		}

		#endregion
	}
}

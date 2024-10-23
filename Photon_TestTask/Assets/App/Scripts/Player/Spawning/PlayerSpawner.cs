using Core.Interfaces;
using Fusion;
using Player.Interfaces;
using Player.Spawning.Data;
using UnityEngine;
using Zenject;

namespace Player.Spawning
{
	public class PlayerSpawner : MonoBehaviour
	{
		#region SerializeFields

		[SerializeField] private AbstractSpawnStrategy m_spawnStrategy;

		#endregion

		#region PrivateFields

		[Inject] private readonly INetworkRunnerProvider m_networkRunnerProvider;
		[Inject] private readonly IPlayerOfflineRigProvider m_playerOfflineRigProvider;
		[Inject] private PlayerOfflineRig.Factory m_playerOfflineRigFactory;
		[Inject] private HostSpawner.Factory m_hostSpawnerFactory;

		private PlayerOfflineRig m_playerOfflineRig;

		// Only host create this class
		private HostSpawner m_hostSpawner;

		#endregion

		#region UnityMethods

		private void Awake()
		{
			SpawnOfflineRig();

			NetworkRunner runner = m_networkRunnerProvider.Runner;

			if (runner.IsServer)
			{
				m_hostSpawner = m_hostSpawnerFactory.Create(m_spawnStrategy);
				m_hostSpawner.Initialize();
			}
		}

		private void OnDestroy()
		{
			if (m_hostSpawner != null)
			{
				m_hostSpawner.Dispatch();
				m_hostSpawner = null;
			}
		}

		#endregion

		#region PrivateMethods


		// Ensure we have an offline rig inside scene, this could be enhanced to first check if there is offline rig GameObject
		// already on scene to prevent doubling offline rigs when placed on scene by hand
		private void SpawnOfflineRig()
		{
			if (m_playerOfflineRig != null)
			{
				Debug.LogWarning("Player offline rig already spawned");

				return;
			}

			Debug.Log($"Spawning {nameof(PlayerOfflineRig)}");

			PlayerSpawnData spawnData = m_spawnStrategy.GetSpawnData();
			m_playerOfflineRig = m_playerOfflineRigFactory.Create();
			m_playerOfflineRig.transform.SetPositionAndRotation(spawnData.Position, spawnData.Rotation);
			m_playerOfflineRigProvider.SetLocalPlayerOfflineRig(m_playerOfflineRig);

			Debug.Log($"{nameof(PlayerOfflineRig)} spawned");
		}

		#endregion
	}
}

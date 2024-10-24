using Core.Interfaces;
using Fusion;
using Player.Interfaces;
using Player.Spawning.Data;
using UnityEngine;
using VR.Offline;
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
		[Inject] private OfflineRig.Factory m_playerOfflineRigFactory;
		[Inject] private HostSpawner.Factory m_hostSpawnerFactory;

		private OfflineRig m_offlineRig;

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
			if (m_offlineRig != null)
			{
				Debug.LogWarning("Player offline rig already spawned");

				return;
			}

			Debug.Log($"Spawning {nameof(OfflineRig)}");

			PlayerSpawnData spawnData = m_spawnStrategy.GetSpawnData();
			m_offlineRig = m_playerOfflineRigFactory.Create();
			m_offlineRig.transform.SetPositionAndRotation(spawnData.Position, spawnData.Rotation);
			m_playerOfflineRigProvider.SetLocalPlayerOfflineRig(m_offlineRig);

			Debug.Log($"{nameof(OfflineRig)} spawned");
		}

		#endregion
	}
}

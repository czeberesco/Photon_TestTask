using Core.Interfaces;
using Fusion;
using UnityEngine;
using Zenject;

namespace Gameplay
{
	public class PlayerSpawner : SimulationBehaviour, IPlayerJoined
	{
		#region SerializeFields

		[SerializeField] private NetworkObject m_playerPrefab;

		#endregion

		#region PrivateFields

		[Inject] private INetworkRunnerProvider m_networkRunnerProvider;

		#endregion

		#region InterfaceImplementations

		public void PlayerJoined(PlayerRef player)
		{
			if (player == Runner.LocalPlayer)
			{
				Runner.Spawn(m_playerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
			}
		}

		#endregion

		#region PrivateMethods

		private void Start()
		{
			if (m_networkRunnerProvider.Runner.State != NetworkRunner.States.Running)
			{
				return;
			}

			m_networkRunnerProvider?.Runner.AddGlobal(this);
		}

		#endregion
	}
}

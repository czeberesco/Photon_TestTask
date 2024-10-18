using Core.Interfaces;
using Data;
using Fusion;
using UnityEngine;
using Zenject;

namespace Gameplay
{
	public class PlayerSpawner : SimulationBehaviour, IPlayerJoined
	{
		#region PrivateFields

		[Inject] private readonly PlayerPrefabsData m_playerPrefabsData;
		[Inject] private readonly INetworkRunnerProvider m_networkRunnerProvider;

		#endregion

		#region InterfaceImplementations

		public void PlayerJoined(PlayerRef player)
		{
			if (player == Runner.LocalPlayer)
			{
				Runner.Spawn(m_playerPrefabsData.PlayerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
			}
		}

		#endregion

		#region PrivateMethods

		private void Awake()
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

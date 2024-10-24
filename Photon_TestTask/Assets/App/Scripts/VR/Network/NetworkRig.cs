using Fusion;
using Player.Interfaces;
using UnityEngine;
using Utils;
using VR.Views;
using Zenject;

namespace VR.Network
{
	[DefaultExecutionOrder(R.ExecutionOrder.NETWORK_RIG)]
	public class NetworkRig : NetworkBehaviour, INetworkViewSetup
	{
		#region Properties

		private bool IsLocalPlayerRig => Object.HasInputAuthority && m_offlineRigProvider != null && m_offlineRigProvider.GetLocalPlayerOfflineRig() != null;

		#endregion

		#region SerializeFields

		[SerializeField] private NetworkRigViewSynchronizer m_viewSynchronizer;
		[SerializeField] private RigVisibilityHandler m_rigVisibilityHandler;

		#endregion

		#region PrivateFields

		[Inject] private IPlayerOfflineRigProvider m_offlineRigProvider;

		#endregion

		#region InterfaceImplementations

		public void SetViewForLocalPlayer()
		{
			m_rigVisibilityHandler.SetViewForLocalPlayer();
		}

		public void SetViewForProxyPlayer()
		{
			m_rigVisibilityHandler.SetViewForProxyPlayer();
		}

		#endregion

		#region PublicMethods

		public override void Spawned()
		{
			base.Spawned();

			Debug.Log($"[{nameof(NetworkRig)}] with name [{gameObject.name}] spawned");

			if (IsLocalPlayerRig)
			{
				SetViewForLocalPlayer();
				m_offlineRigProvider.GetLocalPlayerOfflineRig().SetViewForLocalPlayer();
			}
			else
			{
				SetViewForProxyPlayer();
			}
		}

		public override void Despawned(NetworkRunner runner, bool hasState)
		{
			base.Despawned(runner, hasState);

			if (IsLocalPlayerRig)
			{
				if (m_offlineRigProvider != null && m_offlineRigProvider.GetLocalPlayerOfflineRig() != null)
				{
					m_offlineRigProvider.GetLocalPlayerOfflineRig().SetViewForProxyPlayer();
				}
			}
		}

		public override void FixedUpdateNetwork()
		{
			if (GetInput(out RigInput rigInput))
			{
				m_viewSynchronizer.Synchronize(rigInput);
			}
		}

		// Synchronise view for local player - extrapolating to prevent VR sickness, only the visual positions
		// are updated, network positions are handled separately
		public override void Render()
		{
			if (IsLocalPlayerRig)
			{
				m_viewSynchronizer.Synchronize(m_offlineRigProvider.GetLocalPlayerOfflineRig().GetRigInput());
			}
		}

		#endregion
	}
}

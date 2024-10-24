using Fusion;
using Player.Interfaces;
using UnityEngine;
using Utils;
using Zenject;

namespace VR.Network
{
	[DefaultExecutionOrder(R.ExecutionOrder.NETWORK_RIG)]
	public class NetworkRig : NetworkBehaviour
	{
		#region SerializeFields

		[SerializeField] private NetworkRigViewSynchronizer m_viewSynchronizer;

		#endregion

		#region PrivateFields

		[Inject] private IPlayerOfflineRigProvider m_offlineRigProvider;

		#endregion

		#region PublicMethods

		public override void Spawned()
		{
			base.Spawned();

			Debug.Log($"[{nameof(NetworkRig)}] with name [{gameObject.name}] spawned");
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
			if (Object.HasInputAuthority && m_offlineRigProvider != null && m_offlineRigProvider.GetLocalPlayerOfflineRig() != null)
			{
				m_viewSynchronizer.Synchronize(m_offlineRigProvider.GetLocalPlayerOfflineRig().GetRigInput());
			}
		}

		#endregion
	}
}

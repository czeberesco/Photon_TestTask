using Fusion;
using Player;
using UnityEngine;
using Utils;

namespace VR.Network
{
	[DefaultExecutionOrder(R.ExecutionOrder.NETWORK_RIG)]
	public class NetworkRig : NetworkBehaviour
	{
		#region SerializeFields
		
		[SerializeField] private NetworkRigViewSynchronizer m_viewSynchronizer;

		#endregion

		#region PrivateFields

		private IRigInputProvider m_localOfflineRigInputProvider;

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
			if (Object.HasInputAuthority && m_localOfflineRigInputProvider != null)
			{
				m_viewSynchronizer.Synchronize(m_localOfflineRigInputProvider.GetRigInput());
			}
		}

		public void BindToLocalOfflineRig(IRigInputProvider rigInputProvider)
		{
			m_localOfflineRigInputProvider = rigInputProvider;
		}

		#endregion
	}
}

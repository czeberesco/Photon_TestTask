using Player.Interfaces;

namespace Player
{
	public class PlayerOfflineRigProvider : IPlayerOfflineRigProvider
	{
		#region PrivateFields

		private PlayerOfflineRig m_localPlayerOfflineRig;

		#endregion

		#region InterfaceImplementations

		public PlayerOfflineRig GetLocalPlayerOfflineRig()
		{
			return m_localPlayerOfflineRig;
		}

		public void SetLocalPlayerOfflineRig(PlayerOfflineRig playerOfflineRig)
		{
			m_localPlayerOfflineRig = playerOfflineRig;
		}

		#endregion
	}
}

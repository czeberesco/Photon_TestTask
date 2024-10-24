using Player.Interfaces;

namespace VR.Offline
{
	public class OfflineRigProvider : IPlayerOfflineRigProvider
	{
		#region PrivateFields

		private OfflineRig m_localOfflineRig;

		#endregion

		#region InterfaceImplementations

		public OfflineRig GetLocalPlayerOfflineRig()
		{
			return m_localOfflineRig;
		}

		public void SetLocalPlayerOfflineRig(OfflineRig offlineRig)
		{
			m_localOfflineRig = offlineRig;
		}

		#endregion
	}
}

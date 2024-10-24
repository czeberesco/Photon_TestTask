using VR.Offline;

namespace Player.Interfaces
{
	public interface IPlayerOfflineRigProvider
	{
		#region PublicMethods

		public OfflineRig GetLocalPlayerOfflineRig();
		public void SetLocalPlayerOfflineRig(OfflineRig offlineRig);

		#endregion
	}
}

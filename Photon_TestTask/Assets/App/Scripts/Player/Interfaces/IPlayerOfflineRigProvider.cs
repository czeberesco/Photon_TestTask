namespace Player.Interfaces
{
	public interface IPlayerOfflineRigProvider
	{
		#region PublicMethods

		public PlayerOfflineRig GetLocalPlayerOfflineRig();
		public void SetLocalPlayerOfflineRig(PlayerOfflineRig playerOfflineRig);

		#endregion
	}
}

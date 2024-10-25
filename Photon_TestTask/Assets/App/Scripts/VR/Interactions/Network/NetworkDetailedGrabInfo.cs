using System;
using Fusion;

namespace VR.Interactions.Network
{
	[Serializable]
	public struct NetworkDetailedGrabInfo : INetworkStruct
	{
		#region PublicFields

		public PlayerRef GrabbingUser;
		public NetworkBehaviourId GrabInteractorId;
		public NetworkGrabInfo NetworkGrabInfo;

		#endregion
	}
}

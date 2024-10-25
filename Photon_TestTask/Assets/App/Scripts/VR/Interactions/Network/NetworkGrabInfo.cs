using System;
using Fusion;
using UnityEngine;

namespace VR.Interactions.Network
{
	[Serializable]
	public struct NetworkGrabInfo : INetworkStruct
	{
		#region PublicFields

		public NetworkBehaviourId GrabbedObjectId;
		public Vector3 LocalPositionOffset;
		public Quaternion LocalRotationOffset;

		// We want the local user accurate ungrab position to be enforced on the network,
		// and so shared in the input (to avoid the grabbable following "too long" the grabber)
		public Vector3 UngrabPosition;
		public Quaternion UngrabRotation;
		public Vector3 UngrabVelocity;
		public Vector3 UngrabAngularVelocity;

		#endregion
	}
}

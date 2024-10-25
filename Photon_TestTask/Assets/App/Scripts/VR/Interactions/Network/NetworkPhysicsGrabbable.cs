using Fusion;
using Fusion.XR.Host.Grabbing;
using UnityEngine;
using Utils;
using VR.Interactions.Interactors;

namespace VR.Interactions.Network
{
	[DefaultExecutionOrder(R.ExecutionOrder.NETWORK_GRABBABLE)]
	public class NetworkPhysicsGrabbable : NetworkGrabbable, IInputAuthorityLost
	{
		[SerializeField] private CustomPhysicsGrabbable m_customPhysicsGrabbable;
		public override void Grab(NetworkGrabber newGrabber, NetworkGrabInfo newGrabInfo)
		{
			throw new System.NotImplementedException();
		}

		public override void Ungrab(NetworkGrabber grabber, NetworkGrabInfo newGrabInfo)
		{
			throw new System.NotImplementedException();
		}

		public void InputAuthorityLost()
		{
			throw new System.NotImplementedException();
		}
	}
}

using Fusion;
using UnityEngine;
using UnityEngine.Events;
using Utils;

namespace VR.Interactions.Network
{
	[DefaultExecutionOrder(R.ExecutionOrder.NETWORK_GRABBABLE)]
	public abstract class NetworkGrabbable : NetworkBehaviour
	{
		#region Properties

		protected virtual NetworkGrabber CurrentGrabber { get; set; }
		protected bool IsGrabbed => CurrentGrabber != null;

		#endregion

		#region PublicFields

		[Header("Events")] public UnityEvent Ungrabbed = new();
		public UnityEvent<NetworkGrabber> Grabbed = new();

		#endregion

		#region PublicMethods

		public abstract void Grab(NetworkGrabber newGrabber, NetworkGrabInfo newGrabInfo);
		public abstract void Ungrab(NetworkGrabber grabber, NetworkGrabInfo newGrabInfo);

		#endregion

		#region ProtectedMethods

		protected void DidGrab()
		{
			Grabbed?.Invoke(CurrentGrabber);
		}

		protected void DidUngrab(NetworkGrabber lastGrabber)
		{
			Ungrabbed?.Invoke();
		}

		#endregion
	}
}

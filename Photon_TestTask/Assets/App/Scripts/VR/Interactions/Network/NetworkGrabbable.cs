using Fusion;
using Fusion.XR.Host.Grabbing;
using UnityEngine;
using UnityEngine.Events;
using Utils;

namespace VR.Interactions
{
	[DefaultExecutionOrder(R.ExecutionOrder.NETWORK_GRABBABLE)]
	public abstract class NetworkGrabbable : NetworkBehaviour
	{
		#region Properties

		public virtual NetworkGrabber CurrentGrabber { get; set; }
		public bool IsGrabbed => CurrentGrabber != null;

		#endregion

		#region PublicFields

		[Header("Events")] public UnityEvent Ungrabbed = new();
		public UnityEvent<NetworkGrabber> Grabbed = new();

		#endregion

		#region PublicMethods

		public abstract void Grab(NetworkGrabber newGrabber, NetworkGrabInfo newGrabInfo);
		public abstract void Ungrab(NetworkGrabber grabber, NetworkGrabInfo newGrabInfo);

		public void DidGrab()
		{
			if (Grabbed != null)
			{
				Grabbed.Invoke(CurrentGrabber);
			}
		}

		public void DidUngrab(NetworkGrabber lastGrabber)
		{
			if (Grabbed != null)
			{
				Ungrabbed.Invoke();
			}
		}

		#endregion
	}
}

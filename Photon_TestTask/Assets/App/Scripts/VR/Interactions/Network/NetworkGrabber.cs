using Fusion;
using UnityEngine;
using Utils;
using VR.Network;

namespace VR.Interactions.Network
{
    /**
	 * Allows a NetworkHand to grab NetworkGrabbable objects
	 */
    [DefaultExecutionOrder(R.ExecutionOrder.NETWORK_GRABBER)]
	public class NetworkGrabber : NetworkBehaviour
	{
		#region Properties

		[Networked] public NetworkGrabInfo GrabInfo { get; set; }

		public NetworkHand Hand => m_hand;

		#endregion

		#region PublicFields

		public GrabbingKind SupportedgrabbingKind = GrabbingKind.PhysicsAndKinematic;

		#endregion

		#region SerializeFields

		[SerializeField] private NetworkTransform m_networkTransform;
		[SerializeField] private NetworkHand m_hand;

		#endregion

		#region PrivateFields

		private NetworkGrabbable m_grabbedObject;
		private ChangeDetector m_changeDetector;

		private bool m_isSimulated;

		private NetworkBehaviourId m_lastGrabbedObjectId = NetworkBehaviourId.None;
		private NetworkGrabbable m_lastGrabbedObject;

		#endregion

		#region PublicMethods

		public override void Spawned()
		{
			base.Spawned();

			m_changeDetector = GetChangeDetector(ChangeDetector.Source.SimulationState);
			HandleGrabInfoChange(GrabInfo);

			m_isSimulated = Object.HasInputAuthority || Object.HasStateAuthority;
		}

		public override void FixedUpdateNetwork()
		{
			base.FixedUpdateNetwork();

			if (Runner.IsForward)
			{
				// We only detect grabbing changes in forward, to avoid multiple Grab calls (that would have side effects in current implementation)
				foreach (string changedPropertyName in m_changeDetector.DetectChanges(this))
				{
					if (changedPropertyName == nameof(GrabInfo))
					{
						// Grab info is filled by the NetworkRig, based on the input, and the input are filled with the Hardware rig Grabber GrabInfo
						HandleGrabInfoChange(GrabInfo);
					}
				}
			}
		}

		public override void Render()
		{
			base.Render();

			if (SupportedgrabbingKind == GrabbingKind.PhysicsAndKinematic)
			{
				bool isGrabbing = GrabInfo.GrabbedObjectId != NetworkBehaviourId.None;

				if (m_lastGrabbedObjectId != GrabInfo.GrabbedObjectId)
				{
					m_lastGrabbedObject = null;

					if (isGrabbing && Object.Runner.TryFindBehaviour(GrabInfo.GrabbedObjectId, out NetworkGrabbable grabbedObject))
					{
						m_lastGrabbedObject = grabbedObject;
					}
				}

				if (m_isSimulated == false && isGrabbing && m_lastGrabbedObject && m_lastGrabbedObject is NetworkPhysicsGrabbable)
				{
					// The hands need to be simulated to be at the appropriate position during FUN when a grabbable follow them (physics grabbable are fully simulated)
					m_isSimulated = true;
					Runner.SetIsSimulated(Object, m_isSimulated);
				}

				if (m_isSimulated && isGrabbing == false && Object.HasStateAuthority == false && Object.HasInputAuthority == false)
				{
					m_isSimulated = false;
					Runner.SetIsSimulated(Object, m_isSimulated);
				}
			}
		}

		#endregion

		#region PrivateMethods

		private void HandleGrabInfoChange(NetworkGrabInfo newGrabInfo)
		{
			if (m_grabbedObject != null)
			{
				m_grabbedObject.Ungrab(this, newGrabInfo);
				m_grabbedObject = null;
			}

			// We have to look for the grabbed object has it has changed
			// If an object is grabbed, we look for it through the runner with its Id
			if (newGrabInfo.GrabbedObjectId != NetworkBehaviourId.None && Object.Runner.TryFindBehaviour(newGrabInfo.GrabbedObjectId, out NetworkGrabbable newGrabbedObject))
			{
				m_grabbedObject = newGrabbedObject;

				if (m_grabbedObject != null)
				{
					m_grabbedObject.Grab(this, newGrabInfo);
				}
			}
		}

		#endregion

		#region Enums

		public enum GrabbingKind
		{
			KinematicOnly,
			PhysicsAndKinematic
		}

		#endregion
	}
}

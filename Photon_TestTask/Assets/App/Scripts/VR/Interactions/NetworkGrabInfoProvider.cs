using Fusion;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using VR.Interactions.Interactors;
using VR.Interactions.Network;

namespace VR.Interactions
{
	public class NetworkGrabInfoProvider : MonoBehaviour
	{
		#region Properties

		public NetworkGrabInfo GrabInfo => m_grabInfo;

		#endregion

		#region SerializeFields

		[SerializeField] private NearFarInteractor m_interactor;

		#endregion

		#region PrivateFields

		private CustomAbstractGrabbable m_grabbedObject;
		private NetworkGrabInfo m_grabInfo;

		#endregion

		#region UnityMethods

		private void Awake()
		{
			RegisterToEvents();
		}

		private void OnDestroy()
		{
			UnregisterFromEvents();
		}

		#endregion

		#region PrivateMethods

		private void RegisterToEvents()
		{
			m_interactor.selectEntered.AddListener(OnGrab);
			m_interactor.selectExited.AddListener(OnUngrab);
		}

		private void UnregisterFromEvents()
		{
			m_interactor.selectExited.RemoveListener(OnUngrab);
			m_interactor.selectEntered.RemoveListener(OnGrab);
		}

		private void OnGrab(SelectEnterEventArgs selectEnterEventArgs)
		{
			CustomAbstractGrabbable grabbable = selectEnterEventArgs.interactableObject.transform.GetComponent<CustomAbstractGrabbable>();

			if (grabbable == null)
			{
				return;
			}

			grabbable.UpdateOffsetDataWithInteractor(m_interactor);

			m_grabInfo.LocalPositionOffset = grabbable.LocalPositionOffset;
			m_grabInfo.LocalRotationOffset = grabbable.LocalRotationOffset;

			NetworkGrabbable networkGrabbable = grabbable.GetComponent<NetworkGrabbable>();

			if (networkGrabbable != null)
			{
				m_grabInfo.GrabbedObjectId = networkGrabbable.Id;
			}

			m_grabbedObject = grabbable;
		}

		private void OnUngrab(SelectExitEventArgs selectExitEventArgs)
		{
			if (m_grabbedObject == null || selectExitEventArgs.interactableObject.transform != m_grabbedObject.transform)
			{
				return;
			}

			NetworkGrabbable networkGrabbable = m_grabbedObject.transform.GetComponent<NetworkGrabbable>();

			if (networkGrabbable != null)
			{
				m_grabInfo.GrabbedObjectId = NetworkBehaviourId.None;
				m_grabInfo.UngrabPosition = networkGrabbable.transform.position;
				m_grabInfo.UngrabRotation = networkGrabbable.transform.rotation;
				m_grabInfo.UngrabVelocity = m_grabbedObject.Velocity;
				m_grabInfo.UngrabAngularVelocity = m_grabbedObject.AngularVelocity;
			}

			m_grabbedObject = null;
		}

		#endregion
	}
}

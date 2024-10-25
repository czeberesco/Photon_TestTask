using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

namespace VR.Interactions.Interactors
{
	public abstract class CustomAbstractGrabbable : XRBaseInteractable
	{
		#region Properties

		public abstract Vector3 Velocity { get; }

		public abstract Vector3 AngularVelocity { get; }

		#endregion

		#region SerializeFields

		[SerializeField] protected NetworkGrabbable m_networkGrabbable;

		#endregion

		#region ProtectedFields

		protected IXRSelectInteractor m_currentInteractor;
		protected Vector3 m_localPositionOffset;
		protected Quaternion m_localRotationOffset;
		protected Vector3 m_ungrabPosition;
		protected Quaternion m_ungrabRotation;
		protected Vector3 m_ungrabVelocity;
		protected Vector3 m_ungrabAngularVelocity;

		protected bool m_isGrabbed;

		#endregion

		#region ProtectedMethods

		protected override void OnSelectEntered(SelectEnterEventArgs selectEnterEventArgs)
		{
			if (m_isGrabbed)
			{
				return;
			}

			IXRSelectInteractor interactor = selectEnterEventArgs.interactorObject;

			m_localPositionOffset = interactor.transform.InverseTransformPoint(transform.position);
			m_localRotationOffset = Quaternion.Inverse(interactor.transform.rotation) * transform.rotation;
			m_currentInteractor = interactor;
			m_isGrabbed = true;
		}

		protected override void OnSelectExited(SelectExitEventArgs selectExitEventArgs)
		{
			m_currentInteractor = null;

			if (m_networkGrabbable)
			{
				Transform networkGrabbableTransform = m_networkGrabbable.transform;
				m_ungrabPosition = networkGrabbableTransform.position;
				m_ungrabRotation = networkGrabbableTransform.rotation;
				m_ungrabVelocity = Velocity;
				m_ungrabAngularVelocity = AngularVelocity;
			}

			m_isGrabbed = false;
		}

		#endregion
	}
}

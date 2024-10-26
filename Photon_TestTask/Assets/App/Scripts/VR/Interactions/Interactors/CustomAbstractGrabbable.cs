using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using VR.Interactions.Network;

namespace VR.Interactions.Interactors
{
	public abstract class CustomAbstractGrabbable : XRBaseInteractable
	{
		#region Properties

		public abstract Vector3 Velocity { get; }

		public abstract Vector3 AngularVelocity { get; }

		public Vector3 LocalPositionOffset
		{
			get => m_localPositionOffset;
			set => m_localPositionOffset = value;
		}

		public Quaternion LocalRotationOffset
		{
			get => m_localRotationOffset;
			set => m_localRotationOffset = value;
		}

		public Vector3 UngrabPosition => m_ungrabPosition;
		public Quaternion UngrabRotation => m_ungrabRotation;
		public Vector3 UngrabVelocity => m_ungrabVelocity;
		public Vector3 UngrabAngularVelocity => m_ungrabAngularVelocity;

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

		#region PublicMethods

		public void UpdateOffsetDataWithInteractor(IXRSelectInteractor interactor)
		{
			m_localPositionOffset = interactor.transform.InverseTransformPoint(transform.position);
			m_localRotationOffset = Quaternion.Inverse(interactor.transform.rotation) * transform.rotation;
		}

		#endregion

		#region ProtectedMethods

		protected override void OnSelectEntered(SelectEnterEventArgs selectEnterEventArgs)
		{
			if (m_isGrabbed)
			{
				return;
			}

			IXRSelectInteractor interactor = selectEnterEventArgs.interactorObject;

			UpdateOffsetDataWithInteractor(interactor);
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

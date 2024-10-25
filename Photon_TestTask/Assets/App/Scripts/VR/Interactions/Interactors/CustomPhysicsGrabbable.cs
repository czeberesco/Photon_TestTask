using UnityEngine;
using Utils;
using VelocityExtension = Fusion.XR.Shared.VelocityExtension;

namespace VR.Interactions.Interactors
{
	public class CustomPhysicsGrabbable : CustomAbstractGrabbable
	{
		#region Properties

		public override Vector3 Velocity => m_rigidbody.velocity;

		public override Vector3 AngularVelocity => m_rigidbody.angularVelocity;
		public bool IsGrabbed => m_currentInteractor != null;

		#endregion

		#region SerializeFields

		[SerializeField] private Rigidbody m_rigidbody;

		#endregion

		#region PrivateFields

		private bool m_isCollidingOffline;

		#endregion

		#region ProtectedMethods

		protected override void Awake()
		{
			base.Awake();

			m_rigidbody.isKinematic = false;
		}

		#endregion

		#region PrivateMethods

		private void FixedUpdate()
		{
			// We handle the following if we are not online (in that case, the Follow will be called by the NetworkGrabbable during FUN and Render)
			if (m_networkGrabbable == null || m_networkGrabbable.Object == null)
			{
				// Note that this offline following will not offer the pseudo-haptic feedback (it could easily be recreated offline if needed)
				if (IsGrabbed)
				{
					Follow(m_currentInteractor.transform, Time.fixedDeltaTime, m_isCollidingOffline);
				}
			}

			m_isCollidingOffline = false;
		}

		private void OnCollisionStay(Collision collision)
		{
			m_isCollidingOffline = true;
		}

		#endregion

		#region Follow configuration

		[Header("Follow configuration")] [Range(0, 1)] [SerializeField] private float m_followVelocityAttenuation = 0.5f;
		[SerializeField] private float m_maxVelocity = 10f;

		public enum EInteractablePhysicsFollowMode
		{
			Velocity,
			PID
		}

		[SerializeField] private EInteractablePhysicsFollowMode m_physicsFollowMode = EInteractablePhysicsFollowMode.Velocity;

		[Header("PID")] [SerializeField] private PIDState m_pid = new()
		{
			PidSettings = new PIDSettings
			{
				ProportionalGain = 0.75f,
				IntegralGain = 0.01f,
				DerivativeGain = 0.12f,
				MaxIntegrationMagnitude = 10f
			}
		};
		[SerializeField] private float m_commandScale = 1.5f;
		[SerializeField] private float m_maxCommandMagnitude = 100f;
		[SerializeField] private bool m_ignorePidIntegrationWhileColliding = true;

		#endregion

		#region Following logic

		public virtual void Follow(Transform followedTransform, float elapsedTime, bool isColliding)
		{
			if (m_physicsFollowMode == EInteractablePhysicsFollowMode.PID)
			{
				PIDFollow(followedTransform, elapsedTime, isColliding);
			}
			else if (m_physicsFollowMode == EInteractablePhysicsFollowMode.Velocity)
			{
				VelocityFollow(followedTransform, elapsedTime);
			}
		}

		public virtual void PIDFollow(Transform followedTransform, float elapsedTime, bool isColliding)
		{
			Vector3 targetPosition = followedTransform.TransformPoint(m_localPositionOffset);
			Quaternion targetRotation = followedTransform.rotation * m_localRotationOffset;

			Vector3 error = targetPosition - m_rigidbody.position;
			bool ignoreIntegration = m_ignorePidIntegrationWhileColliding && isColliding;

			if (ignoreIntegration)
			{
				m_pid.ErrorIntegration = Vector3.zero;
			}

			Vector3 command = m_pid.UpdateCommand(error, elapsedTime, ignoreIntegration);
			Vector3 impulse = Vector3.ClampMagnitude(m_commandScale * command, m_maxCommandMagnitude);
			m_rigidbody.AddForce(impulse, ForceMode.Impulse);
			m_rigidbody.angularVelocity = VelocityExtension.AngularVelocityChange(m_rigidbody.transform.rotation, targetRotation, elapsedTime);
		}

		public virtual void VelocityFollow(Transform followedTransform, float elapsedTime)
		{
			// Compute the requested velocity to joined target position during a Runner.DeltaTime
			m_rigidbody.VelocityFollow(followedTransform, m_localPositionOffset, m_localRotationOffset, elapsedTime);

			// To avoid a too aggressive move, we attenuate and limit a bit the expected velocity
			Vector3 velocity = m_rigidbody.velocity;

			velocity *= m_followVelocityAttenuation; // followVelocityAttenuation = 0.5F by default
			m_rigidbody.velocity = velocity;
			m_rigidbody.velocity = Vector3.ClampMagnitude(velocity, m_maxVelocity); // maxVelocity = 10f by default
		}

		#endregion
	}
}

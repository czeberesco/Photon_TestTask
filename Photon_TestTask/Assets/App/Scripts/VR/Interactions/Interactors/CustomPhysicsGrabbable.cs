using UnityEngine;
using Utils;

namespace VR.Interactions.Interactors
{
	public class CustomPhysicsGrabbable : CustomAbstractGrabbable
	{
		#region Properties

		public override Vector3 Velocity => Rigidbody.velocity;

		public override Vector3 AngularVelocity => Rigidbody.angularVelocity;

		public bool IsGrabbed
		{
			get => m_currentInteractor != null;
			set => m_isGrabbed = value;
		}

		public PIDState PID => m_pid;

		public EInteractablePhysicsFollowMode PhysicsFollowMode => m_physicsFollowMode;

		public Rigidbody Rigidbody => m_rigidbody;

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

			Rigidbody.isKinematic = false;
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
			if (PhysicsFollowMode == EInteractablePhysicsFollowMode.PID)
			{
				PIDFollow(followedTransform, elapsedTime, isColliding);
			}
			else if (PhysicsFollowMode == EInteractablePhysicsFollowMode.Velocity)
			{
				VelocityFollow(followedTransform, elapsedTime);
			}
		}

		protected virtual void PIDFollow(Transform followedTransform, float elapsedTime, bool isColliding)
		{
			Vector3 targetPosition = followedTransform.TransformPoint(m_localPositionOffset);
			Quaternion targetRotation = followedTransform.rotation * m_localRotationOffset;

			Vector3 error = targetPosition - Rigidbody.position;
			bool ignoreIntegration = m_ignorePidIntegrationWhileColliding && isColliding;

			if (ignoreIntegration)
			{
				m_pid.ErrorIntegration = Vector3.zero;
			}

			Vector3 command = PID.UpdateCommand(error, elapsedTime, ignoreIntegration);
			Vector3 impulse = Vector3.ClampMagnitude(m_commandScale * command, m_maxCommandMagnitude);
			Rigidbody.AddForce(impulse, ForceMode.Impulse);
			Rigidbody.angularVelocity = Rigidbody.transform.rotation.AngularVelocityChange(targetRotation, elapsedTime);
		}

		protected virtual void VelocityFollow(Transform followedTransform, float elapsedTime)
		{
			// Compute the requested velocity to joined target position during a Runner.DeltaTime
			Rigidbody.VelocityFollow(followedTransform, m_localPositionOffset, m_localRotationOffset, elapsedTime);

			// To avoid a too aggressive move, we attenuate and limit a bit the expected velocity
			Vector3 velocity = Rigidbody.velocity;
			velocity *= m_followVelocityAttenuation; // followVelocityAttenuation = 0.5F by default
			Rigidbody.velocity = velocity;
			Rigidbody.velocity = Vector3.ClampMagnitude(velocity, m_maxVelocity); // maxVelocity = 10f by default
		}

		#endregion
	}
}

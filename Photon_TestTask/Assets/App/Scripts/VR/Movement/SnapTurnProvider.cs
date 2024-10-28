using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Locomotion;

namespace VR.Movement
{
	public class SnapTurnProvider : LocomotionProvider
	{
		#region Events

		[SerializeField] private InputActionReference m_snapTurnInputAction;

		#endregion

		#region SerializeFields

		[SerializeField] private float m_turnAmount = 45f;

		#endregion

		#region PrivateFields

		private XRBodyYawRotation m_transformation = new();

		private float m_currentTurnAmount;
		private float m_timeStarted;
		private float m_delayStartTime;

		#endregion

		#region ProtectedMethods

		protected void OnEnable()
		{
			RegisterToEvents();
		}

		protected void OnDisable()
		{
			UnregisterFromEvents();
		}

		#endregion

		#region PrivateMethods

		private void RegisterToEvents()
		{
			m_snapTurnInputAction.action.performed += TurnButtonPressed;
		}

		private void UnregisterFromEvents()
		{
			m_snapTurnInputAction.action.performed -= TurnButtonPressed;
		}

		private void TurnButtonPressed(InputAction.CallbackContext callbackContext)
		{
			Rotate(callbackContext.ReadValue<float>() > 0 ? m_turnAmount : -m_turnAmount);
		}

		private void Rotate(float angle)
		{
			TryStartLocomotionImmediately();
			m_transformation.angleDelta = angle;
			TryQueueTransformation(m_transformation);
			TryEndLocomotion();
		}

		#endregion
	}
}

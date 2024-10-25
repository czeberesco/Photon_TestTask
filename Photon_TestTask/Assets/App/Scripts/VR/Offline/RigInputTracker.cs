using Player;
using UnityEngine;
using UnityEngine.InputSystem;
using VR.Interactions;

namespace VR.Offline
{
	public class RigInputTracker : MonoBehaviour, IRigInputProvider
	{
		#region Events

		[SerializeField] private InputActionReference m_leftHandGripPressedInputActionReference;
		[SerializeField] private InputActionReference m_leftHandGripInputActionReference;
		[SerializeField] private InputActionReference m_leftHandTriggerPressedInputActionReference;
		[SerializeField] private InputActionReference m_leftHandTriggerInputActionReference;

		[SerializeField] private InputActionReference m_rightHandGripPressedInputActionReference;
		[SerializeField] private InputActionReference m_rightHandGripInputActionReference;
		[SerializeField] private InputActionReference m_rightHandTriggerPressedInputActionReference;
		[SerializeField] private InputActionReference m_rightInputTriggerInputActionReference;

		#endregion

		#region SerializeFields

		[SerializeField] private Transform m_playArea;
		[SerializeField] private Transform m_head;
		[SerializeField] private Transform m_leftHand;
		[SerializeField] private Transform m_rightHand;
		[SerializeField] private NetworkGrabInfoProvider m_leftHandNetworkGrabInfoProvider;
		[SerializeField] private NetworkGrabInfoProvider m_rightHandNetworkGrabInfoProvider;

		#endregion

		#region InterfaceImplementations

		public RigInput GetRigInput()
		{
			return new RigInput
			{
				LeftControllerInput = new ControllerInput
				{
					GripPressed = m_leftHandGripPressedInputActionReference.action.IsPressed(),
					GripInputValue = m_leftHandGripInputActionReference.action.ReadValue<float>(),
					TriggerPressed = m_leftHandTriggerPressedInputActionReference.action.IsPressed(),
					TriggerInputValue = m_leftHandTriggerInputActionReference.action.ReadValue<float>()
				},

				RightControllerInput = new ControllerInput
				{
					GripPressed = m_rightHandGripPressedInputActionReference.action.IsPressed(),
					GripInputValue = m_rightHandGripInputActionReference.action.ReadValue<float>(),
					TriggerPressed = m_rightHandTriggerPressedInputActionReference.action.IsPressed(),
					TriggerInputValue = m_rightInputTriggerInputActionReference.action.ReadValue<float>()
				},
				
				PlayAreaPosition = m_playArea.position,
				PlayAreaRotation = m_playArea.rotation,
				HeadPosition = m_head.position,
				HeadRotation = m_head.rotation,
				LeftHandPosition = m_leftHand.position,
				LeftHandRotation = m_leftHand.rotation,
				RightHandPosition = m_rightHand.position,
				RightHandRotation = m_rightHand.rotation,
				LeftNetworkGrabInfo = m_leftHandNetworkGrabInfoProvider.GrabInfo,
				RightNetworkGrabInfo = m_rightHandNetworkGrabInfoProvider.GrabInfo
			};
		}

		#endregion
	}
}

using Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace VR.Offline
{
	public class RigInputTracker : MonoBehaviour, IRigInputProvider
	{
		#region Events

		[SerializeField] private InputActionReference m_leftHandGripInputActionReference;
		[SerializeField] private InputActionReference m_leftHandTriggerInputActionReference;
		[SerializeField] private InputActionReference m_rightHandGripInputActionReference;
		[SerializeField] private InputActionReference m_rightInputTriggerInputActionReference;

		#endregion

		#region SerializeFields

		[SerializeField] private Transform m_playArea;
		[SerializeField] private Transform m_head;
		[SerializeField] private Transform m_leftHand;
		[SerializeField] private Transform m_rightHand;

		#endregion

		#region InterfaceImplementations

		public RigInput GetRigInput()
		{
			return new RigInput
			{
				PlayAreaPosition = m_playArea.position,
				PlayAreaRotation = m_playArea.rotation,
				HeadPosition = m_head.position,
				HeadRotation = m_head.rotation,
				LeftHandPosition = m_leftHand.position,
				LeftHandRotation = m_leftHand.rotation,
				RightHandPosition = m_rightHand.position,
				RightHandRotation = m_rightHand.rotation,

				LeftControllerInput = new ControllerInput
				{
					GripInputValue = m_leftHandGripInputActionReference.action.ReadValue<float>(),
					TriggerInputValue = m_leftHandTriggerInputActionReference.action.ReadValue<float>()
				},

				RightControllerInput = new ControllerInput
				{
					GripInputValue = m_rightHandGripInputActionReference.action.ReadValue<float>(),
					TriggerInputValue = m_rightInputTriggerInputActionReference.action.ReadValue<float>()
				}
			};
		}

		#endregion
	}
}

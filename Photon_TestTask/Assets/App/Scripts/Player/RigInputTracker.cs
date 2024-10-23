using UnityEngine;
using VR;

namespace Player
{
	public class RigInputTracker : MonoBehaviour, IRigInputProvider
	{
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
				RightHandRotation = m_rightHand.rotation
			};
		}

		#endregion
	}
}

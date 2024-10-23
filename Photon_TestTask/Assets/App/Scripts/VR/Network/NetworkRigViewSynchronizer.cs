using Fusion;
using UnityEngine;

namespace VR.Network
{
	public class NetworkRigViewSynchronizer : MonoBehaviour
	{
		#region SerializeFields

		[SerializeField] private NetworkTransform m_playArea;
		[SerializeField] private NetworkTransform m_head;
		[SerializeField] private NetworkTransform m_leftHand;
		[SerializeField] private NetworkTransform m_rightHand;

		#endregion

		#region PublicMethods

		public void Synchronize(RigInput rigInput)
		{
			Transform playAreaTransform = m_playArea.transform;
			Transform headTransform = m_head.transform;
			Transform leftHandTransform = m_leftHand.transform;
			Transform rightHandTransform = m_rightHand.transform;

			playAreaTransform.position = rigInput.PlayAreaPosition;
			playAreaTransform.rotation = rigInput.PlayAreaRotation;

			headTransform.position = rigInput.HeadPosition;
			headTransform.rotation = rigInput.HeadRotation;

			leftHandTransform.position = rigInput.LeftHandPosition;
			leftHandTransform.rotation = rigInput.LeftHandRotation;

			rightHandTransform.position = rigInput.RightHandPosition;
			rightHandTransform.rotation = rigInput.RightHandRotation;
		}

		#endregion
	}
}

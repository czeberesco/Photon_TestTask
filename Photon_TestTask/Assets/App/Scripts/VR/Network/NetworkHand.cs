using Fusion;
using UnityEngine;
using Utils;
using VR.Animation;

namespace VR.Network
{
	[DefaultExecutionOrder(R.ExecutionOrder.NETWORK_HAND)]
	public class NetworkHand : NetworkBehaviour
	{
		#region SerializeFields

		[SerializeField] private HandAnimationController m_handAnimationController;

		#endregion

		#region PublicMethods

		public void SetControllerInput(ControllerInput controllerInput)
		{
			m_handAnimationController.SetTargetGripValue(controllerInput.GripInputValue);
			m_handAnimationController.SetTargetTriggerValue(controllerInput.TriggerInputValue);
		}

		#endregion
	}
}

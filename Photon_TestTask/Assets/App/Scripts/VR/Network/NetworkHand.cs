using Fusion;
using UnityEngine;
using Utils;
using VR.Views;

namespace VR.Network
{
	[DefaultExecutionOrder(R.ExecutionOrder.NETWORK_HAND)]
	public class NetworkHand : NetworkBehaviour
	{
		#region SerializeFields

		[SerializeField] private HandView m_handView;

		#endregion

		#region PublicMethods

		public void SetControllerInput(ControllerInput controllerInput)
		{
			m_handView.UpdateHandViewInput(controllerInput);
		}

		#endregion
	}
}

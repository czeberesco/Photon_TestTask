using Fusion;
using UnityEngine;
using Utils;

namespace VR.Network
{
	[DefaultExecutionOrder(R.ExecutionOrder.NETWORK_HAND)]
	public class NetworkHand : NetworkBehaviour
	{
		[SerializeField] private NetworkTransform m_networkTransform;
	}
}

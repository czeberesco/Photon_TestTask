using System;
using Player.Interfaces;
using UnityEngine;
using VR.Enums;

namespace VR.Views
{
	public class RigVisibilityHandler : MonoBehaviour, INetworkViewSetup
	{
		#region SerializeFields

		[SerializeField] private VisibilityData m_localPlayerVisibilityData;
		[SerializeField] private VisibilityData m_proxyPlayerVisibilityData;

		[SerializeField] private RigElementVisibilityHandler m_headVisibilityHandler;
		[SerializeField] private RigElementVisibilityHandler m_leftHandVisibilityHandler;
		[SerializeField] private RigElementVisibilityHandler m_rightHandVisibilityHandler;

		#endregion

		#region NestedTypes

		[Serializable]
		public struct VisibilityData
		{
			#region PublicFields

			public EElementVisibilityState Head;
			public EElementVisibilityState LeftHand;
			public EElementVisibilityState RightHand;

			#endregion
		}

		#endregion

		public void SetViewForLocalPlayer()
		{
			m_headVisibilityHandler.SetVisibilityState(m_localPlayerVisibilityData.Head);
			m_leftHandVisibilityHandler.SetVisibilityState(m_localPlayerVisibilityData.LeftHand);
			m_rightHandVisibilityHandler.SetVisibilityState(m_localPlayerVisibilityData.RightHand);
		}

		public void SetViewForProxyPlayer()
		{
			m_headVisibilityHandler.SetVisibilityState(m_proxyPlayerVisibilityData.Head);
			m_leftHandVisibilityHandler.SetVisibilityState(m_proxyPlayerVisibilityData.LeftHand);
			m_rightHandVisibilityHandler.SetVisibilityState(m_proxyPlayerVisibilityData.RightHand);
		}
	}
}

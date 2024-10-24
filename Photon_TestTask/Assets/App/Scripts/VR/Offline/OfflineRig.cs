using Core.Interfaces;
using Fusion;
using Player;
using Player.Interfaces;
using UnityEngine;
using VR.Views;
using Zenject;

namespace VR.Offline
{
	public class OfflineRig : MonoBehaviour, IRigInputProvider, INetworkViewSetup
	{
		#region SerializeFields

		[SerializeField] private RigInputTracker m_rigInputTracker;
		[SerializeField] private RigVisibilityHandler m_rigVisibilityHandler;

		#endregion

		#region PrivateFields

		[Inject] private readonly INetworkRunnerEventsDispatcher m_networkRunnerEventsDispatcher;

		#endregion

		#region UnityMethods

		private void Awake()
		{
			RegisterToEvents();
		}

		private void OnDestroy()
		{
			UnregisterFromEvents();
		}

		#endregion

		#region InterfaceImplementations

		public void SetViewForLocalPlayer()
		{
			m_rigVisibilityHandler.SetViewForLocalPlayer();
		}

		public void SetViewForProxyPlayer()
		{
			m_rigVisibilityHandler.SetViewForProxyPlayer();
		}

		public RigInput GetRigInput()
		{
			return m_rigInputTracker.GetRigInput();
		}

		#endregion

		#region PrivateMethods

		private void RegisterToEvents()
		{
			m_networkRunnerEventsDispatcher.Input += OnInput;
		}

		private void UnregisterFromEvents()
		{
			m_networkRunnerEventsDispatcher.Input -= OnInput;
		}

		private void OnInput(NetworkInput input)
		{
			input.Set(m_rigInputTracker.GetRigInput());
		}

		#endregion

		#region NestedTypes

		public class Factory : PlaceholderFactory<OfflineRig> { }

		#endregion
	}
}

using Core.Interfaces;
using Fusion;
using UnityEngine;
using VR;
using Zenject;

namespace Player
{
	public class PlayerOfflineRig : MonoBehaviour, IRigInputProvider
	{
		#region SerializeFields

		[SerializeField] private RigInputTracker m_rigInputTracker;

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

		public class Factory : PlaceholderFactory<PlayerOfflineRig> { }

		#endregion
	}
}

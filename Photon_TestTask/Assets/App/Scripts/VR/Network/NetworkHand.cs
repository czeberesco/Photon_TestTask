using Core.Interfaces;
using Fusion;
using UnityEngine;
using Utils;
using VR.Views;
using Zenject;

namespace VR.Network
{
	[DefaultExecutionOrder(R.ExecutionOrder.NETWORK_HAND)]
	public class NetworkHand : NetworkBehaviour
	{
		#region Properties

		[Networked] private ControllerInput ControllerInput { get; set; }

		#endregion

		#region SerializeFields

		[SerializeField] private HandView m_handView;

		#endregion

		#region PrivateFields

		[Inject] private INetworkRunnerProvider m_networkRunnerProvider;
		private ChangeDetector m_changeDetector;

		#endregion

		#region PublicMethods

		public void SetControllerInput(ControllerInput controllerInput)
		{
			if (Object.HasInputAuthority && m_networkRunnerProvider.Runner.IsServer)
			{
				ControllerInput = controllerInput;
			}

			m_handView.UpdateHandViewInput(controllerInput);
		}

		public override void Spawned()
		{
			base.Spawned();
			m_changeDetector = GetChangeDetector(ChangeDetector.Source.SnapshotFrom);
		}

		public override void Render()
		{
			base.Render();

			UpdateProxiesControllerInput();
		}

		#endregion

		#region PrivateMethods

		private void UpdateProxiesControllerInput()
		{
			foreach (string change in m_changeDetector.DetectChanges(this))
			{
				if (change == nameof(ControllerInput))
				{
					m_handView.UpdateHandViewInput(ControllerInput);
				}
			}
		}

		#endregion
	}
}

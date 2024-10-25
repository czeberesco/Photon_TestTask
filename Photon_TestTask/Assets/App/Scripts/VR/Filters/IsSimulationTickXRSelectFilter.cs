using Core.Interfaces;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Filtering;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using Zenject;

namespace VR.Filters
{
	public class IsSimulationTickXRSelectFilter : MonoBehaviour, IXRSelectFilter
	{
		#region Properties

		public bool canProcess
		{
			get
			{
				if (m_networkRunnerProvider != null && m_networkRunnerProvider.Runner != null)
				{
					return m_networkRunnerProvider.Runner.IsForward;
				}

				return true;
			}
		}

		#endregion

		#region PrivateFields

		[Inject] private INetworkRunnerProvider m_networkRunnerProvider;

		#endregion

		#region InterfaceImplementations

		public bool Process(IXRSelectInteractor interactor, IXRSelectInteractable interactable)
		{
			return true;
		}

		#endregion
	}
}

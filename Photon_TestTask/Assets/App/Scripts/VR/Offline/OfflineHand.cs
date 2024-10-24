using UnityEngine;
using UnityEngine.InputSystem;
using VR.Views;

namespace VR.Offline
{
	public class OfflineHand : MonoBehaviour
	{
		#region Events

		[SerializeField] private InputActionReference m_gripInputActionReference;
		[SerializeField] private InputActionReference m_triggerInputActionReference;

		#endregion

		#region SerializeFields

		[SerializeField] private HandView m_handView;

		#endregion

		#region UnityMethods

		private void Update()
		{
			m_handView.UpdateHandViewInput(
				new ControllerInput
				{
					GripInputValue = m_gripInputActionReference.action.ReadValue<float>(),
					TriggerInputValue = m_triggerInputActionReference.action.ReadValue<float>()
				}
			);
		}

		#endregion
	}
}

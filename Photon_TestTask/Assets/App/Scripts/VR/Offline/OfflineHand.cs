using UnityEngine;
using UnityEngine.InputSystem;
using VR.Animation;

namespace VR.Offline
{
	public class OfflineHand : MonoBehaviour
	{
		#region Events

		[SerializeField] private InputActionReference m_gripInputActionReference;
		[SerializeField] private InputActionReference m_triggerInputActionReference;

		#endregion

		#region SerializeFields

		[SerializeField] private HandAnimationController m_handAnimationController;

		#endregion

		#region UnityMethods

		private void Update()
		{
			m_handAnimationController.SetTargetGripValue(m_gripInputActionReference.action.ReadValue<float>());
			m_handAnimationController.SetTargetTriggerValue(m_triggerInputActionReference.action.ReadValue<float>());
		}

		#endregion
	}
}

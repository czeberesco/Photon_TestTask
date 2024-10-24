using UnityEngine;
using UnityEngine.InputSystem;

namespace VR.Hands
{
	public class OfflineRigHandAnimationController : HandAnimationControllerBase
	{
		#region Events

		[SerializeField] private InputActionReference m_gripInputActionReference;
		[SerializeField] private InputActionReference m_triggerInputActionReference;

		#endregion

		#region SerializeFields

		[SerializeField] private float m_animationSpeed = 10.0f;

		#endregion

		#region PrivateFields

		private float m_gripAnimatedValue;
		private float m_triggerAnimatedValue;

		#endregion

		#region PrivateMethods

		#region UnityMethods

		private void Update()
		{
			AnimateGrip();
			AnimateTrigger();
		}

		#endregion

		private void AnimateGrip()
		{
			m_gripAnimatedValue = Mathf.MoveTowards(
				m_gripAnimatedValue,
				m_gripInputActionReference.action.ReadValue<float>(),
				Time.deltaTime * m_animationSpeed
			);

			SetGripValue(m_gripAnimatedValue);
		}

		private void AnimateTrigger()
		{
			m_triggerAnimatedValue = Mathf.MoveTowards(
				m_triggerAnimatedValue,
				m_triggerInputActionReference.action.ReadValue<float>(),
				Time.deltaTime * m_animationSpeed
			);

			SetTriggerValue(m_triggerAnimatedValue);
		}

		#endregion
	}
}

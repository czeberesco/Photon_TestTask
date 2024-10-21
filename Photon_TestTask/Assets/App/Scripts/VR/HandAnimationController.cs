using UnityEngine;
using UnityEngine.InputSystem;

namespace VR
{
	public class HandAnimationController : MonoBehaviour
	{
		#region Statics

		private static readonly int m_gripHash = Animator.StringToHash("Grip");
		private static readonly int m_triggerHash = Animator.StringToHash("Trigger");

		#endregion

		#region Events

		[SerializeField] private InputActionReference m_gripInputActionReference;
		[SerializeField] private InputActionReference m_triggerInputActionReference;

		#endregion

		#region SerializeFields

		[SerializeField] private Animator m_animator;
		[SerializeField] private float m_animationSpeed = 10.0f;

		#endregion

		#region PrivateFields

		private float m_gripValue;
		private float m_triggerValue;

		#endregion

		#region UnityMethods

		private void Update()
		{
			AnimateGrip();
			AnimateTrigger();
		}

		#endregion

		#region PrivateMethods

		private void AnimateGrip()
		{
			m_gripValue = Mathf.MoveTowards(
				m_gripValue,
				m_gripInputActionReference.action.ReadValue<float>(),
				Time.deltaTime * m_animationSpeed
			);

			m_animator.SetFloat(m_gripHash, m_gripValue);
		}

		private void AnimateTrigger()
		{
			m_triggerValue = Mathf.MoveTowards(
				m_triggerValue,
				m_triggerInputActionReference.action.ReadValue<float>(),
				Time.deltaTime * m_animationSpeed
			);

			m_animator.SetFloat(m_triggerHash, m_triggerValue);
		}

		#endregion
	}
}
